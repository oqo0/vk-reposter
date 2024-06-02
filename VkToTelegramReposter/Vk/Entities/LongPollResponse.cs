using System.Text.Json.Serialization;

namespace VkTelegramReposter.Vk.Entities;

public class LongPollResponse
{
    [JsonPropertyName("ts")]
    public string Ts { get; set; } = string.Empty;

    [JsonPropertyName("updates")] public List<Update> Updates { get; set; } = [];
}