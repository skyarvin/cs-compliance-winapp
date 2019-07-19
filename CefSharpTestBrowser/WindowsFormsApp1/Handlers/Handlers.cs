using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SkydevCSTool.Handlers
{
    public class BrowserLifeSpanHandler : ILifeSpanHandler
    {
        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName,
            WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo,
            IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;
            if (targetUrl.Contains(Globals.CB_COMPLIANCE_SET_ID_EXP_URL))
            {
                browser.MainFrame.LoadUrl(targetUrl);
                //Block popups
                return true;
            }

            return false;
        }
        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
            //

        }
        public bool DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            return false;
        }
        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
            //nothing
        }
    }
}
