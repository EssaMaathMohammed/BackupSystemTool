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
    /// Interaction logic for AddConnectionDialog.xaml
    /// </summary>
    public partial class AddConnectionDialog : Window
    {
        ConnectionsPage connectionsPage;
        public int UserId { get; set; }
        public AddConnectionDialog(ConnectionsPage connectionsPage, int UserId)
        {
            this.connectionsPage = connectionsPage;
            this.UserId = UserId;

            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            InitializeComponent();
        }

        private void AddConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            // creates a Connection Item which includes ID (auto),
            // Connection Name, Connection String
            ConnectionItem connectionItem = new ConnectionItem()
            {
                ConnectionName = connectionNameTextBox.Text,
                UserId = this.UserId,
                Username = usernameTextBox.Text,
                Password = userPasswordPasswordBox.Password,
                ServerName = serverNameTextBox.Text,
                PortNumber = portNumberTextBox.Text,
            };

            // using with resources, automatically close the connection upon reaching the end of using block
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<ConnectionItem>();
                conn.Insert(connectionItem);
            }
            connectionsPage.UpdateConnectionList();
            this.Close();
        }

        private void testConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckMySQLConnection(serverNameTextBox.Text, usernameTextBox.Text, userPasswordPasswordBox.Password, portNumberTextBox.Text))
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

        private bool CheckMySQLConnection(string server, string username, string password, string portNumber)
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
