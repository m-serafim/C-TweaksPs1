using C_TweaksPs1.Models;
using C_TweaksPs1.Core;

namespace C_TweaksPs1.UI
{
    public class ConsoleUI
    {
        private readonly TweakConfig _config;
        private readonly TweakEngine _engine;
        private readonly Dictionary<string, List<string>> _categories;
        private readonly Dictionary<string, string> _wrappedDescriptions;

        public ConsoleUI(TweakConfig config, TweakEngine engine)
        {
            _config = config;
            _engine = engine;
            _categories = new ConfigurationLoader().GetTweaksByCategory(config);
            _wrappedDescriptions = new Dictionary<string, string>();
        }

        public async Task RunAsync()
        {
            while (true)
            {
                ShowMainMenu();
                var choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        await BrowseAndApplyTweaksAsync();
                        break;
                    case "2":
                        await BrowseAndUndoTweaksAsync();
                        break;
                    case "3":
                        ShowStatistics();
                        break;
                    case "4":
                        ShowAbout();
                        break;
                    case "5":
                    case "q":
                    case "quit":
                    case "exit":
                        Console.WriteLine("\nThank you for using C# Tweak Engine!");
                        return;
                    default:
                        Console.WriteLine("\nInvalid choice. Please try again.");
                        WaitForKey();
                        break;
                }
            }
        }

        private void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              Windows Tweak Engine (C# Edition)                ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            AdminChecker.ShowAdminStatus();
            Console.WriteLine();
            Console.WriteLine("Main Menu:");
            Console.WriteLine("  1. Browse and Apply Tweaks");
            Console.WriteLine("  2. Browse and Undo Tweaks");
            Console.WriteLine("  3. Show Statistics");
            Console.WriteLine("  4. About");
            Console.WriteLine("  5. Exit");
            Console.WriteLine();
            Console.Write("Enter your choice: ");
        }

        private async Task BrowseAndApplyTweaksAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                    Browse and Apply Tweaks                    ║");
                Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
                Console.WriteLine();

                var categoryList = _categories.Keys.ToList();
                for (int i = 0; i < categoryList.Count; i++)
                {
                    var category = categoryList[i];
                    var count = _categories[category].Count;
                    Console.WriteLine($"  {i + 1}. {category} ({count} tweaks)");
                }
                Console.WriteLine($"  {categoryList.Count + 1}. Back to Main Menu");
                Console.WriteLine();
                Console.Write("Select a category: ");

