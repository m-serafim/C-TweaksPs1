# Publish Script - Creates a distribution-ready package
# This script publishes the application and verifies all required files are present

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("win-x64", "win-x86", "win-arm64")]
    [string]$Runtime = "win-x64",
    
    [Parameter(Mandatory=$false)]
    [switch]$SelfContained = $true
)

$ErrorActionPreference = "Stop"

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectDir = Split-Path -Parent $ScriptDir

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  C#TweaksPs1 Publishing Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Configuration:" -ForegroundColor Yellow
Write-Host "  Runtime: $Runtime"
Write-Host "  Self-Contained: $SelfContained"
Write-Host ""

# Clean previous builds
Write-Host "[1/4] Cleaning previous builds..." -ForegroundColor Green
Push-Location $ProjectDir
dotnet clean --configuration Release | Out-Null

# Build the project
Write-Host "[2/4] Building project..." -ForegroundColor Green
dotnet build --configuration Release
if ($LASTEXITCODE -ne 0) {
    Write-Host "? Build failed!" -ForegroundColor Red
    Pop-Location
    exit 1
}

# Publish the project
Write-Host "[3/4] Publishing application..." -ForegroundColor Green
if ($SelfContained) {
    dotnet publish --configuration Release --runtime $Runtime --self-contained true --output "publish\$Runtime"
} else {
    dotnet publish --configuration Release --runtime $Runtime --self-contained false --output "publish\$Runtime"
}

if ($LASTEXITCODE -ne 0) {
    Write-Host "? Publish failed!" -ForegroundColor Red
    Pop-Location
    exit 1
}

$PublishDir = Join-Path $ProjectDir "publish\$Runtime"

# Verify required files
Write-Host "[4/4] Verifying deployment..." -ForegroundColor Green
$ErrorCount = 0

$RequiredFiles = @(
    "C#TweaksPs1.exe",
    "config\tweaks.json"
)

foreach ($file in $RequiredFiles) {
    $filePath = Join-Path $PublishDir $file
    if (Test-Path $filePath) {
        Write-Host "  ? $file" -ForegroundColor Green
    } else {
        Write-Host "  ? $file NOT FOUND" -ForegroundColor Red
        $ErrorCount++
    }
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Results" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

if ($ErrorCount -eq 0) {
    Write-Host "? SUCCESS! Application published successfully." -ForegroundColor Green
    Write-Host ""
    Write-Host "Output location: $PublishDir" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Next steps:" -ForegroundColor Cyan
    Write-Host "  1. Navigate to the publish directory"
    Write-Host "  2. Test the application locally"
    Write-Host "  3. Create a ZIP file for distribution"
    Write-Host "  4. Ensure recipients extract ALL files including the config folder"
    Write-Host ""
    
    # Ask if user wants to create a ZIP
    $createZip = Read-Host "Create a ZIP file for distribution? (Y/N)"
    if ($createZip -eq "Y" -or $createZip -eq "y") {
        $zipPath = Join-Path $ProjectDir "publish\C#TweaksPs1-$Runtime.zip"
        if (Test-Path $zipPath) {
            Remove-Item $zipPath -Force
        }
        
        Compress-Archive -Path "$PublishDir\*" -DestinationPath $zipPath
        Write-Host ""
        Write-Host "? ZIP created: $zipPath" -ForegroundColor Green
    }
} else {
    Write-Host "? FAILED! $ErrorCount required file(s) missing." -ForegroundColor Red
    Write-Host ""
    Write-Host "Please check the build output for errors." -ForegroundColor Yellow
    Pop-Location
    exit 1
}

Pop-Location
Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
