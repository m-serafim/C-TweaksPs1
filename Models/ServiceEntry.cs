using System.Text.Json.Serialization;

namespace C_TweaksPs1.Models
{
    public class ServiceEntry
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("StartupType")]
        public string StartupType { get; set; } = string.Empty;

        [JsonPropertyName("OriginalType")]
        public string OriginalType { get; set; } = string.Empty;
    }
}
