using System;
using System.IO;
using C_TweaksPs1.Core;

namespace C_TweaksPs1.Tests
{
    public class ConfigValidationTest
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Testing configuration loading...");
                var configLoader = new ConfigurationLoader();
                var config = configLoader.LoadConfiguration();
                
                if (config?.Tweaks != null && config.Tweaks.Count > 0)
                {
                    Console.WriteLine($"✓ Configuration loaded successfully: {config.Tweaks.Count} tweaks");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("✗ Configuration is empty");
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error loading configuration: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}
