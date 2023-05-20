using Amazon.Runtime.EventStreams.Internal;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BackupSystemTool.Controls
{
    /// <summary>
    /// Interaction logic for ConnectionItemControl.xaml
    /// </summary>
    public partial class ConnectionItemControl : UserControl
    {
        bool expansionStatus = false;
        
        /// <summary>
        /// The Dependency Propoerty is the ConnectionItem bind from the xaml side
        /// the Property Metadata(default, custom) works as an event listener, whenever a change 
        /// happens (object bind) a method gets called
        /// </summary> 
        public ConnectionItem ConnectionItem
        {
            get { return (ConnectionItem)GetValue(ConnectionItemProperty); }
            set { SetValue(ConnectionItemProperty, value); }
        }

        public ConnectionItemControl()
        {
            InitializeComponent();
        }

        // Using a DependencyProperty as the backing store for ConnectionItem.
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConnectionItemProperty =
            DependencyProperty.Register("ConnectionItem", typeof(ConnectionItem),
                typeof(ConnectionItemControl), new PropertyMetadata(new ConnectionItem { ConnectionName = "Defualt Item",
                    ServerName = "Default Server Name" }, setConnectionDetails));

        // The new item (connection Item) gets bound, we call a custom method to assign
        // the values of the new item to the xaml file text blocks
        private static async void setConnectionDetails(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ConnectionItemControl itemControls)
            {
                ConnectionItem newConnectionItem = (ConnectionItem)e.NewValue;
                itemControls.StatusTextBlock.Text = "Connecting";
                itemControls.ConnectionNameTextBlock.Text = newConnectionItem.ConnectionName;
                itemControls.connectionLocation_TextBlock.Text = newConnectionItem.ServerName;
                itemControls.connectionPort_TextBlock.Text = newConnectionItem.PortNumber;
                itemControls.connectionString_TextBlock.Text = newConnectionItem.ConnectionString;
                itemControls.connectionUsername_TextBlock.Text = newConnectionItem.Username;

                await Task.Run(() =>
                {
                    MysqlConnector mysqlConnector = new MysqlConnector();
                    bool isSuccess = false;
                    try
                    {
                        mysqlConnector.GetDatabases(newConnectionItem.ConnectionString);
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                    }

                    itemControls.Dispatcher.Invoke(() =>
                    {
                        itemControls.StatusTextBlock.Text = isSuccess ? "Status: Up" : "Status: Down";
                    });
                });
            }
        }


        private void expandButton_Click(object sender, RoutedEventArgs e)
        {
            if (!expansionStatus)
            {
                moreInfoStackPanel.Visibility = Visibility.Visible;
                this.Height = 180;
                expansionStatus = true;
            }
            else {
                moreInfoStackPanel.Visibility = Visibility.Collapsed;
                this.Height = 40;
                expansionStatus = false;
            }
        }

        private async void editButton_Click(object sender, RoutedEventArgs e)
        {
            EditConnectionDialog editConnectionDialog = new EditConnectionDialog(ConnectionItem,this);
            editConnectionDialog.ShowDialog();
            ConnectionItem updatedConnectionItem = editConnectionDialog.ConnectionItem;

            this.StatusTextBlock.Text = "Connecting";
            this.ConnectionNameTextBlock.Text = updatedConnectionItem.ConnectionName;
            this.connectionLocation_TextBlock.Text = updatedConnectionItem.ServerName;
            this.connectionPort_TextBlock.Text = updatedConnectionItem.PortNumber;
            this.connectionString_TextBlock.Text = updatedConnectionItem.ConnectionString;
            this.connectionUsername_TextBlock.Text = updatedConnectionItem.Username;
            await Task.Run(() =>
            {
                MysqlConnector mysqlConnector = new MysqlConnector();
                bool isSuccess = false;
                try
                {
                    mysqlConnector.GetDatabases(updatedConnectionItem.ConnectionString);
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }

                this.Dispatcher.Invoke(() =>
                {
                    this.StatusTextBlock.Text = isSuccess ? "Status: Up" : "Status: Down";
                });
            });
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool connectionItemUsed = false;
            MessageBoxResult messageBoxResult = MessageBox.Show(" Are you sure you want to delete this item?", "Delete Confirmation" 
                , System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes) {

                // check if the item is being used in the JobPage
                using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                {
                    conn.CreateTable<JobItem>();
                    List<JobItem> relatedJobItems = conn.Table<JobItem>().Where(jbItem => jbItem.connection_id == ConnectionItem.Id).ToList();
                    if (relatedJobItems.Count > 0) { 
                        connectionItemUsed = true;
                    } 
                }

                if (!connectionItemUsed)
                {
                    using (SQLiteConnection sqliteConnection = new SQLiteConnection(App.databasePath))
                    {
                        sqliteConnection.CreateTable<ConnectionItem>();
                        sqliteConnection.Delete(ConnectionItem);

                        this.Visibility = Visibility.Collapsed;
                    }
                }
                else {
                    MessageBox.Show("The Item is in use, Check JobPage for more details");
                }

            }
        }
    }
}
