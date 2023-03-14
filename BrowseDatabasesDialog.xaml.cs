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


        public BrowseDatabasesDialog(JobPage jobPage, JobItem selectedItem, ConnectionItem selectedConnectionItem)
        {
            InitializeComponent();
            this.selectedConnectionItem = selectedConnectionItem;
            this.selectedItem = selectedItem;
            this.jobPage = jobPage;
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
        public void UpdateDatabasesList() { 
            List<string> databases = GetDatabases();
            if (databases.Count > 0) { 
                databases_ListView.ItemsSource = databases;
            }
        }

        // if in the future the user needs to change the databases related with the job 
        // we need to follow the following steps 
        // 1- bring all databases related to the job, 2- either remove the databases or add the new ones to the old ones
        private void AddDatabasesButton_Click(object sender, RoutedEventArgs e)
        {
            // gets the selected items as a list
            List<string> selectedItemsList = databases_ListView.SelectedItems.Cast<string>().ToList();

            // create the table of the JobDatabases
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<JobDatabases>();
                // adds the selected items getting the job id and the database name and adding it to the JobDatabases table

                foreach (string databaseName in selectedItemsList)
                {
                    JobDatabases jobdbItem = new JobDatabases()
                    {
                        job_id = selectedItem.id,
                        database_name = databaseName,
                    };
                    conn.Insert(jobdbItem);
                }
            }
            // update the related job with the new information. -- NOT DONE --
            jobPage.relatedDatabases_TextBox.Text = jobPage.GetRelatedDatabases();

            this.Close();
        }

            
    }
}
