using System.Text.Json.Serialization;

namespace VkTelegramReposter.Vk.Entities;

public class Photo
{
    [JsonPropertyName("sizes")] public List<PhotoSize> Sizes { get; set; } = [];
}