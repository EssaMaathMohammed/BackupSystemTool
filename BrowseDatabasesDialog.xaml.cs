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
    /// Interaction logic for BrowseDatabasesDialog.xaml
    /// </summary>
    public partial class BrowseDatabasesDialog : Window
    {
        JobPage jobPage;
        JobItem selectedItem;
        ConnectionItem selectedConnectionItem;
        List<string> originalSelectedDatabases = new List<string>();
        public static List<string> selectedDatabases = new List<string>();

        public BrowseDatabasesDialog(JobPage jobPage, JobItem selectedItem, ConnectionItem selectedConnectionItem)
        {
            InitializeComponent();
            this.selectedConnectionItem = selectedConnectionItem;
            this.selectedItem = selectedItem;
            this.jobPage = jobPage;
            // loads the selected database into both litsts original and selected
            LoadSelectedDatabasesList();
            // loads all the databases realte to the mysql server
            UpdateDatabasesList();
        }
       
        public List<string> GetDatabases() {
            MysqlConnector connector = new MysqlConnector();
            List<string> databases = new List<string>();
            foreach (string conn in connector.GetDatabases(selectedConnectionItem.ServerName, selectedConnectionItem.Username, selectedConnectionItem.Password))
            {
                databases.Add(conn);
            }
            return databases;
        }


        // sets the source of the listview to a list of JobDatabase Items
        public void UpdateDatabasesList() {
            // create a list of JobDatabases
            List<JobDatabases> jobDatabases = new List<JobDatabases>();

            // add the databases to the database list
            foreach (string database in GetDatabases())
            {
                jobDatabases.Add(new JobDatabases() { id = selectedItem.id, database_name = database });
            }

            // set the item source of the list to the jobDatabases list
            if (jobDatabases.Count > 0) { 
                databases_ListView.ItemsSource = jobDatabases;
            }
        }
        private void LoadSelectedDatabasesList() {
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<JobDatabases>();
                // Select all database names related to the selected job from the JobDatabases table
                List<JobDatabases> databasesList = conn.Table<JobDatabases>().Where(jdb => jdb.job_id == selectedItem.id).ToList();
                // Add the database names to the selectedDatabases list
                selectedDatabases = databasesList.Select(x => x.database_name).ToList();
                originalSelectedDatabases = databasesList.Select(x => x.database_name).ToList();

            }
        }
        private void AddDatabasesButton_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<JobDatabases>();

                // Add the new databases to the JobDatabases table
                foreach (string databaseName in selectedDatabases)
                {
                    if (!originalSelectedDatabases.Contains(databaseName))
                    {
                        conn.CreateTable<JobDatabases>();
                        // Add the item to the JobDatabases table if it's not already there
                        JobDatabases jobdbItem = new JobDatabases()
                        {
                            job_id = selectedItem.id,
                            database_name = databaseName,
                        };
                        conn.Insert(jobdbItem);
                    }
                }

                // Delete the old databases from the JobDatabases table
                foreach (string databaseName in originalSelectedDatabases)
                {
                    if (!selectedDatabases.Contains(databaseName))
                    {
                        conn.CreateTable<JobDatabases>();
                        // Delete the record from the JobDatabases table with the specified job_id and database_name// Delete the item from the JobDatabases table if it's there
                        conn.Execute("DELETE FROM JobDatabases WHERE job_id = ? AND database_name = ?", selectedItem.id, databaseName);
                    }
                }
            }

            // Update the related job with the new information.
            jobPage.relatedDatabases_TextBox.Text = jobPage.GetRelatedDatabases();

            this.Close();
        }
        private void addDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            Button addButton = sender as Button;
            // set the selected item of the listview to the selected button data context
            databases_ListView.SelectedItem = addButton.DataContext;
            
            // get the selected item as a string
            string selectedDatabaseName = databases_ListView.SelectedItem.ToString();

            if (selectedDatabaseName != null)
            {
                // Check if item already exists in the list
                if (!selectedDatabases.Contains(selectedDatabaseName))
                {
                    // Add the item to the list
                    selectedDatabases.Add(selectedDatabaseName);
                }
            }
        }
        private void deleteDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            // set the selected item of the listview to the selected button data context
            databases_ListView.SelectedItem = deleteButton.DataContext;

            // get the selected item as a string
            string selectedDatabaseName = databases_ListView.SelectedItem.ToString();
        
            if (selectedDatabaseName != null)
            {
                // Check if item exists in the list
                if (selectedDatabases.Contains(selectedDatabaseName))
                {
                    // Remove the item from the list
                    selectedDatabases.Remove(selectedDatabaseName);
                }
            }
        }

    }
}
