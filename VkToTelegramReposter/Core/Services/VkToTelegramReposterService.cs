using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using VkTelegramReposter.Configuration;
using VkTelegramReposter.Core.MessageFormatters;
using VkTelegramReposter.Vk;

namespace VkTelegramReposter.Core.Services;

public class VkToTelegramReposterService(
    TelegramBotClient telegramBotClient,
    Config config,
    IMessageFormatter messageFormatter,
    ILogger<VkToTelegramReposterService> logger) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var reposter in config.Reposters)
        {
            var vkGroupClient = new VkGroupClient(
                reposter.VkGroupId,
                reposter.VkGroupToken,
                TimeSpan.FromSeconds(config.CheckCooldown));
            
            vkGroupClient.OnNewGroupPost += HandleNewPost;
            _ = vkGroupClient.StartListeningAsync(cancellationToken);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task HandleNewPost(ulong groupId, ulong postId, string newPostText, string[] images)
    {
        logger.Log(
            LogLevel.Information, 
            $"New Vk post received: \n" +
            $"group id: {groupId}, " +
            $"post id: {postId}");
        
        string messageText = await messageFormatter.FormatAsync(groupId, postId, newPostText, images);
        
        await telegramBotClient.SendTextMessageAsync(
            chatId: config.Reposters.First(r => r.VkGroupId == groupId).TelegramChannelId,
            text: messageText
        );
    }
}