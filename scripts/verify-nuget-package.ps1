#!/usr/bin/env pwsh
# Script to verify NuGet package contains required metadata and files

param(
    [Parameter(Mandatory=$false)]
    [string]$PackagePath = "./packages/Dameng.Logging.TUnit.*.nupkg"
)

Write-Host "🔍 Verifying NuGet package metadata and contents..." -ForegroundColor Green

# Find the package file
$packageFile = Get-ChildItem $PackagePath | Select-Object -First 1
if (-not $packageFile) {
    Write-Error "❌ No NuGet package found at $PackagePath"
    exit 1
}

Write-Host "📦 Found package: $($packageFile.Name)" -ForegroundColor Cyan

# Create temporary directory for extraction
$tempDir = New-TemporaryFile | ForEach-Object { Remove-Item $_; New-Item -ItemType Directory -Path $_ }

try {
    # Extract package
    Expand-Archive -Path $packageFile.FullName -DestinationPath $tempDir -Force
    
    # Check for required files
    $requiredFiles = @("ReadMe.md", "LICENSE", "Dameng.Logging.TUnit.nuspec")
    foreach ($file in $requiredFiles) {
        $filePath = Join-Path $tempDir $file
        if (Test-Path $filePath) {
            Write-Host "✅ Found: $file" -ForegroundColor Green
        } else {
            Write-Host "❌ Missing: $file" -ForegroundColor Red
            exit 1
        }
    }
    
    # Verify nuspec content
    $nuspecPath = Join-Path $tempDir "Dameng.Logging.TUnit.nuspec"
    $nuspecContent = Get-Content $nuspecPath -Raw
    
    $requiredElements = @(
        "projectUrl",
        "repository.*github.com/dameng324/Dameng.Logging.TUnit",
        "readme.*ReadMe.md",
        "license.*LICENSE",
        "releaseNotes"
    )
    
    foreach ($element in $requiredElements) {
        if ($nuspecContent -match $element) {
            Write-Host "✅ Found metadata: $element" -ForegroundColor Green
        } else {
            Write-Host "❌ Missing metadata: $element" -ForegroundColor Red
            exit 1
        }
    }
    
    # Verify ReadMe content
    $readmePath = Join-Path $tempDir "ReadMe.md"
    $readmeContent = Get-Content $readmePath -Raw
    
    if ($readmeContent -match "Installation" -and $readmeContent -match "Usage" -and $readmeContent -match "Dameng.Logging.TUnit") {
        Write-Host "✅ ReadMe contains required sections" -ForegroundColor Green
    } else {
        Write-Host "❌ ReadMe missing required sections" -ForegroundColor Red
        exit 1
    }
    
    Write-Host "🎉 All verifications passed! NuGet package is properly configured." -ForegroundColor Green
    
} finally {
    # Cleanup
    Remove-Item $tempDir -Recurse -Force
}