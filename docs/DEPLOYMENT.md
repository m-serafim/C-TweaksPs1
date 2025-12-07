# Deployment Guide - Single File Executable

## Overview

This application is now configured for **single-file deployment**. Everything, including the configuration file (`tweaks.json`), is embedded directly into the executable.

## ? Single-File Benefits

- ? **One file only** - Just distribute `C#TweaksPs1.exe`
- ? **No config folder needed** - Configuration is embedded
- ? **Self-contained** - Includes .NET runtime
- ? **Portable** - Copy and run anywhere
- ? **Simple distribution** - Just send the exe

## Publishing the Application

### Method 1: Using the Publish Script (Recommended)

```powershell
.\scripts\publish.ps1
```

This will:
1. Clean previous builds
2. Build the project
3. Publish as a single-file executable
4. Verify the output
5. Optionally create a ZIP file

### Method 2: Manual Publishing

```powershell
# Windows x64 (most common)
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeAllContentForSelfExtract=true /p:EnableCompressionInSingleFile=true

# Windows x86 (32-bit)
dotnet publish -c Release -r win-x86 --self-contained true /p:PublishSingleFile=true /p:IncludeAllContentForSelfExtract=true /p:EnableCompressionInSingleFile=true

# Windows ARM64
dotnet publish -c Release -r win-arm64 --self-contained true /p:PublishSingleFile=true /p:IncludeAllContentForSelfExtract=true /p:EnableCompressionInSingleFile=true
```

Output location: `bin\Release\net8.0\{runtime}\publish\`

## Distribution

### What to Distribute

**Just the executable!**
- `C#TweaksPs1.exe` (~80-100MB with all dependencies)

Optional:
- `C#TweaksPs1.pdb` (debug symbols, not required)

### How to Distribute

1. **Direct Transfer**:
   - Copy `C#TweaksPs1.exe` to USB drive, network share, email, etc.

2. **ZIP Archive**:
   ```powershell
   # The publish script can do this for you
   Compress-Archive -Path "publish\win-x64\C#TweaksPs1.exe" -DestinationPath "C#TweaksPs1.zip"
   ```

3. **Cloud Storage**:
   - Upload `C#TweaksPs1.exe` to OneDrive, Dropbox, Google Drive, etc.

## Running on Another PC

### Requirements

- Windows 10 or Windows 11
- Administrator privileges
- **No .NET installation needed** (runtime is embedded)

### Steps

1. Copy `C#TweaksPs1.exe` to the target PC
2. Right-click the executable
3. Select "Run as administrator"
4. Click "Yes" on UAC prompt
5. Application starts and loads embedded configuration

## How It Works

### Embedded Resources

The configuration file (`config/tweaks.json`) is embedded as a resource during compilation:

```xml
<EmbeddedResource Include="config\tweaks.json">
  <LogicalName>tweaks.json</LogicalName>
</EmbeddedResource>
```

### Loading Strategy

The `ConfigurationLoader` tries loading configuration in this order:

1. **Embedded resource** (primary method for single-file deployment)
2. External file at `config/tweaks.json` (fallback for development)
3. External file relative to executable (fallback)

This means:
- ? Single-file deployment: Uses embedded resource
- ? Development: Can still use external config files
- ? Custom configs: Can override with external file if needed

## Advanced Configuration Override

Users can still provide a custom configuration file by:

1. Creating a `config` folder next to the executable
2. Placing `tweaks.json` in that folder
3. The application will use the external file instead of embedded resource

This is useful for:
- Custom tweak configurations
- Testing modifications
- Organization-specific tweaks

## File Size

The single-file executable will be approximately:
- **Self-contained**: ~80-100 MB (includes .NET runtime)
- **Framework-dependent**: ~10-15 MB (requires .NET 8 on target PC)

The larger size is due to:
- .NET 8 runtime libraries
- All application dependencies
- Embedded configuration
- Compression reduces final size

