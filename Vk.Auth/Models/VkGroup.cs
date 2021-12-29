using System.Text.Json.Serialization;

namespace Vk.Auth.Models;

public record VkGroup
{
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonPropertyName("photo_50")]
    public string Photo { get; set; }
}