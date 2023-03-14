using BackupSystemTool.DatabaseClasses;
using Snowflake.Data.Client;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BackupSystemTool
{
    /// <summary>
    /// Interaction logic for SnowflakeCloudLocationDialog.xaml
    /// </summary>
    public partial class SnowflakeCloudLocationDialog : Window
    {
        public JobPage jobPage { get; set; }
        public JobItem selectedItem { get; set; }
        public SnowflakeLocation snowflakeLocationItem { get; set; }
        public SnowflakeCloudLocationDialog(JobPage jobPage, JobItem selectedItem) 
        {
            this.jobPage = jobPage;
            this.selectedItem = selectedItem;
            InitializeComponent();
        }

        private void addLocationButton_Click(object sender, RoutedEventArgs e)
        {
            // set the inforamtion of the snowflake item
            SetSnowflakeItemInforamtion();

            // inserts the snowflake locatio nitem item to the Snowflake location table
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<SnowflakeLocation>();
                conn.Insert(snowflakeLocationItem);
            }

            // set the type of location in the job table
            selectedItem.location_type = App.Locations.Snowflake.ToString();

            // updates the selected item location type
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<JobItem>();
                conn.Update(selectedItem);
            }

            // update the job list
            jobPage.UpdateJobList();

            // close the dialog
            this.Close();
        }

        private void testConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            // set the inforamtion of the snowflake item
            SetSnowflakeItemInforamtion();
            // get the snowflake item connection string
            string snowflakeConnectionString = GetSnowflakeConnectionString();

            try
            {
                using (SnowflakeDbConnection connection = new SnowflakeDbConnection(snowflakeConnectionString))
                {
                    connection.Open();
                    connectionStatusLabel.Content = "Connection Successful";
                    connectionStatusLabel.Foreground = Brushes.Green;
                    connectionStatusLabel.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                connectionStatusLabel.Content = "Connection Failed";
                connectionStatusLabel.Foreground = Brushes.Red;
                connectionStatusLabel.Visibility = Visibility.Visible;
            }

        }

        // sets the information of the snowflake item to the fields
        private void SetSnowflakeItemInforamtion() {
            snowflakeLocationItem = new SnowflakeLocation()
            {
                job_id = selectedItem.id,
                account = accountName_TextBox.Text,
                user = userName_TextBox.Text,
                password = password_PasswordBox.Password,
                database = database_TextBox.Text,
                schema = schema_TextBox.Text,
                warehouse = warehouse_TextBox.Text
            };
        }

        // creates a connection string from the snowflake item and return it
        private string GetSnowflakeConnectionString()
        {
            string snowflakeConnectionString = $"account={snowflakeLocationItem.account};user={snowflakeLocationItem.user};password={snowflakeLocationItem.password};db={snowflakeLocationItem.database};schema={snowflakeLocationItem.schema};warehouse={snowflakeLocationItem.warehouse};";
            return snowflakeConnectionString;
        }

    }
}
