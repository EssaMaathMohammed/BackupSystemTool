using BackupSystemTool.DatabaseClasses;
using SQLite;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BackupSystemTool
{
    /// <summary>
    /// Interaction logic for AddJobDialog.xaml
    /// </summary>
    public partial class AddJobDialog : Window
    {
        JobPage jobPage;
        public AddJobDialog(JobPage jobPage)
        {
            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            InitializeComponent();

            if (ConnectionsPage.GetConnectionsList().Count > 0)
            {
                List<ConnectionItem> comboBoxItems = new List<ConnectionItem>();
                comboBoxItems.Add(new ConnectionItem
                {
                    ConnectionName = "Select Connection",
                });
                comboBoxItems.AddRange(ConnectionsPage.GetConnectionsList());
                connectionItemsComboBox.ItemsSource = comboBoxItems;
                connectionItemsComboBox.SelectedIndex = 0;
            }
            else {
                List<ConnectionItem> comboBoxItems = new List<ConnectionItem>();
                comboBoxItems.Add(new ConnectionItem
                {
                    ConnectionName = "Please Add Connection Items",
                });
                connectionItemsComboBox.ItemsSource = comboBoxItems;
                connectionItemsComboBox.SelectedIndex = 0;
            }
            this.jobPage = jobPage;
        }
        private void addJobButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectionItem connectionItem = (ConnectionItem)connectionItemsComboBox.SelectedItem;

            JobItem jobItem = new JobItem()
            {
                userId = App.UserId,
                job_name = jobName_TextBox.Text,
                connection_name = connectionItem.ConnectionName,
                connection_id = connectionItem.Id,
            };

            // using with resources, automatically close the connection upon reaching the end of using block
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<JobItem>();
                conn.Insert(jobItem);
            }

            // close the form after adding an item.
            jobPage.UpdateJobList();
            this.Close();
        }
    }
}
