﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegraph.Sharp;
using VkReposter.Configuration;
using VkReposter.Core.MessageFormatters;
using VkReposter.Core.Services;

namespace VkReposter.Extensions;

public static class ServiceCollection
{
    public static void AddVkToTelegramReposter(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = AddConfig(services, configuration);
        
        AddTelegramClient(services, config);
        AddTelegraphClient(services, config);
        AddMessageFormatter(services, config);
        
        services.AddHostedService<VkToTelegramReposterService>();
    }

    private static Config AddConfig(IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.Get<Config>();
        if (config is null)
            throw new InvalidDataException("Invalid configuration file");

        services.AddSingleton(config);
        return config;
    }
    
    private static void AddMessageFormatter(IServiceCollection services, Config config)
    {
        switch (config.RepostWith.ToLower())
        {
            case "normal":
                services.AddSingleton<IMessageFormatter, NormalMessageFormatter>();
                break;
            case "telegraph":
                services.AddSingleton<IMessageFormatter, TelegraphMessageFormatter>();
                break;
        }
    }

    private static void AddTelegraphClient(IServiceCollection services, Config config)
    {
        var emptyTelegraphClient = new TelegraphClient();
        var accountTask = emptyTelegraphClient.CreateAccountAsync(
            config.Telegraph.AuthorShortName, config.Telegraph.AuthorName, config.Telegraph.AuthorUrl);

        var account = accountTask.GetAwaiter().GetResult();

        var telegraphToken = account.AccessToken;
        if (telegraphToken is null)
            throw new InvalidDataException("Telegraph token configuration option is invalid");
        
        var telegraphClient = new TelegraphClient(telegraphToken);
        services.AddSingleton(telegraphClient);
    }
    
    private static void AddTelegramClient(IServiceCollection services, Config config)
    {
        services.AddSingleton<TelegramBotClient>(_ =>
        {
            var botToken = config.TelegramBotToken;
            return new TelegramBotClient(botToken);
        });
    }
}