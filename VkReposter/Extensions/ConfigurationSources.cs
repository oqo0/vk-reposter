using Alexinea.Extensions.Configuration.Yaml;
using Microsoft.Extensions.Configuration;

namespace VkTelegramReposter.Extensions;

public static class ConfigurationSources
{
    public static void AddYaml(this IList<IConfigurationSource> configurationSources, string yamlFileName)
    {
        var yamlConfigurationSource = new YamlConfigurationSource
        {
            Path = yamlFileName
        };
        
        configurationSources.Add(yamlConfigurationSource);
    }
}