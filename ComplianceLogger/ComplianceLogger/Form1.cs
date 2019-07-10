﻿using System;
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
    public ChromiumWebBrowser chromeBrowser;

    public void InitializeChromium()
    {
        CefSettings settings = new CefSettings();
        // Initialize cef with the provided settings
        Cef.Initialize(settings);
        // Create a browser component
        chromeBrowser = new ChromiumWebBrowser("http://ourcodeworld.com");
        // Add it to the form and fill it to the form window.
        this.Controls.Add(chromeBrowser);
        chromeBrowser.Dock = DockStyle.Fill;
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
    }
}
