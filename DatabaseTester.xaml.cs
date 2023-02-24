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
    /// Interaction logic for DatabaseTester.xaml
    /// testing database functionalities
    /// </summary>
    public partial class DatabaseTester : Window
    {
        public DatabaseTester()
        {
            InitializeComponent();
        }
        private void addConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            // creates a Connection Item which includes ID (auto),
            // Connection Name, Connection String
            ConnectionItem connectionItem = new ConnectionItem()
            {
                Name = databaseNameTextBox.Text,
                ConnectionString = databaseConnection.Text
            };

            // using with resources, automatically close the connection upon reaching the end of using block
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath)) 
            {
                conn.CreateTable<ConnectionItem>();
                conn.Insert(connectionItem);
            }
        }
        
        

        // navigate to the Connection Page (where connections will be listed and viewd)
        private void ConnectionPageButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectionsPage connectionsPage = new ConnectionsPage();
            connectionsPage.Show();
            this.Close();
        }
        private void MysqlConnector_Click(object sender, RoutedEventArgs e)
        {
            MysqlConnector connector = new MysqlConnector();
            string databases = "";
            foreach (string conn in connector.GetDatabases("localhost", "root", "")) {
                databases += conn + " ";
            }
            MessageBox.Show(databases);
        }

    }
}
