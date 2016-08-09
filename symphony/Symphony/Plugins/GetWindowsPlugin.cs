using Paragon.Plugins;
using System.Windows;
using System;

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

        [JavaScriptPluginMember(Name = "getScreenMedia")]
        public void cefGetScreenMedia(JavaScriptPluginCallback callback)
        {
            var mainWindow = (Window)this.application.WindowManager.AllWindows[0];

            var showDialog = new Action(() =>
            {
                var window = new MediaStreamPicker.MediaStreamPicker();
                MediaStreamPicker.MediaStreamPickerViewModel vm = new MediaStreamPicker.MediaStreamPickerViewModel(window);
                window.DataContext = vm;

                EventHandler requestCloseHandler = (sender, args) => window.Close();
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

    }
}
