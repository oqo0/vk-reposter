using System.Text.Json.Serialization;

namespace VkReposter.Vk.Entities;

public class Update
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("object")]
    public WallPost? Object { get; set; }
}