using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HWND = System.IntPtr;

namespace Symphony.Plugins.MediaStreamPicker
{

    /// <summary>
    /// Interaction logic for MediaStreamPicker.xaml
    /// </summary>
    public partial class MediaStreamPicker : Window
    {
        public MediaStreamPicker(string[] sources)
        {
            InitializeComponent();
            if (sources != null) {
                //Remove tab Screens if not enabled to share.
                if (!sources.Contains("screen")) {
                    ContentSharePicker.Items.RemoveAt(0);
                }

                //Remove tab Applications if not enabled to share.
                if (!sources.Contains("window"))
                {
                    ContentSharePicker.Items.RemoveAt(1);
                }
            }
        }
    }
}