## Publishing Options Explained

| Option | Description |
|--------|-------------|
| `PublishSingleFile=true` | Bundles all files into one executable |
| `SelfContained=true` | Includes .NET runtime (no installation needed) |
| `IncludeAllContentForSelfExtract=true` | Includes all content files |
| `EnableCompressionInSingleFile=true` | Compresses the bundle |
| `RuntimeIdentifier` | Target platform (win-x64, win-x86, win-arm64) |

## Troubleshooting

### Application Fails to Start

**Symptom**: Double-click does nothing or shows error

**Solutions**:
1. Run as administrator (required for system modifications)
2. Check Windows Defender/antivirus (may block unsigned exe)
3. Ensure target OS is Windows 10/11
4. Check Event Viewer for detailed errors

### "Configuration file not found" Error

**This should NOT happen** with single-file deployment!

If you see this error:
1. The build may have failed to embed resources
2. Check the build output for errors
3. Rebuild: `dotnet clean` then `dotnet publish ...`
4. Verify `config\tweaks.json` exists in project before building

### File Size Too Large

If 80-100MB is too large:

**Option 1: Framework-dependent build** (requires .NET 8 on target PC):
```powershell
dotnet publish -c Release -r win-x64 --self-contained false /p:PublishSingleFile=true
```
Size: ~10-15 MB

**Option 2: Use ready-to-run compilation**:
```powershell
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:PublishReadyToRun=true
```
Slightly larger but starts faster

**Option 3: Trim unused code** (advanced, may break reflection):
```powershell
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true
```

## Development vs Production

### Development (Debug)

- External `config/tweaks.json` file used
- Easier to modify configuration
- Faster build times
- Standard build: `dotnet build`

### Production (Release)

- Embedded configuration
- Single-file executable
- Optimized and compressed
- Build: `dotnet publish` with single-file options

## Security Considerations

### Code Signing

For professional distribution, consider code signing:

```powershell
# After publishing
signtool sign /f "certificate.pfx" /p "password" /t "http://timestamp.server" "C#TweaksPs1.exe"
```

Benefits:
- Removes "Unknown Publisher" warnings
- Builds trust with users
- Reduces antivirus false positives

### Antivirus

Single-file executables may trigger antivirus heuristics:

**Solutions**:
- Code sign the executable
- Submit to antivirus vendors for whitelisting
- Provide SHA256 hash for verification
- Use VirusTotal to demonstrate legitimacy

## Best Practices

1. **Always test** the published executable before distribution
2. **Provide SHA256 hash** for verification
3. **Document system requirements** clearly
4. **Include instructions** on running as administrator
5. **Consider code signing** for professional deployments
6. **Keep source backups** in case rebuild is needed
7. **Version your releases** (e.g., C#TweaksPs1-v1.0.exe)

## Quick Reference

### Build Commands

```powershell
# Clean
dotnet clean

# Build for development
dotnet build

# Publish single-file (using script)
.\scripts\publish.ps1

# Publish single-file (manual)
dotnet publish -c Release -r win-x64 --self-contained /p:PublishSingleFile=true /p:IncludeAllContentForSelfExtract=true /p:EnableCompressionInSingleFile=true
```

### Distribution Checklist

- ? Run `.\scripts\publish.ps1`
- ? Test `publish\win-x64\C#TweaksPs1.exe` locally
- ? Verify file size is reasonable (~80-100MB)
- ? Test on clean VM or different PC
- ? Create ZIP or copy exe
- ? Provide clear instructions to recipients
- ? Include administrator requirement in docs

## Summary

? **Single-file deployment makes distribution incredibly simple:**
- No config folder needed
- No DLL dependencies
- No .NET installation needed
- Just copy and run the exe!

**Perfect for:**
- Corporate IT deployments
- Sharing with non-technical users
- Quick system optimization tasks
- Portable system tools

---

**Made with ?? for Windows power users**
