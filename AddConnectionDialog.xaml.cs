using BackupSystemTool.DatabaseClasses;
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
        public AddConnectionDialog(ConnectionsPage connectionsPage)
        {
            InitializeComponent();
            this.connectionsPage = connectionsPage;
        }

        private void AddConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            // creates a Connection Item which includes ID (auto),
            // Connection Name, Connection String
            ConnectionItem connectionItem = new ConnectionItem()
            {
                ConnectionName = connectionNameTextBox.Text,
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
            connectionsPage.ReadDatabase();
            this.Close();
        }
    }
}
