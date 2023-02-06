using Gu.Wpf.Adorners;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BackupSystemTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidatePin())
            {
                Cryptograpy encryption = new Cryptograpy(); 
                string cipherText =  encryption.encryptStringAES(PINTextBox.Password, App.EncryptionKey);
                Debug.WriteLine(cipherText);
                //ConnectionsPage connectionsPage = new ConnectionsPage();
                //connectionsPage.Show();
                //this.Close();
            }
        }

        private bool ValidatePin()
        {
            if (string.IsNullOrEmpty(PINTextBox.Password))
            {
                MessageBox.Show("Please enter PIN", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (!Regex.IsMatch(PINTextBox.Password, @"^[a-zA-Z0-9!@#\$%\^&\*()_+-=\{\}\[\];':\"",\.<>\/\?]+$"))
            {
                MessageBox.Show("PIN can only contain letters, numbers, and special characters", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (PINTextBox.Password.Length < 6 || PINTextBox.Password.Length > 12)
            {
                MessageBox.Show("PIN must be between 6 and 12 characters long", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            return true;
        }

        

    }
}
