using System.Text.Json;
using VkNet;
using VkNet.Model;
using VkTelegramReposter.Vk.Entities;

namespace VkTelegramReposter.Vk;

public class VkGroupClient
{
    public readonly ulong GroupId;
    private readonly VkApi _vkApi = new();
    private readonly TimeSpan _checkCooldownDelay;
    private LongPollUrl _longPollUrl = null!;
    
    /// <summary>
    /// Parameters: group id, post message, images
    /// </summary>
    public event Func<ulong, ulong, string, long, string[], Task>? OnNewGroupPost;

    public VkGroupClient(ulong groupId, string groupPrivateToken)
        : this(groupId, groupPrivateToken, TimeSpan.FromSeconds(3)) { }

    public VkGroupClient(ulong groupId, string groupPrivateToken, TimeSpan checkCooldownDelay)
    {
        var authParams = new ApiAuthParams
        {
            AccessToken = groupPrivateToken
        };
        _vkApi.Authorize(authParams);

        UpdateLongPollServer();
        
        GroupId = groupId;
        _checkCooldownDelay = checkCooldownDelay;
    }

    public async Task StartListeningAsync(CancellationToken cancellationToken)
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

    private void UpdateLongPollServer()
    {
        var response = _vkApi.Groups.GetLongPollServer(GroupId);
        _longPollUrl = new LongPollUrl(response.Server, response.Key, response.Ts);
    }
    
    private async Task<bool> CheckForUpdates(HttpClient httpClient)
    {
        var response = "None";
        
        try
        {
            response = await httpClient.GetStringAsync(_longPollUrl.Form());
            return CheckResponseForUpdates(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in CheckForUpdates for group {GroupId}: {ex.Message} \n" +
                              $"Response: {response}");
            return false;
        }
    }

    private bool CheckResponseForUpdates(string response)
    {
        var vkResponse = JsonSerializer.Deserialize<LongPollResponse>(response);

        if (vkResponse == null)
            return false;

        if (vkResponse.Failed != null)
        {
            if (vkResponse.Failed == 1)
                _longPollUrl.TimeStamp = Convert.ToUInt32(vkResponse.Ts);
            else if (vkResponse.Failed is 2 or 3)
                UpdateLongPollServer();

            return true;
        }
            
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
        
        if (post.PostType != "post")
            return;
        
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

        OnNewGroupPost?.Invoke(GroupId, post.Id, post.Text, post.OwnerId, attachmentUrlList.ToArray());
    }
}
