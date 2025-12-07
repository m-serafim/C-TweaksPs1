@echo off
REM Deployment Verification Script
REM This script verifies that all required files are present for deployment

echo ================================================
echo   C#TweaksPs1 Deployment Verification
echo ================================================
echo.

set "SCRIPT_DIR=%~dp0"
set "ERROR_COUNT=0"

echo Checking required files...
echo.

REM Check for executable
if exist "%SCRIPT_DIR%C#TweaksPs1.exe" (
    echo [OK] C#TweaksPs1.exe found
) else (
    echo [ERROR] C#TweaksPs1.exe NOT FOUND
    set /a ERROR_COUNT+=1
)

REM Check for config folder
if exist "%SCRIPT_DIR%config\" (
    echo [OK] config folder found
) else (
    echo [ERROR] config folder NOT FOUND
    set /a ERROR_COUNT+=1
)

REM Check for tweaks.json
if exist "%SCRIPT_DIR%config\tweaks.json" (
    echo [OK] config\tweaks.json found
) else (
    echo [ERROR] config\tweaks.json NOT FOUND
    set /a ERROR_COUNT+=1
)

REM Check for required DLLs
if exist "%SCRIPT_DIR%TaskScheduler.dll" (
    echo [OK] TaskScheduler.dll found
) else (
    echo [WARNING] TaskScheduler.dll not found (may cause issues)
)

if exist "%SCRIPT_DIR%System.Management.dll" (
    echo [OK] System.Management.dll found
) else (
    echo [WARNING] System.Management.dll not found (may cause issues)
)

echo.
echo ================================================
echo   Verification Results
echo ================================================
echo.

if %ERROR_COUNT% EQU 0 (
    echo [SUCCESS] All required files are present!
    echo.
    echo You can distribute this folder to other PCs.
    echo Make sure to keep the folder structure intact.
    echo.
    echo To run: Right-click C#TweaksPs1.exe and select "Run as administrator"
) else (
    echo [FAILED] %ERROR_COUNT% critical file(s) missing!
    echo.
    echo Please ensure you have:
    echo   1. Built the project: dotnet build -c Release
    echo   2. Published the project: dotnet publish -c Release
    echo   3. Included the config folder with tweaks.json
)

echo.
pause
