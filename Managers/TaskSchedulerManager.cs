using Microsoft.Win32.TaskScheduler;
using C_TweaksPs1.Models;

namespace C_TweaksPs1.Managers
{
    public class TaskSchedulerManager
    {
        private readonly Dictionary<string, bool> _originalStates = new();

        public bool ApplyScheduledTaskEntry(ScheduledTaskEntry entry)
        {
            try
            {
                using var ts = new TaskService();
                var task = ts.GetTask(entry.Name);

                if (task == null)
                {
                    Console.WriteLine($"  Scheduled task '{entry.Name}' not found, skipping");
                    return true; // Not an error - task might not exist on this system
                }

                // Backup original state
                if (!_originalStates.ContainsKey(entry.Name))
                {
                    _originalStates[entry.Name] = task.Enabled;
                }

                // Apply new state
                bool targetState = entry.State.Equals("Enabled", StringComparison.OrdinalIgnoreCase);
                task.Enabled = targetState;
                Console.WriteLine($"  Set scheduled task '{entry.Name}' to {entry.State}");
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"ERROR: Access denied to scheduled task '{entry.Name}': {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to modify scheduled task '{entry.Name}': {ex.Message}");
                return false;
            }
        }

        public bool RestoreScheduledTaskEntry(ScheduledTaskEntry entry)
        {
            try
            {
                using var ts = new TaskService();
                var task = ts.GetTask(entry.Name);

                if (task == null)
                {
                    Console.WriteLine($"  Scheduled task '{entry.Name}' not found, skipping restore");
                    return true;
                }

                // Get backed up state
                if (!_originalStates.TryGetValue(entry.Name, out bool originalState))
                {
                    Console.WriteLine($"WARNING: No backup found for scheduled task '{entry.Name}'");
                    return false;
                }

                // Restore to original state
                task.Enabled = originalState;
                string stateText = originalState ? "Enabled" : "Disabled";
                Console.WriteLine($"  Restored scheduled task '{entry.Name}' to {stateText}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Failed to restore scheduled task '{entry.Name}': {ex.Message}");
                return false;
            }
        }

        public void ClearBackups()
        {
            _originalStates.Clear();
        }

        public int BackupCount => _originalStates.Count;
    }
}
