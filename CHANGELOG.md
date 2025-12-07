# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Security
- ✅ CodeQL security scan passed with 0 alerts
- All code changes reviewed and validated

### Fixed
- Fixed JSON parsing errors caused by embedded PowerShell code blocks in config/tweaks.json
- Fixed 17 instances of corrupted JSON structure from fix_json.ps1 script
- Fixed missing commas in multiple JSON object definitions (WPFTweaksDiskCleanup, WPFTweaksDeleteTempFiles, WPFTweaksRestorePoint, WPFTweaksDebloat)
- Resolved all JSON syntax errors preventing configuration loading
- Fixed build warning about multiple Main methods in test code

### Added
- Added comprehensive XML documentation to all model classes
- Added XML documentation to ConfigurationLoader class and methods
- Added validation test (ConfigValidationTest.cs) for configuration loading
- Added troubleshooting documentation (docs/TROUBLESHOOTING.md)
- Added JSON validation scripts (bash and PowerShell) for CI/CD integration
- Added CHANGELOG.md to track project changes

### Improved
- Enhanced error handling in ConfigurationLoader
- Added inline comments for code clarity
- Improved documentation throughout the codebase
