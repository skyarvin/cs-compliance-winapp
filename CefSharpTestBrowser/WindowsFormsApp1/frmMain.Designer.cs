namespace WindowsFormsApp1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.pnlURL = new System.Windows.Forms.Panel();
            this.cmbURL = new SkydevCSTool.CustomComboBox();
            this.pnlAction = new System.Windows.Forms.Panel();
            this.lblApproveCount = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.pnlSplitter5 = new System.Windows.Forms.Panel();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.btnClear = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblCountdown = new System.Windows.Forms.Label();
            this.pnlSplitter3 = new System.Windows.Forms.Panel();
            this.btnConnect = new System.Windows.Forms.Button();
            this.pnlSplitter2 = new System.Windows.Forms.Panel();
            this.pnlUser = new System.Windows.Forms.Panel();
            this.lblProfile = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.pbImg = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.switchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlLoader = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnKb = new System.Windows.Forms.Button();
            this.pnlSplitter = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.pnlBrowser = new System.Windows.Forms.Panel();
            this.bgWorkResync = new System.ComponentModel.BackgroundWorker();
            this.updateWorkactivity = new System.Windows.Forms.Timer(this.components);
            this.pnlHeader.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlURL.SuspendLayout();
            this.pnlAction.SuspendLayout();
            this.pnlUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImg)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.pnlLoader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.pnlHeader.Controls.Add(this.pnlSearch);
            this.pnlHeader.Controls.Add(this.pnlSplitter);
            this.pnlHeader.Controls.Add(this.panel1);
            this.pnlHeader.Controls.Add(this.btnRefresh);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1128, 40);
            this.pnlHeader.TabIndex = 2;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(95)))), ((int)(((byte)(167)))));
            this.pnlSearch.Controls.Add(this.pnlURL);
            this.pnlSearch.Controls.Add(this.pnlAction);
            this.pnlSearch.Controls.Add(this.panel4);
            this.pnlSearch.Controls.Add(this.lblCountdown);
            this.pnlSearch.Controls.Add(this.pnlSplitter3);
            this.pnlSearch.Controls.Add(this.btnConnect);
            this.pnlSearch.Controls.Add(this.pnlSplitter2);
            this.pnlSearch.Controls.Add(this.pnlLoader);
            this.pnlSearch.Controls.Add(this.btnKb);
            this.pnlSearch.Controls.Add(this.pnlUser);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(180, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(948, 40);
            this.pnlSearch.TabIndex = 9;
            // 
            // pnlURL
            // 
            this.pnlURL.BackColor = System.Drawing.Color.Transparent;
            this.pnlURL.Controls.Add(this.cmbURL);
            this.pnlURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlURL.Location = new System.Drawing.Point(35, 0);
            this.pnlURL.Name = "pnlURL";
            this.pnlURL.Size = new System.Drawing.Size(244, 40);
            this.pnlURL.TabIndex = 14;
            // 
            // cmbURL
            // 
            this.cmbURL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbURL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(95)))), ((int)(((byte)(167)))));
            this.cmbURL.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(95)))), ((int)(((byte)(167)))));
            this.cmbURL.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cmbURL.ForeColor = System.Drawing.Color.White;
            this.cmbURL.FormattingEnabled = true;
            this.cmbURL.Location = new System.Drawing.Point(6, 7);
            this.cmbURL.Name = "cmbURL";
            this.cmbURL.Size = new System.Drawing.Size(232, 25);
            this.cmbURL.TabIndex = 8;
            this.cmbURL.DropDown += new System.EventHandler(this.CmbURL_DropDown);
            this.cmbURL.SelectedIndexChanged += new System.EventHandler(this.CmbURL_SelectedIndexChanged_1);
            this.cmbURL.Click += new System.EventHandler(this.CmbURL_Click_1);
            this.cmbURL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CmbURL_KeyPress);
            this.cmbURL.Resize += new System.EventHandler(this.CmbURL_Resize);
            // 
            // pnlAction
            // 
            this.pnlAction.Controls.Add(this.lblApproveCount);
            this.pnlAction.Controls.Add(this.lblProgress);
            this.pnlAction.Controls.Add(this.pnlSplitter5);
            this.pnlAction.Controls.Add(this.pbProgress);
            this.pnlAction.Controls.Add(this.btnClear);
            this.pnlAction.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlAction.Location = new System.Drawing.Point(279, 0);
            this.pnlAction.Name = "pnlAction";
            this.pnlAction.Size = new System.Drawing.Size(197, 40);
            this.pnlAction.TabIndex = 10;
            this.pnlAction.Visible = false;
            // 
            // lblApproveCount
            // 
            this.lblApproveCount.AutoSize = true;
            this.lblApproveCount.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblApproveCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApproveCount.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblApproveCount.Location = new System.Drawing.Point(143, 11);
            this.lblApproveCount.Name = "lblApproveCount";
            this.lblApproveCount.Size = new System.Drawing.Size(34, 20);
            this.lblApproveCount.TabIndex = 0;
            this.lblApproveCount.Text = "0/1";
            this.lblApproveCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.BackColor = System.Drawing.Color.White;
            this.lblProgress.Location = new System.Drawing.Point(52, 24);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 13);
            this.lblProgress.TabIndex = 16;
            // 
            // pnlSplitter5
            // 
            this.pnlSplitter5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.pnlSplitter5.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSplitter5.Location = new System.Drawing.Point(120, 0);
            this.pnlSplitter5.Name = "pnlSplitter5";
            this.pnlSplitter5.Size = new System.Drawing.Size(1, 40);
            this.pnlSplitter5.TabIndex = 13;
            // 
            // pbProgress
            // 
            this.pbProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbProgress.Location = new System.Drawing.Point(120, 0);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(77, 40);
            this.pbProgress.TabIndex = 0;
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(169)))), ((int)(((byte)(23)))));
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(0, 0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 40);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "APPROVE";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(476, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1, 40);
            this.panel4.TabIndex = 13;
            // 
            // lblCountdown
            // 
            this.lblCountdown.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblCountdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountdown.ForeColor = System.Drawing.Color.White;
            this.lblCountdown.Location = new System.Drawing.Point(477, 0);
            this.lblCountdown.Name = "lblCountdown";
            this.lblCountdown.Size = new System.Drawing.Size(55, 40);
            this.lblCountdown.TabIndex = 9;
            this.lblCountdown.Text = "0";
            this.lblCountdown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlSplitter3
            // 
            this.pnlSplitter3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.pnlSplitter3.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSplitter3.Location = new System.Drawing.Point(532, 0);
            this.pnlSplitter3.Name = "pnlSplitter3";
            this.pnlSplitter3.Size = new System.Drawing.Size(1, 40);
            this.pnlSplitter3.TabIndex = 12;
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.Transparent;
            this.btnConnect.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnConnect.FlatAppearance.BorderSize = 0;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.Location = new System.Drawing.Point(533, 0);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(115, 40);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "CONNECT";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // pnlSplitter2
            // 
            this.pnlSplitter2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.pnlSplitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSplitter2.Location = new System.Drawing.Point(648, 0);
            this.pnlSplitter2.Name = "pnlSplitter2";
            this.pnlSplitter2.Size = new System.Drawing.Size(1, 40);
            this.pnlSplitter2.TabIndex = 11;
            // 
            // pnlUser
            // 
            this.pnlUser.Controls.Add(this.lblProfile);
            this.pnlUser.Controls.Add(this.lblUser);
            this.pnlUser.Controls.Add(this.pbImg);
            this.pnlUser.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlUser.Location = new System.Drawing.Point(698, 0);
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Size = new System.Drawing.Size(250, 40);
            this.pnlUser.TabIndex = 0;
            // 
            // lblProfile
            // 
            this.lblProfile.BackColor = System.Drawing.Color.Transparent;
            this.lblProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProfile.Font = new System.Drawing.Font("Segoe UI Light", 9.25F);
            this.lblProfile.ForeColor = System.Drawing.Color.MediumSpringGreen;
            this.lblProfile.Location = new System.Drawing.Point(0, 22);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(205, 18);
            this.lblProfile.TabIndex = 9;
            this.lblProfile.Text = "Profile: Enrique";
            this.lblProfile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUser
            // 
            this.lblUser.BackColor = System.Drawing.Color.Transparent;
            this.lblUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.ForeColor = System.Drawing.Color.White;
            this.lblUser.Location = new System.Drawing.Point(0, 0);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(205, 22);
            this.lblUser.TabIndex = 8;
            this.lblUser.Text = "Enrique";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbImg
            // 
            this.pbImg.ContextMenuStrip = this.contextMenuStrip1;
            this.pbImg.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbImg.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pbImg.ErrorImage")));
            this.pbImg.Image = ((System.Drawing.Image)(resources.GetObject("pbImg.Image")));
            this.pbImg.InitialImage = ((System.Drawing.Image)(resources.GetObject("pbImg.InitialImage")));
            this.pbImg.Location = new System.Drawing.Point(205, 0);
            this.pbImg.Name = "pbImg";
            this.pbImg.Size = new System.Drawing.Size(45, 40);
            this.pbImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbImg.TabIndex = 7;
            this.pbImg.TabStop = false;
            this.pbImg.Click += new System.EventHandler(this.PbImg_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.switchToolStripMenuItem,
            this.logoutToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(113, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip1_Opening);
            this.contextMenuStrip1.Opened += new System.EventHandler(this.ContextMenuStrip1_Opened);
            // 
            // switchToolStripMenuItem
            // 
            this.switchToolStripMenuItem.Name = "switchToolStripMenuItem";
            this.switchToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.switchToolStripMenuItem.Text = "Switch";
            this.switchToolStripMenuItem.Visible = false;
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // pnlLoader
            // 
            this.pnlLoader.BackColor = System.Drawing.Color.Transparent;
            this.pnlLoader.Controls.Add(this.pictureBox1);
            this.pnlLoader.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLoader.Location = new System.Drawing.Point(0, 0);
            this.pnlLoader.Name = "pnlLoader";
            this.pnlLoader.Size = new System.Drawing.Size(35, 40);
            this.pnlLoader.TabIndex = 15;
            this.pnlLoader.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SkydevCSTool.Properties.Resources.loader;
            this.pictureBox1.Location = new System.Drawing.Point(5, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 25);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // btnKb
            // 
            this.btnKb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.btnKb.BackgroundImage = global::SkydevCSTool.Properties.Resources.kb_icon;
            this.btnKb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnKb.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnKb.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.btnKb.FlatAppearance.BorderSize = 0;
            this.btnKb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKb.ImageKey = "(none)";
            this.btnKb.Location = new System.Drawing.Point(649, 0);
            this.btnKb.Name = "btnKb";
            this.btnKb.Size = new System.Drawing.Size(49, 40);
            this.btnKb.TabIndex = 16;
            this.btnKb.UseVisualStyleBackColor = false;
            this.btnKb.Click += new System.EventHandler(this.btnKb_Click);
            // 
            // pnlSplitter
            // 
            this.pnlSplitter.BackColor = System.Drawing.Color.White;
            this.pnlSplitter.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSplitter.Location = new System.Drawing.Point(179, 0);
            this.pnlSplitter.Name = "pnlSplitter";
            this.pnlSplitter.Size = new System.Drawing.Size(1, 40);
            this.pnlSplitter.TabIndex = 8;
            this.pnlSplitter.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(49, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(130, 40);
            this.panel1.TabIndex = 7;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(5, 8);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(121, 27);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.TxtSearch_TextChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackgroundImage = global::SkydevCSTool.Properties.Resources.home;
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(137)))), ((int)(((byte)(239)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ImageKey = "(none)";
            this.btnRefresh.Location = new System.Drawing.Point(0, 0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(49, 40);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // pnlBrowser
            // 
            this.pnlBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBrowser.Location = new System.Drawing.Point(0, 40);
            this.pnlBrowser.Name = "pnlBrowser";
            this.pnlBrowser.Size = new System.Drawing.Size(1128, 477);
            this.pnlBrowser.TabIndex = 2;
            // 
            // bgWorkResync
            // 
            this.bgWorkResync.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BgWorkResync_DoWork);
            this.bgWorkResync.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BgWorkResync_RunWorkerCompleted);
            // 
            // updateWorkactivity
            // 
            this.updateWorkactivity.Enabled = true;
            this.updateWorkactivity.Interval = 300000;
            this.updateWorkactivity.Tick += new System.EventHandler(this.UpdateWorkactivity_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1128, 517);
            this.Controls.Add(this.pnlBrowser);
            this.Controls.Add(this.pnlHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmMain";
            this.Text = "Skydev Browser";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMain_KeyDown);
            this.pnlHeader.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlURL.ResumeLayout(false);
            this.pnlAction.ResumeLayout(false);
            this.pnlAction.PerformLayout();
            this.pnlUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbImg)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.pnlLoader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlBrowser;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.Panel pnlSplitter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlSearch;
        private SkydevCSTool.CustomComboBox cmbURL;
        private System.ComponentModel.BackgroundWorker bgWorkResync;
        private System.Windows.Forms.Timer updateWorkactivity;
        private System.Windows.Forms.ToolStripMenuItem switchToolStripMenuItem;
        private System.Windows.Forms.Label lblCountdown;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Panel pnlUser;
        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.PictureBox pbImg;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel pnlSplitter3;
        private System.Windows.Forms.Panel pnlSplitter2;
        private System.Windows.Forms.Panel pnlAction;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Panel pnlSplitter5;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Panel pnlURL;
        private System.Windows.Forms.Label lblApproveCount;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Panel pnlLoader;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnKb;
    }
}

