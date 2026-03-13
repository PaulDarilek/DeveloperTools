namespace SqlReportTools.WinForms
{
    partial class ViewReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnOpenReport = new System.Windows.Forms.Button();
            this.ReportCombo = new System.Windows.Forms.ComboBox();
            this.BtnDataSource = new System.Windows.Forms.Button();
            this.ComboDataSource = new System.Windows.Forms.ComboBox();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.TabText = new System.Windows.Forms.TabPage();
            this.TxtReport = new System.Windows.Forms.TextBox();
            this.TabReports = new System.Windows.Forms.TabPage();
            this.ReportViewer = new Microsoft.Reporting.WinForms.ReportViewer();
            this.TabParms = new System.Windows.Forms.TabPage();
            this.GridView = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.TabSettings = new System.Windows.Forms.TabPage();
            this.LblInputFolder = new System.Windows.Forms.Label();
            this.TxtInputFolder = new System.Windows.Forms.TextBox();
            this.TxtOutputFolder = new System.Windows.Forms.TextBox();
            this.LblOutputFolder = new System.Windows.Forms.Label();
            this.TxtSqlServer = new System.Windows.Forms.TextBox();
            this.LblSqlServer = new System.Windows.Forms.Label();
            this.BtnSaveSettings = new System.Windows.Forms.Button();
            this.Tabs.SuspendLayout();
            this.TabText.SuspendLayout();
            this.TabReports.SuspendLayout();
            this.TabParms.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.TabSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnOpenReport
            // 
            this.BtnOpenReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOpenReport.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BtnOpenReport.Location = new System.Drawing.Point(1003, 12);
            this.BtnOpenReport.Name = "BtnOpenReport";
            this.BtnOpenReport.Size = new System.Drawing.Size(111, 23);
            this.BtnOpenReport.TabIndex = 2;
            this.BtnOpenReport.Text = "Open Report";
            this.BtnOpenReport.UseVisualStyleBackColor = false;
            this.BtnOpenReport.Click += new System.EventHandler(this.BtnOpenReport_Click);
            // 
            // ReportCombo
            // 
            this.ReportCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReportCombo.FormattingEnabled = true;
            this.ReportCombo.Location = new System.Drawing.Point(12, 12);
            this.ReportCombo.Name = "ReportCombo";
            this.ReportCombo.Size = new System.Drawing.Size(984, 21);
            this.ReportCombo.TabIndex = 1;
            this.ReportCombo.SelectedIndexChanged += new System.EventHandler(this.ReportCombo_SelectedIndexChanged);
            // 
            // BtnDataSource
            // 
            this.BtnDataSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDataSource.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BtnDataSource.Location = new System.Drawing.Point(1003, 39);
            this.BtnDataSource.Name = "BtnDataSource";
            this.BtnDataSource.Size = new System.Drawing.Size(111, 23);
            this.BtnDataSource.TabIndex = 5;
            this.BtnDataSource.Text = "Show Reports";
            this.BtnDataSource.UseVisualStyleBackColor = false;
            this.BtnDataSource.Click += new System.EventHandler(this.BtnDataSource_Click);
            // 
            // ComboDataSource
            // 
            this.ComboDataSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ComboDataSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboDataSource.FormattingEnabled = true;
            this.ComboDataSource.Location = new System.Drawing.Point(12, 39);
            this.ComboDataSource.Name = "ComboDataSource";
            this.ComboDataSource.Size = new System.Drawing.Size(984, 21);
            this.ComboDataSource.TabIndex = 4;
            this.ComboDataSource.SelectedIndexChanged += new System.EventHandler(this.ComboDataSource_SelectedIndexChanged);
            // 
            // Tabs
            // 
            this.Tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabs.Controls.Add(this.TabText);
            this.Tabs.Controls.Add(this.TabReports);
            this.Tabs.Controls.Add(this.TabParms);
            this.Tabs.Controls.Add(this.TabSettings);
            this.Tabs.Location = new System.Drawing.Point(5, 70);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(1111, 468);
            this.Tabs.TabIndex = 7;
            // 
            // TabText
            // 
            this.TabText.Controls.Add(this.TxtReport);
            this.TabText.Location = new System.Drawing.Point(4, 22);
            this.TabText.Name = "TabText";
            this.TabText.Size = new System.Drawing.Size(1103, 442);
            this.TabText.TabIndex = 1;
            this.TabText.Text = "Text";
            this.TabText.UseVisualStyleBackColor = true;
            // 
            // TxtReport
            // 
            this.TxtReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtReport.Location = new System.Drawing.Point(3, 13);
            this.TxtReport.Multiline = true;
            this.TxtReport.Name = "TxtReport";
            this.TxtReport.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtReport.Size = new System.Drawing.Size(1097, 426);
            this.TxtReport.TabIndex = 7;
            // 
            // TabReports
            // 
            this.TabReports.Controls.Add(this.ReportViewer);
            this.TabReports.Location = new System.Drawing.Point(4, 22);
            this.TabReports.Name = "TabReports";
            this.TabReports.Size = new System.Drawing.Size(1103, 442);
            this.TabReports.TabIndex = 0;
            this.TabReports.Text = "Report";
            this.TabReports.UseVisualStyleBackColor = true;
            // 
            // ReportViewer
            // 
            this.ReportViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReportViewer.DocumentMapWidth = 1;
            this.ReportViewer.Location = new System.Drawing.Point(3, 5);
            this.ReportViewer.Name = "ReportViewer";
            this.ReportViewer.ServerReport.BearerToken = null;
            this.ReportViewer.Size = new System.Drawing.Size(1088, 369);
            this.ReportViewer.TabIndex = 2;
            // 
            // TabParms
            // 
            this.TabParms.Controls.Add(this.GridView);
            this.TabParms.Location = new System.Drawing.Point(4, 22);
            this.TabParms.Name = "TabParms";
            this.TabParms.Size = new System.Drawing.Size(1103, 442);
            this.TabParms.TabIndex = 2;
            this.TabParms.Text = "Parameters";
            this.TabParms.UseVisualStyleBackColor = true;
            // 
            // GridView
            // 
            this.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridView.Location = new System.Drawing.Point(4, 4);
            this.GridView.Name = "GridView";
            this.GridView.Size = new System.Drawing.Size(1096, 424);
            this.GridView.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 538);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1126, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(156, 17);
            this.StatusLabel.Text = "Select Report or Data Source";
            // 
            // TabSettings
            // 
            this.TabSettings.Controls.Add(this.BtnSaveSettings);
            this.TabSettings.Controls.Add(this.TxtSqlServer);
            this.TabSettings.Controls.Add(this.LblSqlServer);
            this.TabSettings.Controls.Add(this.TxtOutputFolder);
            this.TabSettings.Controls.Add(this.LblOutputFolder);
            this.TabSettings.Controls.Add(this.TxtInputFolder);
            this.TabSettings.Controls.Add(this.LblInputFolder);
            this.TabSettings.Location = new System.Drawing.Point(4, 22);
            this.TabSettings.Name = "TabSettings";
            this.TabSettings.Size = new System.Drawing.Size(1103, 442);
            this.TabSettings.TabIndex = 3;
            this.TabSettings.Text = "Settings";
            this.TabSettings.UseVisualStyleBackColor = true;
            // 
            // LblInputFolder
            // 
            this.LblInputFolder.Location = new System.Drawing.Point(16, 31);
            this.LblInputFolder.Name = "LblInputFolder";
            this.LblInputFolder.Size = new System.Drawing.Size(141, 23);
            this.LblInputFolder.TabIndex = 0;
            this.LblInputFolder.Text = "Input Folder (*.rdl)";
            // 
            // TxtInputFolder
            // 
            this.TxtInputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtInputFolder.Location = new System.Drawing.Point(163, 33);
            this.TxtInputFolder.Name = "TxtInputFolder";
            this.TxtInputFolder.Size = new System.Drawing.Size(906, 20);
            this.TxtInputFolder.TabIndex = 1;
            // 
            // TxtOutputFolder
            // 
            this.TxtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtOutputFolder.Location = new System.Drawing.Point(163, 76);
            this.TxtOutputFolder.Name = "TxtOutputFolder";
            this.TxtOutputFolder.Size = new System.Drawing.Size(906, 20);
            this.TxtOutputFolder.TabIndex = 3;
            // 
            // LblOutputFolder
            // 
            this.LblOutputFolder.Location = new System.Drawing.Point(16, 74);
            this.LblOutputFolder.Name = "LblOutputFolder";
            this.LblOutputFolder.Size = new System.Drawing.Size(141, 23);
            this.LblOutputFolder.TabIndex = 2;
            this.LblOutputFolder.Text = "Output Folder";
            // 
            // TxtSqlServer
            // 
            this.TxtSqlServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtSqlServer.Location = new System.Drawing.Point(163, 125);
            this.TxtSqlServer.Name = "TxtSqlServer";
            this.TxtSqlServer.Size = new System.Drawing.Size(906, 20);
            this.TxtSqlServer.TabIndex = 5;
            // 
            // LblSqlServer
            // 
            this.LblSqlServer.Location = new System.Drawing.Point(16, 123);
            this.LblSqlServer.Name = "LblSqlServer";
            this.LblSqlServer.Size = new System.Drawing.Size(141, 23);
            this.LblSqlServer.TabIndex = 4;
            this.LblSqlServer.Text = "SQL Server (override)";
            // 
            // BtnSave
            // 
            this.BtnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSaveSettings.Location = new System.Drawing.Point(994, 184);
            this.BtnSaveSettings.Name = "BtnSave";
            this.BtnSaveSettings.Size = new System.Drawing.Size(75, 23);
            this.BtnSaveSettings.TabIndex = 6;
            this.BtnSaveSettings.Text = "Save Settings";
            this.BtnSaveSettings.UseVisualStyleBackColor = true;
            this.BtnSaveSettings.Click += new System.EventHandler(this.BtnSaveSettings_Click);
            // 
            // ViewReport
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1126, 560);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.Tabs);
            this.Controls.Add(this.ComboDataSource);
            this.Controls.Add(this.BtnDataSource);
            this.Controls.Add(this.ReportCombo);
            this.Controls.Add(this.BtnOpenReport);
            this.Name = "ViewReport";
            this.Text = "View Reports";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Tabs.ResumeLayout(false);
            this.TabText.ResumeLayout(false);
            this.TabText.PerformLayout();
            this.TabReports.ResumeLayout(false);
            this.TabParms.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.TabSettings.ResumeLayout(false);
            this.TabSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button BtnOpenReport;
        private System.Windows.Forms.ComboBox ReportCombo;
        private System.Windows.Forms.Button BtnDataSource;
        private System.Windows.Forms.ComboBox ComboDataSource;
        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage TabText;
        private System.Windows.Forms.TabPage TabReports;
        private System.Windows.Forms.TextBox TxtReport;
        private Microsoft.Reporting.WinForms.ReportViewer ReportViewer;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.TabPage TabParms;
        private System.Windows.Forms.DataGridView GridView;
        private System.Windows.Forms.TabPage TabSettings;
        private System.Windows.Forms.TextBox TxtOutputFolder;
        private System.Windows.Forms.Label LblOutputFolder;
        private System.Windows.Forms.TextBox TxtInputFolder;
        private System.Windows.Forms.Label LblInputFolder;
        private System.Windows.Forms.Button BtnSaveSettings;
        private System.Windows.Forms.TextBox TxtSqlServer;
        private System.Windows.Forms.Label LblSqlServer;
    }
}