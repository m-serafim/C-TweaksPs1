using System.Reflection;
using System.Text.Json;
using C_TweaksPs1.Models;

namespace C_TweaksPs1.Core
{
    /// <summary>
    /// Loads and validates the tweak configuration from embedded resources or JSON files.
    /// </summary>
    public class ConfigurationLoader
    {
        private const string DefaultConfigPath = "config/tweaks.json";
        private const string EmbeddedResourceName = "tweaks.json";

        /// <summary>
        /// Loads the configuration from embedded resources.
        /// </summary>
        /// <returns>JSON content as string, or null if not found.</returns>
        private string? LoadFromEmbeddedResource()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = EmbeddedResourceName;

                // Try to find the resource
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    return null;
                }

                using var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Finds the configuration file by searching multiple possible locations.
        /// </summary>
        /// <param name="configPath">Optional custom path to the configuration file.</param>
        /// <returns>The full path to the configuration file if found, null otherwise.</returns>
        private string? FindConfigFile(string? configPath = null)
        {
            // If a custom path is provided, use it directly
            if (!string.IsNullOrEmpty(configPath))
            {
                if (File.Exists(configPath))
                {
                    return Path.GetFullPath(configPath);
                }
                return null;
            }

            // Try multiple locations in order of preference
            var locationsToTry = new List<string>();

            // 1. Relative to current working directory
            locationsToTry.Add(Path.Combine(Directory.GetCurrentDirectory(), DefaultConfigPath));

            // 2. Relative to executable location
            var exePath = Environment.ProcessPath ?? Assembly.GetExecutingAssembly().Location;
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

            return null;
        }

        /// <summary>
        /// Loads the tweak configuration from embedded resources or external files.
        /// </summary>
        /// <param name="configPath">Optional path to the configuration file. Uses default if not provided.</param>
        /// <returns>A TweakConfig object containing all loaded tweaks.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the configuration cannot be found.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no tweaks are found in the file.</exception>
        public TweakConfig LoadConfiguration(string? configPath = null)
        {
            try
            {
                string? jsonContent = null;
                string source = "";

                // Try embedded resource first (for single-file deployment)
                jsonContent = LoadFromEmbeddedResource();
                if (jsonContent != null)
                {
                    source = "embedded resource";
                    Console.WriteLine($"? Loading configuration from embedded resource");
                }
                else
                {
                    // Fallback to external file
                    var path = FindConfigFile(configPath);
                    if (path != null)
                    {
                        jsonContent = File.ReadAllText(path);
                        source = path;
                        Console.WriteLine($"? Loading configuration from: {path}");
                    }
                }

                // If still no config found, throw error
                if (jsonContent == null)
                {
                    var errorMessage = "Configuration file not found.\n";
                    errorMessage += "This is a single-file executable with embedded configuration.\n";
                    errorMessage += "If you see this error, the build may have failed to embed resources properly.\n";
                    errorMessage += "\nAlternatively, you can place a 'config/tweaks.json' file next to the executable.";
                    throw new FileNotFoundException(errorMessage);
                }

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
                    throw new InvalidOperationException("No tweaks found in configuration");
                }

                Console.WriteLine($"? Loaded {tweaks.Count} tweaks from {source}");
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
