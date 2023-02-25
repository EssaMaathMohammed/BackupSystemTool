using BackupSystemTool.Controls;
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
    /// Interaction logic for EditConnectionDialog.xaml
    /// </summary>
    public partial class EditConnectionDialog : Window
    {
        public ConnectionItem ConnectionItem { get; set; }
        public ConnectionItemControl currentItem { get; set; }
        public EditConnectionDialog(ConnectionItem connectionItem, ConnectionItemControl currentItem)
        {
            InitializeComponent();
            this.ConnectionItem = connectionItem;
            this.currentItem = currentItem;
            connectionNameTextBox.Text = connectionItem.ConnectionName;
            connectionServerNameTextBox.Text = connectionItem.ServerName;
        }

        private void saveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectionItem.ConnectionName = connectionNameTextBox.Text;
            ConnectionItem.ServerName = connectionServerNameTextBox.Text;

            using (SQLiteConnection sqliteConnection = new SQLiteConnection(App.databasePath))
            {
                sqliteConnection.CreateTable<ConnectionItem>();
                sqliteConnection.Update(this.ConnectionItem);
            }
            this.currentItem.ConnectionNameTextBlock.Text = connectionNameTextBox.Text;
            this.Close();
        }
    }
}
