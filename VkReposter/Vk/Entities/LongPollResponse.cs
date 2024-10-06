using System.Text.Json.Serialization;

namespace VkReposter.Vk.Entities;

public class LongPollResponse
{
    [JsonPropertyName("ts")]
    public string Ts { get; set; } = string.Empty;

    [JsonPropertyName("failed")]
    public int? Failed { get; set; }
    
    [JsonPropertyName("updates")] public List<Update> Updates { get; set; } = [];
}