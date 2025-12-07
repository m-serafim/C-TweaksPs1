using System.Text.Json.Serialization;

namespace C_TweaksPs1.Models
{
    /// <summary>
    /// Represents a Windows Service modification entry.
    /// </summary>
    public class ServiceEntry
    {
        /// <summary>
        /// The name of the Windows Service to modify.
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("StartupType")]
        public string StartupType { get; set; } = string.Empty;

        [JsonPropertyName("OriginalType")]
        public string OriginalType { get; set; } = string.Empty;
    }
}
