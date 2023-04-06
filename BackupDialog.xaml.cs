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
    /// Interaction logic for BackupDialog.xaml
    /// </summary>
    public partial class BackupDialog : Window
    {
        public BackupDialog()
        {
            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            InitializeComponent();
        }

        private void backupOnceRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            backupOnceStackPanel.Visibility = Visibility.Visible;
            backupStackPanel.Visibility = Visibility.Collapsed;
        }

        private void scheduleBackupRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            backupOnceStackPanel.Visibility = Visibility.Collapsed;
            backupStackPanel.Visibility = Visibility.Visible;
        }
    }
}
