using C_TweaksPs1.Core;
using C_TweaksPs1.UI;

namespace C_TweaksPs1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Check for administrator privileges
                AdminChecker.EnsureAdminOrExit();

                Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║              Windows Tweak Engine (C# Edition)                ║");
                Console.WriteLine("║                    Initializing...                            ║");
                Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
                Console.WriteLine();

                // Load configuration
                var configLoader = new ConfigurationLoader();
                var config = configLoader.LoadConfiguration();

                // Initialize tweak engine
                var engine = new TweakEngine();

                Console.WriteLine("Initialization complete.");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue to the main menu...");
                Console.ReadKey(true);

                // Run the UI
                var ui = new ConsoleUI(config, engine);
                await ui.RunAsync();
            }
            catch (FileNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nERROR: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine("\nPlease ensure the config/tweaks.json file exists.");
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey(true);
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nCRITICAL ERROR: {ex.Message}");
                Console.WriteLine($"\nStack Trace:\n{ex.StackTrace}");
                Console.ResetColor();
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey(true);
                Environment.Exit(1);
            }
        }
    }
}
