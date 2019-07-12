using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using CefSharp.WinForms.Internals;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Services;

namespace WindowsFormsApp1
{
    public partial class frmMain : Form
    {
        public   ChromiumWebBrowser chromeBrowser;
        public string CurrentUrl;
        public string CB_COMPLIANCE_URL = "https://chaturbate.com/compliance/";
        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            // Initialize cef with the provided settings
            Cef.Initialize(settings);
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser(CB_COMPLIANCE_URL);
            // Add it to the form and fill it to the form window.
            this.pnlBrowser.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
            lblUser.Text = Globals.ComplianceAgent.name;
        }
        public frmMain()
        {
            InitializeComponent();
            InitializeChromium();
            
            chromeBrowser.AddressChanged += Browser_AddressChanged;
            var obj = new BoundObject(chromeBrowser);
            obj.HtmlItemClicked += Obj_HtmlItemClicked;
          
            chromeBrowser.RegisterJsObject("bound", obj);
            chromeBrowser.FrameLoadEnd += obj.OnFrameLoadEnd;
            chromeBrowser.MenuHandler = new MyCustomMenuHandler();
            
        }


        private void Browser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            this.InvokeOnUiThreadIfRequired(() => {
                string sCurrAddress = e.Address;
                if (sCurrAddress.Contains("https://chaturbate.com/compliance/show/"))
                {
                    chromeBrowser.ShowDevTools();
                    CurrentUrl = sCurrAddress;
                }
            });
            
        }

        private void Obj_HtmlItemClicked(object sender, BoundObject.HtmlItemClickedEventArgs e)
        {
            this.InvokeOnUiThreadIfRequired(() => ProcessActionButtons(e.Id));
        }

        public Dictionary<string, string> Actions = new Dictionary<string, string>
        {
            { "violation-submit", "VR" },
            { "id-missing", "IM" },
            { "spammer-submit", "SR" },
            { "request-review-submit", "RR" },
            { "approve_button", "AP" },
        };

        private string myStr(object o) {
            try
            {
                return o.ToString();
            }
            catch {
                return "";
            }
        }
        private void ProcessActionButtons(string element_id) {
            string violation = "";
            string notes = myStr(chromeBrowser.EvaluateScriptAsync(@"$('#id_description').val()").Result.Result);
            if (element_id == "violation-submit")
                violation = myStr(chromeBrowser.EvaluateScriptAsync(@"$('#id_violation option:selected').text()").Result.Result);

            LoggerServices.Save(new Logger()
            {
                action = Actions[element_id],
                url = CurrentUrl,
                agent_id = Globals.ComplianceAgent.id.ToString(),
                remarks = !String.IsNullOrEmpty(violation) ? String.Concat(violation, System.Environment.NewLine, notes) : notes
            }) ;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //REMOVE THIS 
            System.Windows.Forms.Clipboard.SetText("FBBAFyAw%[r{)5z?");
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
            Application.Exit();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            chromeBrowser.Load(CB_COMPLIANCE_URL);
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            Find(true);
        }

        private void Find(bool next)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                chromeBrowser.Find(0, txtSearch.Text, next, false, false);
            }
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            Find(true);
        }
    }




    public class BoundObject
    {
        public delegate void ItemClickedEventHandler(object sender, HtmlItemClickedEventArgs e);
        public event ItemClickedEventHandler HtmlItemClicked;
        public delegate void ItemResponseEventHandler(object sender, GetResponseEventArgs e);
        public event ItemResponseEventHandler ItemResponse;
        private ChromiumWebBrowser browser;

        public BoundObject(ChromiumWebBrowser br) { browser = br; }

        public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                // browser.EvaluateScriptAsync(@"document.getElementById('u_0_2').onclick = function(e) { e.preventDefault(); bound.onClicked(e.target.outerHTML); }");


                //  var script = "document.getElementById('u_0_2').onclick = function(e) { bound.onClicked(); })";
                var submit_script = @"
                    var violation_interval = setInterval(function(){
                        if(document.getElementById('violation-submit') != undefined){
                            console.log('binded');
                            document.getElementById('violation-submit').addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(violation_interval);
                        }
                    }, 1000);

                    var id_missing = $(`input[value='Report Identification Missing Problem']`)[0];
                    var id_missing_interval = setInterval(function(){
                        if(id_missing != undefined){
                            console.log('binded');
                            id_missing.addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked('id-missing');
                            },false)
                            clearInterval(id_missing_interval);
                        }
                    }, 1000);

                    var spammer_interval = setInterval(function(){
                        if($('#spammer-submit')[0] != undefined){
                            console.log('binded');
                            $('#spammer-submit')[0].addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(spammer_interval);
                        }
                    }, 1000)

                    var request_review_interval = setInterval(function(){
                        if($('#request-review-submit')[0] != undefined){
                            console.log('binded');
                            $('#request-review-submit')[0].addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(request_review_interval);
                        }
                    }, 1000);

                    $('#approve_button')[0].addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                ";
                browser.EvaluateScriptAsync(@submit_script);

                //var violation_submit_script = @"
                //    document.getElementById('violation-submit').addEventListener('click', 
                //        function(e)
                //        {
                //            bound.onClicked(e.target.id);
                //        },false)
                //";
                //browser.EvaluateScriptAsync(violation_submit_script);
            }
        }
        public void OnClicked(string id)
        {          
            if (HtmlItemClicked != null)
            {
                HtmlItemClicked(this, new HtmlItemClickedEventArgs() { Id = id });
            }
        }
        public class HtmlItemClickedEventArgs : EventArgs
        {
            public string Id { get; set; }
        }

        public class GetResponseEventArgs : EventArgs
        {
            public string ProjectId { get; set; }
            public string AutomationTaskId { get; set; }
            public string ResponseText { get; set; }
        }

        
    }
}
