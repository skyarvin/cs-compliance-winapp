using CefSharp.WinForms.Internals;
using SkydevCSTool.Class;
using SkydevCSTool.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frmSetPreferences : Form
    {
        private static string chatlog_user = "CL";

        public static Dictionary<String, String> preferences = new Dictionary<String, String>
        {
            { "CL", "Chatlog_user" },
            { "BO", "Bio" },
            { "PT", "Photos" },
            { "CLBO", "Bio and Chatlog_user" },
            { "PTCL", "Chatlog_user and Photos" },
            { "PTBO", "Photos and Bio" }
        };

        public frmSetPreferences()
        {
            InitializeComponent();
        }

        private void frmSetPreferences_Load(object sender, EventArgs e)
        {
            label1.Visible = false;
            label2.Visible = true;
            lblMissingPref.Visible = false;

            comboPreferences.DataSource = new BindingSource(preferences, null);
            comboPreferences.DisplayMember = "Value";
            comboPreferences.ValueMember = "Key";

            if (!string.IsNullOrEmpty(Settings.Default.preference))
            {
                comboPreferences.SelectedValue = Settings.Default.preference;
            }


            if (Globals.IsBuddySystem() && !Globals.IsPreferenceSetupValid())
            {
                label1.Visible = true;
                label2.Visible = false;
                lblMissingPref.Text = string.Concat("Missing Preference: ", Globals.MissingPreference());
                lblMissingPref.Visible = true;
            }

        }

        private void btnSelectPreference_Click(object sender, EventArgs e)
        {
            switch (comboPreferences.SelectedValue.ToString())
            {
                case "BO":
                case  "CLBO":
                    Settings.Default.compliance_default_view = "bio";
                    break;
                case "PT":
                case "PTBO":
                    Settings.Default.compliance_default_view = "photos";
                    break;
                case "CL":
                case "PTCL":
                    Settings.Default.compliance_default_view = "chatlog_user";
                    break;
            }

            Settings.Default.preference = comboPreferences.SelectedValue.ToString();
            Settings.Default.Save();

            Globals.BroadcastPreferenceChanges();
            
            if (Globals.IsBuddySystem() && !Globals.IsPreferenceSetupValid())
            {
                label1.Visible = true;
                label2.Visible = false;
                lblMissingPref.Text = string.Concat("Missing Preference: ", Globals.MissingPreference());
                lblMissingPref.Visible = true;
            }
            else
            {
                this.Close();
            }
            
        }
    }
}
