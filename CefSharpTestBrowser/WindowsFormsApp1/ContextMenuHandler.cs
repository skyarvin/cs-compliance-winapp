using System;
using CefSharp;
using System.Windows.Forms;
using System.Diagnostics;

public class MyCustomMenuHandler : IContextMenuHandler
{
    public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
    {
        // Remove any existent option using the Clear method of the model
        //
        //model.Clear();

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

    }

    public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
    {
        // React to the first ID (show dev tools method)
        if (commandId == (CefMenuCommand)26501)
        {
            if (parameters.MediaType == CefSharp.ContextMenuMediaType.Image) {
                browserControl.EvaluateScriptAsync(string.Concat("window.open('",parameters.SourceUrl,"', '_blank');"));
            }
           // browserControl.EvaluateScriptAsync(@"alert(e.target.outerHTML);");
            return true;
        }

        if (commandId == (CefMenuCommand)26502)
        {
            if (parameters.MediaType == CefSharp.ContextMenuMediaType.None && !String.IsNullOrEmpty(parameters.LinkUrl))
            {
                Process.Start("chrome.exe", parameters.LinkUrl);
            }
            // browserControl.EvaluateScriptAsync(@"alert(e.target.outerHTML);");
            return true;
        }

        if (commandId == (CefMenuCommand)26503)
        {
            if (!String.IsNullOrEmpty(parameters.PageUrl))
            {
                System.Windows.Forms.Clipboard.SetText(parameters.PageUrl) ;
            }
            // browserControl.EvaluateScriptAsync(@"alert(e.target.outerHTML);");
            return true;
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