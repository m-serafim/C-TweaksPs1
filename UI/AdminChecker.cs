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

        public static void EnsureAdminOrExit()
        {
            if (!IsRunningAsAdmin())
            {
                Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                    ADMINISTRATOR REQUIRED                     ║");
                Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("This application requires administrator privileges to modify");
                Console.WriteLine("system settings, registry entries, and Windows services.");
                Console.WriteLine();
                Console.WriteLine("Please run this application as an Administrator.");
                Console.WriteLine();
                Console.WriteLine("To do this:");
                Console.WriteLine("  1. Right-click on the executable");
                Console.WriteLine("  2. Select 'Run as administrator'");
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(true);
                Environment.Exit(1);
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
