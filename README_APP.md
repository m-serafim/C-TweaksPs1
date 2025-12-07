# Windows Tweak Engine (C# Edition)

A production-ready C# console application that replicates the winutil PowerShell tweak engine functionality.

## Features

### Core Functionality
- **Registry Management**: Modify Windows registry entries with automatic backup and restore capabilities
  - Supports: String, DWord, QWord, MultiString, Binary, and ExpandString values
  - Automatic backup before changes
  - Full restore functionality
  
- **Windows Service Management**: Configure Windows services startup types
  - Change service startup modes (Automatic, Manual, Disabled)
  - Preserve user-modified service configurations
  - Backup and restore service states

- **Scheduled Task Management**: Enable or disable Windows scheduled tasks
  - Modify task states
  - Backup original states for restoration

- **PowerShell Script Execution**: Execute PowerShell scripts for advanced tweaks
  - Safe execution with error handling
  - Support for both apply (InvokeScript) and undo (UndoScript) operations

### Architecture

The application follows a clean architecture pattern:

```
C-TweaksPs1/
├── Models/           # Data models for configuration and state
│   ├── Tweak.cs
│   ├── RegistryEntry.cs
│   ├── ServiceEntry.cs
│   ├── ScheduledTaskEntry.cs
│   ├── TweakConfig.cs
│   └── RegistryBackup.cs
├── Managers/         # Business logic for system modifications
│   ├── RegistryManager.cs
│   ├── ServiceManager.cs
│   ├── TaskSchedulerManager.cs
│   └── ScriptRunner.cs
├── Core/             # Core engine and configuration loading
│   ├── TweakEngine.cs
│   └── ConfigurationLoader.cs
├── UI/               # User interface layer
│   ├── ConsoleUI.cs
│   └── AdminChecker.cs
├── config/           # Configuration files
│   └── tweaks.json
└── Program.cs        # Application entry point
```

## Requirements

- Windows operating system
- .NET 8.0 Runtime
- Administrator privileges (required for system modifications)

## Installation

1. Build the project:
   ```bash
   dotnet build
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

   Or run the compiled executable from `bin/Debug/net8.0/C#TweaksPs1.exe`

**Important**: The application must be run with Administrator privileges.

## Configuration

The application loads tweaks from `config/tweaks.json`, which contains:
- 60+ Windows tweaks organized by category
- Registry modifications
- Service configurations
- Scheduled task settings
- PowerShell scripts for complex operations

The JSON configuration format is compatible with the [m-serafim/winutil](https://github.com/m-serafim/winutil) project.

### Configuration Structure

Each tweak in `tweaks.json` can contain:

```json
{
  "TweakKey": {
    "Content": "Tweak Name",
    "Description": "What this tweak does",
    "category": "Category Name",
    "Order": "a001_",
    "registry": [
      {
        "Path": "HKLM:\\Path\\To\\Key",
        "Name": "ValueName",
        "Type": "DWord",
        "Value": "0",
        "OriginalValue": "1"
      }
    ],
    "service": [
      {
        "Name": "ServiceName",
        "StartupType": "Disabled",
        "OriginalType": "Automatic"
      }
    ],
    "ScheduledTask": [
      {
        "Name": "Microsoft\\Windows\\TaskName",
        "State": "Disabled",
        "OriginalState": "Enabled"
      }
    ],
    "InvokeScript": [
      "powershell commands to execute"
    ],
    "UndoScript": [
      "powershell commands to undo changes"
    ]
  }
}
```

## Usage

1. **Launch the application** (as Administrator)
2. **Browse categories** to find tweaks
3. **Select a tweak** to view details
4. **Apply or undo** the tweak

The application provides:
- Interactive menu system
- Category-based tweak organization
- Detailed tweak descriptions
- Statistics and backup tracking
- Graceful error handling

### Menu Structure

```
Main Menu
├── Browse and Apply Tweaks
│   └── Select Category → Select Tweak → Apply
├── Browse and Undo Tweaks
│   └── Select Category → Select Tweak → Undo
├── Show Statistics
│   └── View loaded tweaks and backup counts
├── About
│   └── Application information
└── Exit
```

## Security and Safety

- **Administrator Check**: The application verifies admin privileges on startup
- **Backup System**: All modifications are backed up before changes
- **Restore Capability**: Every tweak can be undone to restore original state
- **Error Handling**: Comprehensive error handling with user-friendly messages
- **Non-destructive**: Failed operations are logged without corrupting the system

## Technical Details

### Dependencies
- `TaskScheduler` (v2.10.1) - For Windows Task Scheduler management
- `System.Management` (v8.0.0) - For WMI-based service management
- `System.ServiceProcess.ServiceController` (v8.0.0) - For Windows service control

### Supported Registry Types
- String (REG_SZ)
- DWord (REG_DWORD)
- QWord (REG_QWORD)
- Binary (REG_BINARY)
- MultiString (REG_MULTI_SZ)
- ExpandString (REG_EXPAND_SZ)

### Service Startup Types
- Automatic
- Manual
- Disabled

### Async Operations
The application uses `async/await` for:
- PowerShell script execution
- Tweak application and restoration
- Long-running operations

## Extending the Application

To add new tweaks:
1. Edit `config/tweaks.json`
2. Add your tweak following the existing format
3. No code changes required - tweaks are loaded dynamically

## Based On

This application replicates functionality from:
- **Source**: [m-serafim/winutil](https://github.com/m-serafim/winutil)
- **Reference Files**: 
  - `config/tweaks.json` - Tweak definitions
  - `functions/private/Invoke-WinUtilTweaks.ps1` - Original PowerShell implementation

## License

This project follows the license terms of the source repository.

## Warnings

⚠️ **IMPORTANT**: 
- This application modifies system settings and requires Administrator privileges
- Always review tweak descriptions before applying
- Some tweaks may affect system stability or functionality
- Create a system restore point before making major changes
- Advanced tweaks are marked with "CAUTION" - use with care

## Support

For issues or questions:
1. Review the tweak description carefully
2. Check if the application is running with admin rights
3. Review error messages in the console
4. Verify that `config/tweaks.json` is present and valid
