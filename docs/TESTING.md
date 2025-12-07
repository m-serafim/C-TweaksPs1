# Testing the Single-File Executable

## Quick Test

To verify your single-file executable works correctly:

### 1. Publish the Application

```powershell
.\scripts\publish.ps1
```

### 2. Navigate to Output

```powershell
cd publish\win-x64
```

### 3. Verify Single File

```powershell
dir
```

You should see:
- `C#TweaksPs1.exe` (~35-100 MB)
- `C#TweaksPs1.pdb` (optional, debug symbols)

### 4. Test Locally

Right-click `C#TweaksPs1.exe` ? Run as administrator

**Expected output**:
```
?????????????????????????????????????????????????????????????????
?              Windows Tweak Engine (C# Edition)                ?
?                    Initializing...                            ?
?????????????????????????????????????????????????????????????????

? Loading configuration from embedded resource
? Loaded 90 tweaks from embedded resource
Initialization complete.
```

**? Success indicators**:
- Shows "embedded resource" (not a file path)
- Loads correct number of tweaks
- No "Configuration file not found" error

### 5. Test on Clean System (Optional)

For thorough testing:

#### Option A: Copy to Different Location
```powershell
# Copy exe to Desktop
Copy-Item C#TweaksPs1.exe $env:USERPROFILE\Desktop\

# Run from Desktop
cd $env:USERPROFILE\Desktop
.\C#TweaksPs1.exe
```

#### Option B: Test on Virtual Machine
1. Create a Windows VM (without .NET installed)
2. Copy only the `C#TweaksPs1.exe` file
3. Run as administrator
4. Should work perfectly!

#### Option C: Test on Another PC
1. Copy `C#TweaksPs1.exe` to USB drive
2. Take to a different PC
3. Copy to any location
4. Right-click ? Run as administrator
5. Should work without any setup!

## Verification Checklist

When testing, verify:

- ? No "Configuration file not found" error
- ? Shows "Loading configuration from embedded resource"
- ? Displays correct number of tweaks (should be 90+)
- ? Main menu appears correctly
- ? Can browse tweak categories
- ? Can view tweak details
- ? Executable works from any location
- ? No external files needed

## Common Test Scenarios

### Scenario 1: Fresh PC Without .NET

**Setup**: Windows PC without .NET 8 installed

**Test**: Copy and run `C#TweaksPs1.exe`

**Expected**: Works perfectly (runtime is embedded)

### Scenario 2: Different Folders

**Test Locations**:
- Desktop
- Documents
- USB Drive
- Network Share
- Program Files

**Expected**: Works from all locations

### Scenario 3: Antivirus Enabled

**Test**: Run with Windows Defender active

**Possible Issues**:
- SmartScreen warning (click "More info" ? "Run anyway")
- Quarantine (add exception)

**Solution**: Code signing prevents this (optional)

### Scenario 4: Custom Configuration

**Test**: Place external `config/tweaks.json` next to exe

**Expected**: Uses external file instead of embedded

**Verification**: Console shows file path instead of "embedded resource"

## Troubleshooting Test Issues

### Issue: Slow First Start

**Symptom**: Takes 10-15 seconds to start

**Status**: ? Normal behavior

**Cause**: Single-file exe extracts to temp on first run

**Solution**: This is expected, subsequent runs are faster

### Issue: "File not found" Error

**Symptom**: Shows "Configuration file not found"

**Status**: ? Problem with build

**Solution**:
1. Clean: `dotnet clean`
2. Verify `config/tweaks.json` exists in project
3. Rebuild: `dotnet build`
4. Publish again with correct flags

### Issue: Missing Embedded Resource

**Verification**:
```powershell
# Check if tweaks.json is in build
dotnet build -c Release -v detailed | Select-String "tweaks.json"
```

Should show: `Embedding file config\tweaks.json`

If not shown:
- Check `C#TweaksPs1.csproj` has `<EmbeddedResource>` entry
- Verify `config/tweaks.json` exists
- Clean and rebuild

### Issue: Antivirus Blocks

**Symptom**: File deleted or execution blocked

