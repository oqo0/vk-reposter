using YamlDotNet.Serialization;

namespace VkReposter.Configuration;

public class Config
{
    [YamlMember(typeof(string), Alias = "RepostWith")]
    public string RepostWith { get; init; } = string.Empty;

    [YamlMember(typeof(string), Alias = "TelegramBotToken")]
    public string TelegramBotToken { get; init; } = string.Empty;

    [YamlMember(typeof(Telegraph), Alias = "Telegraph")]
    public Telegraph Telegraph { get; init; } = null!;
    
    [YamlMember(typeof(int), Alias = "CheckCooldown")]
    public int CheckCooldown { get; init;  }
    
    [YamlMember(typeof(List<Reposter>), Alias = "Reposters")]
    public IList<Reposter> Reposters { get; init; } = new List<Reposter>();
}