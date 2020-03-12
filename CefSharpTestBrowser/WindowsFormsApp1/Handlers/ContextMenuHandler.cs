using System;
using CefSharp;
using System.Windows.Forms;
using System.Diagnostics;
using System.Web;
using System.Linq;
using WindowsFormsApp1;
using SkydevCSTool;
using SkydevCSTool.Class;
using SkydevCSTool.Properties;
using SkydevCSTool.Models;
using CefSharp.WinForms.Internals;

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
   

        model.AddItem((CefMenuCommand)26512, "Send Internal Review");
        model.AddItem((CefMenuCommand)26503, "Copy URL");

        // Add a new item to the list using the AddItem method of the model
        model.AddItem((CefMenuCommand)26501, "Open Image");
        model.AddItem((CefMenuCommand)26502, "Open room in Chrome");

        model.AddSeparator();
        model.AddItem((CefMenuCommand)26504, "Translate");

        model.AddSeparator();
        model.AddItem((CefMenuCommand)26505, "View User");
        model.AddItem((CefMenuCommand)26507, "Log Viewer");
        model.AddSeparator();
        model.AddItem((CefMenuCommand)26506, "Devtools");
        model.AddItem((CefMenuCommand)26513, "Set Preference");

        if (Globals.IsClient())
        {
            model.SetEnabled((CefMenuCommand)26512, false);
        }
       
        //string defaultview = Settings.Default.preference;
        //IMenuModel submenu = model.AddSubMenu((CefMenuCommand)26508, "Preference");
        //submenu.AddCheckItem((CefMenuCommand)1, "Chatlog_user");
        //submenu.AddCheckItem((CefMenuCommand)2, "Bio");
        //submenu.AddCheckItem((CefMenuCommand)3, "Photos");
        //submenu.AddCheckItem((CefMenuCommand)4, "Bio and Chatlog_user ");
        //submenu.AddCheckItem((CefMenuCommand)5, "Chatlog_user and Photos");
        //submenu.AddCheckItem((CefMenuCommand)6, "Photos and Bio");

        //submenu.SetChecked((CefMenuCommand)1, defaultview == "CL");
        //submenu.SetChecked((CefMenuCommand)2, defaultview == "BO");
        //submenu.SetChecked((CefMenuCommand)3, defaultview == "PT");

        //submenu.SetChecked((CefMenuCommand)4, defaultview == "CLBO");
        //submenu.SetChecked((CefMenuCommand)5, defaultview == "PTCL");
        //submenu.SetChecked((CefMenuCommand)6, defaultview == "PTBO");
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


        if (commandId == (CefMenuCommand)26513)
        {
            Globals.frmMain.InvokeOnUiThreadIfRequired(() =>
            {
                Globals.FrmSetPreferences.ShowDialog(Globals.frmMain);
            });
        }

        if (commandId == (CefMenuCommand)26512)
        {
           if (Globals.INTERNAL_RR.id != 0)
            {
                if (Globals.INTERNAL_RR.url == Globals.CurrentUrl)
                {
                    Globals.frmMain.InvokeOnUiThreadIfRequired(() =>
                    {

                        if (Globals.FrmInternalRequestReview == null || Globals.FrmInternalRequestReview.IsDisposed)
                            Globals.FrmInternalRequestReview = new frmInternalRequestReview();
                        Globals.FrmInternalRequestReview.update_info();
                        Globals.FrmInternalRequestReview.Show();
                    });
                    return false;
                }
            }

            Globals.frmMain.InvokeOnUiThreadIfRequired(() =>
            {
                string followRaw = Globals.myStr(Globals.chromeBrowser.EvaluateScriptAsync(@"$('#room_info').children()[1].textContent").Result.Result);
                followRaw = new String(followRaw.Where(Char.IsDigit).ToArray());
                if (!String.IsNullOrEmpty(followRaw)) Globals.followers = int.Parse(followRaw);
                    

                Globals.FrmSendInternalRequestReview = new frmSendInternalRequestReview();
                Globals.FrmSendInternalRequestReview.ShowDialog(Globals.frmMain);
                if(Globals.FrmSendInternalRequestReview.DialogResult == DialogResult.OK)
                {
                    if ( Globals.FrmInternalRequestReview == null || Globals.FrmInternalRequestReview.IsDisposed)
                        Globals.FrmInternalRequestReview = new frmInternalRequestReview();
                    Globals.FrmInternalRequestReview.Show();
                }
            });
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