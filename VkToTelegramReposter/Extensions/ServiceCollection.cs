using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using VkTelegramReposter.Configuration;
using VkTelegramReposter.Services;

namespace VkTelegramReposter.Extensions;

public static class ServiceCollection
{
    public static IServiceCollection AddVkToTelegramReposter(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddSingleton(_ => configuration.Get<Config>()!);
    
        serviceCollection.AddSingleton<TelegramBotClient>(_ =>
        {
            var botToken = configuration.GetRequiredSection("TelegramBotToken").Value!;
            return new TelegramBotClient(botToken);
        });
    
        serviceCollection.AddHostedService<VkToTelegramReposterService>();
        
        return serviceCollection;
    }
}