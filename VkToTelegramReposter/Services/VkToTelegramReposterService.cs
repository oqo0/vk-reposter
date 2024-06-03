using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using VkTelegramReposter.Configuration;
using VkTelegramReposter.Vk;

namespace VkTelegramReposter.Services;

public class VkToTelegramReposterService(
    TelegramBotClient telegramBotClient,
    Config config) : IHostedService
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
            vkGroupClient.StartListening(cancellationToken);
        }
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void HandleNewPost(ulong groupId, string newPostText, string[] images)
    {
        string telegramMessageText = newPostText + "\n" + string.Join("\n", images);;
        
        telegramBotClient.SendTextMessageAsync(
            chatId: config.Reposters.First(r => r.VkGroupId == groupId).TelegramChannelId,
            text: telegramMessageText
        );
    }
}