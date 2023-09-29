namespace CSTool
{
    partial class frmCamSettings
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
            this.cmbSource = new System.Windows.Forms.ComboBox();
            this.lblCameraSource = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.pbVideoHandler = new System.Windows.Forms.PictureBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideoHandler)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbSource
            // 
            this.cmbSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSource.FormattingEnabled = true;
            this.cmbSource.Location = new System.Drawing.Point(132, 25);
            this.cmbSource.Name = "cmbSource";
            this.cmbSource.Size = new System.Drawing.Size(234, 26);
            this.cmbSource.TabIndex = 0;
            this.cmbSource.SelectedIndexChanged += new System.EventHandler(this.cmbSource_SelectedIndexChanged);
            this.cmbSource.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbSource_KeyDown);
            // 
            // lblCameraSource
            // 
            this.lblCameraSource.AutoSize = true;
            this.lblCameraSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCameraSource.Location = new System.Drawing.Point(12, 30);
            this.lblCameraSource.Name = "lblCameraSource";
            this.lblCameraSource.Size = new System.Drawing.Size(114, 16);
            this.lblCameraSource.TabIndex = 1;
            this.lblCameraSource.Text = "Camera Source";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Preview";
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(169)))), ((int)(((byte)(23)))));
            this.btnSaveSettings.FlatAppearance.BorderSize = 0;
            this.btnSaveSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnSaveSettings.ForeColor = System.Drawing.Color.White;
            this.btnSaveSettings.Location = new System.Drawing.Point(132, 244);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(326, 40);
            this.btnSaveSettings.TabIndex = 15;
            this.btnSaveSettings.Text = "SET";
            this.btnSaveSettings.UseVisualStyleBackColor = false;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // pbVideoHandler
            // 
            this.pbVideoHandler.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbVideoHandler.Location = new System.Drawing.Point(132, 57);
            this.pbVideoHandler.Name = "pbVideoHandler";
            this.pbVideoHandler.Size = new System.Drawing.Size(326, 181);
            this.pbVideoHandler.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbVideoHandler.TabIndex = 3;
            this.pbVideoHandler.TabStop = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(372, 25);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(86, 27);
            this.btnRefresh.TabIndex = 16;
            this.btnRefresh.Text = "Load Source";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // frmCamSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(473, 305);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbVideoHandler);
            this.Controls.Add(this.lblCameraSource);
            this.Controls.Add(this.cmbSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmCamSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Camera Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCamSettings_FormClosing);
            this.Load += new System.EventHandler(this.frmCamSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbVideoHandler)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbSource;
        private System.Windows.Forms.Label lblCameraSource;
        private System.Windows.Forms.PictureBox pbVideoHandler;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnRefresh;
    }
}