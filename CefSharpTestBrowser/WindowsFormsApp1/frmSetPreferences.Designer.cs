namespace WindowsFormsApp1
{
    partial class frmSetPreferences
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
            this.comboPreferences = new System.Windows.Forms.ComboBox();
            this.btnSelectPreference = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMissingPref = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboPreferences
            // 
            this.comboPreferences.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPreferences.FormattingEnabled = true;
            this.comboPreferences.Location = new System.Drawing.Point(34, 85);
            this.comboPreferences.Name = "comboPreferences";
            this.comboPreferences.Size = new System.Drawing.Size(298, 21);
            this.comboPreferences.TabIndex = 0;
            // 
            // btnSelectPreference
            // 
            this.btnSelectPreference.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnSelectPreference.ForeColor = System.Drawing.Color.White;
            this.btnSelectPreference.Location = new System.Drawing.Point(231, 132);
            this.btnSelectPreference.Name = "btnSelectPreference";
            this.btnSelectPreference.Size = new System.Drawing.Size(101, 29);
            this.btnSelectPreference.TabIndex = 1;
            this.btnSelectPreference.Text = "Ok";
            this.btnSelectPreference.UseVisualStyleBackColor = false;
            this.btnSelectPreference.Click += new System.EventHandler(this.btnSelectPreference_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(29, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 63);
            this.label1.TabIndex = 2;
            this.label1.Text = "Your group preference must have(Chatlog, Photos and Bio)";
            this.label1.UseCompatibleTextRendering = true;
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(92, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 38);
            this.label2.TabIndex = 3;
            this.label2.Text = "Select Preference";
            this.label2.UseCompatibleTextRendering = true;
            // 
            // lblMissingPref
            // 
            this.lblMissingPref.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMissingPref.ForeColor = System.Drawing.Color.Red;
            this.lblMissingPref.Location = new System.Drawing.Point(34, 109);
            this.lblMissingPref.Name = "lblMissingPref";
            this.lblMissingPref.Size = new System.Drawing.Size(298, 21);
            this.lblMissingPref.TabIndex = 4;
            this.lblMissingPref.Text = "Missing Preference:";
            this.lblMissingPref.UseCompatibleTextRendering = true;
            // 
            // frmSetPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(95)))), ((int)(((byte)(167)))));
            this.ClientSize = new System.Drawing.Size(367, 179);
            this.ControlBox = false;
            this.Controls.Add(this.lblMissingPref);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelectPreference);
            this.Controls.Add(this.comboPreferences);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmSetPreferences";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Preference";
            this.Load += new System.EventHandler(this.frmSetPreferences_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboPreferences;
        private System.Windows.Forms.Button btnSelectPreference;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lblMissingPref;
    }
}