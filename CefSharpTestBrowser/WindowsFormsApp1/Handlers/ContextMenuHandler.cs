using System;
using CefSharp;
using System.Windows.Forms;
using System.Diagnostics;
using System.Web;
using WindowsFormsApp1;
using SkydevCSTool;
using SkydevCSTool.Class;

public class MyCustomMenuHandler : IContextMenuHandler
{
    public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
    {
        // Remove any existent option using the Clear method of the model
        //
        model.Clear();

        Console.WriteLine("Context menu opened !");

        // You can add a separator in case that there are more items on the list
        if (model.Count > 0)
        {
            model.AddSeparator();
        }

        model.AddItem((CefMenuCommand)26503, "Copy URL");
        model.AddSeparator();

        // Add a new item to the list using the AddItem method of the model
        model.AddItem((CefMenuCommand)26501, "Open Image");
        model.AddSeparator();

        model.AddItem((CefMenuCommand)26502, "Open room in Chrome");

        model.AddSeparator();
        model.AddItem((CefMenuCommand)26504, "Translate");

        model.AddSeparator();
        model.AddItem((CefMenuCommand)26505, "View User");

        model.AddSeparator();
        model.AddItem((CefMenuCommand)26506, "Devtools");

        model.AddSeparator();
        model.AddItem((CefMenuCommand)26507, "Log Viewer");
    }

    public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
    {
        // React to the first ID (show dev tools method)
        if (commandId == (CefMenuCommand)26501)
        {
            if (parameters.MediaType == CefSharp.ContextMenuMediaType.Image) {
                frmPopup frmpop = new frmPopup(parameters.SourceUrl);
                frmpop.Show();

            }
            return true;
        }

        if (commandId == (CefMenuCommand)26502)
        {
            if (parameters.MediaType == CefSharp.ContextMenuMediaType.None && !String.IsNullOrEmpty(parameters.LinkUrl))
            {
                Process.Start("chrome.exe", string.Concat("--app=", parameters.LinkUrl));
            }
            return true;
        }

        if (commandId == (CefMenuCommand)26503)
        {
            if (!String.IsNullOrEmpty(parameters.PageUrl))
            {
                System.Windows.Forms.Clipboard.SetText(parameters.PageUrl) ;
            }
            return true;
        }

        if (commandId == (CefMenuCommand)26504)
        {
            var surl = string.Concat(Url.GOOGLE_TRANSLATE_URL, Uri.EscapeDataString(parameters.SelectionText));
            browserControl.EvaluateScriptAsync(string.Concat("window.open('", surl, "', '_blank');"));
            return true;
        }

        if (commandId == (CefMenuCommand)26505)
        {
            if (!String.IsNullOrEmpty((parameters.SelectionText))){
                browserControl.Load(String.Concat(Url.CB_COMPLIANCE_URL, "/show/", parameters.SelectionText));
                return true;
            }
        }

        if (commandId == (CefMenuCommand)26506)
        {
            browserControl.ShowDevTools();
        }

        if (commandId == (CefMenuCommand)26507)
        {
            frmLogViewer frmLogViewer = new frmLogViewer();
            frmLogViewer.Show();
        }

        // Return false should ignore the selected option of the user !
        return false;
    }

    public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
    {

    }

    public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
    {
        return false;
    }
}