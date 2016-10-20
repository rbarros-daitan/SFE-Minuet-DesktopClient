using System;
using Paragon.Plugins;
using System.Windows;

namespace Symphony.Behaviors
{
    public class AppWinBoundsChangeBehavior
    {
        private IApplication application;
        private Action<IApplicationWindow, Point, Size> onBoundsChange;

        public void AttachTo(IApplication application)
        {
            this.application = application;
            this.application.WindowManager.CreatedWindow += OnWindowCreated;
        }

        public void Subscribe(Action<IApplicationWindow, Point, Size> onBoundsChange)
        {
            this.onBoundsChange = onBoundsChange;
        }

        private void OnWindowCreated(IApplicationWindow applicationWindow, bool isMainWindow)
        {
            applicationWindow.WindowBoundsChanged += onWindowBoundsChanged;
            application.Closed += onApplicationClosed;
        }

        void onWindowBoundsChanged(params object[] args)
        {
            if (args.Length == 3 && args[0] is IApplicationWindow && args[1] is Point && args[2] is Size)
            {
                IApplicationWindow applicationWindow = args[0] as IApplicationWindow;
                Point point = (Point)args[1];
                Size size = (Size)args[2];

                if (this.onBoundsChange != null)
                    this.onBoundsChange(applicationWindow, point, size);
            }
        }

        void onApplicationClosed(object sender, EventArgs e)
        {
            if (sender is IApplicationWindow) 
            {
                IApplicationWindow applicationWindow = sender as IApplicationWindow;
                applicationWindow.WindowBoundsChanged -= onWindowBoundsChanged;
                application.Closed -= onApplicationClosed;
            }
        }

    }
}