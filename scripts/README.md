# Scripts Directory

This directory contains utility scripts for building, testing, validating, and deploying the application.

## Deployment Scripts

### `publish.ps1`

**Purpose**: Publishes the application for distribution to other PCs.

**Usage**:
```powershell
# Default: Self-contained for win-x64
.\scripts\publish.ps1

# Specify runtime
.\scripts\publish.ps1 -Runtime win-x64

# Framework-dependent (requires .NET 8 on target PC)
.\scripts\publish.ps1 -SelfContained:$false
```

**Features**:
- Cleans previous builds
- Builds the project
- Publishes with all dependencies
- Verifies required files are present
- Optionally creates a ZIP file for distribution

### `verify-deployment.bat`

**Purpose**: Verifies that all required files are present for deployment.

**Usage**:
```batch
# Run from the output/publish directory
.\scripts\verify-deployment.bat
```

**Checks**:
- Executable file exists
- Config folder exists
- tweaks.json exists
- Required DLL dependencies

## Validation Scripts

### `validate-json.ps1` / `validate-json.sh`

**Purpose**: Validates the JSON syntax of `config/tweaks.json`.

**Usage**:
```powershell
# Windows
.\scripts\validate-json.ps1

# Linux/Mac
./scripts/validate-json.sh
```

### `count-tweaks.ps1` / `count-tweaks.sh`

**Purpose**: Counts and displays statistics about the tweaks configuration.

**Usage**:
```powershell
# Windows
.\scripts\count-tweaks.ps1

# Linux/Mac
./scripts/count-tweaks.sh
```

## Deployment Workflow

For distributing the application to other PCs:

1. **Publish the application**:
   ```powershell
   .\scripts\publish.ps1
   ```

2. **Verify the output** (optional):
   ```batch
   cd publish\win-x64
   ..\..\scripts\verify-deployment.bat
   ```

3. **Create a ZIP** (done automatically by publish.ps1 if requested)

4. **Distribute**:
   - Send the ZIP file or entire publish folder
   - Instruct users to extract ALL files
   - Users must run as Administrator

## Common Issues

### Config File Not Found

**Problem**: Users get "Configuration file not found" error.

**Solution**: 
- Ensure the `config` folder is included in the distribution
- The folder structure must be preserved:
  ```
  C#TweaksPs1.exe
  config/
    ??? tweaks.json
  ```

### Missing Dependencies

**Problem**: Application fails to start due to missing DLLs.

**Solution**:
- Use self-contained publishing: `.\scripts\publish.ps1 -SelfContained`
- Or ensure target PC has .NET 8 installed

### Administrator Privileges

**Problem**: Application exits immediately or shows access denied errors.

**Solution**:
- Right-click the executable
- Select "Run as administrator"
- This is required for registry and service modifications

## See Also

- [docs/DEPLOYMENT.md](../docs/DEPLOYMENT.md) - Complete deployment guide
- [README.md](../README.md) - General documentation
- [docs/TROUBLESHOOTING.md](../docs/TROUBLESHOOTING.md) - Troubleshooting guide
