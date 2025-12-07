using System.Text.Json;
using C_TweaksPs1.Models;

namespace C_TweaksPs1.Core
{
    /// <summary>
    /// Loads and validates the tweak configuration from JSON files.
    /// </summary>
    public class ConfigurationLoader
    {
        private const string DefaultConfigPath = "config/tweaks.json";

        /// <summary>
        /// Loads the tweak configuration from the specified JSON file.
        /// </summary>
        /// <param name="configPath">Optional path to the configuration file. Uses default if not provided.</param>
        /// <returns>A TweakConfig object containing all loaded tweaks.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the configuration file cannot be found.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no tweaks are found in the file.</exception>
        public TweakConfig LoadConfiguration(string? configPath = null)
        {
            try
            {
                var path = configPath ?? DefaultConfigPath;
                
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"Configuration file not found: {path}");
                }

                // Read JSON file with proper error handling
                var jsonContent = File.ReadAllText(path);
                
                // Configure JSON deserialization options for flexibility
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };

                // Deserialize JSON into tweak dictionary
                var tweaks = JsonSerializer.Deserialize<Dictionary<string, Tweak>>(jsonContent, options);
                
                if (tweaks == null || tweaks.Count == 0)
                {
                    throw new InvalidOperationException("No tweaks found in configuration file");
                }

                Console.WriteLine($"Loaded {tweaks.Count} tweaks from configuration");
                return new TweakConfig { Tweaks = tweaks };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to load configuration: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Organizes tweaks by their category for easier navigation.
        /// </summary>
        /// <param name="config">The loaded tweak configuration.</param>
        /// <returns>A dictionary mapping category names to lists of tweak keys, ordered by the tweak's Order property.</returns>
        public Dictionary<string, List<string>> GetTweaksByCategory(TweakConfig config)
        {
            var categories = new Dictionary<string, List<string>>();

            foreach (var (key, tweak) in config.Tweaks.OrderBy(t => t.Value.Order))
            {
                if (!categories.ContainsKey(tweak.Category))
                {
                    categories[tweak.Category] = new List<string>();
                }
                categories[tweak.Category].Add(key);
            }

            return categories;
        }
    }
}
