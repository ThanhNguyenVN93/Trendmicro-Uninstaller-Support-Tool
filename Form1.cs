using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace frmtrendunins
{
    public partial class Form1 : Form
    {
        // ── P/Invoke — auto-click Trend Micro dialogs ──────────────────
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsDelegate lpEnumFunc, IntPtr lParam);

        private delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindowEnabled(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private const uint BM_CLICK      = 0x00F5;
        private const byte VK_RETURN     = 0x0D;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        // ── Constants ──────────────────────────────────────────────────────
        private const string UninstallRegPath     = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        private const string TrendUninstallerPath = @"C:\Program Files\Trend Micro\Titanium\Remove.exe";
        private const string TrendInstallDir      = @"C:\Program Files\Trend Micro";

        private static readonly string[] KnownProducts =
        {
            "Trend Micro Antivirus+",
            "Trend Micro Internet Security",
            "Trend Micro Maximum Security"
        };

        // ── Residual scan: folders to check ────────────────────────────────
        private static readonly string[] ResidualFolders =
        {
            @"C:\Program Files\Trend Micro",
            @"C:\Program Files (x86)\Trend Micro",
            @"C:\ProgramData\Trend Micro",
            @"C:\ProgramData\Trend Micro Installer",
        };

        // ── Residual scan: registry trees to delete entirely ───────────────
        private static readonly RegPath[] FullTreePaths =
        {
            new RegPath(RegistryHive.LocalMachine, RegistryView.Registry64, @"SOFTWARE\TrendMicro"),
            new RegPath(RegistryHive.LocalMachine, RegistryView.Registry64, @"SOFTWARE\WOW6432Node\TrendMicro"),
            new RegPath(RegistryHive.CurrentUser,  RegistryView.Default,    @"SOFTWARE\TrendMicro"),
            new RegPath(RegistryHive.LocalMachine, RegistryView.Registry64, @"SOFTWARE\WOW6432Node\Google\Chrome\NativeMessagingHosts\com.trendmicro.tmtoolbar"),
        };

        // ── Residual scan: keys whose values may reference Trend Micro ─────
        private static readonly RegPath[] ValueScanPaths =
        {
            new RegPath(RegistryHive.CurrentUser, RegistryView.Default,
                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Store"),
            new RegPath(RegistryHive.CurrentUser, RegistryView.Default,
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FeatureUsage\AppSwitched"),
            new RegPath(RegistryHive.CurrentUser, RegistryView.Default,
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FeatureUsage\AppBadgeUpdated"),
            new RegPath(RegistryHive.CurrentUser, RegistryView.Default,
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FeatureUsage\AppLaunch"),
        };

        // ── Residual scan: parent keys whose subkeys may match patterns ────
        private static readonly SubkeyScan[] SubkeyScanPaths =
        {
            new SubkeyScan(RegistryHive.LocalMachine, RegistryView.Registry64,
                @"DRIVERS\DriverDatabase\DriverPackages",
                new[] { "tmel" }),
            new SubkeyScan(RegistryHive.LocalMachine, RegistryView.Registry64,
                @"SYSTEM\CurrentControlSet\Services",
                new[] { "tmcomm", "tmel", "tmpreflt", "tmactmon", "tmevtmgr",
                        "tmnciesc", "tmusa", "tmwfp", "Amsp" }),
        };

        private static readonly string[] MatchPatterns = { "trend micro", "trendmicro" };

        // ── Scan result types ──────────────────────────────────────────────
        private class RegPath
        {
            public RegistryHive Hive;
            public RegistryView View;
            public string Path;
            public RegPath(RegistryHive h, RegistryView v, string p) { Hive = h; View = v; Path = p; }
        }

        private class SubkeyScan
        {
            public RegistryHive Hive;
            public RegistryView View;
            public string ParentPath;
            public string[] Patterns;
            public SubkeyScan(RegistryHive h, RegistryView v, string p, string[] pat)
            { Hive = h; View = v; ParentPath = p; Patterns = pat; }
        }

        private class RegResult
        {
            public RegistryHive Hive;
            public RegistryView View;
            public string KeyPath;
            public string ValueName;   // null → delete entire key/tree
            public bool IsTree;
            public string Col1, Col2, Col3;
        }

        private class FileResult
        {
            public string Path;
            public long SizeKb;
            public string Desc;
        }

        // ── Runtime state ──────────────────────────────────────────────────
        private Label  lblStatus;
        private string detectedProductName;

        private readonly List<RegResult>  _regResults  = new List<RegResult>();
        private readonly List<FileResult> _fileResults = new List<FileResult>();

        // ── Constructor ────────────────────────────────────────────────────
        public Form1()
        {
            InitializeComponent();
            picWarning.Image    = SystemIcons.Warning.ToBitmap();
            picWarning.SizeMode = PictureBoxSizeMode.Zoom;
            this.Load += Form1_Load;
        }

        // ══════════════════════════════════════════════════════════════════
        //  FORM LOAD — kicks off the fully-automatic pipeline
        // ══════════════════════════════════════════════════════════════════
        private async void Form1_Load(object sender, EventArgs e)
        {
            lblStatus = new Label
            {
                Name      = "lblStatus",
                Font      = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.DimGray,
                AutoSize  = true,
                Location  = new Point(15, 80),
                MaximumSize = new Size(290, 0),
                Text      = "Initializing..."
            };
            gbDetection.Controls.Add(lblStatus);

            btnUninstall.Enabled = false;
            btnScan.Enabled      = false;
            btnClean.Enabled     = false;

            btnDonate.Click += (s, ev) =>
            {
                try { Process.Start("https://ko-fi.com/thanhnguyen150993"); }
                catch { }
            };

            if (!CheckAdminPrivileges()) return;

            LoadSystemInfo();

            bool detected = DetectTrendMicro();

            if (!detected)
            {
                MessageBox.Show(
                    "Trend Micro is not installed on this computer.\nThe application will now exit.",
                    "Not Detected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Application.Exit();
                return;
            }

            await RunAutoPipelineAsync();
        }

        // ══════════════════════════════════════════════════════════════════
        //  SYSTEM INFORMATION
        // ══════════════════════════════════════════════════════════════════
        private void LoadSystemInfo()
        {
            try
            {
                lblOS.Text = "Windows Version: " + Environment.OSVersion.VersionString;
                string bits = Environment.Is64BitOperatingSystem ? "64-bit OS" : "32-bit OS";
                string arch = Environment.Is64BitProcess          ? "x64"       : "x86";
                lblArchitecture.Text = $"System Type: {bits}, {arch}-based processor";
            }
            catch
            {
                lblOS.Text           = "Windows Version: Unavailable";
                lblArchitecture.Text = "System Type: Unavailable";
            }
        }

        // ══════════════════════════════════════════════════════════════════
        //  DETECT — registry + file system (only main products)
        // ══════════════════════════════════════════════════════════════════
        private bool DetectTrendMicro()
        {
            try
            {
                string detectedName    = null;
                string detectedVersion = null;

                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(
                           RegistryHive.LocalMachine, RegistryView.Registry64))
                using (RegistryKey uninstallKey = baseKey.OpenSubKey(UninstallRegPath, writable: false))
                {
                    if (uninstallKey != null)
                    {
                        foreach (string sub in uninstallKey.GetSubKeyNames())
                        {
                            using (RegistryKey entry = uninstallKey.OpenSubKey(sub, writable: false))
                            {
                                if (entry == null) continue;
                                string name = entry.GetValue("DisplayName") as string ?? string.Empty;
                                bool match = KnownProducts.Any(p =>
                                    string.Equals(name, p, StringComparison.OrdinalIgnoreCase));
                                if (!match) continue;
                                detectedName    = name;
                                detectedVersion = entry.GetValue("DisplayVersion") as string ?? "Unknown";
                                break;
                            }
                        }
                    }
                }

                detectedProductName = detectedName;
                bool removeExeExists  = File.Exists(TrendUninstallerPath);
                bool installDirExists = Directory.Exists(TrendInstallDir);
                bool somethingFound   = detectedName != null || removeExeExists || installDirExists;

                if (!somethingFound) return false;

                lblRegistryInfo.Text = detectedName != null
                    ? $"Detected: {detectedName} (v{detectedVersion})"
                    : "Trend Micro residual files found.";

                if (removeExeExists)
                    SetStatusLabel("Remove.exe found. Ready to uninstall.", Color.ForestGreen);
                else
                    SetStatusLabel("No Remove.exe — will clean residual data.", Color.DarkOrange);

                return true;
            }
            catch (Exception ex)
            {
                SetStatusError("Detection error: " + ex.Message);
                return false;
            }
        }

        // ══════════════════════════════════════════════════════════════════
        //  AUTO PIPELINE — detect → remove → scan → clean
        // ══════════════════════════════════════════════════════════════════
        private async Task RunAutoPipelineAsync()
        {
            try
            {
                // ── Step 1: Remove via Remove.exe ──────────────────────
                btnUninstall.Enabled = true;
                await Task.Delay(600);
                btnUninstall.Enabled = false;
                await RunRemoveExeAsync();

                // ── Step 2: Scan residual ──────────────────────────────
                btnScan.Enabled = true;
                await Task.Delay(600);
                btnScan.Enabled = false;
                await ScanResidualAsync();

                // ── Step 3: Clean all ──────────────────────────────────
                btnClean.Enabled = true;
                await Task.Delay(600);
                btnClean.Enabled = false;
                await CleanAllAsync();

                // ── Step 4: Verify loop — re-scan until clean ──────────
                int pass = 1;
                while (true)
                {
                    SetStatusLabel($"Verifying system (pass {pass})...", Color.DodgerBlue);
                    lblRegistryInfo.Text = $"Re-scanning entire system — pass {pass}...";
                    await Task.Delay(800);

                    await FullSystemVerifyAsync();

                    int remaining = _regResults.Count + _fileResults.Count;

                    if (remaining == 0)
                        break; // system is clean

                    // Still dirty — show what's left, clean again
                    SetStatusLabel(
                        $"Pass {pass}: {remaining} item(s) still found — cleaning...",
                        Color.DarkOrange);
                    lblRegistryInfo.Text = $"{_regResults.Count} reg + {_fileResults.Count} file entries remain.";

                    dgvRegistry.Rows.Clear();
                    foreach (var r in _regResults)
                        dgvRegistry.Rows.Add(r.Col1, r.Col2, r.Col3);

                    dgvFiles.Rows.Clear();
                    foreach (var f in _fileResults)
                        dgvFiles.Rows.Add(f.Path, f.SizeKb.ToString("N0"), f.Desc);

                    await Task.Delay(600);
                    await CleanAllAsync();

                    pass++;
                    if (pass > 5) // safety limit
                    {
                        SetStatusLabel(
                            "⚠ Some items could not be removed (files in use).",
                            Color.DarkOrange);
                        lblRegistryInfo.Text = "Cleanup mostly done. A restart may release locked files.";
                        break;
                    }
                }

                // ── Step 5: All clean — prompt restart ─────────────────
                SetStatusLabel("✔ System is clean! Preparing to restart...", Color.ForestGreen);
                lblRegistryInfo.Text = "No Trend Micro traces found. Ready to reboot.";

                dgvRegistry.Rows.Clear();
                dgvRegistry.Rows.Add("(none)", "", "✔ System clean");
                dgvFiles.Rows.Clear();
                dgvFiles.Rows.Add("(none)", "", "✔ System clean");

                var result = MessageBox.Show(
                    "Trend Micro has been completely removed!\n\n" +
                    "It is recommended to restart your computer now\n" +
                    "to finalize the cleanup.\n\n" +
                    "Restart now?",
                    "Cleanup Complete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    SetStatusLabel("Restarting computer...", Color.DodgerBlue);
                    await Task.Delay(1000);
                    Process.Start("shutdown", "/r /t 5 /c \"TrendMicro Uninstaller: cleanup complete — restarting.\"");
                    Application.Exit();
                }
                else
                {
                    SetStatusLabel("✔ All done! You can restart manually later.", Color.ForestGreen);
                }
            }
            catch (Exception ex)
            {
                SetStatusError("Pipeline error: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        //  FULL SYSTEM VERIFY — deep re-scan registry + file system
        //  Populates _regResults / _fileResults for the verify loop.
        // ══════════════════════════════════════════════════════════════════
        private async Task FullSystemVerifyAsync()
        {
            _regResults.Clear();
            _fileResults.Clear();

            await Task.Run(() =>
            {
                // Registry: same comprehensive scan as ScanResidualAsync
                foreach (var t in FullTreePaths)
                    ScanTree(t.Hive, t.View, t.Path);

                ScanUninstallEntries(RegistryHive.LocalMachine, RegistryView.Registry64);
                ScanUninstallEntries(RegistryHive.LocalMachine, RegistryView.Registry32);
                ScanUninstallEntries(RegistryHive.CurrentUser,  RegistryView.Default);

                foreach (var v in ValueScanPaths)
                    ScanValuesContaining(v.Hive, v.View, v.Path, MatchPatterns);

                foreach (var s in SubkeyScanPaths)
                    ScanSubkeysMatching(s.Hive, s.View, s.ParentPath, s.Patterns);

                // File system: all known + user-profile paths
                foreach (string dir in ResidualFolders)
                    AddFolderIfExists(dir);

                string local   = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                AddFolderIfExists(Path.Combine(local,   "Trend Micro"));
                AddFolderIfExists(Path.Combine(local,   "TrendMicro"));
                AddFolderIfExists(Path.Combine(roaming, "Trend Micro"));
                AddFolderIfExists(Path.Combine(roaming, "TrendMicro"));

                string pf86 = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86);
                string pf64 = Environment.GetEnvironmentVariable("CommonProgramW6432") ?? "";
                AddFolderIfExists(Path.Combine(pf86, "Trend Micro"));
                if (!string.IsNullOrEmpty(pf64))
                    AddFolderIfExists(Path.Combine(pf64, "Trend Micro"));

                // Extra: scan the system drive root for stray Trend Micro folders
                string sysDrive = Path.GetPathRoot(Environment.SystemDirectory);
                AddFolderIfExists(Path.Combine(sysDrive, "Trend Micro"));
            });
        }

        // ══════════════════════════════════════════════════════════════════
        //  STEP 1: RUN REMOVE.EXE + WAIT FOR ALL TM PROCESSES TO FINISH
        //
        //  Remove.exe typically exits quickly after launching the real
        //  child uninstaller.  We must wait for that child to finish too,
        //  otherwise the scan runs while Titanium\plugin is still locked.
        // ══════════════════════════════════════════════════════════════════
        private async Task RunRemoveExeAsync()
        {
            if (!File.Exists(TrendUninstallerPath))
            {
                SetStatusLabel("Remove.exe not found — skipping to scan...", Color.Orange);
                return;
            }

            SetStatusLabel("Running Remove.exe — auto-clicking dialogs...", Color.DodgerBlue);

            using (var proc = new Process())
            {
                proc.StartInfo = new ProcessStartInfo
                {
                    FileName        = TrendUninstallerPath,
                    UseShellExecute = false
                };
                proc.Start();

                // Phase A: wait for Remove.exe itself, auto-clicking dialogs
                while (!proc.HasExited)
                {
                    await Task.Delay(1000);
                    await Task.Run(() => AutoClickTrendMicroDialogs());
                }

                lblRegistryInfo.Text = proc.ExitCode == 0
                    ? "Remove.exe finished — waiting for child uninstaller..."
                    : $"Remove.exe exited ({proc.ExitCode}) — waiting for child processes...";
            }

            // Phase B: Remove.exe may have spawned a child uninstaller process.
            // Give it 2 s to start, then wait until every TM process has exited.
            await Task.Delay(2000);
            await WaitForTrendMicroProcessesAsync();

            SetStatusLabel("Uninstall complete — all processes finished.", Color.ForestGreen);
        }

        // ══════════════════════════════════════════════════════════════════
        //  WAIT — polls until no Trend Micro WINDOW is visible (max 10 min)
        //
        //  Why window-title detection instead of process exe path:
        //  The child uninstaller launched by Remove.exe runs from
        //  C:\Windows\Installer\{GUID}\ — its exe path never contains
        //  "Trend Micro", so process-path polling exits immediately while
        //  the installer is still at 80%.  Window title is always reliable.
        // ══════════════════════════════════════════════════════════════════
        private async Task WaitForTrendMicroProcessesAsync()
        {
            var deadline = DateTime.UtcNow.AddMinutes(10);
            int elapsed  = 0;

            while (DateTime.UtcNow < deadline)
            {
                bool anyWindow = false;

                await Task.Run(() =>
                {
                    // Auto-click dialogs (including "Restart Later" on completion)
                    AutoClickTrendMicroDialogs();

                    // Check whether any visible Trend Micro window still exists
                    EnumWindows((hWnd, lParam) =>
                    {
                        if (!IsWindowVisible(hWnd)) return true;

                        var buf = new StringBuilder(512);
                        GetWindowText(hWnd, buf, buf.Capacity);
                        if (buf.ToString().IndexOf("Trend Micro",
                                StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            anyWindow = true;
                            return false; // found one — stop enumeration
                        }
                        return true;
                    }, IntPtr.Zero);
                });

                if (!anyWindow) break; // all TM windows gone — uninstall done

                elapsed += 2;
                SetStatusLabel(
                    $"Waiting for uninstaller to finish... ({elapsed}s)",
                    Color.DodgerBlue);
                lblRegistryInfo.Text = "Trend Micro uninstaller is still running — please wait.";

                await Task.Delay(2000);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        //  AUTO-CLICK — finds Trend Micro windows and clicks the right button
        //
        //  Button priority (highest first):
        //    1. "Restart Later"           — dismisses completion dialog, no reboot
        //    2. Survey "Sorry to see you go!" — check first option, then continue
        //    3. "Uninstall" / "Continue" / "Next" / "OK" — advances wizard
        //    4. Enter key fallback        — WPF / custom-drawn default button
        //
        //  "Restart Now" is intentionally NOT clicked — our tool must stay
        //  running to finish the scan and cleanup after uninstall.
        // ══════════════════════════════════════════════════════════════════
        private static void AutoClickTrendMicroDialogs()
        {
            try
            {
                EnumWindows((hWnd, lParam) =>
                {
                    if (!IsWindowVisible(hWnd)) return true;

                    var title = new StringBuilder(512);
                    GetWindowText(hWnd, title, title.Capacity);
                    string titleStr = title.ToString();

                    // "Trend Micro" (with space) won't match our own tool
                    // title "TrendMicro Uninstaller Support Tool" (no space)
                    if (titleStr.IndexOf("Trend Micro", StringComparison.OrdinalIgnoreCase) < 0)
                        return true;

                    // ── Priority 1: "Restart Later" — close completion dialog ──
                    bool handled = false;
                    EnumChildWindows(hWnd, (childHwnd, childLParam) =>
                    {
                        var btnText = new StringBuilder(256);
                        GetWindowText(childHwnd, btnText, btnText.Capacity);
                        string text = btnText.ToString();

                        if (text.IndexOf("Restart Later", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            text.IndexOf("Later",         StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            SendMessage(childHwnd, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
                            handled = true;
                            return false;
                        }
                        return true;
                    }, IntPtr.Zero);

                    if (handled) return false;

                    // ── Priority 2: Survey dialog "Sorry to see you go!" ──────
                    // "Continue to Uninstall" is disabled until a reason is selected.
                    // Find the button handle, check the first survey option, then click it.
                    IntPtr continueBtn = IntPtr.Zero;
                    EnumChildWindows(hWnd, (childHwnd, childLParam) =>
                    {
                        var t = new StringBuilder(256);
                        GetWindowText(childHwnd, t, t.Capacity);
                        if (t.ToString().IndexOf("Continue to Uninstall",
                                StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            continueBtn = childHwnd;
                            return false;
                        }
                        return true;
                    }, IntPtr.Zero);

                    if (continueBtn != IntPtr.Zero)
                    {
                        if (!IsWindowEnabled(continueBtn))
                        {
                            // Click the first survey option (long descriptive text, not an action button)
                            EnumChildWindows(hWnd, (childHwnd, childLParam) =>
                            {
                                if (childHwnd == continueBtn) return true;
                                var t = new StringBuilder(256);
                                GetWindowText(childHwnd, t, t.Capacity);
                                string optText = t.ToString().Trim();
                                if (optText.Length > 8 &&
                                    optText.IndexOf("Cancel",   StringComparison.OrdinalIgnoreCase) < 0 &&
                                    optText.IndexOf("Continue", StringComparison.OrdinalIgnoreCase) < 0)
                                {
                                    SendMessage(childHwnd, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
                                    handled = true;
                                    return false;
                                }
                                return true;
                            }, IntPtr.Zero);

                            if (handled)
                                System.Threading.Thread.Sleep(400); // wait for button to enable
                        }
                        // Click "Continue to Uninstall" (now enabled)
                        SendMessage(continueBtn, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
                        return false;
                    }

                    // ── Priority 3: advance the wizard ───────────────────────
                    EnumChildWindows(hWnd, (childHwnd, childLParam) =>
                    {
                        var btnText = new StringBuilder(256);
                        GetWindowText(childHwnd, btnText, btnText.Capacity);
                        string text = btnText.ToString();

                        if (text.IndexOf("Uninstall", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            text.IndexOf("Continue",  StringComparison.OrdinalIgnoreCase) >= 0 ||
                            text.IndexOf("Next",      StringComparison.OrdinalIgnoreCase) >= 0  ||
                            text.IndexOf("OK",        StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            SendMessage(childHwnd, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
                            handled = true;
                            return false;
                        }
                        return true;
                    }, IntPtr.Zero);

                    if (handled) return false;

                    // ── Priority 4: WPF fallback — bring to front + Enter ─────
                    SetForegroundWindow(hWnd);
                    System.Threading.Thread.Sleep(300);
                    keybd_event(VK_RETURN, 0, 0, UIntPtr.Zero);
                    System.Threading.Thread.Sleep(50);
                    keybd_event(VK_RETURN, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);

                    return false;
                }, IntPtr.Zero);
            }
            catch { /* window may disappear mid-scan */ }
        }

        // ══════════════════════════════════════════════════════════════════
        //  STEP 2: COMPREHENSIVE RESIDUAL SCAN
        // ══════════════════════════════════════════════════════════════════
        private async Task ScanResidualAsync()
        {
            _regResults.Clear();
            _fileResults.Clear();
            dgvRegistry.Rows.Clear();
            dgvFiles.Rows.Clear();

            // ── Phase 1: Registry ────────────────────────────────────────
            SetStatusLabel("Scanning registry for Trend Micro traces...", Color.DodgerBlue);

            await Task.Run(() =>
            {
                // 1a. Known full key trees
                foreach (var t in FullTreePaths)
                    ScanTree(t.Hive, t.View, t.Path);

                // 1b. Uninstall entries referencing Trend Micro
                ScanUninstallEntries(RegistryHive.LocalMachine, RegistryView.Registry64);
                ScanUninstallEntries(RegistryHive.LocalMachine, RegistryView.Registry32);
                ScanUninstallEntries(RegistryHive.CurrentUser,  RegistryView.Default);

                // 1c. Values whose name or string data contains Trend Micro
                foreach (var v in ValueScanPaths)
                    ScanValuesContaining(v.Hive, v.View, v.Path, MatchPatterns);

                // 1d. Subkeys whose name matches known patterns
                foreach (var s in SubkeyScanPaths)
                    ScanSubkeysMatching(s.Hive, s.View, s.ParentPath, s.Patterns);
            });

            foreach (var r in _regResults)
                dgvRegistry.Rows.Add(r.Col1, r.Col2, r.Col3);
            if (_regResults.Count == 0)
                dgvRegistry.Rows.Add("(none)", "", "No residual registry entries found");

            // ── Phase 2: File system ─────────────────────────────────────
            SetStatusLabel("Scanning file system for Trend Micro folders...", Color.DodgerBlue);

            await Task.Run(() =>
            {
                // 2a. Known installation / data directories
                foreach (string dir in ResidualFolders)
                    AddFolderIfExists(dir);

                // 2b. Per-user AppData folders
                string local   = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                AddFolderIfExists(Path.Combine(local,   "Trend Micro"));
                AddFolderIfExists(Path.Combine(local,   "TrendMicro"));
                AddFolderIfExists(Path.Combine(roaming, "Trend Micro"));
                AddFolderIfExists(Path.Combine(roaming, "TrendMicro"));

                // 2c. Common files (x86 and x64)
                string pf86 = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86);
                string pf64 = Environment.GetEnvironmentVariable("CommonProgramW6432") ?? "";
                AddFolderIfExists(Path.Combine(pf86, "Trend Micro"));
                if (!string.IsNullOrEmpty(pf64))
                    AddFolderIfExists(Path.Combine(pf64, "Trend Micro"));
            });

            foreach (var f in _fileResults)
                dgvFiles.Rows.Add(f.Path, f.SizeKb.ToString("N0"), f.Desc);
            if (_fileResults.Count == 0)
                dgvFiles.Rows.Add("(none)", "", "No residual files found");

            SetStatusLabel(
                $"Scan complete — {_regResults.Count} registry, {_fileResults.Count} file entries found.",
                Color.ForestGreen);
        }

        // ══════════════════════════════════════════════════════════════════
        //  STEP 3: CLEAN ALL (uses stored scan results)
        // ══════════════════════════════════════════════════════════════════
        private async Task CleanAllAsync()
        {
            if (_regResults.Count == 0 && _fileResults.Count == 0)
            {
                SetStatusLabel("Nothing to clean.", Color.ForestGreen);
                return;
            }

            // ── Registry cleanup ─────────────────────────────────────────
            SetStatusLabel("Removing residual registry entries...", Color.DodgerBlue);

            await Task.Run(() =>
            {
                foreach (var r in _regResults)
                {
                    try
                    {
                        using (var root = RegistryKey.OpenBaseKey(r.Hive, r.View))
                        {
                            if (r.IsTree)
                            {
                                root.DeleteSubKeyTree(r.KeyPath, throwOnMissingSubKey: false);
                            }
                            else if (r.ValueName != null)
                            {
                                using (var key = root.OpenSubKey(r.KeyPath, writable: true))
                                    key?.DeleteValue(r.ValueName, throwOnMissingValue: false);
                            }
                        }
                    }
                    catch { /* best-effort */ }
                }
            });

            dgvRegistry.Rows.Clear();
            foreach (var r in _regResults)
                dgvRegistry.Rows.Add(r.Col1, r.Col2, "✔ Deleted");

            // ── File system cleanup ──────────────────────────────────────
            SetStatusLabel("Removing residual files and folders...", Color.DodgerBlue);

            await Task.Run(() =>
            {
                foreach (var f in _fileResults)
                {
                    try
                    {
                        if (Directory.Exists(f.Path))
                        {
                            try
                            {
                                Directory.Delete(f.Path, recursive: true);
                            }
                            catch
                            {
                                // ACL-locked folders (e.g. Titanium\plugin) — force delete
                                ForceDeleteDirectory(f.Path);
                            }
                        }
                        else if (File.Exists(f.Path))
                            File.Delete(f.Path);
                    }
                    catch { /* files in use are left behind */ }
                }
            });

            dgvFiles.Rows.Clear();
            foreach (var f in _fileResults)
                dgvFiles.Rows.Add(f.Path, f.SizeKb.ToString("N0"), "✔ Removed");

            SetStatusLabel("✔ Cleanup complete!", Color.ForestGreen);
        }

        // ══════════════════════════════════════════════════════════════════
        //  ADMIN PRIVILEGE GUARD
        // ══════════════════════════════════════════════════════════════════
        private bool CheckAdminPrivileges()
        {
            if (IsRunningAsAdministrator()) return true;

            picWarning.Image     = SystemIcons.Shield.ToBitmap();
            lblRegistryInfo.Text = "Requesting administrator privileges...";
            SetStatusLabel("Elevating privileges automatically...", Color.Orange);
            Application.DoEvents();

            if (RestartAsAdministrator())
                return false;

            lblRegistryInfo.Text = "Elevation denied by user.";
            SetStatusLabel("Cancelled by user. Application will close.", Color.Red);
            Application.DoEvents();

            var closeTimer = new System.Windows.Forms.Timer { Interval = 2000 };
            closeTimer.Tick += (s, ev) => { closeTimer.Stop(); Application.Exit(); };
            closeTimer.Start();

            return false;
        }

        private static bool IsRunningAsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
                return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static bool RestartAsAdministrator()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName        = Application.ExecutablePath,
                    UseShellExecute = true,
                    Verb            = "runas"
                });
                Application.Exit();
                return true;
            }
            catch (System.ComponentModel.Win32Exception) { return false; }
            catch (Exception ex)
            {
                MessageBox.Show("Could not restart as Administrator:\n\n" + ex.Message,
                    "Elevation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // ══════════════════════════════════════════════════════════════════
        //  SCAN HELPERS — Registry
        // ══════════════════════════════════════════════════════════════════

        /// <summary>Check if a full registry tree exists and add it to results.</summary>
        private void ScanTree(RegistryHive hive, RegistryView view, string subKeyPath)
        {
            try
            {
                using (var root = RegistryKey.OpenBaseKey(hive, view))
                using (var key = root.OpenSubKey(subKeyPath, false))
                {
                    if (key == null) return;

                    string hName = HiveName(hive);
                    int subs = key.SubKeyCount;
                    int vals = key.ValueCount;
                    _regResults.Add(new RegResult
                    {
                        Hive = hive, View = view, KeyPath = subKeyPath,
                        ValueName = null, IsTree = true,
                        Col1 = $@"{hName}\{subKeyPath}",
                        Col2 = "(entire tree)",
                        Col3 = $"{subs} subkeys, {vals} values"
                    });
                }
            }
            catch { }
        }

        /// <summary>Scan Uninstall keys for entries whose DisplayName contains Trend Micro.</summary>
        private void ScanUninstallEntries(RegistryHive hive, RegistryView view)
        {
            try
            {
                using (var root = RegistryKey.OpenBaseKey(hive, view))
                using (var parent = root.OpenSubKey(UninstallRegPath, false))
                {
                    if (parent == null) return;
                    string hName = HiveName(hive);

                    foreach (string sub in parent.GetSubKeyNames())
                    {
                        try
                        {
                            using (var entry = parent.OpenSubKey(sub, false))
                            {
                                if (entry == null) continue;
                                string displayName = entry.GetValue("DisplayName") as string ?? "";
                                if (!ContainsAny(displayName, MatchPatterns)) continue;

                                string fullPath = UninstallRegPath + @"\" + sub;

                                // Avoid duplicates (already covered by a full tree)
                                if (_regResults.Exists(r => r.IsTree &&
                                    fullPath.StartsWith(r.KeyPath, StringComparison.OrdinalIgnoreCase)))
                                    continue;

                                _regResults.Add(new RegResult
                                {
                                    Hive = hive, View = view, KeyPath = fullPath,
                                    ValueName = null, IsTree = true,
                                    Col1 = $@"{hName}\{fullPath}",
                                    Col2 = displayName,
                                    Col3 = "Uninstall entry"
                                });
                            }
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }

        /// <summary>Scan individual values whose name or string data matches patterns.</summary>
        private void ScanValuesContaining(RegistryHive hive, RegistryView view,
            string subKeyPath, string[] patterns)
        {
            try
            {
                using (var root = RegistryKey.OpenBaseKey(hive, view))
                using (var key = root.OpenSubKey(subKeyPath, false))
                {
                    if (key == null) return;
                    string hName = HiveName(hive);

                    foreach (string valName in key.GetValueNames())
                    {
                        bool match = ContainsAny(valName, patterns);

                        if (!match)
                        {
                            // Also check string data
                            object data = key.GetValue(valName);
                            if (data is string strData)
                                match = ContainsAny(strData, patterns);
                        }

                        if (!match) continue;

                        _regResults.Add(new RegResult
                        {
                            Hive = hive, View = view, KeyPath = subKeyPath,
                            ValueName = valName, IsTree = false,
                            Col1 = $@"{hName}\{subKeyPath}",
                            Col2 = Truncate(valName, 90),
                            Col3 = "Trend Micro reference"
                        });
                    }
                }
            }
            catch { }
        }

        /// <summary>Scan subkeys of a parent key whose name matches patterns.</summary>
        private void ScanSubkeysMatching(RegistryHive hive, RegistryView view,
            string parentPath, string[] patterns)
        {
            try
            {
                using (var root = RegistryKey.OpenBaseKey(hive, view))
                using (var parent = root.OpenSubKey(parentPath, false))
                {
                    if (parent == null) return;
                    string hName = HiveName(hive);

                    foreach (string sub in parent.GetSubKeyNames())
                    {
                        if (!patterns.Any(p =>
                                sub.IndexOf(p, StringComparison.OrdinalIgnoreCase) >= 0))
                            continue;

                        string fullPath = parentPath + @"\" + sub;
                        _regResults.Add(new RegResult
                        {
                            Hive = hive, View = view, KeyPath = fullPath,
                            ValueName = null, IsTree = true,
                            Col1 = $@"{hName}\{fullPath}",
                            Col2 = "(subkey tree)",
                            Col3 = "Found"
                        });
                    }
                }
            }
            catch { }
        }

        // ══════════════════════════════════════════════════════════════════
        //  SCAN HELPERS — File system
        // ══════════════════════════════════════════════════════════════════

        private void AddFolderIfExists(string path)
        {
            try
            {
                if (!Directory.Exists(path)) return;

                // Avoid adding a path already covered by a parent entry
                if (_fileResults.Exists(f =>
                        path.StartsWith(f.Path + @"\", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(path, f.Path, StringComparison.OrdinalIgnoreCase)))
                    return;

                long kb = GetDirectorySizeKb(path);
                _fileResults.Add(new FileResult
                {
                    Path = path,
                    SizeKb = kb,
                    Desc = $"Folder — {kb:N0} KB"
                });
            }
            catch { }
        }

        // ══════════════════════════════════════════════════════════════════
        //  FORCE DELETE — takeown + icacls + rd /s /q
        //  Used when Directory.Delete fails due to Trend Micro ACL locks.
        // ══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Takes ownership, grants full control, then force-deletes a directory.
        /// Falls back to cmd /c rd /s /q if managed delete still fails.
        /// </summary>
        private static bool ForceDeleteDirectory(string path)
        {
            if (!Directory.Exists(path)) return true;

            try
            {
                // Step 1: Take ownership recursively
                RunCmd("takeown", $"/F \"{path}\" /R /D Y");

                // Step 2: Grant Administrators full control recursively
                RunCmd("icacls", $"\"{path}\" /grant Administrators:F /T /C /Q");

                // Step 3: Try managed delete again
                try
                {
                    Directory.Delete(path, recursive: true);
                    if (!Directory.Exists(path)) return true;
                }
                catch { }

                // Step 4: Final fallback — cmd /c rd /s /q
                RunCmd("cmd.exe", $"/c rd /s /q \"{path}\"");

                return !Directory.Exists(path);
            }
            catch
            {
                return !Directory.Exists(path);
            }
        }

        /// <summary>Runs a system command silently and waits for exit (max 30 s).</summary>
        private static void RunCmd(string fileName, string arguments)
        {
            try
            {
                using (var proc = new Process())
                {
                    proc.StartInfo = new ProcessStartInfo
                    {
                        FileName = fileName,
                        Arguments = arguments,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };
                    proc.Start();
                    proc.WaitForExit(30000);
                }
            }
            catch { }
        }

        // ══════════════════════════════════════════════════════════════════
        //  GENERAL HELPERS
        // ══════════════════════════════════════════════════════════════════

        private static long GetDirectorySizeKb(string path)
        {
            try
            {
                if (!Directory.Exists(path)) return 0L;
                long bytes = 0;
                foreach (string f in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
                {
                    try { bytes += new FileInfo(f).Length; } catch { }
                }
                return bytes / 1024;
            }
            catch { return 0L; }
        }

        private static string HiveName(RegistryHive hive)
        {
            return hive == RegistryHive.LocalMachine ? "HKLM" : "HKCU";
        }

        private static bool ContainsAny(string text, string[] patterns)
        {
            if (string.IsNullOrEmpty(text)) return false;
            return patterns.Any(p => text.IndexOf(p, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private static string Truncate(string s, int max)
        {
            if (s == null) return "";
            return s.Length <= max ? s : s.Substring(0, max) + "…";
        }

        private void SetStatusLabel(string text, Color color)
        {
            lblStatus.Text      = text;
            lblStatus.ForeColor = color;
        }

        private void SetStatusError(string message)
        {
            lblStatus.Text      = message;
            lblStatus.ForeColor = Color.Red;
        }
    }
}
