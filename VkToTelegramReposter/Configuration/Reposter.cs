using YamlDotNet.Serialization;

namespace VkTelegramReposter.Configuration;

public class Reposter
{
    [YamlMember(typeof(ulong), Alias = "VkGroupId")]
    public ulong VkGroupId { get; set; }
    
    [YamlMember(typeof(string), Alias = "VkGroupToken")]
    public string VkGroupToken { get; set; } = string.Empty;
    
    [YamlMember(typeof(string), Alias = "TelegramChannelId")]
    public string TelegramChannelId { get; set; } = string.Empty;
}