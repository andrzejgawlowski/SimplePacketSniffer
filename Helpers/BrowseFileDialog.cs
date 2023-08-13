using System;
using System.Windows.Controls;

namespace packetSniffer
{
    class BrowseFileDialog:MainWindow
    {
        public static void OpenFileDialog(object o, EventArgs ea, TextBox dialogBox)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            //dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Text files (*.txt)|*.txt|Json files (*.js)|*.js|All files (*.*)|*.*"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
                dialogBox.Text = dialog.FileName;
        }

    }
}
