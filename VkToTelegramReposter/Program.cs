using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using VkTelegramReposter.Configuration;
using VkTelegramReposter.Services;

var builder = Host.CreateDefaultBuilder();

builder.ConfigureAppConfiguration(options =>
{
    options.AddYamlFile("config.yml", false);
});

builder.ConfigureServices((context, services) =>
{
    services.AddSingleton(_ => context.Configuration.Get<Config>()!);
    
    services.AddSingleton<TelegramBotClient>(_ =>
    {
        var botToken = context.Configuration.GetRequiredSection("TelegramBotToken").Value!;
        return new TelegramBotClient(botToken);
    });
    
    services.AddHostedService<VkToTelegramReposterService>();
});

await builder.Build().StartAsync();