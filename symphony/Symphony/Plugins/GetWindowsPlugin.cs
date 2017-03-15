using Paragon.Plugins;
using System.Windows;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Symphony.Plugins
{
    [JavaScriptPlugin(Name = "cef", IsBrowserSide = true)]
    public class GetWindowsPlugin : IParagonPlugin
    {
        private IApplication application;

        public void Initialize(IApplication application)
        {
            this.application = application;
        }

        public void Shutdown()
        {
        }

        private class Sources
        {
            public string[] sources { get; set; }
        }

        [JavaScriptPluginMember(Name = "getScreenMedia")]
        public void cefGetScreenMedia(JObject arguments, JavaScriptPluginCallback callback)
        {
            var mainWindow = (Window)this.application.WindowManager.AllWindows[0];
            var showDialog = new Action(() =>
            {

                string[] sources = null;

                if (arguments != null) {
                    var values = JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, string[]>>(arguments.ToString());
                    values.TryGetValue("sources", out sources);
                }

                var window = new MediaStreamPicker.MediaStreamPicker(sources);


                MediaStreamPicker.MediaStreamPickerViewModel vm = new MediaStreamPicker.MediaStreamPickerViewModel(window, sources);
                window.DataContext = vm;

                EventHandler requestCloseHandler = (sender, args) =>
                {
                    callback("permission-denied", null);
                    window.Close();
                };
                vm.RequestCancel += requestCloseHandler;

                EventHandler<MediaStreamPicker.RequestShareEventArgs> requestShareHandler = (sender, args) =>
                {
                    string stream = args.mediaStream;
                    string fileName = args.fileName;
                    string windowTitle = args.windowTitle;

                    if (String.IsNullOrEmpty(stream))
                        callback("media stream selected is null/empty", stream, fileName, windowTitle);
                    else
                        callback(null, stream, fileName, windowTitle);

                    window.Close();
                };
                vm.RequestShare += requestShareHandler;

                window.Owner = mainWindow;
                window.ShowDialog();
            });

            if (!mainWindow.Dispatcher.CheckAccess())
                mainWindow.Dispatcher.Invoke(showDialog);
            else
                showDialog.Invoke();
        }

        //RTC-968 - GetRTCCapabilities 
        [JavaScriptPluginMember(Name = "getRtcCapabilities")]
        public JObject GetRtcCapabilities()
        {
            var json = new JObject();
            json["allowSources"] = true;
            return json;
        }
    }
}
