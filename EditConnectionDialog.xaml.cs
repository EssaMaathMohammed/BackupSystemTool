using BackupSystemTool.Controls;
using BackupSystemTool.DatabaseClasses;
using MySqlConnector;
using SQLite;
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
    /// Interaction logic for EditConnectionDialog.xaml
    /// </summary>
    public partial class EditConnectionDialog : Window
    {
        public ConnectionItem ConnectionItem { get; set; }
        public ConnectionItemControl currentItem { get; set; }
        public EditConnectionDialog(ConnectionItem connectionItem, ConnectionItemControl currentItem)
        {
            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitializeComponent();

            this.ConnectionItem = connectionItem;
            this.currentItem = currentItem;
            connectionNameTextBox.Text = ConnectionItem.ConnectionName;
            connectionServerNameTextBox.Text = ConnectionItem.ServerName;
            usernameTextBox.Text = ConnectionItem.Username;
            userPasswordPasswordBox.Password = ConnectionItem.Password;
            portNumberTextBox.Text = ConnectionItem.PortNumber;
        }

        private void saveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectionItem.ConnectionName = connectionNameTextBox.Text;
            ConnectionItem.ServerName = connectionServerNameTextBox.Text;
            ConnectionItem.Username = usernameTextBox.Text;
            ConnectionItem.Password = userPasswordPasswordBox.Password;
            ConnectionItem.PortNumber = portNumberTextBox.Text;
            
            using (SQLiteConnection sqliteConnection = new SQLiteConnection(App.databasePath))
            {
                sqliteConnection.CreateTable<ConnectionItem>();
                sqliteConnection.Update(this.ConnectionItem);
            }
            this.currentItem.ConnectionNameTextBlock.Text = connectionNameTextBox.Text;
            this.Close();
        }

        private void testConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckMySQLConnection(connectionServerNameTextBox.Text, usernameTextBox.Text, userPasswordPasswordBox.Password, portNumberTextBox.Text))
            {
                connectionStatucLabel.Content = "Connection Successful";
                connectionStatucLabel.Foreground = Brushes.Green;
            }
            else
            {
                connectionStatucLabel.Content = "Connection Failed";
                connectionStatucLabel.Foreground = Brushes.Red;
            }
            connectionStatucLabel.Visibility = Visibility.Visible;
        }

        private bool CheckMySQLConnection(string server, string username, string password,string portNumber)
        {
            string connectionString = $"Server={server};port={portNumber};Uid={username};Pwd={password};";
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (MySqlException)
                {
                    return false;
                }
            }
        }
    }
}
