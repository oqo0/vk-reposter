using System.Text.Json.Serialization;

namespace VkReposter.Vk.Entities;

public class WallPost
{
    [JsonPropertyName("id")]
    public ulong Id { get; set; }
    
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("post_type")]
    public string PostType { get; set; } = string.Empty;

    [JsonPropertyName("owner_id")]
    public long OwnerId { get; set; }
    
    [JsonPropertyName("attachments")]
    public List<Attachment> Attachments { get; set; } = [];
}