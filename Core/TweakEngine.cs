using C_TweaksPs1.Models;
using C_TweaksPs1.Managers;

namespace C_TweaksPs1.Core
{
    public class TweakEngine
    {
        private readonly RegistryManager _registryManager;
        private readonly ServiceManager _serviceManager;
        private readonly TaskSchedulerManager _taskSchedulerManager;
        private readonly ScriptRunner _scriptRunner;

        public TweakEngine()
        {
            _registryManager = new RegistryManager();
            _serviceManager = new ServiceManager();
            _taskSchedulerManager = new TaskSchedulerManager();
            _scriptRunner = new ScriptRunner();
        }

        public async Task<bool> ApplyTweakAsync(string tweakKey, Tweak tweak)
        {
            Console.WriteLine($"\nApplying tweak: {tweak.Content}");
            Console.WriteLine($"Description: {tweak.Description}");
            
            bool success = true;

            try
            {
                // Apply registry changes
                if (tweak.Registry != null && tweak.Registry.Count > 0)
                {
                    Console.WriteLine($"  Processing {tweak.Registry.Count} registry entries...");
                    foreach (var entry in tweak.Registry)
                    {
                        if (!_registryManager.ApplyRegistryEntry(entry))
                        {
                            success = false;
                        }
                    }
                }

                // Apply service changes
                if (tweak.Service != null && tweak.Service.Count > 0)
                {
                    Console.WriteLine($"  Processing {tweak.Service.Count} service entries...");
                    foreach (var entry in tweak.Service)
                    {
                        if (!_serviceManager.ApplyServiceEntry(entry))
                        {
                            success = false;
                        }
                    }
                }

                // Apply scheduled task changes
                if (tweak.ScheduledTask != null && tweak.ScheduledTask.Count > 0)
                {
                    Console.WriteLine($"  Processing {tweak.ScheduledTask.Count} scheduled task entries...");
                    foreach (var entry in tweak.ScheduledTask)
                    {
                        if (!_taskSchedulerManager.ApplyScheduledTaskEntry(entry))
                        {
                            success = false;
                        }
                    }
                }

                // Execute invoke scripts
                if (tweak.InvokeScript != null && tweak.InvokeScript.Count > 0)
                {
                    Console.WriteLine($"  Executing {tweak.InvokeScript.Count} invoke scripts...");
                    for (int i = 0; i < tweak.InvokeScript.Count; i++)
                    {
                        var script = tweak.InvokeScript[i];
                        var description = $"invoke script {i + 1}/{tweak.InvokeScript.Count} for '{tweak.Content}'";
                        if (!await _scriptRunner.ExecuteScriptAsync(script, description))
                        {
                            success = false;
                        }
                    }
                }

                if (success)
                {
                    Console.WriteLine($"✓ Successfully applied tweak: {tweak.Content}");
                }
                else
                {
                    Console.WriteLine($"⚠ Tweak applied with warnings: {tweak.Content}");
                }

                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to apply tweak '{tweak.Content}': {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UndoTweakAsync(string tweakKey, Tweak tweak)
        {
            Console.WriteLine($"\nUndoing tweak: {tweak.Content}");
            
            bool success = true;

            try
            {
                // Execute undo scripts first
                if (tweak.UndoScript != null && tweak.UndoScript.Count > 0)
                {
                    Console.WriteLine($"  Executing {tweak.UndoScript.Count} undo scripts...");
                    for (int i = 0; i < tweak.UndoScript.Count; i++)
                    {
                        var script = tweak.UndoScript[i];
                        var description = $"undo script {i + 1}/{tweak.UndoScript.Count} for '{tweak.Content}'";
                        if (!await _scriptRunner.ExecuteScriptAsync(script, description))
                        {
                            success = false;
                        }
                    }
                }

                // Restore scheduled task changes
                if (tweak.ScheduledTask != null && tweak.ScheduledTask.Count > 0)
                {
                    Console.WriteLine($"  Restoring {tweak.ScheduledTask.Count} scheduled task entries...");
                    foreach (var entry in tweak.ScheduledTask)
                    {
                        if (!_taskSchedulerManager.RestoreScheduledTaskEntry(entry))
                        {
                            success = false;
                        }
                    }
                }

                // Restore service changes
                if (tweak.Service != null && tweak.Service.Count > 0)
                {
                    Console.WriteLine($"  Restoring {tweak.Service.Count} service entries...");
                    foreach (var entry in tweak.Service)
                    {
                        if (!_serviceManager.RestoreServiceEntry(entry))
                        {
                            success = false;
                        }
                    }
                }

                // Restore registry changes
                if (tweak.Registry != null && tweak.Registry.Count > 0)
                {
                    Console.WriteLine($"  Restoring {tweak.Registry.Count} registry entries...");
                    foreach (var entry in tweak.Registry)
                    {
                        if (!_registryManager.RestoreRegistryEntry(entry))
                        {
                            success = false;
                        }
                    }
                }

                if (success)
                {
                    Console.WriteLine($"✓ Successfully undid tweak: {tweak.Content}");
                }
                else
                {
                    Console.WriteLine($"⚠ Tweak undo completed with warnings: {tweak.Content}");
                }

                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to undo tweak '{tweak.Content}': {ex.Message}");
                return false;
            }
        }

        public void ClearAllBackups()
        {
            _registryManager.ClearBackups();
            _serviceManager.ClearBackups();
            _taskSchedulerManager.ClearBackups();
        }

        public (int registry, int services, int tasks) GetBackupCounts()
        {
            return (
                _registryManager.BackupCount,
                _serviceManager.BackupCount,
                _taskSchedulerManager.BackupCount
            );
        }
    }
}
