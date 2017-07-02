namespace Core.TraceViewer
{
    partial class Form1
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
            this.btnCreateEventLog = new System.Windows.Forms.Button();
            this.btnUnInstallEventLog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCreateEventLog
            // 
            this.btnCreateEventLog.Location = new System.Drawing.Point(103, 36);
            this.btnCreateEventLog.Name = "btnCreateEventLog";
            this.btnCreateEventLog.Size = new System.Drawing.Size(144, 23);
            this.btnCreateEventLog.TabIndex = 0;
            this.btnCreateEventLog.Text = "Install Event Log";
            this.btnCreateEventLog.UseVisualStyleBackColor = true;
            this.btnCreateEventLog.Click += new System.EventHandler(this.btnCreateEventLog_Click);
            // 
            // btnUnInstallEventLog
            // 
            this.btnUnInstallEventLog.Location = new System.Drawing.Point(103, 79);
            this.btnUnInstallEventLog.Name = "btnUnInstallEventLog";
            this.btnUnInstallEventLog.Size = new System.Drawing.Size(144, 23);
            this.btnUnInstallEventLog.TabIndex = 1;
            this.btnUnInstallEventLog.Text = "Uninstall EventLog";
            this.btnUnInstallEventLog.UseVisualStyleBackColor = true;
            this.btnUnInstallEventLog.Click += new System.EventHandler(this.btnUnInstallEventLog_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 131);
            this.Controls.Add(this.btnUnInstallEventLog);
            this.Controls.Add(this.btnCreateEventLog);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCreateEventLog;
        private System.Windows.Forms.Button btnUnInstallEventLog;
    }
}

