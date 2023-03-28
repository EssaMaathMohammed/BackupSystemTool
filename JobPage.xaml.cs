using Amazon.S3.Transfer;
using Amazon.S3;
using BackupSystemTool.DatabaseClasses;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Amazon;

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
                        location = "Local Location: " + GetRelatedLocation<string>();
                    }
                    else if (locationType.Equals(App.Locations.S3Location.ToString())) {
                        location = "S3 Bucket: " + GetRelatedLocation<S3Item>().BucketName;
                    }
                }

                // updates the info grid to the info of the selected item
                UpdateInfoGrid(selectedItem.job_name, selectedItem.connection_name, selectedConnectionItem.ServerName, location, relatedDatabases); ;
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

        private void backupNowButton_Click(object sender, RoutedEventArgs e)
        {
            // checks if the selected item is not null
            if (selectedItem != null)
            {
                // checks if the job has a database/s that is connected to
                // gets the database/s as a list of string
                if (GetRelatedDatabases() != "")
                {
                    // checks if the job has a location/s for backup
                    // gets the location of the backup and sets it as the destination
                    if (selectedItem.location_type == App.Locations.LocalLocation.ToString())
                    {
                        try
                        {
                            // get the backup file name and path
                            string strBackupFileName = GetRelatedLocation<string>();

                            // create a unique name for the backup
                            string backupFileName = selectedItem.job_name + "_backup_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".sql";

                            // combine the path and unique name of the file
                            string backupFullPath = Path.Combine(strBackupFileName, backupFileName);

                            // create a new StreamWriter to write the backup file
                            StreamWriter strBackupFile = new StreamWriter(backupFullPath);

                            // set up the process to execute mysqldump
                            ProcessStartInfo psInfo = new ProcessStartInfo();
                            psInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mysqldumpbin", "mysqldump.exe");
                            psInfo.RedirectStandardInput = false;
                            psInfo.RedirectStandardOutput = false;
                            psInfo.Arguments = "-u root -h localhost --databases testingdatabase --hex-blob";
                            psInfo.UseShellExecute = false;
                            psInfo.RedirectStandardOutput = true;

                            // start the backup process and capture the standard output
                            Process backup_process = Process.Start(psInfo);

                            // the backup data
                            string stdout = backup_process.StandardOutput.ReadToEnd();

                            strBackupFile.WriteLine(stdout);
                            backup_process.WaitForExit();

                            // close the file and the backup process
                            strBackupFile.Close();
                            backup_process.Close();

                            // show a message box to indicate the backup is done
                            MessageBox.Show("Backup done at file:" + strBackupFileName);
                        }
                        catch (Exception ex)
                        {
                            // show an error message if the backup process fails
                            MessageBox.Show("Error during the backup: \n\n" + ex.Message);
                        }
                    }
                    else if (selectedItem.location_type == App.Locations.S3Location.ToString())
                    {
                        try
                        {
                            // get the backup file name and path
                            S3Item s3Item = GetRelatedLocation<S3Item>();

                            // create a unique name for the backup
                            string backupFileName = selectedItem.job_name + "_backup_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".sql";

                            // set up the process to execute mysqldump
                            ProcessStartInfo psInfo = new ProcessStartInfo();
                            psInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mysqldumpbin", "mysqldump.exe");
                            psInfo.RedirectStandardInput = false;
                            psInfo.RedirectStandardOutput = false;
                            psInfo.Arguments = "-u root -h localhost --databases testingdatabase --hex-blob";
                            psInfo.UseShellExecute = false;
                            psInfo.RedirectStandardOutput = true;

                            // start the backup process and capture the standard output
                            Process backup_process = Process.Start(psInfo);

                            // read the backup data from the standard output
                            string backupData = backup_process.StandardOutput.ReadToEnd();

                            backup_process.WaitForExit();

                            // create a new Amazon S3 client
                            AmazonS3Client s3Client = new AmazonS3Client(s3Item.AccessKeyId, s3Item.SecretAccessKey, RegionEndpoint.EUCentral1);

                            // create a transfer utility to upload the backup file to the S3 bucket
                            TransferUtility transferUtility = new TransferUtility(s3Client);

                            // create a transfer request to upload the backup data to the S3 bucket
                            TransferUtilityUploadRequest transferRequest = new TransferUtilityUploadRequest
                            {
                                BucketName = s3Item.BucketName,
                                Key = backupFileName,
                                InputStream = new MemoryStream(Encoding.UTF8.GetBytes(backupData)),
                                CannedACL = S3CannedACL.Private // set the access control to private
                            };

                            // upload the backup data to the S3 bucket
                            transferUtility.Upload(transferRequest);

                            // show a message box to indicate the backup is done
                            MessageBox.Show("Backup done and uploaded to S3 bucket:" + s3Item.BucketName);
                        }
                        catch (AmazonS3Exception s3Exception)
                        {
                            // show an error message if the S3 upload fails
                            MessageBox.Show("Error uploading backup to S3: \n\n" + s3Exception.Message);
                        }
                        catch (Exception ex)
                        {
                            // show an error message if the backup process fails
                            MessageBox.Show("Error during the backup: \n\n" + ex.Message);
                        }
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


        private void scheduleBackupButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
