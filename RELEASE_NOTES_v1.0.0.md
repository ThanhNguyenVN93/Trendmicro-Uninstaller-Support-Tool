# 🚀 Release v1.0.0 — TrendMicro Uninstaller Support Tool

> **Fully automated, one-click removal of Trend Micro products with deep residual cleanup.**

---

## 📦 Download

| File | Description |
|---|---|
| `TrendMicroUninstallerTool-v1.0.0.zip` | **Recommended** — EXE + README + LICENSE |
| `Trendmicro Uninstaller Support Tool.exe` | Standalone executable only |

---

## ✅ What's New in v1.0.0

### Core Pipeline
- **Auto-detect** installed Trend Micro products (Maximum Security, Internet Security, Antivirus+) via Windows registry
- **Fully automatic** uninstall pipeline: detect → remove → scan → clean → verify → restart prompt

### Auto-Click Engine
Handles all Trend Micro uninstaller dialogs automatically:
- 🗳️ **Survey "Sorry to see you go!"** — checks first survey option to enable the "Continue to Uninstall" button
- ✅ **Confirmation dialog** — clicks "Uninstall" automatically
- ✔️ **Completion dialog** — clicks "Restart Later" to keep the tool running for cleanup
- ⌨️ **WPF fallback** — Enter key for any remaining dialogs

### Smart Wait Logic
- Detects uninstall completion by **window title monitoring** (not process path)  
  → Fixes race condition where scan started before the child uninstaller finished
- Timeout: 10 minutes maximum

### Deep Cleanup
- Scans and removes residual **files/folders** across all known Trend Micro paths
- Scans and removes residual **registry keys** (TrendMicro trees, services, drivers, uninstall entries)
- **Force-delete** for ACL-locked folders (e.g. `Titanium\plugin`):  
  `takeown /F` → `icacls /grant` → `Directory.Delete` → `rd /s /q`
- **Verify loop**: re-scans up to 5 passes until the system is 100% clean

### Other
- Auto-UAC elevation on startup
- System info display (Windows version + CPU architecture)
- Ko-fi donation button

---

## ⚙️ Requirements

| | |
|---|---|
| **OS** | Windows 10 / Windows 11 (64-bit recommended) |
| **Runtime** | .NET Framework 4.6 (pre-installed on Win10+) |
| **Privileges** | Administrator (auto-requested) |

---

## 🚀 How to Use

1. Download `TrendMicroUninstallerTool-v1.0.0.zip`
2. Extract and run `Trendmicro Uninstaller Support Tool.exe`
3. Click **Yes** on the UAC prompt
4. The tool runs fully automatically — no interaction needed

---

## ⚠️ Disclaimer

This tool is **not affiliated with Trend Micro Inc.**  
Always back up important data before running system cleanup tools.
