@echo off
REM Launcher Script - Automatically runs C#TweaksPs1 as Administrator
REM Place this file in the same directory as C#TweaksPs1.exe

echo Starting C#TweaksPs1 as Administrator...
echo.

REM Check if we're already running as admin
net session >nul 2>&1
if %errorLevel% == 0 (
    REM Already admin, just run the app
    "%~dp0C#TweaksPs1.exe"
) else (
    REM Not admin, request elevation
    powershell -Command "Start-Process '%~dp0C#TweaksPs1.exe' -Verb RunAs"
)
