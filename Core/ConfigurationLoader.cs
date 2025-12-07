using System.Text.Json;
using C_TweaksPs1.Models;

namespace C_TweaksPs1.Core
{
    public class ConfigurationLoader
    {
        private const string DefaultConfigPath = "config/tweaks.json";

        public TweakConfig LoadConfiguration(string? configPath = null)
        {
            try
            {
                var path = configPath ?? DefaultConfigPath;
                
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"Configuration file not found: {path}");
                }

                var jsonContent = File.ReadAllText(path);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };

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
