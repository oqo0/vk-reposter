using System.Text.Json.Serialization;

namespace VkTelegramReposter.Vk.Entities;

public class Attachment
{
    [JsonPropertyName("photo")]
    public Photo? Photo { get; set; }
}