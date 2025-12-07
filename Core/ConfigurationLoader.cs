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
        /// Finds the configuration file by searching multiple possible locations.
        /// </summary>
        /// <param name="configPath">Optional custom path to the configuration file.</param>
        /// <returns>The full path to the configuration file if found.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the configuration file cannot be found in any location.</exception>
        private string FindConfigFile(string? configPath = null)
        {
            // If a custom path is provided, use it directly
            if (!string.IsNullOrEmpty(configPath))
            {
                if (File.Exists(configPath))
                {
                    return Path.GetFullPath(configPath);
                }
                throw new FileNotFoundException($"Custom configuration file not found: {Path.GetFullPath(configPath)}");
            }

            // Try multiple locations in order of preference
            var locationsToTry = new List<string>();

            // 1. Relative to current working directory
            locationsToTry.Add(Path.Combine(Directory.GetCurrentDirectory(), DefaultConfigPath));

            // 2. Relative to executable location
            var exePath = Environment.ProcessPath ?? System.Reflection.Assembly.GetExecutingAssembly().Location;
            if (!string.IsNullOrEmpty(exePath))
            {
                var exeDir = Path.GetDirectoryName(exePath);
                if (!string.IsNullOrEmpty(exeDir))
                {
                    locationsToTry.Add(Path.Combine(exeDir, DefaultConfigPath));
                }
            }

            // 3. Relative to application base directory
            var appBase = AppContext.BaseDirectory;
            if (!string.IsNullOrEmpty(appBase))
            {
                locationsToTry.Add(Path.Combine(appBase, DefaultConfigPath));
            }

            // Try each location
            foreach (var location in locationsToTry.Distinct())
            {
                if (File.Exists(location))
                {
                    return location;
                }
            }

            // Config file not found - build helpful error message
            var errorMessage = $"Configuration file '{DefaultConfigPath}' not found in any of the following locations:\n";
            foreach (var location in locationsToTry.Distinct())
            {
                errorMessage += $"  - {location}\n";
            }
            errorMessage += "\nPlease ensure the 'config' folder and 'tweaks.json' file are in the same directory as the executable.";

            throw new FileNotFoundException(errorMessage);
        }

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
                var path = FindConfigFile(configPath);
                
                Console.WriteLine($"Loading configuration from: {path}");

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

                Console.WriteLine($"? Loaded {tweaks.Count} tweaks from configuration");
                return new TweakConfig { Tweaks = tweaks };
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nERROR: Failed to load configuration: {ex.Message}");
                Console.ResetColor();
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
