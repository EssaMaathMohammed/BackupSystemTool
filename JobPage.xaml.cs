using BackupSystemTool.DatabaseClasses;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BackupSystemTool
{
    /// <summary>
    /// Interaction logic for JobPage.xaml
    /// </summary>
    public partial class JobPage : Window
    {
        public JobPage()
        {
            InitializeComponent();
            UpdateJobList();
        }

        public List<JobItem> GetJobList()
        {
            // Creates a list of JobItem
            List<JobItem> jobItems = new List<JobItem>();
            // using with resources, automatically close the connection upon reaching the end of using block
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<JobItem>();
                jobItems = conn.Table<JobItem>().ToList();
            }
            return jobItems;
        }

        public void UpdateJobList()
        {
            if (GetJobList().Count > 0)
            {
                jobItemsListView.ItemsSource = GetJobList();
            }
        }

        private void AddJobButton_Click(object sender, RoutedEventArgs e)
        {
            AddJobDialog addJobDialog = new AddJobDialog(this);
            addJobDialog.ShowDialog();
        }

        // click function that gets called when a list item gets selected
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            jobInformation_Grid.Visibility = Visibility.Visible;

            // sets the inforamtion of the jobInfo grid to the selected item information
            JobItem selectedItem = (JobItem)jobItemsListView.SelectedItem;
            ConnectionItem selectedConnectionItem;
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<JobItem>();
                List<ConnectionItem> itemsList = conn.Table<ConnectionItem>().Where(c => c.Id == selectedItem.connection_id).ToList();
                selectedConnectionItem = itemsList[0];
            }
            jobName_TextBox.Text = selectedItem.job_name;
            ConnectionName_TextBox.Text = selectedItem.connection_name + " " + selectedConnectionItem.ServerName;
            Location_TextBox.Text = selectedItem.location;
        }

        private void BrowseDatabasesButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
