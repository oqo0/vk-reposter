using System.Text.Json.Serialization;

namespace VkTelegramReposter.Vk.Entities;

public class WallPost
{
    [JsonPropertyName("id")]
    public ulong Id { get; set; }
    
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("attachments")]
    public List<Attachment> Attachments { get; set; } = [];
}