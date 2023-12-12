namespace SystemTextManager.Launcher
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressNow = new DevExpress.XtraEditors.ProgressBarControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.lbPercent = new DevExpress.XtraEditors.LabelControl();
            this.lbNow = new DevExpress.XtraEditors.LabelControl();
            this.progressTotal = new DevExpress.XtraEditors.ProgressBarControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressNow.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressTotal.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.progressNow);
            this.panel1.Controls.Add(this.labelControl4);
            this.panel1.Controls.Add(this.pictureEdit1);
            this.panel1.Controls.Add(this.lbPercent);
            this.panel1.Controls.Add(this.lbNow);
            this.panel1.Controls.Add(this.progressTotal);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(593, 249);
            this.panel1.TabIndex = 1;
            this.panel1.TabStop = true;
            // 
            // progressNow
            // 
            this.progressNow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressNow.Location = new System.Drawing.Point(28, 188);
            this.progressNow.Name = "progressNow";
            this.progressNow.Size = new System.Drawing.Size(535, 14);
            this.progressNow.TabIndex = 6;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("굴림", 13F, System.Drawing.FontStyle.Bold);
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl4.Location = new System.Drawing.Point(40, 89);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(279, 30);
            this.labelControl4.TabIndex = 5;
            this.labelControl4.Text = "System Text Manager";
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.EditValue = global::SystemTextManager.Launcher.Properties.Resources.logo_v21;
            this.pictureEdit1.Location = new System.Drawing.Point(28, 15);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.AllowFocused = false;
            this.pictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Properties.ShowMenu = false;
            this.pictureEdit1.Size = new System.Drawing.Size(291, 101);
            this.pictureEdit1.TabIndex = 4;
            // 
            // lbPercent
            // 
            this.lbPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPercent.Appearance.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbPercent.Appearance.Options.UseFont = true;
            this.lbPercent.Appearance.Options.UseTextOptions = true;
            this.lbPercent.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lbPercent.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbPercent.Location = new System.Drawing.Point(504, 120);
            this.lbPercent.Name = "lbPercent";
            this.lbPercent.Size = new System.Drawing.Size(59, 39);
            this.lbPercent.TabIndex = 3;
            // 
            // lbNow
            // 
            this.lbNow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbNow.Appearance.Font = new System.Drawing.Font("굴림", 9F);
            this.lbNow.Appearance.Options.UseFont = true;
            this.lbNow.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbNow.Location = new System.Drawing.Point(28, 162);
            this.lbNow.Name = "lbNow";
            this.lbNow.Size = new System.Drawing.Size(535, 22);
            this.lbNow.TabIndex = 1;
            // 
            // progressTotal
            // 
            this.progressTotal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressTotal.Location = new System.Drawing.Point(28, 205);
            this.progressTotal.Margin = new System.Windows.Forms.Padding(20);
            this.progressTotal.Name = "progressTotal";
            this.progressTotal.Size = new System.Drawing.Size(535, 14);
            this.progressTotal.TabIndex = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 249);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("frmMain.IconOptions.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SSM Launcher";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.progressNow.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressTotal.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.ProgressBarControl progressTotal;
        private DevExpress.XtraEditors.LabelControl lbPercent;
        private DevExpress.XtraEditors.LabelControl lbNow;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ProgressBarControl progressNow;
    }
}

