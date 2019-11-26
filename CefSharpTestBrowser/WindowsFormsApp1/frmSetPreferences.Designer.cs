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
            this.SuspendLayout();
            // 
            // comboPreferences
            // 
            this.comboPreferences.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPreferences.FormattingEnabled = true;
            this.comboPreferences.Location = new System.Drawing.Point(34, 77);
            this.comboPreferences.Name = "comboPreferences";
            this.comboPreferences.Size = new System.Drawing.Size(298, 21);
            this.comboPreferences.TabIndex = 0;
            // 
            // btnSelectPreference
            // 
            this.btnSelectPreference.Location = new System.Drawing.Point(231, 149);
            this.btnSelectPreference.Name = "btnSelectPreference";
            this.btnSelectPreference.Size = new System.Drawing.Size(101, 29);
            this.btnSelectPreference.TabIndex = 1;
            this.btnSelectPreference.Text = "Ok";
            this.btnSelectPreference.UseVisualStyleBackColor = true;
            this.btnSelectPreference.Click += new System.EventHandler(this.btnSelectPreference_Click);
            // 
            // frmSetPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 198);
            this.Controls.Add(this.btnSelectPreference);
            this.Controls.Add(this.comboPreferences);
            this.Name = "frmSetPreferences";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Preference";
            this.Load += new System.EventHandler(this.frmSetPreferences_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboPreferences;
        private System.Windows.Forms.Button btnSelectPreference;
    }
}