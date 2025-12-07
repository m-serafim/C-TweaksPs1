# Deployment Guide

## Distributing the Application

When distributing this application to other PCs, you **must include the config folder** along with the executable.

### Required Files

The following files/folders are required for the application to run:

```
C#TweaksPs1.exe          (or C#TweaksPs1.dll if framework-dependent)
config/
  ??? tweaks.json        (configuration file with all tweaks)
*.dll files              (all dependency DLLs)
```

### Publishing the Application

#### Self-Contained Deployment (Recommended for distribution)

This creates a standalone executable that includes .NET runtime:

```bash
dotnet publish -c Release -r win-x64 --self-contained true
```

Output will be in: `bin\Release\net8.0\win-x64\publish\`

#### Framework-Dependent Deployment

This requires .NET 8 to be installed on the target PC:

```bash
dotnet publish -c Release
```

Output will be in: `bin\Release\net8.0\publish\`

### Distribution Checklist

? **Before distributing:**

1. Build or publish the application
2. Navigate to the output directory
3. **Verify the `config` folder exists** in the output directory
4. **Verify `config\tweaks.json` exists** and is valid
5. Create a ZIP file containing:
   - The executable
   - All `.dll` files
   - The entire `config` folder
   - Optional: README files

?? **Common Mistakes:**

- ? Distributing only the `.exe` file without the `config` folder
- ? Forgetting to include the `config\tweaks.json` file
- ? Breaking the folder structure (config must be a subfolder)

### Troubleshooting

If users receive "Configuration file not found" errors:

1. **Check the config folder exists**: Ensure `config\tweaks.json` is in the same directory as the executable
2. **Verify file permissions**: The application needs read access to the config folder
3. **Check file paths**: The config folder must be named exactly `config` (lowercase)

### Example Distribution Structure

```
MyDistribution.zip
??? C#TweaksPs1.exe
??? config/
?   ??? tweaks.json
??? TaskScheduler.dll
??? System.Management.dll
??? ... (other DLL files)
??? README.md
```

### Running on Another PC

1. Extract all files to a folder
2. **Ensure the `config` folder is present**
3. Right-click on `C#TweaksPs1.exe`
4. Select "Run as administrator"

The application will automatically find the config file relative to the executable location.

## Technical Details

The application searches for `config/tweaks.json` in the following locations (in order):

1. Relative to the current working directory
2. Relative to the executable location
3. Relative to the application base directory

This ensures the config file is found regardless of how the application is launched.
