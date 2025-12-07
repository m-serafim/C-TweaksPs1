# Quick Start Guide for End Users

## What You Received

You should have received a folder or ZIP file containing:
- `C#TweaksPs1.exe` - The main application
- `config` folder with `tweaks.json` - Configuration file
- Several `.dll` files - Required dependencies

## Important: File Structure

**DO NOT SEPARATE THESE FILES!** The application requires the `config` folder to be in the same directory as the executable.

? **Correct structure:**
```
MyFolder/
??? C#TweaksPs1.exe
??? config/
?   ??? tweaks.json
??? (other .dll files)
```

? **Incorrect:**
- Moving `C#TweaksPs1.exe` to Desktop without the `config` folder
- Renaming or moving the `config` folder
- Running the `.exe` from a different location

## How to Run

### First Time Setup

1. **Extract all files** (if you received a ZIP):
   - Right-click the ZIP file
   - Select "Extract All..."
   - Choose a permanent location (e.g., `C:\Programs\TweaksPs1\`)

2. **Keep files together**:
   - Do NOT move just the `.exe` file
   - The entire folder must stay together

### Running the Application

1. **Locate** `C#TweaksPs1.exe` in the extracted folder

2. **Right-click** on `C#TweaksPs1.exe`

3. **Select** "Run as administrator"

4. **Click** "Yes" when prompted by User Account Control (UAC)

5. The application will start and load the configuration

## System Requirements

- **Operating System**: Windows 10 or Windows 11
- **Privileges**: Administrator rights required
- **.NET Runtime**: 
  - Self-contained version: No additional software needed
  - Framework-dependent version: .NET 8 Runtime required ([Download here](https://dotnet.microsoft.com/download/dotnet/8.0))

## Troubleshooting

### Error: "Configuration file not found: config/tweaks.json"

**Cause**: The `config` folder is missing or not in the correct location.

**Solution**:
1. Ensure the `config` folder is in the same directory as `C#TweaksPs1.exe`
2. Verify `tweaks.json` exists inside the `config` folder
3. Re-extract all files from the original ZIP if necessary

### Error: "This application requires administrator privileges"

**Cause**: The application was not run as administrator.

**Solution**:
1. Right-click `C#TweaksPs1.exe`
2. Select "Run as administrator"
3. Click "Yes" on the UAC prompt

### Application Won't Start (No Error Message)

**Possible causes**:
1. Missing .NET 8 Runtime (framework-dependent version only)
   - Solution: Install [.NET 8 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)

2. Antivirus blocking the application
   - Solution: Add an exception for `C#TweaksPs1.exe`

3. Corrupted download
   - Solution: Re-download and extract again

### Other Issues

If you encounter any other problems:
1. Take a screenshot of the error message
2. Note down exactly what you were doing when the error occurred
3. Contact the person who provided you with this application

## What This Application Does

This is a Windows tweaking utility that allows you to:
- Modify Windows registry settings
- Configure Windows services
- Manage scheduled tasks
- Apply performance optimizations
- Customize Windows behavior

**?? IMPORTANT SAFETY NOTES:**
- Always create a system restore point before applying tweaks
- Read the description of each tweak before applying it
- You can undo most tweaks using the built-in undo feature
- Some tweaks may require a restart to take effect

## Using the Application

Once launched:
1. The main menu will appear
2. Choose "Browse and Apply Tweaks" to apply modifications
3. Choose "Browse and Undo Tweaks" to revert changes
4. Follow the on-screen prompts
5. Tweaks are organized by category for easy navigation

## Need Help?

If you need assistance:
1. Check this guide first
2. Review error messages carefully
3. Contact your IT support or the person who provided this application
4. For technical details, see the full documentation in the project repository

---

**Made with ?? for Windows power users**
