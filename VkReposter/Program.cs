using Microsoft.Extensions.Hosting;
using VkReposter.Extensions;
using VkTelegramReposter.Extensions;

var builder = Host.CreateApplicationBuilder();

builder.Configuration.Sources.AddYaml("config.yml");
builder.Services.AddVkToTelegramReposter(builder.Configuration);

await builder.Build().RunAsync();