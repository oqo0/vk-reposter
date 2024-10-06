using System.Text.Json.Serialization;

namespace VkReposter.Vk.Entities;

public class Photo
{
    [JsonPropertyName("sizes")] public List<PhotoSize> Sizes { get; set; } = [];
}