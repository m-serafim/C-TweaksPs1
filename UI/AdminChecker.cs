using System.Diagnostics;
using System.Security.Principal;

namespace C_TweaksPs1.UI
{
    public static class AdminChecker
    {
        public static bool IsRunningAsAdmin()
        {
            try
            {
                using var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }

        public static void EnsureAdminOrRestart()
        {
            if (!IsRunningAsAdmin())
            {
                try
                {
                    Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
                    Console.WriteLine("║                    ADMINISTRATOR REQUIRED                     ║");
                    Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
                    Console.WriteLine();
                    Console.WriteLine("This application requires administrator privileges to modify");
                    Console.WriteLine("system settings, registry entries, and Windows services.");
                    Console.WriteLine();
                    Console.WriteLine("Attempting to restart with administrator privileges...");
                    Console.WriteLine();

                    var processInfo = new ProcessStartInfo
                    {
                        UseShellExecute = true,
                        FileName = Environment.ProcessPath ?? System.Reflection.Assembly.GetExecutingAssembly().Location,
                        Verb = "runas",
                        Arguments = string.Join(" ", Environment.GetCommandLineArgs().Skip(1)),
                        WindowStyle = ProcessWindowStyle.Normal
                    };

                    var process = Process.Start(processInfo);
                    if (process != null)
                    {
                        Environment.Exit(0);
                    }
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Administrator privileges were not granted.");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine("The application cannot continue without administrator rights.");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey(true);
                    Environment.Exit(1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Failed to restart with administrator privileges: {ex.Message}");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine("Please manually run this application as Administrator:");
                    Console.WriteLine("  1. Right-click on the executable");
                    Console.WriteLine("  2. Select 'Run as administrator'");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey(true);
                    Environment.Exit(1);
                }
            }
        }

        public static void ShowAdminStatus()
        {
            if (IsRunningAsAdmin())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✓ Running with Administrator privileges");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("✗ NOT running with Administrator privileges");
                Console.ResetColor();
            }
        }
    }
}
