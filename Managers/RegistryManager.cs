using Microsoft.Win32;
using C_TweaksPs1.Models;
using System.Text;

namespace C_TweaksPs1.Managers
{
    public class RegistryManager
    {
        private readonly List<RegistryBackup> _backups = new();

        public bool ApplyRegistryEntry(RegistryEntry entry)
        {
            try
            {
                var registryPath = ConvertPath(entry.Path);
                var (hive, subKey) = SplitRegistryPath(registryPath);

                using var baseKey = GetRegistryKey(hive);
                if (baseKey == null)
                {
                    Console.WriteLine($"ERROR: Unable to open registry hive for {entry.Path}");
                    return false;
                }

                // Backup current value before modifying
                BackupRegistryValue(baseKey, subKey, entry.Name);

                using var key = baseKey.CreateSubKey(subKey, true);
                if (key == null)
                {
                    Console.WriteLine($"ERROR: Unable to create/open registry key {subKey}");
                    return false;
                }

                // Handle special case where value should be removed
                if (entry.Value == "<RemoveEntry>")
                {
                    if (key.GetValue(entry.Name) != null)
                    {
                        key.DeleteValue(entry.Name, false);
                        Console.WriteLine($"  Removed registry value: {entry.Path}\\{entry.Name}");
                    }
                    return true;
                }

                object valueToSet = ConvertValue(entry.Type, entry.Value);
                RegistryValueKind valueKind = GetRegistryValueKind(entry.Type);

                key.SetValue(entry.Name, valueToSet, valueKind);
                Console.WriteLine($"  Set registry: {entry.Path}\\{entry.Name} = {entry.Value}");
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"ERROR: Access denied to registry path {entry.Path}: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to apply registry entry {entry.Path}\\{entry.Name}: {ex.Message}");
                return false;
            }
        }

        public bool RestoreRegistryEntry(RegistryEntry entry)
        {
            try
            {
                var registryPath = ConvertPath(entry.Path);
                var (hive, subKey) = SplitRegistryPath(registryPath);

                using var baseKey = GetRegistryKey(hive);
                if (baseKey == null)
                {
                    Console.WriteLine($"ERROR: Unable to open registry hive for {entry.Path}");
                    return false;
                }

                // Find backup for this entry
                var backup = _backups.FirstOrDefault(b =>
                    b.Path.Equals(registryPath, StringComparison.OrdinalIgnoreCase) &&
                    b.Name.Equals(entry.Name, StringComparison.OrdinalIgnoreCase));

                if (backup == null)
                {
                    Console.WriteLine($"WARNING: No backup found for {entry.Path}\\{entry.Name}");
                    return false;
                }

                using var key = baseKey.OpenSubKey(subKey, true);
                if (key == null)
                {
                    Console.WriteLine($"WARNING: Registry key not found: {subKey}");
                    return false;
                }

                if (backup.Existed && backup.Value != null)
                {
                    RegistryValueKind valueKind = GetRegistryValueKind(backup.Type);
                    key.SetValue(backup.Name, backup.Value, valueKind);
                    Console.WriteLine($"  Restored registry: {entry.Path}\\{entry.Name}");
                }
                else
                {
                    if (key.GetValue(backup.Name) != null)
                    {
                        key.DeleteValue(backup.Name, false);
                        Console.WriteLine($"  Removed registry value: {entry.Path}\\{entry.Name}");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to restore registry entry {entry.Path}\\{entry.Name}: {ex.Message}");
                return false;
            }
        }

        private void BackupRegistryValue(RegistryKey baseKey, string subKey, string valueName)
        {
            try
            {
                using var key = baseKey.OpenSubKey(subKey, false);
                var backup = new RegistryBackup
                {
                    Path = subKey,
                    Name = valueName
                };

                if (key != null)
                {
                    var value = key.GetValue(valueName);
                    if (value != null)
                    {
                        backup.Existed = true;
                        backup.Value = value;
                        backup.Type = key.GetValueKind(valueName).ToString();
                    }
                    else
                    {
                        backup.Existed = false;
                    }
                }
                else
                {
                    backup.Existed = false;
                }

                _backups.Add(backup);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WARNING: Failed to backup registry value {subKey}\\{valueName}: {ex.Message}");
            }
        }

        private string ConvertPath(string path)
        {
            // Convert PowerShell-style paths to standard Windows Registry paths
            return path
                .Replace("HKLM:\\", "HKEY_LOCAL_MACHINE\\")
                .Replace("HKCU:\\", "HKEY_CURRENT_USER\\")
                .Replace("HKU:\\", "HKEY_USERS\\")
                .Replace("HKCR:\\", "HKEY_CLASSES_ROOT\\")
                .Replace("HKCC:\\", "HKEY_CURRENT_CONFIG\\");
        }

        private (string hive, string subKey) SplitRegistryPath(string path)
        {
            var parts = path.Split('\\', 2);
            if (parts.Length < 2)
            {
                throw new ArgumentException($"Invalid registry path: {path}");
            }
            return (parts[0], parts[1]);
        }

        private RegistryKey? GetRegistryKey(string hive)
        {
            return hive.ToUpperInvariant() switch
            {
                "HKEY_LOCAL_MACHINE" => Registry.LocalMachine,
                "HKEY_CURRENT_USER" => Registry.CurrentUser,
                "HKEY_USERS" => Registry.Users,
                "HKEY_CLASSES_ROOT" => Registry.ClassesRoot,
                "HKEY_CURRENT_CONFIG" => Registry.CurrentConfig,
                _ => null
            };
        }

        private RegistryValueKind GetRegistryValueKind(string type)
        {
            return type.ToLowerInvariant() switch
            {
                "string" => RegistryValueKind.String,
                "dword" => RegistryValueKind.DWord,
                "qword" => RegistryValueKind.QWord,
                "binary" => RegistryValueKind.Binary,
                "multistring" => RegistryValueKind.MultiString,
                "expandstring" => RegistryValueKind.ExpandString,
                _ => RegistryValueKind.String
            };
        }

        private object ConvertValue(string type, string value)
        {
            switch (type.ToLowerInvariant())
            {
                case "dword":
                    return int.TryParse(value, out int dwordVal) ? dwordVal : 0;
                
                case "qword":
                    return long.TryParse(value, out long qwordVal) ? qwordVal : 0L;
                
                case "binary":
                    // Parse hex string to byte array
                    try
                    {
                        if (value.StartsWith("0x"))
                            value = value.Substring(2);
                        var cleanedValue = value.Replace(",", "").Replace(" ", "");
                        return Convert.FromHexString(cleanedValue);
                    }
                    catch (FormatException ex)
                    {
                        throw new ArgumentException($"Invalid binary value format: '{value}'. Expected hex string like '0x01,0x02' or '0102'.", ex);
                    }
                
                case "multistring":
                    return value.Split(new[] { "\\0" }, StringSplitOptions.RemoveEmptyEntries);
                
                case "string":
                case "expandstring":
                default:
                    return value;
            }
        }

        public void ClearBackups()
        {
            _backups.Clear();
        }

        public int BackupCount => _backups.Count;
    }
}
