using System;
using Paragon.Plugins;
using System.Runtime.InteropServices;

namespace Symphony.Plugins
{
    /* 
     * This plugin listens for protocol handler being invoked when app is run via /uri option
     * In Minuet core see: Paragon.Runtime proj --> Kernel/Applications/ApplicationManager.cs
     */

    [JavaScriptPlugin(Name = "symphony.protocolHandler", IsBrowserSide = true)]
    public class ProtocolHandlerPlugin : IParagonPlugin
    {
        [JavaScriptPluginMember(Name = "onInvoked")]
        public event JavaScriptPluginCallback onInvoked;

        IApplication app;
        bool hasMainPageLoaded = false;
        string uriReceivedOnLaunch = null;

        public void Initialize(IApplication application)
        {
            app = application;
            app.ProtocolInvoke += application_ProtocolInvoke;
            app.WindowManager.CreatedWindow += WindowManager_CreatedWindow;
        }

        IApplicationWindow mainWindow;

        void WindowManager_CreatedWindow(IApplicationWindow window, bool arg2)
        {
            if (!hasMainPageLoaded && window != null)
            {
                mainWindow = window;
                mainWindow.PageLoaded += window_PageLoaded;
            }
        }

        void window_PageLoaded(params object[] args)
        {
            hasMainPageLoaded = true;
            mainWindow.PageLoaded -= window_PageLoaded;

            if (!String.IsNullOrEmpty(uriReceivedOnLaunch))
            {
                String key = "protocolHandlerUri";
                String js = "var value = JSON.stringify({ uri: '" + uriReceivedOnLaunch + "'}); window.localStorage.setItem('" + key + "', value)";
                mainWindow.ExecuteJavaScript(js);
            }
        }

        void application_ProtocolInvoke(object sender, ProtocolInvocationEventArgs e)
        {
            var evnt = onInvoked;

            if (evnt != null && 
                e.Uri != null && e.Uri.StartsWith("symphony:") && e.Uri.Length < 2000)
            {
                // if app main window is not loaded then save uri local storage
                // client app will then pick up from local storage after user logs in and
                // app is fully started.  this will happen when uri is called on app launch
                if (!hasMainPageLoaded)
                    uriReceivedOnLaunch = e.Uri;
                else
                    evnt(e.Uri);
            }
        }

        public void Shutdown()
        {
            app.ProtocolInvoke -= application_ProtocolInvoke;
            app.WindowManager.CreatedWindow -= WindowManager_CreatedWindow;
        }
    }
}
