using BackupSystemTool.DatabaseClasses;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for ConnectionsPage.xaml
    /// </summary>
    public partial class ConnectionsPage : Window
    {
        public ConnectionsPage()
        {
            InitializeComponent();
            UpdateConnectionList();
        }

        // Reading the fields of the database ConnectionItem Table
        public static List<ConnectionItem> GetConnectionsList()
        {
            List<ConnectionItem> connectionItems = new List<ConnectionItem>();
            // using with resources, automatically close the connection upon reaching the end of using block
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<ConnectionItem>();
                connectionItems = conn.Table<ConnectionItem>().ToList();
            }

            return connectionItems;
        }

        public void UpdateConnectionList() {
            if (GetConnectionsList().Count > 0)
            {
                ConnectionsListView.ItemsSource = GetConnectionsList();
            }
        }

        private void addConnection_Click(object sender, RoutedEventArgs e)
        {
            AddConnectionDialog dialog = new AddConnectionDialog(this);
            dialog.ShowDialog();
        }
    }
}
