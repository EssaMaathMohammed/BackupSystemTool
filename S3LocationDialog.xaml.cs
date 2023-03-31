using Amazon.S3.Model;
using Amazon.S3;
using BackupSystemTool.DatabaseClasses;
using System;
using System.Windows;
using System.Windows.Media;
using Amazon;
using SQLite;

namespace BackupSystemTool
{
    /// <summary>
    /// Interaction logic for S3LocationDialog.xaml
    /// </summary>
    public partial class S3LocationDialog : Window
    {
        public JobPage jobPage { get; set; }
        public JobItem selectedItem { get; set; }
        public S3LocationDialog(JobPage jobPage, JobItem selectedItem)
        {
            this.jobPage = jobPage;
            this.selectedItem = selectedItem;
            InitializeComponent();
        }

        private void testConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            string bucketName = bucketName_TextBox.Text;
            string region = region_TextBox.Text;
            string accessKey = accessKey_TextBox.Text;
            string secretKey = secretKey_TextBox.Text;
            try
            {
                var s3Config = new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(region)
                };

                var s3Client = new AmazonS3Client(accessKey, secretKey, s3Config);

                var listObjectsRequest = new ListObjectsRequest
                {
                    BucketName = bucketName,
                    MaxKeys = 1
                };

                var response = s3Client.ListObjectsAsync(listObjectsRequest).Result;

                connectionStatusLabel.Content = "Connection Successful";
                connectionStatusLabel.Foreground = Brushes.Green;
                connectionStatusLabel.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                connectionStatusLabel.Content = "Connection Failed";
                connectionStatusLabel.Foreground = Brushes.Red;
                connectionStatusLabel.Visibility = Visibility.Visible;
            }
        }

        private void addLocationButton_Click(object sender, RoutedEventArgs e)
        {
            S3Item insertItem = new S3Item() {
                JobId = selectedItem.id,
                BucketName = bucketName_TextBox.Text,
                Region = region_TextBox.Text,
                AccessKeyId = accessKey_TextBox.Text,
                SecretAccessKey = secretKey_TextBox.Text
            };
            // inserts the snowflake locatio nitem item to the Snowflake location table
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<S3Item>();
                conn.Insert(insertItem);
            }

            // set the type of location in the job table
            selectedItem.location_type = App.Locations.S3Location.ToString();

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
    }
}
