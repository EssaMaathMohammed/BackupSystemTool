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
        public ConnectionItem connectionItem { get; set; }
        public EditConnectionDialog(ConnectionItem connectionItem)
        {
            InitializeComponent();
            this.connectionItem = connectionItem;
            connectionNameTextBox.Text = connectionItem.Name;
        }

        private void saveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            this.connectionItem.Name = connectionNameTextBox.Text;
            using (SQLiteConnection sqliteConnection = new SQLiteConnection(App.databasePath))
            {
                sqliteConnection.CreateTable<ConnectionItem>();
                sqliteConnection.Update(this.connectionItem);
            }

        }
    }
}
