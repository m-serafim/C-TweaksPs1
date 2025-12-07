# Troubleshooting Guide

## Configuration Loading Errors

### JSON Parse Errors

If you encounter errors like:
```
ERROR: Failed to load configuration: 'p' is an invalid start of a property name
```

This indicates a JSON syntax error in the `config/tweaks.json` file.

#### Common Causes:

1. **Missing Commas**: Properties in JSON objects must be separated by commas
2. **Embedded Code**: PowerShell or other code accidentally inserted into the JSON
3. **Invalid Characters**: Special characters not properly escaped
4. **Malformed Structure**: Unclosed brackets, braces, or quotes

#### How to Fix:

1. **Validate the JSON**:
   ```bash
   python3 -m json.tool config/tweaks.json
   ```

2. **Look for the specific line mentioned in the error**:
   The error message will indicate the line number and position.

3. **Check for common issues**:
   - Missing commas after closing brackets: `]` should be followed by `,`
   - Missing commas after property values: `"value"` should be followed by `,`
   - Embedded non-JSON content (like PowerShell code)

#### Prevention:

- Always validate JSON after manual edits
- Use a proper JSON editor with syntax highlighting
- Run the validation test after changes:
  ```bash
  dotnet run --project Tests/ConfigValidationTest.cs
  ```

## Build Errors

### Administrator Privileges

The application requires administrator privileges to run. If you see errors about access denied, ensure you're running as Administrator.

### Missing Dependencies

Run `dotnet restore` to ensure all NuGet packages are installed.

## Runtime Errors

### Configuration File Not Found

Ensure `config/tweaks.json` exists in the working directory or specified path.

### Registry Access Errors

Some tweaks require specific permissions. Run as Administrator.

### Service Management Errors

Windows services require elevated privileges to modify.
