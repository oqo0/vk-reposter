using System.Text.Json;
using VkNet;
using VkNet.Model;
using VkTelegramReposter.Vk.Entities;

namespace VkTelegramReposter.Vk;

public class VkGroupClient
{
    private readonly VkApi _vkApi = new();
    private readonly LongPollUrl _longPollUrl;
    private readonly TimeSpan _checkCooldownDelay;

    public readonly ulong GroupId;
    
    /// <summary>
    /// Parameters: group id, post message, images
    /// </summary>
    public event Func<ulong, ulong, string, string[], Task>? OnNewGroupPost;

    public VkGroupClient(ulong groupId, string groupPrivateToken)
        : this(groupId, groupPrivateToken, TimeSpan.FromSeconds(3)) { }

    public VkGroupClient(ulong groupId, string groupPrivateToken, TimeSpan checkCooldownDelay)
    {
        var authParams = new ApiAuthParams
        {
            AccessToken = groupPrivateToken
        };
        _vkApi.Authorize(authParams);

        var response = _vkApi.Groups.GetLongPollServer(groupId);
        _longPollUrl = new LongPollUrl(response.Server, response.Key, response.Ts);

        GroupId = groupId;
        _checkCooldownDelay = checkCooldownDelay;
    }

    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        using var httpClient = new HttpClient();
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                // await Task.Delay(_checkCooldownDelay, cancellationToken);

                var updatesCheckResult = await CheckForUpdates(httpClient);
                if (!updatesCheckResult)
                    return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in StartListeningAsync for group {GroupId}: {ex.Message}");
            }
        }
    }

    private async Task<bool> CheckForUpdates(HttpClient httpClient)
    {
        try
        {
            var response = await httpClient.GetStringAsync(_longPollUrl.Form());
            var vkResponse = JsonSerializer.Deserialize<LongPollResponse>(response);

            if (vkResponse == null)
                return false;

            foreach (var update in vkResponse.Updates)
            {
                ProcessUpdate(update);
            }

            _longPollUrl.TimeStamp = Convert.ToUInt32(vkResponse.Ts);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in CheckForUpdates for group {GroupId}: {ex.Message}");
            return false;
        }
    }

    private void ProcessUpdate(Update update)
    {
        if (update.Type != "wall_post_new" || update.Object == null)
            return;

        var post = update.Object;
        var attachmentUrlList = new List<string>();

        foreach (var attachment in post.Attachments)
        {
            if (attachment.Photo == null)
                continue;

            var photoUrl = attachment.Photo.Sizes.LastOrDefault()?.Url;
            if (!string.IsNullOrEmpty(photoUrl))
            {
                attachmentUrlList.Add(photoUrl);
            }
        }

        OnNewGroupPost?.Invoke(GroupId, post.Id, post.Text, attachmentUrlList.ToArray());
    }
}
