# TrendMicro Uninstaller Support Tool

A fully automated Windows utility that silently removes **Trend Micro Maximum Security / Internet Security / Antivirus+** and cleans up every residual file, folder, and registry key left behind after uninstallation.

---

## ✨ Features

| Feature | Description |
|---|---|
| 🔍 **Auto-detect** | Finds installed Trend Micro products via registry |
| 🤖 **Auto-click dialogs** | Automatically navigates the uninstaller (survey, confirm, completion) |
| ⏳ **Wait for completion** | Detects uninstall finish by monitoring window titles — no race conditions |
| 🧹 **Residual file scan** | Finds leftover folders in Program Files, ProgramData, AppData, etc. |
| 🗝️ **Residual registry scan** | Scans known TrendMicro registry trees, uninstall entries, service keys |
| 💪 **Force delete** | Uses `takeown` + `icacls` + `rd /s /q` for ACL-locked folders (e.g. `Titanium\plugin`) |
| 🔁 **Verify loop** | Re-scans up to 5 passes until the system is completely clean |
| 🔒 **Admin auto-elevate** | Automatically requests UAC elevation if not running as Administrator |
| 🖥️ **System info** | Displays Windows version and CPU architecture |

---

## 📋 Requirements

- **OS:** Windows 10 / Windows 11 (64-bit recommended)
- **Runtime:** [.NET Framework 4.6](https://dotnet.microsoft.com/download/dotnet-framework) or higher
- **Privileges:** Administrator (auto-requested on launch)

---

## 🚀 Usage

1. **Download** the latest release from the [Releases](../../releases) page
2. **Run** `TrendMicroUninstallerTool.exe` — UAC elevation is requested automatically
3. The tool runs **fully automatically**:
   - Detects Trend Micro installation
   - Launches `Remove.exe` and clicks through all dialogs
   - Waits until every Trend Micro process has exited
   - Scans for residual files and registry entries
   - Cleans everything (force-deletes locked folders)
   - Verifies the system is clean (up to 5 passes)
   - Prompts to restart the computer

> **Note:** If Trend Micro is not detected, the tool exits immediately.

---

## 🔧 Build from Source

**Prerequisites:** Visual Studio 2019+ with `.NET Framework 4.6` workload

```
git clone https://github.com/YOUR_USERNAME/frmtrendunins.git
cd frmtrendunins
# Open frmtrendunins.sln in Visual Studio and build (Ctrl+Shift+B)
```

Or via MSBuild:
```
msbuild frmtrendunins.sln /p:Configuration=Release
```

Output binary: `bin\Release\frmtrendunins.exe`

---

## 🗂️ What Gets Removed

### Files & Folders
| Path |
|---|
| `C:\Program Files\Trend Micro` |
| `C:\Program Files (x86)\Trend Micro` |
| `C:\ProgramData\Trend Micro` |
| `C:\ProgramData\Trend Micro Installer` |
| `%LocalAppData%\Trend Micro` |
| `%AppData%\Trend Micro` |
| Common Files (x86 and x64) |

### Registry Keys
| Hive | Path |
|---|---|
| `HKLM` | `SOFTWARE\TrendMicro` |
| `HKLM` | `SOFTWARE\WOW6432Node\TrendMicro` |
| `HKCU` | `SOFTWARE\TrendMicro` |
| `HKLM` | `SOFTWARE\...\Uninstall\{TM entries}` |
| `HKLM` | `SYSTEM\CurrentControlSet\Services\tmcomm` (and related) |
| `HKLM` | `DRIVERS\DriverDatabase\DriverPackages\tmel*` |

---

## ⚠️ Disclaimer

This tool is provided **as-is** for personal use. It modifies the Windows registry and deletes files permanently. Always **back up important data** before running system cleanup tools.

This project is **not affiliated with or endorsed by Trend Micro Inc.**

---

## 💖 Support the Tool

If this tool saved you time, consider supporting its development:

[![Ko-fi](https://img.shields.io/badge/Support-Ko--fi-FF5E5B?logo=ko-fi&logoColor=white)](https://ko-fi.com/thanhnguyen150993)

---

## 📄 License

[MIT License](LICENSE) — free to use, modify, and distribute.
