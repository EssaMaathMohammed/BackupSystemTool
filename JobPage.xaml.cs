using BackupSystemTool.DatabaseClasses;
using Microsoft.Win32;
using MySqlConnector;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Windows.Forms.Design.AxImporter;

namespace BackupSystemTool
{
    /// <summary>
    /// Interaction logic for JobPage.xaml
    /// </summary>
    public partial class JobPage : Window
    {

        private JobItem selectedItem;
        private Button selectedButton;
        private ConnectionItem selectedConnectionItem;
        public JobPage()
        {
            InitializeComponent();
            UpdateJobList();
        }

        private List<JobItem> GetJobList()
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
            jobItemsListView.ItemsSource = GetJobList();
        }

        private void AddJobButton_Click(object sender, RoutedEventArgs e)
        {
            AddJobDialog addJobDialog = new AddJobDialog(this);
            addJobDialog.ShowDialog();
        }

        // click function that gets called when a list item gets selected
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }

        public string GetRelatedDatabases() {
            string relatedDatabases = "";

            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<JobDatabases>();
                List<JobDatabases> jobDatabases = conn.Table<JobDatabases>().Where(jdb => jdb.job_id == selectedItem.id).ToList();

                foreach (JobDatabases database in jobDatabases)
                {
                    relatedDatabases += database.database_name + ", ";
                }
            }

            return relatedDatabases;
        }

        private void UpdateInfoGrid(string jobName, string connectionName, string serverName, string location, string relatedDatabases) {
            jobName_TextBox.Text = jobName;
            ConnectionName_TextBox.Text = connectionName + ", " + serverName;
            Location_TextBox.Text = location;
            relatedDatabases_TextBox.Text = relatedDatabases;
        }

        private void BrowseDatabasesButton_Click(object sender, RoutedEventArgs e)
        {

            if (selectedItem != null)
            {
                BrowseDatabasesDialog browseDatabasesDialog = new BrowseDatabasesDialog(this ,selectedItem, selectedConnectionItem);
                browseDatabasesDialog.ShowDialog();
            }
            else {
                MessageBox.Show("Please Select A Job Before browsing databases");
            }
        }

        private void optionsButton_Click(object sender, RoutedEventArgs e)
        {
            Button contextMenuButton = (Button)sender;
            contextMenuButton.ContextMenu.IsOpen = true;
            selectedButton = contextMenuButton;
        }
         
        private void deleteJobMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this job?", "Confirm Deletion", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // goes to the parent tree until it finds a parent of the button that is a ListViewItem.
                ListViewItem selectedItemObj = FindAncestor<ListViewItem>(selectedButton);
                // sets the item retreived from the FindAncestor method to be selected.
                selectedItemObj.IsSelected = true;

                // cast the selected item to a job item 
                JobItem selectedJopItem = (JobItem)jobItemsListView.SelectedItem;

                // delete the related databases from the jobDatabases table
                using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                {
                    conn.CreateTable<JobDatabases>();
                    List<JobDatabases> jobDatabases = conn.Table<JobDatabases>().Where(jdb => jdb.job_id == selectedItem.id).ToList();

                    foreach (JobDatabases database in jobDatabases)
                    {
                        conn.Delete(database);
                    }
                }
                // delete the job from the job table
                using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                {
                    conn.CreateTable<JobItem>();
                    conn.Delete(selectedItem);
                }

                // update the table
                UpdateJobList();
                UpdateInfoGrid("", "", "", "", "");
            }
        }

        private void jobItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            jobInformation_Grid.Visibility = Visibility.Visible;
            // clear the fields after every change in selection
            UpdateInfoGrid("", "", "", "", "");


            // get the selected item and convert it to a job item
            selectedItem = (JobItem)jobItemsListView.SelectedItem;
            if (selectedItem != null)
            {

                // gets the related Connection item using the ID of the selected item
                using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                {
                    conn.CreateTable<JobItem>();
                    List<ConnectionItem> itemsList = conn.Table<ConnectionItem>().Where(c => c.Id == selectedItem.connection_id).ToList();
                    selectedConnectionItem = itemsList[0];
                }

                // gets the related databases to the job
                string relatedDatabases = GetRelatedDatabases();

                string location = "";
                // get the location depending on the location type of the job item
                if (selectedItem.location_type != null)
                {
                    string locationType = selectedItem.location_type;

                    // select the location item from the related table
                    if (locationType.Equals(App.Locations.LocalLocation.ToString()))
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                        {
                            conn.CreateTable<LocalLocation>();
                            List<LocalLocation> itemsList = conn.Table<LocalLocation>().Where(c => c.job_id == selectedItem.id).ToList();
                            location = "Local Location: " + itemsList[0].local_path;
                        }
                    }
                    else if (locationType.Equals(App.Locations.Snowflake.ToString())) {
                        using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                        {
                            conn.CreateTable<SnowflakeLocation>();
                            List<SnowflakeLocation> itemsList = conn.Table<SnowflakeLocation>().Where(c => c.job_id == selectedItem.id).ToList();
                            location = "Snowflake Location: " + itemsList[0].schema + " " + itemsList[0].database;
                        }
                    }
                    
                }

                // updates the info grid to the info of the selected item
                UpdateInfoGrid(selectedItem.job_name, selectedItem.connection_name, selectedConnectionItem.ServerName, location, relatedDatabases); ;
            }
        }

        // finds the visual parent that is equal to T of the Current Object
        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            // Traverse up the visual tree until we find a parent of the specified type
            while (current != null && !(current is T))
            {
                current = VisualTreeHelper.GetParent(current);
            }

            // Cast the current object to the specified type, or return null if it is not of that type
            return current as T;
        }

        private void backupLocationOptions_Click(object sender, RoutedEventArgs e)
        {
            if (selectedItem != null) { 
                Button? locationsContextMenuButton = sender as Button;
                if (locationsContextMenuButton != null)
                {
                    locationsContextMenuButton.ContextMenu.IsOpen = true;
                }
            }
            else
            {
                MessageBox.Show("Please Select A Job Before selecting location");
            }
        }

        private void localLocationMenuOption_Click(object sender, RoutedEventArgs e)
        {
            // open a directory dialog so the user selectes a location 
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            dialog.Description = "Selecting Local Location";
            dialog.UseDescriptionForTitle = true;
            dialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                if (!string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    // set the location of the saving to the selected location
                    selectedItem.location_type = App.Locations.LocalLocation.ToString();

                    // updates the selected item location type to local location
                    using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                    {
                        conn.CreateTable<JobItem>();
                        conn.Update(selectedItem);
                    }

                    // creates a local location object to be inserted based on the path provided by the dialog
                    LocalLocation location = new LocalLocation()
                    {
                        job_id = selectedItem.id,
                        local_path = dialog.SelectedPath
                    };

                    // insert the item into the LocalLocation Table
                    using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                    {
                        conn.CreateTable<LocalLocation>();
                        conn.Insert(location);
                    }
                    UpdateJobList();
                }
                else {
                    MessageBox.Show("Please Select a valid location!");
                }
            }
        }

        private async void cloudLocationMenuOption_Click(object sender, RoutedEventArgs e)
        {
            SnowflakeCloudLocationDialog locationsDialog = new SnowflakeCloudLocationDialog(this, selectedItem);
            locationsDialog.Show();
        }
    }
}
