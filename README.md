# C-TweaksPs1

A production-ready C# console application that replicates the [winutil PowerShell tweak engine](https://github.com/m-serafim/winutil) functionality for Windows system modifications.

## Overview

This application provides a clean, interactive console interface to apply and undo Windows system tweaks including:
- Registry modifications with automatic backup/restore
- Windows service configuration
- Scheduled task management  
- PowerShell script execution

## Quick Start

### Prerequisites
- Windows operating system
- .NET 8.0 Runtime or SDK
- Administrator privileges

### Installation

1. Clone this repository:
   ```bash
   git clone https://github.com/m-serafim/C-TweaksPs1.git
   cd C-TweaksPs1
   ```

2. Build the project:
   ```bash
   dotnet build --configuration Release
   ```

3. Run the application (as Administrator):
   ```bash
   dotnet run
   ```
   
   Or run the executable directly:
   ```bash
   .\bin\Release\net8.0\C#TweaksPs1.exe
   ```

**⚠️ IMPORTANT**: The application MUST be run with Administrator privileges.

## Features

### ✅ Implemented Features

- **Clean Architecture**: Models, Managers, Core Engine, UI layers with separation of concerns
- **Registry Management**: Full CRUD operations with backup/restore for String, DWord, QWord, MultiString, Binary, ExpandString values
- **Service Management**: Configure Windows services with startup type modifications
- **Task Scheduler**: Enable/disable scheduled tasks
- **Script Execution**: Run PowerShell scripts for complex operations
- **Interactive UI**: Category-based browsing with detailed tweak descriptions
- **Admin Checks**: Validates elevation and exits gracefully if not admin
- **Error Handling**: Comprehensive error handling with user-friendly messages
- **Configuration-Driven**: 90 tweaks loaded from JSON, extensible without code changes
- **3-Tier Preset System**: Tweaks organized into Minimum, Recommended, and Gaming categories

### 📋 Configuration

The application loads tweaks from `config/tweaks.json`, sourced from the [m-serafim/winutil](https://github.com/m-serafim/winutil) project.

#### Tweak Categories

All tweaks are organized into three preset categories:

- **🔹 Minimum** (34 tweaks): Light tweaks, visual adjustments, and safe privacy enhancements
- **⭐ Recommended** (38 tweaks): Balanced optimization, power plans, DNS settings, and standard performance improvements
- **🎮 Gaming** (18 tweaks): Aggressive performance tweaks, latency reduction, MSI mode, and hidden power plans

#### JSON Validation

Always validate the JSON configuration after making changes:
```bash
./scripts/validate-json.sh  # Linux/Mac
.\scripts\validate-json.ps1 # Windows
```

See [docs/JSON_SCHEMA.md](docs/JSON_SCHEMA.md) for complete schema documentation.

The JSON format includes:

```json
{
  "TweakKey": {
    "Content": "Tweak Name",
    "Description": "What this tweak does",
    "category": "🔹 Minimum",
    "registry": [...],
    "service": [...],
    "ScheduledTask": [...],
    "InvokeScript": [...],
    "UndoScript": [...]
  }
}
```

#### New Tweaks (24 added)

The following performance and optimization tweaks have been added:
- Game Mode
- High Performance Power Plan
- Ultimate Performance Power Plan
- Disable Fast Startup
- Disable Window Animations
- Disable Transparency Effects
- Adjust for Best Performance
- Optimize DNS Settings (Cloudflare 1.1.1.1)
- TCP/IP Optimization
- Disable Network Throttling
- Hardware Accelerated GPU Scheduling
- Enable MSI Mode for GPU
- Disable Superfetch/Prefetch
- Optimize Virtual Memory
- Disable Search Indexing
- Enable TRIM for SSD
- Disable Telemetry & Data Collection
- Disable Background Apps
- Disable Cortana
- Disable Windows Tips & Suggestions
- Optimize CPU Priority for Games
- Disable Nagle's Algorithm
- High Precision Timer
- Disable CPU Core Parking

## Architecture

```
C-TweaksPs1/
├── Models/              # Data models and entities
├── Managers/            # Business logic for system modifications
│   ├── RegistryManager
│   ├── ServiceManager
│   ├── TaskSchedulerManager
│   └── ScriptRunner
├── Core/                # Core engine and configuration
│   ├── TweakEngine
│   └── ConfigurationLoader
├── UI/                  # Console user interface
│   ├── ConsoleUI
│   └── AdminChecker
└── config/
    └── tweaks.json      # Tweak definitions
```

## Usage Examples

### Applying a Tweak
1. Launch the application as Administrator
2. Select "Browse and Apply Tweaks"
3. Choose a category (e.g., "Essential Tweaks")
4. Select a specific tweak to view details
5. Confirm to apply the tweak

### Undoing a Tweak
1. Select "Browse and Undo Tweaks"
2. Navigate to the category and tweak
3. Confirm to restore original settings

All modifications are backed up automatically and can be restored.

## Development

### Building
```bash
dotnet build
```

### Release Build
```bash
dotnet build --configuration Release
```

### Dependencies
- `TaskScheduler` (2.10.1) - Windows Task Scheduler management
- `System.Management` (8.0.0) - WMI-based service management
- `System.ServiceProcess.ServiceController` (8.0.0) - Windows service control

## Documentation

See [README_APP.md](README_APP.md) for detailed documentation including:
- Full feature list
- Configuration format
- Security considerations
- Extending the application
- Troubleshooting

## Based On

This project replicates functionality from:
- **Source Repository**: [m-serafim/winutil](https://github.com/m-serafim/winutil)
- **Key References**: 
  - `config/tweaks.json` - Tweak definitions
  - `functions/private/Invoke-WinUtilTweaks.ps1` - PowerShell implementation

## Safety and Security

✅ **Built-in Safety Features**:
- Administrator privilege validation
- Automatic backup before all changes
- Full restore/undo capability
- Comprehensive error handling
- Non-destructive failure modes
- JSON schema validation on load

⚠️ **Important Warnings**:
- Always run as Administrator
- Review tweak descriptions before applying
- Create a system restore point before major changes
- Advanced tweaks may affect system stability

## Contributing

This is a replication project based on m-serafim/winutil. Improvements and bug fixes are welcome via pull requests.

## License

This project follows the license terms of the source repository.

## Support

For issues or questions:
1. Check that you're running as Administrator
2. Review error messages in the console
3. Verify `config/tweaks.json` exists and is valid
4. Open an issue with details about your environment

---

**Made with ❤️ for Windows power users**
