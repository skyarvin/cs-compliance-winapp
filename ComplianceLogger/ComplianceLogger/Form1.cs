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

namespace ComplianceLogger
{
  
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeChromium();
        }

        public ChromiumWebBrowser browser;

        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            // Initialize cef with the provided settings
          Cef.Initialize(settings);
            // Create a browser component
            browser = new ChromiumWebBrowser("http://chaturbate.com");
            // Add it to the form and fill it to the form window.
            this.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
        }

    }
}
