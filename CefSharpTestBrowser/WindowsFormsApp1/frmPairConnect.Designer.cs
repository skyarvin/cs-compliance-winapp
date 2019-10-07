namespace SkydevCSTool
{
    partial class frmPairConnect
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
            this.txtIPaddress = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.pnlWaiting = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlWaiting.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtIPaddress
            // 
            this.txtIPaddress.Location = new System.Drawing.Point(75, 26);
            this.txtIPaddress.Name = "txtIPaddress";
            this.txtIPaddress.Size = new System.Drawing.Size(225, 20);
            this.txtIPaddress.TabIndex = 0;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(143, 63);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // pnlWaiting
            // 
            this.pnlWaiting.Controls.Add(this.label1);
            this.pnlWaiting.Location = new System.Drawing.Point(12, 12);
            this.pnlWaiting.Name = "pnlWaiting";
            this.pnlWaiting.Size = new System.Drawing.Size(337, 87);
            this.pnlWaiting.TabIndex = 2;
            this.pnlWaiting.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Waiting for approval";
            // 
            // frmPairConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 111);
            this.Controls.Add(this.pnlWaiting);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtIPaddress);
            this.Name = "frmPairConnect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connect to Server";
            this.Load += new System.EventHandler(this.FrmPairConnect_Load);
            this.pnlWaiting.ResumeLayout(false);
            this.pnlWaiting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIPaddress;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Panel pnlWaiting;
        private System.Windows.Forms.Label label1;
    }
}