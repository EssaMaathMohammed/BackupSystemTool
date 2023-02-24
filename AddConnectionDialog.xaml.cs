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
    /// Interaction logic for AddConnectionDialog.xaml
    /// </summary>
    public partial class AddConnectionDialog : Window
    {
        public AddConnectionDialog()
        {
            InitializeComponent();
        }

        private void LocalDatabaseRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            localDatabaseStackPanel.Visibility = Visibility.Visible;
            cloudDatabaseStackPanel.Visibility = Visibility.Collapsed;  
            lanDatabaseStackPanel.Visibility = Visibility.Collapsed;
        }

        private void CloudDatabaseRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            cloudDatabaseStackPanel.Visibility = Visibility.Visible;
            localDatabaseStackPanel.Visibility = Visibility.Collapsed;
            lanDatabaseStackPanel.Visibility = Visibility.Collapsed;
        }

        private void LanDatabaseRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            lanDatabaseStackPanel.Visibility = Visibility.Visible;
            cloudDatabaseStackPanel.Visibility = Visibility.Collapsed;
            localDatabaseStackPanel.Visibility = Visibility.Collapsed;
        }

        private void useConnectionStringRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void useParametersStringRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
