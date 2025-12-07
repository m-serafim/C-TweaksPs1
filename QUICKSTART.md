# Quick Start Guide - Single File Executable

## What You Received

You received **one file**:
- `C#TweaksPs1.exe` - The complete application (everything included!)

That's it! No config files, no folders, no DLLs - just one executable.

## ? Single-File Deployment

This application uses advanced packaging to include:
- ? All program code
- ? All dependencies
- ? Configuration file (tweaks.json)
- ? .NET runtime

Everything you need is in that one file!

## How to Run

### Step 1: Save the File

Save `C#TweaksPs1.exe` anywhere you want:
- Desktop
- Documents folder
- USB drive
- Network location
- Any folder on your PC

### Step 2: Run as Administrator

**IMPORTANT**: This application needs administrator rights to modify Windows settings.

1. **Right-click** on `C#TweaksPs1.exe`
2. **Select** "Run as administrator"
3. **Click** "Yes" when Windows asks for permission (UAC prompt)

### Step 3: Use the Application

The application will start and show you the menu!

## ?? Common Mistakes

### ? Double-clicking the file
**Problem**: The app needs administrator rights  
**Solution**: Right-click ? Run as administrator

### ? "Windows protected your PC" message
**Problem**: Windows SmartScreen blocks unsigned apps  
**Solution**: Click "More info" ? "Run anyway"

### ? Antivirus blocks or deletes the file
**Problem**: Some antivirus software flags new executables  
**Solution**: Add exception for C#TweaksPs1.exe or temporarily disable antivirus

## System Requirements

- **Operating System**: Windows 10 or Windows 11
- **Privileges**: Administrator rights
- **.NET Runtime**: Not required! (included in the executable)
- **Disk Space**: ~100 MB for the single file

## What This Application Does

This is a Windows tweaking utility that allows you to:
- Modify Windows registry settings
- Configure Windows services
- Manage scheduled tasks
- Apply performance optimizations
- Customize Windows behavior

All tweaks can be applied and undone safely.

## Safety Notes

?? **Before using:**
1. Create a system restore point (Start ? "Create a restore point")
2. Read the description of each tweak before applying
3. Start with small changes to see how they affect your system
4. You can undo most tweaks using the built-in undo feature

## Using the Application

Once launched as administrator:

1. **Main Menu** appears with options
2. **Select** "Browse and Apply Tweaks" to make changes
3. **Select** "Browse and Undo Tweaks" to revert changes
4. **Follow** the on-screen prompts
5. **Tweaks** are organized by category (Minimum, Recommended, Gaming)

Each tweak shows:
- What it does
- Why you might want it
- How to undo it

## Troubleshooting

### "This app can't run on your PC"

**Possible causes:**
- Wrong version for your Windows (32-bit vs 64-bit)
- Incompatible Windows version (needs Windows 10/11)

**Solution:**
- Check your Windows version (Settings ? System ? About)
- Request the correct version from the distributor

### Application Won't Start

**Try these steps:**
1. Make sure you're running as administrator
2. Check Windows Event Viewer for error details
3. Temporarily disable antivirus
4. Redownload the file (may be corrupted)

### "Configuration file not found" Error

**This should NOT happen** with the single-file version!

If you see this:
- The file may be corrupted
- Request a fresh copy from the distributor

### Application is Slow to Start

**Normal behavior:**
- First launch: 10-15 seconds (extracting embedded files)
- Subsequent launches: 2-5 seconds
- This is normal for single-file executables

## Portability

### Can I run it from a USB drive?
**Yes!** Just copy the exe to the USB drive and run it.

### Can I copy it to multiple PCs?
**Yes!** The same file works on any compatible Windows PC.

### Do I need internet?
**No!** Everything is self-contained.

### Can I move it after running?
**Yes!** You can move or rename it anytime.

## Sharing with Others

If you want to share this with others:

1. **Copy** the `C#TweaksPs1.exe` file
2. **Share** via USB, email, network, cloud storage, etc.
3. **Tell them** to run as administrator
4. **Remind them** it needs admin rights to work

That's it! No need to zip files or include folders.

## Performance Note

The single file is large (~100 MB) because it includes:
- Complete .NET runtime
- All program dependencies  
- Embedded configuration

This makes it portable and easy to distribute, but takes a moment to start.

## Need Help?

If you need assistance:
1. Make sure you're running as administrator
2. Check the error message carefully
3. Contact your IT support or the person who gave you this file
4. For developers: Check the GitHub repository documentation

## Version Information

To check what version you have:
1. Right-click `C#TweaksPs1.exe`
2. Select "Properties"
3. Go to "Details" tab
4. Check "File version"

## Advanced: Custom Configuration

If you want to use a custom tweak configuration:

1. Create a folder named `config` next to the executable
2. Place your custom `tweaks.json` file in that folder
3. The application will use your custom config instead of embedded

This is optional - most users don't need this!

## Summary

? **It's really this simple:**
1. Get the file: `C#TweaksPs1.exe`
2. Right-click ? Run as administrator
3. Use the application

No installation, no setup, no configuration needed!

---

**Made with ?? for Windows power users**

**Questions?** Contact the person who shared this with you or check the GitHub repository.
