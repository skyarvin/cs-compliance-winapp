using CSTool.Handlers.ErrorsHandler;
using CSTool.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Windows.Forms;
using WindowsFormsApp1.Models;
using WindowsFormsApp1;
using CSTool.Properties;

namespace CSTool
{
    public partial class frmRecoveryCode : Form
    {
        private UserTFA userTfa;
        private TFA tfa = new TFA();
        private bool bExitApp = true;

        public frmRecoveryCode(UserTFA userTFA)
        {
            InitializeComponent();
            this.userTfa = userTFA;
        }

        private void BackToAuthTfa(object sender, EventArgs e)
        {
            bExitApp = false;
            this.Close();
        }

        private void SubmitRecoveryCode(object sender, EventArgs e)
        {
            try
            {
                this.tfa.device_id = "recovery_code";
                this.tfa.nonce = this.userTfa.nonce;
                this.tfa.tfa_code = recoveryCodeInput.Text;
                this.tfa.user_id = this.userTfa.user_id;
                this.tfa.ValidateTfa(true);
                Globals.ComplianceAgent = Agent.Get(Globals.user_account.username);
                if (Globals.ComplianceAgent != null)
                {
                    if (Globals.ComplianceAgent.tier_level is null)
                    {
                        MessageBox.Show("Tier level is not registered! Please contact admin.", "Error");
                        return;
                    }

                    bExitApp = false;
                    Globals.SaveUserSettings();
                    if (Settings.Default.user_type.ToUpper().Contains("AGENT") && Settings.Default.role == "CSA" ||
                        Settings.Default.user_type.ToUpper().Contains("TRAINEE") && Settings.Default.role == "TRAINEE")
                    {
                        Globals.frmMain = new frmMain();
                        Globals.frmMain.Show();
                        this.Close();
                    }
                    else if (Settings.Default.user_type.ToUpper().Contains("QA") && Settings.Default.role == "CSQA")
                    {
 
                        Globals.FrmQA = new frmQA();
                        Globals.FrmQA.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please check your User Type.", "Error");
                        bExitApp = false;
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("We cannot find an account with that username.", "Error");
                }
            }
            catch (UnauthorizeException unauthorize)
            {
                using (HttpContent data = unauthorize.responseContent)
                {
                    var jsonString = data.ReadAsStringAsync();
                    jsonString.Wait();
                    UserTFA result = JsonConvert.DeserializeObject<UserTFA>(jsonString.Result);
                    this.userTfa.nonce = result.nonce;
                    MessageBox.Show("Invalid Recovery Code! \nPlease Try Again.", "Error");
                    return;
                }
            }
            catch (AggregateException ex)
            {
                Globals.SaveToLogFile(ex.ToString(), (int)LogType.Error);
                MessageBox.Show(String.Concat("Error connecting to Compliance servers", System.Environment.NewLine, "Please refresh and try again.",
                System.Environment.NewLine, "If internet is NOT down and you are still getting the error, Please contact dev team"), "Error");
            }
            catch (Exception ex)
            {
                Globals.SaveToLogFile(ex.ToString(), (int)LogType.Error);
                MessageBox.Show(String.Concat(ex.Message.ToString(), System.Environment.NewLine, "Please contact Admin."), "Error");
            }
        }

        private void FrmTfa_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bExitApp)
            {
                Application.Exit();
            }
        }
    }
}