**Solutions**:
1. Temporarily disable antivirus for testing
2. Add exception for the file
3. For production: Code sign the executable

### Issue: "App can't run on your PC"

**Cause**: Wrong architecture (32-bit vs 64-bit)

**Solution**: Publish for correct architecture:
```powershell
# 64-bit
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true

# 32-bit
dotnet publish -c Release -r win-x86 --self-contained true /p:PublishSingleFile=true
```

## Performance Testing

### File Size Test

```powershell
$file = Get-Item "publish\win-x64\C#TweaksPs1.exe"
$sizeMB = [math]::Round($file.Length / 1MB, 2)
Write-Host "File size: $sizeMB MB"
```

**Expected ranges**:
- With compression: 35-50 MB ?
- Without compression: 80-100 MB ??
- Framework-dependent: 10-15 MB ?

### Startup Time Test

```powershell
# First run (extracts files)
Measure-Command { Start-Process -FilePath "C#TweaksPs1.exe" -Wait -WindowStyle Hidden }

# Second run (cached)
Measure-Command { Start-Process -FilePath "C#TweaksPs1.exe" -Wait -WindowStyle Hidden }
```

**Expected**:
- First run: 10-15 seconds
- Second run: 2-5 seconds

### Memory Usage Test

1. Run Task Manager
2. Start application
3. Check memory usage under Details tab

**Expected**: 50-100 MB (similar to multi-file version)

## Automated Test Script

Create `test-singlefile.ps1`:

```powershell
# Quick test script for single-file executable

$ExePath = "publish\win-x64\C#TweaksPs1.exe"

Write-Host "Testing Single-File Executable..." -ForegroundColor Cyan
Write-Host ""

# Check file exists
if (-not (Test-Path $ExePath)) {
    Write-Host "? Executable not found at: $ExePath" -ForegroundColor Red
    Write-Host "Run .\scripts\publish.ps1 first" -ForegroundColor Yellow
    exit 1
}

# Check file size
$file = Get-Item $ExePath
$sizeMB = [math]::Round($file.Length / 1MB, 2)
Write-Host "? File found: $sizeMB MB" -ForegroundColor Green

# Verify it's a single file deployment
$fileCount = (Get-ChildItem "publish\win-x64" -File | Where-Object { $_.Extension -eq ".exe" -or $_.Extension -eq ".dll" }).Count
if ($fileCount -eq 1) {
    Write-Host "? Single file deployment confirmed (1 exe)" -ForegroundColor Green
} else {
    Write-Host "??  Warning: Found $fileCount executable/DLL files" -ForegroundColor Yellow
}

# Check for config folder (shouldn't need it)
if (Test-Path "publish\win-x64\config") {
    Write-Host "??  Warning: config folder exists (not needed for single-file)" -ForegroundColor Yellow
} else {
    Write-Host "? No config folder (embedded resource will be used)" -ForegroundColor Green
}

Write-Host ""
Write-Host "Test the executable:" -ForegroundColor Cyan
Write-Host "  cd publish\win-x64"
Write-Host "  Right-click C#TweaksPs1.exe ? Run as administrator"
Write-Host ""
Write-Host "Expected: Should show 'Loading configuration from embedded resource'" -ForegroundColor Cyan
```

Run it:
```powershell
.\test-singlefile.ps1
```

## Final Verification

Before distributing to users:

1. ? Publish completes without errors
2. ? Only one .exe file in output
3. ? File size is reasonable (35-100 MB)
4. ? Test run shows "embedded resource" message
5. ? Works from different locations
6. ? Test on clean system without .NET
7. ? Create test tweak successfully
8. ? Undo tweak successfully

## Success Criteria

Your single-file executable is ready for distribution when:

? **Build succeeds** without warnings  
? **Single executable** generated  
? **Configuration loads** from embedded resource  
? **Works on clean system** without .NET  
? **Portable** - runs from any location  
? **No external files** needed  
? **Tweaks apply** and undo correctly  

---

**Happy testing!** ??

If all tests pass, you're ready to distribute the single `C#TweaksPs1.exe` file!
