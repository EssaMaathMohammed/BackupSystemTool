using Amazon.S3.Transfer;
using Amazon.S3;
using BackupSystemTool.DatabaseClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SQLite;
using Amazon;

namespace BackupSystemTool
{
    // used for managing backup operations across the application
    public class BackupManager
    {
        public JobItem SelectedItem { get; set; }
        public BackupManager(JobItem SelectedItem) {
            this.SelectedItem = SelectedItem;
        }
        // creates a backup using GetFullDatabaseBackup and saves it to the selected Local Location
        public void BackupToLocalLocation(string databaseName)
        {
            try
            {
                // get the backup file name and path
                string strBackupFileName = GetRelatedLocation<string>();

                // create a unique name for the backup
                string backupFileName = GenerateBackupFileName(databaseName);

                // combine the path and unique name of the file
                string backupFullPath = Path.Combine(strBackupFileName, backupFileName);

                // create a new StreamWriter to write the backup file
                StreamWriter strBackupFile = new StreamWriter(backupFullPath);

                // the backup data
                string stdout = GetFullDatabaseBackup(databaseName);

                // write the backup file into the local location
                strBackupFile.WriteLine(stdout);

                // close the file and the backup process
                strBackupFile.Close();

                // show a message box to indicate the backup is done
                MessageBox.Show("Backup done at file:" + strBackupFileName);
            }
            catch (Exception ex)
            {
                // show an error message if the backup process fails
                MessageBox.Show("Error during the backup: \n\n" + ex.Message);
            }
        }

        // creates a backup using GetFullDatabaseBackup and saves it to the selected S3 Bucket
        public void BackupToS3Bucket(string databaseName)
        {
            try
            {
                // get the backup file name and path
                S3Item s3Item = GetRelatedLocation<S3Item>();

                // create a unique name for the backup
                string backupFileName = SelectedItem.job_name + "_backup_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".sql";

                // read the backup data from the standard output
                string backupData = GetFullDatabaseBackup(databaseName);

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

        // creates a full backup of the database name passed and returns it
        private string GetFullDatabaseBackup(string databaseName)
        {
            // set up the process to execute mysqldump
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mysqldumpbin", "mysqldump.exe");
            psInfo.RedirectStandardInput = false;
            psInfo.RedirectStandardOutput = false;

            // create a default connection item
            ConnectionItem connectionInfo = new ConnectionItem() { 
                ServerName= "localhost",
                Username= "root",
                Password= ""
            };

            // select the inforamtion related to the job connections
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<ConnectionItem>();
                List<ConnectionItem> itemsList = conn.Table<ConnectionItem>().Where(c => c.Id == SelectedItem.connection_id).ToList();
                if (itemsList.Count > 0)
                {
                   connectionInfo = itemsList[0];
                }
            }

            // if the password exists put a -p before otherwaise keep it as an empty string
            string password = !connectionInfo.Password.Equals("") ? "-p" + connectionInfo.Password : "";
            Debug.Write($"-u {connectionInfo.Username} {password} -h {connectionInfo.ServerName} --databases {databaseName} --hex-blob");
            // set the needed arguments for the backup
            psInfo.Arguments = $"-u {connectionInfo.Username} {password} -h {connectionInfo.ServerName} --databases {databaseName} --hex-blob";
           
            // pass the username, host parameters from the related job item connection 
            psInfo.UseShellExecute = false;
            psInfo.RedirectStandardOutput = true;

            // start the backup process and capture the standard output
            Process backup_process = Process.Start(psInfo);
            string backupResult = backup_process.StandardOutput.ReadToEnd();

            backup_process.WaitForExit();
            backup_process.Close();

            return backupResult;
        }

        // creates a unique name for the backup file
        private string GenerateBackupFileName(string databasename)
        {
            return SelectedItem.job_name + "_"  + databasename + "_backup_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".sql";
        }

        // gets the backup location of the selected item
        private T GetRelatedLocation<T>() where T : class
        {
            // get the location depending on the location type of the job item
            if (SelectedItem.location_type != null)
            {
                string locationType = SelectedItem.location_type;

                // select the location item from the related table
                if (locationType.Equals(App.Locations.LocalLocation.ToString()))
                {
                    using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                    {
                        conn.CreateTable<LocalLocation>();
                        List<LocalLocation> itemsList = conn.Table<LocalLocation>().Where(c => c.job_id == SelectedItem.id).ToList();
                        return itemsList[0].local_path as T;
                    }
                }
                else if (locationType.Equals(App.Locations.S3Location.ToString()))
                {
                    using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                    {
                        conn.CreateTable<S3Item>();
                        List<S3Item> itemsList = conn.Table<S3Item>().Where(c => c.JobId == SelectedItem.id).ToList();
                        return itemsList[0] as T;
                    }
                }
            }
            return null;
        }
    }
}
