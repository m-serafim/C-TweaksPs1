using System.Diagnostics;

namespace C_TweaksPs1.Managers
{
    public class ScriptRunner
    {
        public bool ExecuteScript(string script, string description = "script")
        {
            try
            {
                // Clean up the script - remove leading/trailing whitespace and empty lines
                script = script.Trim();
                if (string.IsNullOrWhiteSpace(script))
                {
                    return true; // Empty script is not an error
                }

                Console.WriteLine($"  Executing {description}...");

                var startInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{EscapeForPowerShell(script)}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    Verb = "runas"
                };

                using var process = new Process { StartInfo = startInfo };
                process.Start();

                // Read output
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();

                const int TimeoutMilliseconds = 30000; // 30 second timeout
                process.WaitForExit(TimeoutMilliseconds);

                if (!string.IsNullOrWhiteSpace(output))
                {
                    Console.WriteLine($"    {output.Trim()}");
                }

                if (!string.IsNullOrWhiteSpace(error))
                {
                    Console.WriteLine($"    WARNING: {error.Trim()}");
                }

                if (process.ExitCode != 0)
                {
                    Console.WriteLine($"    Script exited with code: {process.ExitCode}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to execute {description}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ExecuteScriptAsync(string script, string description = "script")
        {
            try
            {
                script = script.Trim();
                if (string.IsNullOrWhiteSpace(script))
                {
                    return true;
                }

                Console.WriteLine($"  Executing {description}...");

                var startInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{EscapeForPowerShell(script)}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    Verb = "runas"
                };

                using var process = new Process { StartInfo = startInfo };
                process.Start();

                var output = await process.StandardOutput.ReadToEndAsync();
                var error = await process.StandardError.ReadToEndAsync();

                await process.WaitForExitAsync();

                if (!string.IsNullOrWhiteSpace(output))
                {
                    Console.WriteLine($"    {output.Trim()}");
                }

                if (!string.IsNullOrWhiteSpace(error))
                {
                    Console.WriteLine($"    WARNING: {error.Trim()}");
                }

                if (process.ExitCode != 0)
                {
                    Console.WriteLine($"    Script exited with code: {process.ExitCode}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to execute {description}: {ex.Message}");
                return false;
            }
        }

        private string EscapeForPowerShell(string script)
        {
            // Escape double quotes for PowerShell command line
            return script.Replace("\"", "`\"");
        }
    }
}
