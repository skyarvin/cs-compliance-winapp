﻿using CefSharp;
using CefSharp.WinForms;
using CefSharp.WinForms.Internals;
using System;
using System.Windows.Forms;
using WindowsFormsApp1;
using SkydevCSTool.Handlers;
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
            var obj = new BoundObjectPopUp(this);
            Globals.chromePopup.RegisterJsObject("bound", obj);
            Globals.chromePopup.FindHandler = new FindHandler(this);
            Globals.chromePopup.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(OnFrameLoadEnd);
            Globals.chromePopup.FrameLoadStart += new EventHandler<FrameLoadStartEventArgs>(onFrameLoadStart);
           this.Text = url;
            
          
        }

        public void UpdateTextSearchCount(int matchcount,int index)
        {
           this.InvokeOnUiThreadIfRequired(() => {
               if (matchcount == 0 && index == 0)
               {
                   lblFindCount.Visible = false;
                   return;
               }
               lblFindCount.Visible = true;
               lblFindCount.Text = String.Concat(index.ToString(),"/", matchcount.ToString());
            });
        }
        public void Find() 
        {
            this.InvokeOnUiThreadIfRequired(() => {
                pnlSearch.BringToFront();
                pnlSearch.Visible = true;
                txtSearch.Select();
            });
        }

        public void hideSearchPanel()
        {
            this.InvokeOnUiThreadIfRequired(() =>
            {
                pnlSearch.Visible = false;
                this.Focus();
            });
        }
        public void onFrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            this.InvokeOnUiThreadIfRequired(() =>
            {
                this.Focus();
            });
          
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

                    window.onkeydown = function(e){
                         if(e.keyCode == 70 && e.ctrlKey){
                            bound.triggerFind();
                        }

                        if(e.key == 'Escape'){
                            bound.triggerHide();

                        }
                    }



                 ");
            }
        }

        public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlSearch.Visible = false;
            this.Focus();
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
           
            if (string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                lblFindCount.Text = "";
                Globals.chromePopup.StopFinding(true);
            }
            else 
            {
                Globals.chromePopup.Find(0, txtSearch.Text, true, false, false);
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                hideSearchPanel();
            }
            if (e.KeyCode == Keys.Enter)
            {
                Globals.chromePopup.Find(0, txtSearch.Text, true, false, false);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void frmPopup_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("frm KeyDown");
            if (e.KeyCode == Keys.F && e.Control)
            {
                Find();
            }
        }

        private void frmPopup_Load(object sender, EventArgs e)
        {
            Globals.chromePopup.BringToFront();
            this.Focus();
        }

        private void frmPopup_Activated(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            Globals.chromePopup.Find(0, txtSearch.Text, false, false, false);
        }

        private void btnNxt_Click(object sender, EventArgs e)
        {
            Globals.chromePopup.Find(0, txtSearch.Text, true, false, false);
        }

        private void pnlSearch_VisibleChanged(object sender, EventArgs e)
        {
            if (pnlSearch.Visible)
            {
                txtSearch.SelectAll();
            }
            else 
            {
                Globals.chromePopup.StopFinding(true);
            }
        }
    }
    public class BoundObjectPopUp
    {
        private frmPopup  frmpopup;
        public BoundObjectPopUp(frmPopup fp) { frmpopup = fp; }
        public void TriggerFind()
        {
            frmpopup.Find();
        }

        public void TriggerHide()
        {
            frmpopup.hideSearchPanel();
        }


    }
}
