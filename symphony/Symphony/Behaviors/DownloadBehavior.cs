using System.Diagnostics;
using Paragon.Plugins;

namespace Symphony.Behaviors
{
    public class DownloadBehavior
    {
        public void AttachTo(IApplicationWindow applicationWindow)
        {
            applicationWindow.DownloadProgress += this.OnDownloadProgress;
        }
        
        private void OnDownloadProgress(object sender, DownloadProgressEventArgs args)
        {
            /* This code has to be supressed. Download Control dont requires to launch explorer at download´s end.
            if (args.IsComplete)
            {
                Process.Start("explorer.exe", "/Select, " + args.FullPath);
            }
            */ 
        }
    }
}
