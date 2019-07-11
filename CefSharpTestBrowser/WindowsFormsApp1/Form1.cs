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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public   ChromiumWebBrowser chromeBrowser;
        public string CurrentUrl;
        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            // Initialize cef with the provided settings
            Cef.Initialize(settings);
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser("https://chaturbate.com/compliance/");
            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
            
        }
        public Form1()
        {
            InitializeComponent();
            InitializeChromium();
            
            chromeBrowser.AddressChanged += Browser_AddressChanged;
            var obj = new BoundObject(chromeBrowser);
            obj.HtmlItemClicked += Obj_HtmlItemClicked;
          
            chromeBrowser.RegisterJsObject("bound", obj);
            chromeBrowser.FrameLoadEnd += obj.OnFrameLoadEnd;
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
        private void ProcessActionButtons(string element_id) {

        //TO DO USE variable : status
            if (element_id == "violation-submit") {
                var violation = chromeBrowser.EvaluateScriptAsync(@"$('#id_violation option:selected').text()").Result;
                var notes = chromeBrowser.EvaluateScriptAsync(@"$('#id_description').val()").Result;
                MessageBox.Show(violation.Result.ToString() + notes.Result.ToString());
                // MessageBox.Show(s);
            }
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var notes = chromeBrowser.EvaluateScriptAsync(@"document.getElementById('id_description').value").Result;
            MessageBox.Show(notes.Result.ToString());
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
                    var denied_buttons = document.getElementsByClassName('photoselect_undisable');
                    for( var i=0; i<denied_buttons.length;i++){
                        denied_buttons[i].addEventListener('click', 
                            function(e)
                            {
                                window.status = this.value;
                            },false)
                    }

                    var bindingInterval = setInterval(function(){
                        if(document.getElementById('violation-submit') != undefined){
                            console.log('binded');
                            document.getElementById('violation-submit').addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(bindingInterval);
                        }
                    }, 1000);
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
