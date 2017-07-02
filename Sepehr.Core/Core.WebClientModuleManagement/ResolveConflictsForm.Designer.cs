namespace Core.WebClientModuleManagement
{
    partial class ResolveConflictsForm
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
            this.lbConflicts = new System.Windows.Forms.ListBox();
            this.btnKeepLocal = new System.Windows.Forms.Button();
            this.btnTakeSource = new System.Windows.Forms.Button();
            this.btnMerge = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbConflicts
            // 
            this.lbConflicts.DisplayMember = "SourcePath";
            this.lbConflicts.FormattingEnabled = true;
            this.lbConflicts.Location = new System.Drawing.Point(12, 50);
            this.lbConflicts.Name = "lbConflicts";
            this.lbConflicts.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbConflicts.Size = new System.Drawing.Size(660, 303);
            this.lbConflicts.TabIndex = 0;
            this.lbConflicts.SelectedIndexChanged += new System.EventHandler(this.lbConflicts_SelectedIndexChanged);
            this.lbConflicts.DoubleClick += new System.EventHandler(this.lbConflicts_DoubleClick);
            // 
            // btnKeepLocal
            // 
            this.btnKeepLocal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(240)))));
            this.btnKeepLocal.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnKeepLocal.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnKeepLocal.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnKeepLocal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeepLocal.Location = new System.Drawing.Point(320, 21);
            this.btnKeepLocal.Name = "btnKeepLocal";
            this.btnKeepLocal.Size = new System.Drawing.Size(121, 23);
            this.btnKeepLocal.TabIndex = 2;
            this.btnKeepLocal.Text = "Keep Local Version";
            this.btnKeepLocal.UseVisualStyleBackColor = false;
            this.btnKeepLocal.Click += new System.EventHandler(this.btnKeepLocal_Click);
            // 
            // btnTakeSource
            // 
            this.btnTakeSource.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(240)))));
            this.btnTakeSource.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnTakeSource.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnTakeSource.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnTakeSource.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTakeSource.Location = new System.Drawing.Point(447, 21);
            this.btnTakeSource.Name = "btnTakeSource";
            this.btnTakeSource.Size = new System.Drawing.Size(129, 23);
            this.btnTakeSource.TabIndex = 2;
            this.btnTakeSource.Text = "Take Source Version";
            this.btnTakeSource.UseVisualStyleBackColor = false;
            this.btnTakeSource.Click += new System.EventHandler(this.btnTakeSource_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(240)))));
            this.btnMerge.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnMerge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnMerge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnMerge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMerge.Location = new System.Drawing.Point(139, 21);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(175, 23);
            this.btnMerge.TabIndex = 2;
            this.btnMerge.Text = "Merge Changes In Merge Tool";
            this.btnMerge.UseVisualStyleBackColor = false;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(240)))));
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Location = new System.Drawing.Point(12, 21);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(121, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Get All Conflicts";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // ResolveConflictsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(684, 359);
            this.Controls.Add(this.btnTakeSource);
            this.Controls.Add(this.btnMerge);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnKeepLocal);
            this.Controls.Add(this.lbConflicts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ResolveConflictsForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Resolve Conflicts";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbConflicts;
        private System.Windows.Forms.Button btnKeepLocal;
        private System.Windows.Forms.Button btnTakeSource;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Button btnRefresh;
    }
}