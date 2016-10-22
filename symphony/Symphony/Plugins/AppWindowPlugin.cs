using System;
using System.Linq;
using System.Windows;
using Paragon.Plugins;
using Symphony.Behaviors;
using Symphony.Win32;

namespace Symphony.Plugins
{
    [JavaScriptPlugin(Name = "symphony.app.window", IsBrowserSide = true)]
    public class AppWindowPlugin : IParagonPlugin
    {
        [JavaScriptPluginMember(Name = "onWindowsBoundChanged")]
        public event JavaScriptPluginCallback onWindowsBoundChanged;

        private IApplication application;
        private CloseAllWindowBehavior closeAllWindowBehavior;

        public void Initialize(IApplication application)
        {
            this.application = application;

            this.closeAllWindowBehavior = new CloseAllWindowBehavior();

            var applicationLoadBehavior = new ApplicationLoadBehavior();
            applicationLoadBehavior.AttachTo(application);
            applicationLoadBehavior.Subscribe(
                applicationWindow =>
                {
                    this.closeAllWindowBehavior.AttachTo(application, applicationWindow);
                });

            var appWinBoundsChangeBehavior = new AppWinBoundsChangeBehavior();
            appWinBoundsChangeBehavior.AttachTo(application);
            appWinBoundsChangeBehavior.Subscribe(
                (appWindow, point, size) =>
                {
                    string windowName = appWindow.GetId();
                    int x = (int)point.X;
                    int y = (int)point.Y;
                    int w = (int)size.Width;
                    int h = (int)size.Height;
                    this.onWindowsBoundChanged(windowName, x, y, w, h);
                });
        }

        public void Shutdown()
        {
            
        }

        [JavaScriptPluginMember]
        public void raiseBoundsChangeEvent(string windowName)
        {
            IApplicationWindow applicationWindow;
            var window = this.GetWindowByName(windowName, out applicationWindow);

            if (applicationWindow != null)
            {
                window.Dispatcher.Invoke(new Action(() =>
                {
                    applicationWindow.RaiseBoundsChangeEvent();
                }));
            }
        }

        [JavaScriptPluginMember]
        public bool IsWindowActive(string name)
        {
            IApplicationWindow applicationWindow;
            var window = this.GetWindowByName(name, out applicationWindow);
            
            return (bool)window.Dispatcher.Invoke(new Func<bool>(() =>
            {
                return window.IsActive;
            }));
        }

        [JavaScriptPluginMember]
        public void ShowWindow(string name)
        {
            IApplicationWindow applicationWindow;
            var window = this.GetWindowByName(name, out applicationWindow);

            window.Dispatcher.Invoke(new Action(() =>
            {
                Win32Api.ShowWindow(applicationWindow.Handle);
            }));
        }

        [JavaScriptPluginMember]
        public void ShowWindowWithNoActivate(string name)
        {
            IApplicationWindow applicationWindow;
            var window = this.GetWindowByName(name, out applicationWindow);

            window.Dispatcher.Invoke(new Action(() =>
            {
                window.WindowState = WindowState.Normal;
                Win32Api.ShowWithNoActivate(applicationWindow.Handle, handler => window.Loaded += handler);
                window.Show();
            }));
        }

        private Window GetWindowByName(string name, out IApplicationWindow applicationWindow)
        {
            applicationWindow = this.application
                .WindowManager
                .AllWindows.
                FirstOrDefault(win => win.GetId() == name);

            return applicationWindow as Window;
        }


    }
}
