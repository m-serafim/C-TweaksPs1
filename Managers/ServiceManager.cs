using System.ServiceProcess;
using System.Management;
using C_TweaksPs1.Models;

namespace C_TweaksPs1.Managers
{
    public class ServiceManager
    {
        private readonly Dictionary<string, string> _originalStates = new();

        public bool ApplyServiceEntry(ServiceEntry entry, bool keepServiceStartup = true)
        {
            try
            {
                // Check if service exists
                var service = GetServiceByName(entry.Name);
                if (service == null)
                {
                    Console.WriteLine($"  Service '{entry.Name}' not found, skipping");
                    return true; // Not an error - service might not exist on this system
                }

                // Get current startup type
                var currentStartupType = GetServiceStartupType(entry.Name);
                if (currentStartupType == null)
                {
                    Console.WriteLine($"WARNING: Unable to determine current startup type for service '{entry.Name}'");
                    return false;
                }

                // Backup original state
                if (!_originalStates.ContainsKey(entry.Name))
                {
                    _originalStates[entry.Name] = currentStartupType;
                }

                // Check if we should keep user-modified service startup
                if (keepServiceStartup && currentStartupType != entry.OriginalType)
                {
                    Console.WriteLine($"  Service '{entry.Name}' was modified from {entry.OriginalType} to {currentStartupType}, keeping user changes");
                    return true;
                }

                // Apply the new startup type
                SetServiceStartupType(entry.Name, entry.StartupType);
                Console.WriteLine($"  Set service '{entry.Name}' to {entry.StartupType}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to apply service configuration for '{entry.Name}': {ex.Message}");
                return false;
            }
        }

        public bool RestoreServiceEntry(ServiceEntry entry)
        {
            try
            {
                var service = GetServiceByName(entry.Name);
                if (service == null)
                {
                    Console.WriteLine($"  Service '{entry.Name}' not found, skipping restore");
                    return true;
                }

                // Get backed up state
                if (!_originalStates.TryGetValue(entry.Name, out string? originalType))
                {
                    Console.WriteLine($"WARNING: No backup found for service '{entry.Name}'");
                    return false;
                }

                // Restore to original state
                SetServiceStartupType(entry.Name, originalType);
                Console.WriteLine($"  Restored service '{entry.Name}' to {originalType}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to restore service '{entry.Name}': {ex.Message}");
                return false;
            }
        }

        private ServiceController? GetServiceByName(string serviceName)
        {
            try
            {
                var services = ServiceController.GetServices();
                return services.FirstOrDefault(s => 
                    s.ServiceName.Equals(serviceName, StringComparison.OrdinalIgnoreCase) ||
                    s.DisplayName.Equals(serviceName, StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                return null;
            }
        }

        private string? GetServiceStartupType(string serviceName)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher(
                    $"SELECT StartMode FROM Win32_Service WHERE Name = '{serviceName.Replace("'", "''")}'");
                
                foreach (ManagementObject service in searcher.Get())
                {
                    var startMode = service["StartMode"]?.ToString();
                    return ConvertStartModeToStartupType(startMode);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private void SetServiceStartupType(string serviceName, string startupType)
        {
            try
            {
                var startMode = ConvertStartupTypeToStartMode(startupType);
                using var searcher = new ManagementObjectSearcher(
                    $"SELECT * FROM Win32_Service WHERE Name = '{serviceName.Replace("'", "''")}'");
                
                foreach (ManagementObject service in searcher.Get())
                {
                    var inParams = service.GetMethodParameters("ChangeStartMode");
                    inParams["StartMode"] = startMode;
                    service.InvokeMethod("ChangeStartMode", inParams, null);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to set startup type for service '{serviceName}'", ex);
            }
        }

        private string ConvertStartModeToStartupType(string? startMode)
        {
            return startMode?.ToLowerInvariant() switch
            {
                "auto" => "Automatic",
                "boot" => "Automatic",
                "system" => "Automatic",
                "manual" => "Manual",
                "disabled" => "Disabled",
                _ => "Manual"
            };
        }

        private string ConvertStartupTypeToStartMode(string startupType)
        {
            return startupType.ToLowerInvariant() switch
            {
                "automatic" => "Automatic",
                "manual" => "Manual",
                "disabled" => "Disabled",
                _ => "Manual"
            };
        }

        public void ClearBackups()
        {
            _originalStates.Clear();
        }

        public int BackupCount => _originalStates.Count;
    }
}
