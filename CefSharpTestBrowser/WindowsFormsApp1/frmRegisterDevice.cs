using CSTool.Class;
using CSTool.Handlers;
using CSTool.Handlers.ErrorsHandler;
using CSTool.Handlers.Interfaces;
using CSTool.Properties;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;
using WindowsFormsApp1.Models;

namespace CSTool
{
    public partial class frmRegisterDevice : Form
    {
        private readonly string nonce;
        private bool exitApp = true;
        private readonly string user_id;
        private const string device = "tool";

        public frmRegisterDevice(string nonce, string user_id)
        {
            InitializeComponent();
            this.nonce = nonce;
            this.user_id = user_id;
            Console.WriteLine($"Nonce is {this.nonce}");
        }

        private void frmRegisterDevice_Load(object sender, EventArgs e)
        {

        }

        private void registerDevice(object sender, EventArgs e)
        {
            if (registrationCode.Text.Trim().IsNullOrEmpty())
            {
                return;
            }

            try
            {
                this.ValidateDeviceRegistrationCode();
                this.Close();
            }
            catch (UnauthorizeException unauthorize)
            {
                using (HttpContent data = unauthorize.responseContent)
                {
                    var jsonString = data.ReadAsStringAsync();
                    jsonString.Wait();
                    MessageBox.Show("Invalid code! \nPlease Try Again.", "Error");
                    return;
                }
            }
            catch (Exception ex)
            {
                Globals.SaveToLogFile(ex.ToString(), (int)LogType.Error);
                MessageBox.Show(String.Concat(ex.Message.ToString(), System.Environment.NewLine, "Please contact Admin."), "Error");
            }
        }

        public void ValidateDeviceRegistrationCode()
        {
            try
            {
                using (IHttpHandler client = new HttpHandler())
                {
                    var uri = string.Concat(Url.AUTH_URL, "/device/add/");
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var code = registrationCode.Text;
                    var content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        code,
                        nonce,
                        device,
                        this.user_id
                    }), Encoding.UTF8, "application/json");
                    var response = client.CustomPostAsync(uri, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Device registered. Very cool!");
                        exitApp = false;
                    } 
                }
            }
            catch (AggregateException e) when (e.InnerException is UnauthorizeException unauthorized)
            {
                throw unauthorized;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private void closeForm(object sender, FormClosingEventArgs e)
        {
            // might need to reconsider this when user is in the middle of compliance
            if (exitApp)
            {
                Application.Exit();
            }
        }
    }
}
