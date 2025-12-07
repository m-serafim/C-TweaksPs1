# Configuration Directory

This directory contains the JSON configuration files that define all available system tweaks.

## Files

### tweaks.json

The main configuration file containing all tweak definitions. This file is loaded at application startup and provides:

- 66+ Windows system tweaks
- Registry modifications
- Service configuration changes
- Scheduled task modifications
- PowerShell script executions

## Validation

After modifying `tweaks.json`, always validate the JSON syntax:

```bash
# Using Python
python3 -m json.tool tweaks.json

# Using our validation scripts
cd ..
./scripts/validate-json.sh
# or
.\scripts\validate-json.ps1
```

## Schema

See [../docs/JSON_SCHEMA.md](../docs/JSON_SCHEMA.md) for complete schema documentation.

## Source

The tweaks definitions are sourced from [m-serafim/winutil](https://github.com/m-serafim/winutil).

## Important Notes

- **Always backup before editing**: JSON syntax errors will prevent the application from loading
- **Validate after changes**: Use the validation scripts to ensure correctness
- **Test modifications**: Run the application after changes to verify functionality
- **Follow the schema**: Ensure all required fields are present

## Common Issues

1. **Missing commas**: Properties must be separated by commas
2. **Trailing commas**: JSON does not allow trailing commas (though our parser handles them)
3. **Unescaped quotes**: Use `\"` for quotes within string values
4. **Invalid property names**: Property names must be in double quotes

For more troubleshooting information, see [../docs/TROUBLESHOOTING.md](../docs/TROUBLESHOOTING.md).
