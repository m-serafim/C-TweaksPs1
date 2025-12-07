using System.Text.Json.Serialization;

namespace C_TweaksPs1.Models
{
    public class Tweak
    {
        [JsonPropertyName("Content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("Description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("panel")]
        public string Panel { get; set; } = string.Empty;

        [JsonPropertyName("Order")]
        public string Order { get; set; } = string.Empty;

        [JsonPropertyName("link")]
        public string? Link { get; set; }

        [JsonPropertyName("registry")]
        public List<RegistryEntry>? Registry { get; set; }

        [JsonPropertyName("service")]
        public List<ServiceEntry>? Service { get; set; }

        [JsonPropertyName("ScheduledTask")]
        public List<ScheduledTaskEntry>? ScheduledTask { get; set; }

        [JsonPropertyName("InvokeScript")]
        public List<string>? InvokeScript { get; set; }

        [JsonPropertyName("UndoScript")]
        public List<string>? UndoScript { get; set; }

        [JsonPropertyName("appx")]
        public List<string>? Appx { get; set; }
    }
}
