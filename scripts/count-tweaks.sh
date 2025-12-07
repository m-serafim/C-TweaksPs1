#!/bin/bash
# Count Tweaks Script
# Displays statistics about the tweaks configuration

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"
JSON_FILE="$PROJECT_DIR/config/tweaks.json"

if [ ! -f "$JSON_FILE" ]; then
    echo "❌ ERROR: Configuration file not found: $JSON_FILE"
    exit 1
fi

echo "==================================="
echo "Tweaks Configuration Statistics"
echo "==================================="
echo ""

# Count total tweaks
TOTAL_TWEAKS=$(python3 -c "import json; print(len(json.load(open('$JSON_FILE'))))")
echo "📊 Total Tweaks: $TOTAL_TWEAKS"

# Count tweaks by category
echo ""
echo "Tweaks by Category:"
python3 << 'EOF'
import json

with open('$JSON_FILE') as f:
    tweaks = json.load(f)

categories = {}
for key, tweak in tweaks.items():
    cat = tweak.get('category', 'Unknown')
    if cat not in categories:
        categories[cat] = 0
    categories[cat] += 1

for cat in sorted(categories.keys()):
    print(f"  • {cat}: {categories[cat]}")
EOF
