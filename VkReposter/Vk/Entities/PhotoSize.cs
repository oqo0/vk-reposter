﻿using System.Text.Json.Serialization;

namespace VkReposter.Vk.Entities;

public class PhotoSize
{
    [JsonPropertyName("url")] public string Url { get; set; } = string.Empty;
}