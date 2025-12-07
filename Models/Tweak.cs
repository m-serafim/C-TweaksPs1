using System.Text.Json.Serialization;

namespace C_TweaksPs1.Models
{
    /// <summary>
    /// Represents a Windows system tweak with associated modifications.
    /// </summary>
    public class Tweak
    {
        /// <summary>
        /// The display name of the tweak.
        /// </summary>
        [JsonPropertyName("Content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// A detailed description of what the tweak does.
        /// </summary>
        [JsonPropertyName("Description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The category this tweak belongs to (e.g., "Essential Tweaks", "z__Advanced Tweaks - CAUTION").
        /// </summary>
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// The UI panel number where this tweak should be displayed.
        /// </summary>
        [JsonPropertyName("panel")]
        public string Panel { get; set; } = string.Empty;

        /// <summary>
        /// The ordering key used to sort tweaks within their category.
        /// </summary>
        [JsonPropertyName("Order")]
        public string Order { get; set; } = string.Empty;

        /// <summary>
        /// Optional documentation link for more information about the tweak.
        /// </summary>
        [JsonPropertyName("link")]
        public string? Link { get; set; }

        /// <summary>
        /// List of Windows Registry modifications to apply when executing this tweak.
        /// </summary>
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
