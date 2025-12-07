using System.Text.Json.Serialization;

namespace C_TweaksPs1.Models
{
    /// <summary>
    /// Represents a Windows Scheduled Task modification entry.
    /// </summary>
    public class ScheduledTaskEntry
    {
        /// <summary>
        /// The path and name of the scheduled task to modify.
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The state to set for the task when applying the tweak (Enabled or Disabled).
        /// </summary>
        [JsonPropertyName("State")]
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// The original state to restore when undoing the tweak.
        /// </summary>
        [JsonPropertyName("OriginalState")]
        public string OriginalState { get; set; } = string.Empty;
    }
}
