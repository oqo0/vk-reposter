using VkTelegramReposter.Core.MessageFormatters;
using YamlDotNet.Serialization;

namespace VkTelegramReposter.Configuration;

public class Config
{
    [YamlMember(typeof(string), Alias = "RepostWith")]
    public string RepostWith { get; set; } = string.Empty;

    [YamlMember(typeof(string), Alias = "TelegramBotToken")]
    public string TelegramBotToken { get; set; } = string.Empty;

    [YamlMember(typeof(Telegraph), Alias = "Telegraph")]
    public Telegraph Telegraph { get; set; }
    
    [YamlMember(typeof(int), Alias = "CheckCooldown")]
    public int CheckCooldown { get; set;  }
    
    [YamlMember(typeof(List<Reposter>), Alias = "Reposters")]
    public IList<Reposter> Reposters { get; set; } = new List<Reposter>();
}