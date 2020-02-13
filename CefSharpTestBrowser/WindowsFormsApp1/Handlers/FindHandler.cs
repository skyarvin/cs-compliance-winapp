using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.Structs;

namespace SkydevCSTool.Handlers
{
    class FindHandler : IFindHandler
    {
        private frmPopup frmPopup;

        public FindHandler(frmPopup frmPopup)
        {
            this.frmPopup = frmPopup;
        }

        public void OnFindResult(IWebBrowser chromiumWebBrowser, IBrowser browser, int identifier, int count, Rect selectionRect, int activeMatchOrdinal, bool finalUpdate)
        {
            this.frmPopup.UpdateTextSearchCount(count, activeMatchOrdinal);
        }
    }
}
