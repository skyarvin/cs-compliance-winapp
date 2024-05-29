using CefSharp;
using System;

namespace CSTool.Extensions
{
    internal class CefExtensionHandler : IExtensionHandler
    {
        public bool CanAccessBrowser(IExtension extension, IBrowser browser, bool includeIncognito, IBrowser targetBrowser)
        {
            return true;
        }

        public void Dispose()
        {
            Console.WriteLine("disposing");
        }

        public IBrowser GetActiveBrowser(IExtension extension, IBrowser browser, bool includeIncognito)
        {
            return browser;
        }

        public bool GetExtensionResource(IExtension extension, IBrowser browser, string file, IGetExtensionResourceCallback callback)
        {
            return true;
        }

        public bool OnBeforeBackgroundBrowser(IExtension extension, string url, IBrowserSettings settings)
        {
            return true;
        }

        public bool OnBeforeBrowser(IExtension extension, IBrowser browser, IBrowser activeBrowser, int index, string url, bool active, IWindowInfo windowInfo, IBrowserSettings settings)
        {
            return true;
        }

        public void OnExtensionLoaded(IExtension extension)
        {
            Console.WriteLine("Extension loaded");
        }

        public void OnExtensionLoadFailed(CefErrorCode errorCode)
        {
            Console.WriteLine($"Failed to load extension: {errorCode}");
        }

        public void OnExtensionUnloaded(IExtension extension)
        {
            Console.WriteLine("Extension unloaded");
        }
    }
}
