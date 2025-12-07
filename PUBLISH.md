# Quick Publish Guide

## Publish in 3 Steps

### Step 1: Run the Publish Script

```powershell
.\scripts\publish.ps1
```

### Step 2: Find Your Executable

Location: `publish\win-x64\C#TweaksPs1.exe`

### Step 3: Distribute

Just send that **one file**!

## That's It!

The executable includes everything:
- ? All code
- ? All dependencies
- ? Configuration file
- ? .NET runtime

## Quick Commands

### Publish (Recommended)
```powershell
.\scripts\publish.ps1
```

### Publish (Manual)
```powershell
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeAllContentForSelfExtract=true /p:EnableCompressionInSingleFile=true
```

### Test
```powershell
cd publish\win-x64
# Right-click C#TweaksPs1.exe ? Run as administrator
```

### Create ZIP
```powershell
Compress-Archive -Path "publish\win-x64\C#TweaksPs1.exe" -DestinationPath "C#TweaksPs1.zip"
```

## Platform Options

### Windows 64-bit (Most Common)
```powershell
.\scripts\publish.ps1 -Runtime win-x64
```

### Windows 32-bit
```powershell
.\scripts\publish.ps1 -Runtime win-x86
```

### Windows ARM64
```powershell
.\scripts\publish.ps1 -Runtime win-arm64
```

## File Size Options

### Default (Self-Contained) - Recommended
```powershell
.\scripts\publish.ps1
```
- Size: ~35-100 MB
- No .NET needed on target PC
- Most convenient

### Framework-Dependent (Smaller)
```powershell
.\scripts\publish.ps1 -SelfContained:$false
```
- Size: ~10-15 MB
- Requires .NET 8 on target PC
- Smaller but less portable

## Troubleshooting

### Build Fails
```powershell
dotnet clean
.\scripts\publish.ps1
```

### File Too Large
```powershell
# Use framework-dependent
.\scripts\publish.ps1 -SelfContained:$false
```

### Need to Rebuild
```powershell
dotnet clean
dotnet build
.\scripts\publish.ps1
```

## What You Get

After publishing, in `publish\win-x64\`:
- `C#TweaksPs1.exe` - The complete application ?
- `C#TweaksPs1.pdb` - Debug symbols (optional, can delete)

## Distribution

### Simple Way
Just send `C#TweaksPs1.exe` via:
- Email
- USB drive
- Network share
- Cloud storage (OneDrive, Dropbox, etc.)

### Professional Way
1. Create a ZIP:
   ```powershell
   Compress-Archive -Path "publish\win-x64\C#TweaksPs1.exe" -DestinationPath "C#TweaksPs1-v1.0.zip"
   ```

2. Provide SHA256 hash:
   ```powershell
   Get-FileHash "publish\win-x64\C#TweaksPs1.exe" -Algorithm SHA256 | Select-Object Hash
   ```

3. Include QUICKSTART.md for users

## User Instructions

Tell recipients:
1. Right-click `C#TweaksPs1.exe`
2. Select "Run as administrator"
3. That's it!

## More Info

- Full deployment guide: [docs/DEPLOYMENT.md](../docs/DEPLOYMENT.md)
- Testing guide: [docs/TESTING.md](../docs/TESTING.md)
- End-user guide: [QUICKSTART.md](../QUICKSTART.md)
- Complete summary: [SOLUTION_SUMMARY.md](../SOLUTION_SUMMARY.md)

---

**Questions?** Check the guides above or open an issue on GitHub.
