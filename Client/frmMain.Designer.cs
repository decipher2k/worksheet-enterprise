namespace WorksheetLog
{
    partial class frmMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.eventLog1 = new System.Diagnostics.EventLog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbLogBreaks = new System.Windows.Forms.CheckBox();
            this.tbWiFi = new System.Windows.Forms.TextBox();
            this.rbWiFi = new System.Windows.Forms.RadioButton();
            this.rbProgramm = new System.Windows.Forms.RadioButton();
            this.bnExport = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // eventLog1
            // 
            this.eventLog1.SynchronizingObject = this;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbLogBreaks);
            this.groupBox1.Controls.Add(this.tbWiFi);
            this.groupBox1.Controls.Add(this.rbWiFi);
            this.groupBox1.Controls.Add(this.rbProgramm);
            this.groupBox1.Location = new System.Drawing.Point(9, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(212, 118);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "When to log";
            // 
            // cbLogBreaks
            // 
            this.cbLogBreaks.AutoSize = true;
            this.cbLogBreaks.Checked = true;
            this.cbLogBreaks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLogBreaks.Location = new System.Drawing.Point(6, 86);
            this.cbLogBreaks.Name = "cbLogBreaks";
            this.cbLogBreaks.Size = new System.Drawing.Size(167, 17);
            this.cbLogBreaks.TabIndex = 3;
            this.cbLogBreaks.Text = "Log breaks if screen is locked";
            this.cbLogBreaks.UseVisualStyleBackColor = true;
            // 
            // tbWiFi
            // 
            this.tbWiFi.Location = new System.Drawing.Point(22, 61);
            this.tbWiFi.Margin = new System.Windows.Forms.Padding(2);
            this.tbWiFi.Name = "tbWiFi";
            this.tbWiFi.Size = new System.Drawing.Size(167, 20);
            this.tbWiFi.TabIndex = 2;
            this.tbWiFi.TextChanged += new System.EventHandler(this.tbWiFi_TextChanged);
            // 
            // rbWiFi
            // 
            this.rbWiFi.AutoSize = true;
            this.rbWiFi.Location = new System.Drawing.Point(4, 39);
            this.rbWiFi.Margin = new System.Windows.Forms.Padding(2);
            this.rbWiFi.Name = "rbWiFi";
            this.rbWiFi.Size = new System.Drawing.Size(150, 17);
            this.rbWiFi.TabIndex = 1;
            this.rbWiFi.Text = "Connected to WiFi (SSID):";
            this.rbWiFi.UseVisualStyleBackColor = true;
            this.rbWiFi.CheckedChanged += new System.EventHandler(this.rbWiFi_CheckedChanged);
            // 
            // rbProgramm
            // 
            this.rbProgramm.AutoSize = true;
            this.rbProgramm.Checked = true;
            this.rbProgramm.Location = new System.Drawing.Point(4, 17);
            this.rbProgramm.Margin = new System.Windows.Forms.Padding(2);
            this.rbProgramm.Name = "rbProgramm";
            this.rbProgramm.Size = new System.Drawing.Size(112, 17);
            this.rbProgramm.TabIndex = 0;
            this.rbProgramm.TabStop = true;
            this.rbProgramm.Text = "Program is running";
            this.rbProgramm.UseVisualStyleBackColor = true;
            // 
            // bnExport
            // 
            this.bnExport.Location = new System.Drawing.Point(4, 78);
            this.bnExport.Margin = new System.Windows.Forms.Padding(2);
            this.bnExport.Name = "bnExport";
            this.bnExport.Size = new System.Drawing.Size(200, 19);
            this.bnExport.TabIndex = 1;
            this.bnExport.Text = "Export";
            this.bnExport.UseVisualStyleBackColor = true;
            this.bnExport.Click += new System.EventHandler(this.bnExport_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dateTimePicker2);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.bnExport);
            this.groupBox2.Controls.Add(this.dateTimePicker1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(8, 154);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(212, 110);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Export";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(37, 51);
            this.dateTimePicker2.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(168, 20);
            this.dateTimePicker2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 51);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "To:";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(37, 21);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(168, 20);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "From:";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 277);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "Time Sheet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Diagnostics.EventLog eventLog1;
        private System.Windows.Forms.Button bnExport;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbWiFi;
        private System.Windows.Forms.RadioButton rbWiFi;
        private System.Windows.Forms.RadioButton rbProgramm;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbLogBreaks;
    }
}

