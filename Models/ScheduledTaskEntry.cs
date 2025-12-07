using System.Text.Json.Serialization;

namespace C_TweaksPs1.Models
{
    public class ScheduledTaskEntry
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("State")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("OriginalState")]
        public string OriginalState { get; set; } = string.Empty;
    }
}
