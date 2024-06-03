using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using VkTelegramReposter.Extensions;

var builder = Host.CreateDefaultBuilder();

builder.ConfigureAppConfiguration(options =>
{
    options.AddYamlFile("config.yml", false);
});

builder.ConfigureServices((context, services) =>
{
    services.AddVkToTelegramReposter(context.Configuration);
});

await builder.Build().StartAsync();