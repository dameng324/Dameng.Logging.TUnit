#!/bin/bash
# Script to verify NuGet package contains required metadata and files

set -e

PACKAGE_PATH="${1:-./packages/Dameng.Logging.TUnit.*.nupkg}"

echo "🔍 Verifying NuGet package metadata and contents..."

# Find the package file
PACKAGE_FILE=$(ls $PACKAGE_PATH 2>/dev/null | head -1)
if [ -z "$PACKAGE_FILE" ]; then
    echo "❌ No NuGet package found at $PACKAGE_PATH"
    exit 1
fi

echo "📦 Found package: $(basename "$PACKAGE_FILE")"

# Create temporary directory
TEMP_DIR=$(mktemp -d)
cleanup() {
    rm -rf "$TEMP_DIR"
}
trap cleanup EXIT

# Extract package
unzip -q "$PACKAGE_FILE" -d "$TEMP_DIR"

# Check for required files
echo "Checking required files..."
REQUIRED_FILES=("ReadMe.md" "LICENSE" "Dameng.Logging.TUnit.nuspec")
for file in "${REQUIRED_FILES[@]}"; do
    if [ -f "$TEMP_DIR/$file" ]; then
        echo "✅ Found: $file"
    else
        echo "❌ Missing: $file"
        exit 1
    fi
done

# Verify nuspec content
echo "Checking metadata..."
NUSPEC_FILE="$TEMP_DIR/Dameng.Logging.TUnit.nuspec"
REQUIRED_PATTERNS=(
    "projectUrl"
    "github.com/dameng324/Dameng.Logging.TUnit"
    "ReadMe.md"
    "LICENSE"
    "releaseNotes"
)

for pattern in "${REQUIRED_PATTERNS[@]}"; do
    if grep -q "$pattern" "$NUSPEC_FILE"; then
        echo "✅ Found metadata: $pattern"
    else
        echo "❌ Missing metadata: $pattern"
        exit 1
    fi
done

# Verify ReadMe content
echo "Checking ReadMe content..."
README_FILE="$TEMP_DIR/ReadMe.md"
if grep -q "Installation" "$README_FILE" && grep -q "Usage" "$README_FILE" && grep -q "Dameng.Logging.TUnit" "$README_FILE"; then
    echo "✅ ReadMe contains required sections"
else
    echo "❌ ReadMe missing required sections"
    exit 1
fi

echo "🎉 All verifications passed! NuGet package is properly configured."