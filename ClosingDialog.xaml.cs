using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BackupSystemTool
{
    /// <summary>
    /// Interaction logic for ClosingDialog.xaml
    /// </summary>
    public partial class ClosingDialog : Window
    {
        // Enum for representing user choices
        public enum UserChoice
        {
            Close,
            RunInBackground,
            Cancel
        }

        // Property to store the user's choice
        public UserChoice Choice { get; private set; }

        public ClosingDialog()
        {
            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitializeComponent();

        }

        // Event handler for Close button click
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Choice = UserChoice.Close;
            Close();
        }

        // Event handler for Run in Background button click
        private void BackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            Choice = UserChoice.RunInBackground;
            Close();
        }

        // Event handler for Cancel button click
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Choice = UserChoice.Cancel;
            Close();
        }
    }
}
