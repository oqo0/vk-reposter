using System.Text.Json.Serialization;

namespace VkReposter.Vk.Entities;

public class Attachment
{
    [JsonPropertyName("photo")]
    public Photo? Photo { get; set; }
}