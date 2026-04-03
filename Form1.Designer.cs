namespace frmtrendunins
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.topPanel = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.containerPanel = new System.Windows.Forms.Panel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.gbFiles = new System.Windows.Forms.GroupBox();
            this.dgvFiles = new System.Windows.Forms.DataGridView();
            this.colFilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFileDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbRegistry = new System.Windows.Forms.GroupBox();
            this.dgvRegistry = new System.Windows.Forms.DataGridView();
            this.colKeyPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValueName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValueData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bottomActionPanel = new System.Windows.Forms.Panel();
            this.btnDonate = new System.Windows.Forms.Button();
            this.btnClean = new System.Windows.Forms.Button();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.gbSystem = new System.Windows.Forms.GroupBox();
            this.lblArchitecture = new System.Windows.Forms.Label();
            this.lblOS = new System.Windows.Forms.Label();
            this.gbActions = new System.Windows.Forms.GroupBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnUninstall = new System.Windows.Forms.Button();
            this.gbDetection = new System.Windows.Forms.GroupBox();
            this.lblRegistryInfo = new System.Windows.Forms.Label();
            this.picWarning = new System.Windows.Forms.PictureBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.topPanel.SuspendLayout();
            this.containerPanel.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.gbFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).BeginInit();
            this.gbRegistry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegistry)).BeginInit();
            this.bottomActionPanel.SuspendLayout();
            this.leftPanel.SuspendLayout();
            this.gbSystem.SuspendLayout();
            this.gbActions.SuspendLayout();
            this.gbDetection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWarning)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.topPanel.Controls.Add(this.lblTitle);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(960, 56);
            this.topPanel.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "TRENDMICRO UNINSTALLER SUPPORT TOOL";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // containerPanel
            // 
            this.containerPanel.Controls.Add(this.mainPanel);
            this.containerPanel.Controls.Add(this.leftPanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Name = "containerPanel";
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.mainPanel.Controls.Add(this.gbFiles);
            this.mainPanel.Controls.Add(this.gbRegistry);
            this.mainPanel.Controls.Add(this.bottomActionPanel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(12, 12, 12, 0);
            // 
            // gbFiles
            // 
            this.gbFiles.Controls.Add(this.dgvFiles);
            this.gbFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFiles.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.gbFiles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.gbFiles.Name = "gbFiles";
            this.gbFiles.Padding = new System.Windows.Forms.Padding(6);
            this.gbFiles.TabStop = false;
            this.gbFiles.Text = "Residual Files and Folders";
            // 
            // dgvFiles
            // 
            this.dgvFiles.AllowUserToAddRows = false;
            this.dgvFiles.AllowUserToDeleteRows = false;
            this.dgvFiles.BackgroundColor = System.Drawing.Color.White;
            this.dgvFiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvFiles.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvFiles.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFilePath, this.colFileSize, this.colFileDesc});
            this.dgvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFiles.EnableHeadersVisualStyles = false;
            this.dgvFiles.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvFiles.Name = "dgvFiles";
            this.dgvFiles.ReadOnly = true;
            this.dgvFiles.RowHeadersVisible = false;
            this.dgvFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // colFilePath
            // 
            this.colFilePath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFilePath.FillWeight = 60F;
            this.colFilePath.HeaderText = "File Path";
            this.colFilePath.Name = "colFilePath";
            this.colFilePath.ReadOnly = true;
            // 
            // colFileSize
            // 
            this.colFileSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFileSize.FillWeight = 15F;
            this.colFileSize.HeaderText = "Size (KB)";
            this.colFileSize.Name = "colFileSize";
            this.colFileSize.ReadOnly = true;
            // 
            // colFileDesc
            // 
            this.colFileDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFileDesc.FillWeight = 25F;
            this.colFileDesc.HeaderText = "Description";
            this.colFileDesc.Name = "colFileDesc";
            this.colFileDesc.ReadOnly = true;
            // 
            // gbRegistry
            // 
            this.gbRegistry.Controls.Add(this.dgvRegistry);
            this.gbRegistry.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbRegistry.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.gbRegistry.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.gbRegistry.Name = "gbRegistry";
            this.gbRegistry.Padding = new System.Windows.Forms.Padding(6);
            this.gbRegistry.Size = new System.Drawing.Size(520, 195);
            this.gbRegistry.TabStop = false;
            this.gbRegistry.Text = "Residual Registry Keys";
            // 
            // dgvRegistry
            // 
            this.dgvRegistry.AllowUserToAddRows = false;
            this.dgvRegistry.AllowUserToDeleteRows = false;
            this.dgvRegistry.BackgroundColor = System.Drawing.Color.White;
            this.dgvRegistry.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRegistry.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvRegistry.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvRegistry.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRegistry.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colKeyPath, this.colValueName, this.colValueData});
            this.dgvRegistry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRegistry.EnableHeadersVisualStyles = false;
            this.dgvRegistry.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvRegistry.Name = "dgvRegistry";
            this.dgvRegistry.ReadOnly = true;
            this.dgvRegistry.RowHeadersVisible = false;
            this.dgvRegistry.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // colKeyPath
            // 
            this.colKeyPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colKeyPath.FillWeight = 50F;
            this.colKeyPath.HeaderText = "Key Path";
            this.colKeyPath.Name = "colKeyPath";
            this.colKeyPath.ReadOnly = true;
            // 
            // colValueName
            // 
            this.colValueName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colValueName.FillWeight = 25F;
            this.colValueName.HeaderText = "Value Name";
            this.colValueName.Name = "colValueName";
            this.colValueName.ReadOnly = true;
            // 
            // colValueData
            // 
            this.colValueData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colValueData.FillWeight = 25F;
            this.colValueData.HeaderText = "Data";
            this.colValueData.Name = "colValueData";
            this.colValueData.ReadOnly = true;
            // 
            // bottomActionPanel
            // 
            this.bottomActionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.bottomActionPanel.Controls.Add(this.btnDonate);
            this.bottomActionPanel.Controls.Add(this.btnClean);
            this.bottomActionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomActionPanel.Name = "bottomActionPanel";
            this.bottomActionPanel.Padding = new System.Windows.Forms.Padding(12);
            this.bottomActionPanel.Size = new System.Drawing.Size(520, 68);
            // 
            // btnClean
            // 
            this.btnClean.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(139)))), ((int)(((byte)(34)))));
            this.btnClean.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnClean.Enabled = false;
            this.btnClean.FlatAppearance.BorderSize = 0;
            this.btnClean.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClean.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClean.ForeColor = System.Drawing.Color.White;
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(220, 44);
            this.btnClean.Text = "CLEAN ALL";
            this.btnClean.UseVisualStyleBackColor = false;
            // 
            // btnDonate
            // 
            this.btnDonate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.btnDonate.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDonate.FlatAppearance.BorderSize = 0;
            this.btnDonate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDonate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDonate.ForeColor = System.Drawing.Color.White;
            this.btnDonate.Name = "btnDonate";
            this.btnDonate.Size = new System.Drawing.Size(220, 44);
            this.btnDonate.Text = "SUPPORT THE TOOL";
            this.btnDonate.UseVisualStyleBackColor = false;
            // 
            // leftPanel
            // 
            this.leftPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(248)))));
            this.leftPanel.Controls.Add(this.gbSystem);
            this.leftPanel.Controls.Add(this.gbActions);
            this.leftPanel.Controls.Add(this.gbDetection);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Padding = new System.Windows.Forms.Padding(12);
            this.leftPanel.Size = new System.Drawing.Size(340, 500);
            // 
            // gbDetection
            // 
            this.gbDetection.Controls.Add(this.lblRegistryInfo);
            this.gbDetection.Controls.Add(this.picWarning);
            this.gbDetection.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbDetection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.gbDetection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.gbDetection.Name = "gbDetection";
            this.gbDetection.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.gbDetection.Size = new System.Drawing.Size(316, 165);
            this.gbDetection.TabStop = false;
            this.gbDetection.Text = "Detection Status";
            // 
            // picWarning
            // 
            this.picWarning.Location = new System.Drawing.Point(14, 28);
            this.picWarning.Name = "picWarning";
            this.picWarning.Size = new System.Drawing.Size(36, 36);
            this.picWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picWarning.TabStop = false;
            // 
            // lblRegistryInfo
            // 
            this.lblRegistryInfo.AutoSize = true;
            this.lblRegistryInfo.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblRegistryInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblRegistryInfo.Location = new System.Drawing.Point(56, 26);
            this.lblRegistryInfo.MaximumSize = new System.Drawing.Size(246, 0);
            this.lblRegistryInfo.Name = "lblRegistryInfo";
            this.lblRegistryInfo.Text = "Scan registry to check installation status...";
            // 
            // gbActions
            // 
            this.gbActions.Controls.Add(this.btnScan);
            this.gbActions.Controls.Add(this.btnUninstall);
            this.gbActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbActions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.gbActions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.gbActions.Name = "gbActions";
            this.gbActions.Padding = new System.Windows.Forms.Padding(10, 8, 10, 10);
            this.gbActions.Size = new System.Drawing.Size(316, 150);
            this.gbActions.TabStop = false;
            this.gbActions.Text = "Actions";
            // 
            // btnUninstall
            // 
            this.btnUninstall.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(195)))));
            this.btnUninstall.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUninstall.Enabled = false;
            this.btnUninstall.FlatAppearance.BorderSize = 0;
            this.btnUninstall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUninstall.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnUninstall.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(296, 48);
            this.btnUninstall.Text = "Remove Trend Micro";
            this.btnUninstall.UseVisualStyleBackColor = false;
            // 
            // btnScan
            // 
            this.btnScan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnScan.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnScan.Enabled = false;
            this.btnScan.FlatAppearance.BorderSize = 0;
            this.btnScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScan.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnScan.ForeColor = System.Drawing.Color.White;
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(296, 48);
            this.btnScan.Text = "Scan for Residual Files && Registry";
            this.btnScan.UseVisualStyleBackColor = false;
            // 
            // gbSystem
            // 
            this.gbSystem.Controls.Add(this.lblArchitecture);
            this.gbSystem.Controls.Add(this.lblOS);
            this.gbSystem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSystem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.gbSystem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.gbSystem.Name = "gbSystem";
            this.gbSystem.Padding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.gbSystem.Size = new System.Drawing.Size(316, 70);
            this.gbSystem.TabStop = false;
            this.gbSystem.Text = "System Information";
            // 
            // lblOS
            // 
            this.lblOS.AutoSize = true;
            this.lblOS.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOS.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblOS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblOS.Name = "lblOS";
            this.lblOS.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblOS.Size = new System.Drawing.Size(100, 19);
            this.lblOS.Text = "Windows Version:";
            // 
            // lblArchitecture
            // 
            this.lblArchitecture.AutoSize = true;
            this.lblArchitecture.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblArchitecture.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblArchitecture.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblArchitecture.Name = "lblArchitecture";
            this.lblArchitecture.Size = new System.Drawing.Size(100, 15);
            this.lblArchitecture.Text = "System Type:";
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel, this.progressBar});
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(960, 22);
            this.statusStrip.SizingGrip = false;
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Ready";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(200, 16);
            this.progressBar.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(960, 580);
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TrendMicro Uninstaller Support Tool";
            this.topPanel.ResumeLayout(false);
            this.containerPanel.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.gbFiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).EndInit();
            this.gbRegistry.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegistry)).EndInit();
            this.bottomActionPanel.ResumeLayout(false);
            this.leftPanel.ResumeLayout(false);
            this.gbSystem.ResumeLayout(false);
            this.gbSystem.PerformLayout();
            this.gbActions.ResumeLayout(false);
            this.gbDetection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picWarning)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel containerPanel;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.GroupBox gbFiles;
        private System.Windows.Forms.DataGridView dgvFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFilePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileDesc;
        private System.Windows.Forms.GroupBox gbRegistry;
        private System.Windows.Forms.DataGridView dgvRegistry;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKeyPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValueName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValueData;
        private System.Windows.Forms.Panel bottomActionPanel;
        private System.Windows.Forms.Button btnDonate;
        private System.Windows.Forms.Button btnClean;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.GroupBox gbDetection;
        private System.Windows.Forms.Label lblRegistryInfo;
        private System.Windows.Forms.PictureBox picWarning;
        private System.Windows.Forms.GroupBox gbActions;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnUninstall;
        private System.Windows.Forms.GroupBox gbSystem;
        private System.Windows.Forms.Label lblOS;
        private System.Windows.Forms.Label lblArchitecture;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
    }
}
