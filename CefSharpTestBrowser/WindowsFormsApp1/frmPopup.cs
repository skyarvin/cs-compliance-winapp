using CefSharp;
using CefSharp.WinForms;
using System;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace SkydevCSTool
{
    public partial class frmPopup : Form
    {
        public string url { get; set; }

        public frmPopup(string url)
        {
            Globals.chromePopup = new ChromiumWebBrowser(url);
            this.Controls.Add(Globals.chromePopup);
            Globals.chromePopup.Dock = DockStyle.Fill;
            InitializeComponent();
            Globals.chromePopup.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(OnFrameLoadEnd);
            this.Text = url;
        }

        public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                 Globals.chromePopup.EvaluateScriptAsync(@"
                   var zoomLevel = 1;
                    window.addEventListener('wheel', function(e) {
                        if (e.deltaY < 0) {
                             if(event.ctrlKey && zoomLevel < 2){
                                    console.log('scrolling up');
                                    zoomLevel += 0.1;
                                     document.body.style.zoom = zoomLevel;
                             };        
                        }
                        
                        if (e.deltaY > 0) {
                           if(event.ctrlKey && zoomLevel > 0.5){
                                    console.log('scrolling down');
                                    zoomLevel -= 0.1;
                                    document.body.style.zoom = zoomLevel;
                             }; 
                        }
                    });
                 ");
            }
        }
    }
}
