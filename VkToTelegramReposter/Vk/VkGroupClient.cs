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

    public event Action<string, string[]>? OnNewGroupPost;

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

        _checkCooldownDelay = checkCooldownDelay;
    }

    public async Task StartListening(CancellationToken cancellationToken)
    {
        using var httpClient = new HttpClient();

        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(_checkCooldownDelay, cancellationToken);

            var updatesCheckResult = await CheckForUpdates(httpClient);
            if (!updatesCheckResult)
                return;
        }
    }

    private async Task<bool> CheckForUpdates(HttpClient httpClient)
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

        OnNewGroupPost?.Invoke(post.Text, attachmentUrlList.ToArray());
    }
}