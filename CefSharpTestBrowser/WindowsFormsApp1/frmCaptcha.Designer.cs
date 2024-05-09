namespace CSTool
{
    partial class frmCaptcha
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlCaptcha = new System.Windows.Forms.Panel();
            this.imgCaptcha = new System.Windows.Forms.PictureBox();
            this.imgProgress = new System.Windows.Forms.PictureBox();
            this.btnRotateClockwise = new System.Windows.Forms.Button();
            this.btnRotateCounterClockwise = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlCaptcha.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgCaptcha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgProgress)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlCaptcha, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRotateClockwise, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRotateCounterClockwise, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(16);
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(450, 318);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label1.Location = new System.Drawing.Point(107, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 21);
            this.label1.TabIndex = 4;
            this.label1.Text = "Turn the image upright";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlCaptcha
            // 
            this.pnlCaptcha.Controls.Add(this.imgCaptcha);
            this.pnlCaptcha.Controls.Add(this.imgProgress);
            this.pnlCaptcha.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCaptcha.Location = new System.Drawing.Point(107, 61);
            this.pnlCaptcha.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCaptcha.Name = "pnlCaptcha";
            this.pnlCaptcha.Size = new System.Drawing.Size(200, 200);
            this.pnlCaptcha.TabIndex = 3;
            // 
            // imgCaptcha
            // 
            this.imgCaptcha.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imgCaptcha.BackColor = System.Drawing.Color.Transparent;
            this.imgCaptcha.Image = global::CSTool.Properties.Resources.test;
            this.imgCaptcha.InitialImage = null;
            this.imgCaptcha.Location = new System.Drawing.Point(10, 10);
            this.imgCaptcha.Name = "imgCaptcha";
            this.imgCaptcha.Size = new System.Drawing.Size(180, 180);
            this.imgCaptcha.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgCaptcha.TabIndex = 0;
            this.imgCaptcha.TabStop = false;
            // 
            // imgProgress
            // 
            this.imgProgress.BackColor = System.Drawing.Color.Transparent;
            this.imgProgress.Cursor = System.Windows.Forms.Cursors.Default;
            this.imgProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgProgress.Location = new System.Drawing.Point(0, 0);
            this.imgProgress.Name = "imgProgress";
            this.imgProgress.Size = new System.Drawing.Size(200, 200);
            this.imgProgress.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgProgress.TabIndex = 3;
            this.imgProgress.TabStop = false;
            // 
            // btnRotateClockwise
            // 
            this.btnRotateClockwise.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnRotateClockwise.BackgroundImage = global::CSTool.Properties.Resources.rotate_clockwise;
            this.btnRotateClockwise.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRotateClockwise.FlatAppearance.BorderSize = 0;
            this.btnRotateClockwise.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRotateClockwise.Location = new System.Drawing.Point(16, 61);
            this.btnRotateClockwise.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.btnRotateClockwise.Name = "btnRotateClockwise";
            this.btnRotateClockwise.Size = new System.Drawing.Size(75, 200);
            this.btnRotateClockwise.TabIndex = 2;
            this.btnRotateClockwise.UseVisualStyleBackColor = true;
            this.btnRotateClockwise.Click += new System.EventHandler(this.rotateClockwise);
            // 
            // btnRotateCounterClockwise
            // 
            this.btnRotateCounterClockwise.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnRotateCounterClockwise.BackgroundImage = global::CSTool.Properties.Resources.rotate_counterclockwise;
            this.btnRotateCounterClockwise.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRotateCounterClockwise.FlatAppearance.BorderSize = 0;
            this.btnRotateCounterClockwise.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRotateCounterClockwise.Location = new System.Drawing.Point(341, 61);
            this.btnRotateCounterClockwise.Margin = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnRotateCounterClockwise.Name = "btnRotateCounterClockwise";
            this.btnRotateCounterClockwise.Size = new System.Drawing.Size(75, 200);
            this.btnRotateCounterClockwise.TabIndex = 1;
            this.btnRotateCounterClockwise.UseVisualStyleBackColor = true;
            this.btnRotateCounterClockwise.Click += new System.EventHandler(this.rotateCounterClockwise);
            // 
            // frmCaptcha
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(450, 318);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmCaptcha";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.TopMost = true;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.pnlCaptcha.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgCaptcha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgProgress)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox imgProgress;
        private System.Windows.Forms.Button btnRotateCounterClockwise;
        private System.Windows.Forms.Button btnRotateClockwise;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlCaptcha;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox imgCaptcha;
    }
}