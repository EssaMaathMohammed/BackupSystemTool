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

        // Using a DependencyProperty as the backing store for ConnectionItem.
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConnectionItemProperty =
            DependencyProperty.Register("ConnectionItem", typeof(ConnectionItem),
                typeof(ConnectionItemControl), new PropertyMetadata(new ConnectionItem { Name = "Defualt Item",
                    ConnectionString = "Default Connection String" }, setConnectionDetails));

        // The new item (connection Item) gets bound, we call a custom method to assign
        // the values of the new item to the xaml file text blocks
        private static void setConnectionDetails(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ConnectionItemControl itemControls)
            {
                ConnectionItem newConnectionItem = (ConnectionItem)e.NewValue;
                itemControls.ConnectionNameTextBlock.Text = newConnectionItem.Name;
                itemControls.StatusTextBlock.Text = "Status: UP";
                Debug.WriteLine(e.NewValue.ToString());

            }
        }

        public ConnectionItemControl()
        {
            InitializeComponent();
            this.Height = 30;
        }

        private void expandButton_Click(object sender, RoutedEventArgs e)
        {
            if (!expansionStatus)
            {
                moreInfoStackPanel.Visibility = Visibility.Visible;
                this.Height = 150;
                expansionStatus = true;
            }
            else {
                moreInfoStackPanel.Visibility = Visibility.Collapsed;
                this.Height = 30;
                expansionStatus = false;
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            EditConnectionDialog editConnectionDialog = new EditConnectionDialog(ConnectionItem);
            editConnectionDialog.ShowDialog();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(" Are you sure you want to delete this item?", "Delete Confirmation" 
                , System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes) {
                using (SQLiteConnection sqliteConnection = new SQLiteConnection(App.databasePath))
                {
                    sqliteConnection.CreateTable<ConnectionItem>();
                    sqliteConnection.Delete(ConnectionItem);
                }
            }
        }
    }
}
