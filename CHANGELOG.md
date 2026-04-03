# Changelog

All notable changes to this project are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

---

## [1.0.0] - 2026-01-01

### Added
- Full auto-detection of Trend Micro products via Windows registry
- Auto-click pipeline for all uninstaller dialogs:
  - Survey dialog "Sorry to see you go!" — auto-checks first option, clicks "Continue to Uninstall"
  - Confirmation dialog — auto-clicks "Uninstall"
  - Completion dialog — auto-clicks "Restart Later" to stay running
- Wait-for-completion via window-title polling (fixes race condition with child installer process)
- Comprehensive residual file scan (Program Files, ProgramData, AppData, Common Files)
- Comprehensive residual registry scan (TrendMicro trees, uninstall entries, driver/service keys)
- Force-delete for ACL-locked folders using `takeown` + `icacls` + `rd /s /q`
- Verify loop — re-scans and re-cleans up to 5 passes until system is clean
- Auto UAC elevation on startup
- System information display (Windows version, CPU architecture)
- Ko-fi donation button
