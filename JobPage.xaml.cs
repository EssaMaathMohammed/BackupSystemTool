
using BackupSystemTool.DatabaseClasses;
using SQLite;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Reflection;
using System.IO;

namespace BackupSystemTool
{
    /// <summary>
    /// Interaction logic for JobPage.xaml
    /// </summary>
    public partial class JobPage : Window
    {

        private JobItem selectedItem;
        private Button selectedButton;
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private ConnectionItem selectedConnectionItem;
        public JobPage()
        {
            InitializeComponent();
            StartSchedules();
            UpdateJobList();

            // Get the icon file path
            string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string relativeIconPath = @"..\..\..\resources\icons\icon-testing-note-book-report-testing-177786230.ico";
            string absoluteIconPath = Path.GetFullPath(Path.Combine(basePath, relativeIconPath));

            // Initialize the notification icon
            _notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = new System.Drawing.Icon(absoluteIconPath),
                Visible = false,
            };
            // Add event handler for restoring the app on double-click
            _notifyIcon.MouseDoubleClick += (s, e) => ShowAndActivate();
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

        private void jobItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            jobInformation_Grid.Visibility = Visibility.Visible;
            // clear the fields after every change in selection
            UpdateInfoGrid("", "", "", "", "", "");


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
                        location = "Local Location: " + GetRelatedLocation<string>();
                    }
                    else if (locationType.Equals(App.Locations.S3Location.ToString())) {
                        location = "S3 Bucket: " + GetRelatedLocation<S3Item>().BucketName;
                    }
                }

                string schedule = "";
                // gets the related Connection item using the ID of the selected item
                using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                {
                    conn.CreateTable<BackupSchedule>();
                    List<BackupSchedule> itemsList = conn.Table<BackupSchedule>().Where(c => c.job_id == selectedItem.id).ToList();

                    if (itemsList.Count > 0)
                    {
                        schedule = "Every " + itemsList[0].Interval + " " + itemsList[0].IntervalType;
                    }
                }

                // updates the info grid to the info of the selected item
                UpdateInfoGrid(selectedItem.job_name, selectedItem.connection_name,
                    selectedConnectionItem.ServerName, location, relatedDatabases, schedule); ;
            }
        }

        private void UpdateInfoGrid(string jobName, string connectionName, string serverName, string location, string relatedDatabases, string schedule) {
            jobName_TextBox.Text = jobName;
            ConnectionName_TextBox.Text = connectionName + ", " + serverName;
            Location_TextBox.Text = location;
            relatedDatabases_TextBox.Text = relatedDatabases;
            ScheduleBackup_TextBox.Text = schedule;
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
                UpdateInfoGrid("", "", "", "", "", "");
            }
        }

        // gets the backup location of the selected item
        private T GetRelatedLocation<T>() where T : class
        {
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
                        return itemsList[0].local_path as T;
                    }
                }
                else if (locationType.Equals(App.Locations.S3Location.ToString()))
                {
                    using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                    {
                        conn.CreateTable<S3Item>();
                        List<S3Item> itemsList = conn.Table<S3Item>().Where(c => c.JobId == selectedItem.id).ToList();
                        return itemsList[0] as T;
                    }
                }
            }
            return null;
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
                    MessageBox.Show(dialog.SelectedPath);
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

        private void cloudLocationMenuOption_Click(object sender, RoutedEventArgs e)
        {
            S3LocationDialog locationsDialog = new S3LocationDialog(this, selectedItem);
            locationsDialog.Show();
        }

        // starts the backup operation
        private void backupNowButton_Click(object sender, RoutedEventArgs e)
        {
            // checks if the selected item is not null
            if (selectedItem != null)
            {
                BackupManager backupManager = new BackupManager(selectedItem);
                // checks if the job has a database/s that is connected to
                // gets the database/s as a list of string
                if (GetRelatedDatabases() != "")
                {
                    // checks if the job has a location/s for backup
                    // gets the location of the backup and sets it as the destination
                    if (selectedItem.location_type == App.Locations.LocalLocation.ToString())
                    {
                        backupManager.BackupToLocalLocation("testingdatabase");
                    }
                    else if (selectedItem.location_type == App.Locations.S3Location.ToString())
                    {
                        backupManager.BackupToS3Bucket("testingdatabase");
                    }
                    else if (selectedItem.location_type == null)
                    {
                        // show a message if no location is selected for the backup
                        MessageBox.Show("Please Select a location for the backup");
                    }
                }
                else
                {
                    // show a message if no databases are selected for the backup
                    MessageBox.Show("Please Select Databases to be backed up first.");
                }
            }
            else
            {
                // show a message if no job is selected for the backup
                MessageBox.Show("Please Select an item first to start backup.");
            }
        }

        // opens InsertScheduleDialog to create a new Schedule
        private void scheduleBackupButton_Click(object sender, RoutedEventArgs e)
        {
            // checks if the selected item is not null
            if (selectedItem != null)
            {
                // gets the database/s as a list of string
                if (GetRelatedDatabases() != "")
                {
                    InsertScheduleDialog insertScheduleDialog = new InsertScheduleDialog(this, selectedItem); insertScheduleDialog.Show();
                }
                else
                {
                    // show a message if no databases are selected for the backup
                    MessageBox.Show("Please Select Databases to be backed up first.");
                }
            }
            else
            {
                // show a message if no job is selected for the backup
                MessageBox.Show("Please Select an item first to start backup.");
            }
        }

        // this function needs to be moved from this page to after the login operation of the user
        private void StartSchedules()
        {
            // get a list of all the job items
            List<JobItem> jobItems;
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<JobItem>();
                jobItems = conn.Table<JobItem>().ToList();
            }

            // for each job item start select the schedules related to them and start scheduling them
            foreach (JobItem jobItem in jobItems)
            {
                // create a schedule manager to add the schedules
                BackupScheduleManager scheduleManager = new BackupScheduleManager(null,jobItem);

                using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                {
                    conn.CreateTable<BackupSchedule>();
                    List<BackupSchedule> jobSchedules = conn.Table<BackupSchedule>().Where(schd => schd.job_id == jobItem.id).ToList();

                    // for each schedule related to the job (each job may have multiple schedules)
                    // we need to add it to the timer list and start it
                    foreach (BackupSchedule backupSchedule in jobSchedules)
                    {
                        scheduleManager.AddSchedule(backupSchedule);
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Show the custom closing dialog
            var closeDialog = new ClosingDialog();
            closeDialog.ShowDialog();

            // Get the user's choice from the dialog
            ClosingDialog.UserChoice result = closeDialog.Choice;

            // Perform action based on the user's choice
            switch (result)
            {
                case ClosingDialog.UserChoice.Close:
                    _notifyIcon.Dispose(); // Dispose of the notification icon resources
                    break;
                case ClosingDialog.UserChoice.RunInBackground:
                    Hide(); // Hide the main window
                    _notifyIcon.Visible = true; // Show the notification icon
                    e.Cancel = true; // Cancel the closing event
                    break;
                case ClosingDialog.UserChoice.Cancel:
                    e.Cancel = true; // Cancel the closing event
                    break;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _notifyIcon.Dispose(); // Dispose of the notification icon resources when the window is closed
        }

        private void ShowAndActivate()
        {
            Show(); // Show the main window
            WindowState = WindowState.Normal; // Set the window state to normal
            Activate(); // Activate the window to bring it to the top and give it focus
            _notifyIcon.Visible = false; // Hide the notification icon
        }
    }
}