                var choice = Console.ReadLine()?.Trim();
                if (int.TryParse(choice, out int categoryIndex))
                {
                    if (categoryIndex == categoryList.Count + 1)
                    {
                        return;
                    }
                    else if (categoryIndex > 0 && categoryIndex <= categoryList.Count)
                    {
                        await ShowCategoryTweaksAsync(categoryList[categoryIndex - 1], true);
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice.");
                        WaitForKey();
                    }
                }
            }
        }

        private async Task BrowseAndUndoTweaksAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                    Browse and Undo Tweaks                     ║");
                Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
                Console.WriteLine();

                var categoryList = _categories.Keys.ToList();
                for (int i = 0; i < categoryList.Count; i++)
                {
                    var category = categoryList[i];
                    var count = _categories[category].Count;
                    Console.WriteLine($"  {i + 1}. {category} ({count} tweaks)");
                }
                Console.WriteLine($"  {categoryList.Count + 1}. Back to Main Menu");
                Console.WriteLine();
                Console.Write("Select a category: ");

                var choice = Console.ReadLine()?.Trim();
                if (int.TryParse(choice, out int categoryIndex))
                {
                    if (categoryIndex == categoryList.Count + 1)
                    {
                        return;
                    }
                    else if (categoryIndex > 0 && categoryIndex <= categoryList.Count)
                    {
                        await ShowCategoryTweaksAsync(categoryList[categoryIndex - 1], false);
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice.");
                        WaitForKey();
                    }
                }
            }
        }

        private async Task ShowCategoryTweaksAsync(string category, bool isApply)
        {
            while (true)
            {
                Console.Clear();
                var action = isApply ? "Apply" : "Undo";
                Console.WriteLine($"╔═══════════════════════════════════════════════════════════════╗");
                Console.WriteLine($"║  Category: {category,-52} ║");
                Console.WriteLine($"║  Action: {action,-54} ║");
                Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
                Console.WriteLine();

                var tweakKeys = _categories[category];
                for (int i = 0; i < tweakKeys.Count; i++)
                {
                    var key = tweakKeys[i];
                    var tweak = _config.Tweaks[key];
                    Console.WriteLine($"  {i + 1}. {tweak.Content}");
                }
                Console.WriteLine($"  {tweakKeys.Count + 1}. Back");
                Console.WriteLine();
                Console.Write("Select a tweak to view details (or enter number): ");

                var choice = Console.ReadLine()?.Trim();
                if (int.TryParse(choice, out int tweakIndex))
                {
                    if (tweakIndex == tweakKeys.Count + 1)
                    {
                        return;
                    }
                    else if (tweakIndex > 0 && tweakIndex <= tweakKeys.Count)
                    {
                        var key = tweakKeys[tweakIndex - 1];
                        var tweak = _config.Tweaks[key];
                        await ShowTweakDetailsAsync(key, tweak, isApply);
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice.");
                        WaitForKey();
                    }
                }
            }
        }

        private async Task ShowTweakDetailsAsync(string key, Tweak tweak, bool isApply)
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                        Tweak Details                          ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine($"Name: {tweak.Content}");
            Console.WriteLine($"Category: {tweak.Category}");
            Console.WriteLine();
            Console.WriteLine("Description:");
            
            // Cache wrapped descriptions for performance
            if (!_wrappedDescriptions.TryGetValue(key, out string? wrappedDesc))
            {
                wrappedDesc = WrapText(tweak.Description, 60);
                _wrappedDescriptions[key] = wrappedDesc;
            }
            Console.WriteLine($"  {wrappedDesc}");
            Console.WriteLine();

            // Show what will be modified
            if (tweak.Registry != null && tweak.Registry.Count > 0)
            {
                Console.WriteLine($"Registry Entries: {tweak.Registry.Count}");
            }
            if (tweak.Service != null && tweak.Service.Count > 0)
            {
                Console.WriteLine($"Services: {tweak.Service.Count}");
            }
            if (tweak.ScheduledTask != null && tweak.ScheduledTask.Count > 0)
            {
                Console.WriteLine($"Scheduled Tasks: {tweak.ScheduledTask.Count}");
            }
            if (isApply && tweak.InvokeScript != null && tweak.InvokeScript.Count > 0)
            {
                Console.WriteLine($"Invoke Scripts: {tweak.InvokeScript.Count}");
            }
            if (!isApply && tweak.UndoScript != null && tweak.UndoScript.Count > 0)
            {
                Console.WriteLine($"Undo Scripts: {tweak.UndoScript.Count}");
            }

            Console.WriteLine();
            var action = isApply ? "Apply" : "Undo";
            Console.Write($"Do you want to {action} this tweak? (y/n): ");
            var confirm = Console.ReadLine()?.Trim().ToLowerInvariant();

            if (confirm == "y" || confirm == "yes")
            {
                Console.WriteLine();
                if (isApply)
                {
                    await _engine.ApplyTweakAsync(key, tweak);
                }
                else
                {
                    await _engine.UndoTweakAsync(key, tweak);
                }
                Console.WriteLine();
                WaitForKey();
            }
        }

        private void ShowStatistics()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                          Statistics                           ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine($"Total Tweaks Loaded: {_config.Tweaks.Count}");
            Console.WriteLine($"Total Categories: {_categories.Count}");
            Console.WriteLine();
            Console.WriteLine("Tweaks per Category:");
            foreach (var category in _categories.OrderByDescending(c => c.Value.Count))
            {
                Console.WriteLine($"  {category.Key}: {category.Value.Count} tweaks");
            }
            
            var (registry, services, tasks) = _engine.GetBackupCounts();
            Console.WriteLine();
            Console.WriteLine("Current Session Backups:");
            Console.WriteLine($"  Registry Entries: {registry}");
            Console.WriteLine($"  Services: {services}");
            Console.WriteLine($"  Scheduled Tasks: {tasks}");
            Console.WriteLine();
            WaitForKey();
        }

        private void ShowAbout()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                             About                             ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("Windows Tweak Engine (C# Edition)");
            Console.WriteLine("Version: 1.0.0");
            Console.WriteLine();
            Console.WriteLine("A production-ready C# console application that replicates");
            Console.WriteLine("the winutil PowerShell tweak engine functionality.");
            Console.WriteLine();
            Console.WriteLine("Features:");
            Console.WriteLine("  • Registry modifications with backup/restore");
            Console.WriteLine("  • Windows service management");
            Console.WriteLine("  • Scheduled task configuration");
            Console.WriteLine("  • PowerShell script execution");
            Console.WriteLine("  • Clean architecture with separation of concerns");
            Console.WriteLine();
            Console.WriteLine("This application requires Administrator privileges.");
            Console.WriteLine();
            Console.WriteLine("Based on: m-serafim/winutil");
            Console.WriteLine();
            WaitForKey();
        }

        private string WrapText(string text, int maxWidth)
        {
            if (text.Length <= maxWidth)
                return text;

            var words = text.Split(' ');
            var lines = new List<string>();
            var currentLine = "";

            foreach (var word in words)
            {
                if (currentLine.Length + word.Length + 1 <= maxWidth)
                {
                    currentLine += (currentLine.Length > 0 ? " " : "") + word;
                }
                else
                {
                    if (currentLine.Length > 0)
                        lines.Add(currentLine);
                    currentLine = word;
                }
            }

            if (currentLine.Length > 0)
                lines.Add(currentLine);

            return string.Join("\n  ", lines);
        }

        private void WaitForKey()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }
}
