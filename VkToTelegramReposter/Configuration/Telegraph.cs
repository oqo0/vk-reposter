using YamlDotNet.Serialization;

namespace VkTelegramReposter.Configuration;

public class Telegraph
{
    [YamlMember(typeof(ulong), Alias = "AuthorShortName")]
    public string AuthorShortName { get; set; } = string.Empty;
    
    [YamlMember(typeof(ulong), Alias = "AuthorName")]
    public string AuthorName { get; set; } = string.Empty;
    
    [YamlMember(typeof(ulong), Alias = "AuthorUrl")]
    public string AuthorUrl { get; set; } = string.Empty;
    
    [YamlMember(typeof(ulong), Alias = "PageName")]
    public string PageName { get; set; } = string.Empty;

    [YamlMember(typeof(string), Alias = "VkPostLinkMessage")]
    public string VkPostLinkMessage { get; set; } = string.Empty;
}