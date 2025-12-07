using System.Text.Json.Serialization;

namespace C_TweaksPs1.Models
{
    public class RegistryEntry
    {
        [JsonPropertyName("Path")]
        public string Path { get; set; } = string.Empty;

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("Type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("Value")]
        public string Value { get; set; } = string.Empty;

        [JsonPropertyName("OriginalValue")]
        public string OriginalValue { get; set; } = string.Empty;
    }
}
