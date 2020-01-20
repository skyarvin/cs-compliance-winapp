using CefSharp;
using CefSharp.WinForms;
using CefSharp.WinForms.Internals;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frmQA : Form
    {
        private ChromiumWebBrowser chromeBrowserQA;
        private string currentUrl;
        public string chatStart;
        public string chatEnd;

        public frmQA()
        {
            InitializeComponent();
            InitializeChromium("https://chaturbate.com");
        }
        public void InitializeChromium(string url)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            CefSettings settings = new CefSettings();
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            settings.CachePath = @path + "/cache/cache/";
            settings.PersistSessionCookies = true;
            if (!Cef.IsInitialized)
            {
                Cef.Initialize(settings);
            }

            Cef.GetGlobalCookieManager().SetStoragePath(@path + "/SkydevCsTool/cookies/" + Globals.ComplianceAgent.profile + "_qa/", true);

            var requestContextSettings = new RequestContextSettings();
            requestContextSettings.CachePath = @path + "/cache/cache/";
            requestContextSettings.PersistSessionCookies = true;
            requestContextSettings.PersistUserPreferences = true;

            chromeBrowserQA = new ChromiumWebBrowser(url);
            chromeBrowserQA.RequestContext = new RequestContext(requestContextSettings);
            this.pnlBrowser.Controls.Add(chromeBrowserQA);
            chromeBrowserQA.Dock = DockStyle.Fill;


            var obj = new BoundObjectQA(chromeBrowserQA,this);


            chromeBrowserQA.RegisterJsObject("bound", obj);
            chromeBrowserQA.FrameLoadStart += obj.OnFrameLoadStart;
            chromeBrowserQA.FrameLoadEnd += obj.OnFrameLoadEnd;
            chromeBrowserQA.MenuHandler = new MyCustomMenuHandler();
            chromeBrowserQA.AddressChanged += Browser_AddressChanged;

            lblUser.Text = Globals.ComplianceAgent.name;
            try
            {
                pbImg.Load(Globals.ComplianceAgent.photo);
            }
            catch { }
        }

        //private void txtUrl_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        chromeBrowserQA.Load(txtUrl.Text);
        //    }
        //}

        private void Browser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            string sCurrAddress = e.Address;
            this.InvokeOnUiThreadIfRequired(() => {
                if (sCurrAddress != txtUrl.Text)
                {
                    txtUrl.Text = sCurrAddress;
                }
                this.currentUrl = sCurrAddress;
            });
            
        }

        

        private void frmQA_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void txtUrl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                try
                {
                    Uri myUri = new Uri(txtUrl.Text);
                    lblChatStart.Text = HttpUtility.ParseQueryString(myUri.Query).Get("chatstart");
                    lblChatEnd.Text = HttpUtility.ParseQueryString(myUri.Query).Get("chatend");
                    chatStart = lblChatStart.Text;
                    chatEnd = lblChatEnd.Text;
                    chromeBrowserQA.Load(txtUrl.Text);
                }
                catch { }
            }
        }

        private void frmQA_Load(object sender, EventArgs e)
        {
            this.Text += String.Concat(" ", Globals.CurrentVersion(), " | IP Address:", Globals.MyIP);
        }
    }
    public class BoundObjectQA
    {
        private ChromiumWebBrowser browser;
        private frmQA formQA;
        public BoundObjectQA(ChromiumWebBrowser br, frmQA qa ) { 
            browser = br;
            formQA = qa;
        }

        public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {

        }
        public void OnFrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            Console.WriteLine(e.Url);
            if (!e.Frame.IsMain)
                return;
 
            
            

            var submit_script = @"
                    var zoomLevel = 1;
                    window.addEventListener('wheel', function(e) {
                        if (e.deltaY < 0) {
                             if(event.ctrlKey && zoomLevel < 2){
                                    zoomLevel += 0.1;
                                     document.body.style.zoom = zoomLevel;
                             }     
                        }
                        if (e.deltaY > 0) {
                           if(event.ctrlKey && zoomLevel > 0.5){
                                    zoomLevel -= 0.1;
                                    document.body.style.zoom = zoomLevel;
                            }
                        }
                    });

                    function waitUntil(selector, max_ms_timeout){
                        return new Promise( (resolve, reject) => {
	                        var ms = 0;
                            var finder = setInterval( () => {
		                        if (ms >= max_ms_timeout){
			                        clearInterval(finder);
			                        reject(false);
		                        }
                                if (document.querySelectorAll(selector).length > 0){
        	                        clearInterval(finder);
                                    resolve(document.querySelectorAll(selector));
                                }
		                        ms+=100;
                            }, 100);
	
                        });
                    }

                    function highlight(selector, last_chatlog, color)
                    {
                        if (!color){
                            color = '#da1b1b';
                        }
                        var chatlog_position = 0;
                        if (selector == null || selector.length == 0)
                            return; 
                        var found = false
                        var row_index = 0;
                                
                        if (last_chatlog == '' || last_chatlog == undefined || last_chatlog == null){
                            return selector.length;
                        }
                                
                        selector.forEach(function(el)
                        {
                            row_index++;
                                    
                            el.childNodes.forEach(function(td)
                            {
                                if ((td.className == 'chatlog_date') && (td.innerText.indexOf(last_chatlog) >= 0))
                                {
                                    
                                    td.parentNode.style.background = color;
                                    chatlog_position = row_index;
                                }
                            })

                        });

                        if (chatlog_position == 0)
                            return row_index 

                        return chatlog_position;
                                
                    }
                ";

            browser.EvaluateScriptAsync(submit_script);
            if (!String.IsNullOrEmpty(formQA.chatStart) && !String.IsNullOrEmpty(formQA.chatEnd))
            {
                browser.EvaluateScriptAsync(@"
                    document.addEventListener('DOMContentLoaded', function(){
                        
                        function qa_chatlog_highlight(){
                            waitUntil('#data .chatlog tbody tr', 5000).then((element) => highlight(element, '{{chat_start}}','#0f0'), (error) => console.log(error));
                            waitUntil('#data .chatlog tbody tr', 5000).then((element) => highlight(element, '{{chat_end}}' ), (error) => console.log(error));
                            waitUntil('#chatlog_user .chatlog tbody tr', 5000).then((element) => highlight(element, '{{chat_start}}', '#0f0'), (error) => console.log(error));
                            waitUntil('#chatlog_user .chatlog tbody tr', 5000).then((element) => highlight(element, '{{chat_end}}' ), (error) => console.log(error));
                        }
                        qa_chatlog_highlight();
                        document.getElementById('chatlog_user').addEventListener('DOMSubtreeModified', function()
                        {
                            qa_chatlog_highlight();
                        });
                       
                    });
                ".Replace("{{chat_start}}", formQA.chatStart)
                 .Replace("{{chat_end}}",formQA.chatEnd)
                 );

            }



        }
    }
}
