# JSON Validation Script (PowerShell)
# Validates the tweaks.json configuration file

$ErrorActionPreference = "Stop"

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectDir = Split-Path -Parent $ScriptDir
$JsonFile = Join-Path $ProjectDir "config\tweaks.json"

Write-Host "===================================" -ForegroundColor Cyan
Write-Host "JSON Configuration Validator" -ForegroundColor Cyan
Write-Host "===================================" -ForegroundColor Cyan
Write-Host ""

if (-not (Test-Path $JsonFile)) {
    Write-Host "❌ ERROR: Configuration file not found: $JsonFile" -ForegroundColor Red
    exit 1
}

Write-Host "📄 Validating: $JsonFile" -ForegroundColor White
Write-Host ""

try {
    # Try to parse the JSON
    $jsonContent = Get-Content -Path $JsonFile -Raw
    $tweaks = $jsonContent | ConvertFrom-Json
    
    Write-Host "✅ JSON syntax is valid" -ForegroundColor Green
    
    # Count tweaks
    $tweakCount = ($tweaks | Get-Member -MemberType NoteProperty).Count
    Write-Host "📊 Found $tweakCount tweaks in configuration" -ForegroundColor White
    
    exit 0
}
catch {
    Write-Host "❌ JSON syntax is INVALID" -ForegroundColor Red
    Write-Host ""
    Write-Host "Error details:" -ForegroundColor Yellow
    Write-Host $_.Exception.Message -ForegroundColor Red
    exit 1
}
