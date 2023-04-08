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
        private JobItem SelectedItem { get; set; }
        public BackupManager(JobItem SelectedItem) {
            this.SelectedItem = SelectedItem;
        }

        // creates a backup using GetFullDatabaseBackup and saves it to the selected Local Location
        private void BackupToLocalLocation(string backupFileName, string encryptedBackupData)
        {
            try
            {
                // Get the local backup location
                string localBackupLocation = GetRelatedLocation<string>();

                // Combine the path and unique name of the file
                string backupFullPath = Path.Combine(localBackupLocation, backupFileName);

                // Create a new StreamWriter to write the backup file
                StreamWriter strBackupFile = new StreamWriter(backupFullPath);

                // Write the encrypted backup data into the local location
                strBackupFile.WriteLine(encryptedBackupData);

                // Close the file and the backup process
                strBackupFile.Close();

                // Show a message box to indicate the backup is done
                MessageBox.Show("Backup done at file:" + localBackupLocation);
            }
            catch (Exception ex)
            {
                // Show an error message if the backup process fails
                MessageBox.Show("Error during the backup: \n\n" + ex.Message);
            }
        }

        // creates a backup using GetFullDatabaseBackup and saves it to the selected S3 Bucket
        private void BackupToS3Bucket(string backupFileName, string encryptedBackupData)
        {
            try
            {
                // Get the S3 backup location
                S3Item s3Item = GetRelatedLocation<S3Item>();

                // Create a new Amazon S3 client
                AmazonS3Client s3Client = new AmazonS3Client(s3Item.AccessKeyId, s3Item.SecretAccessKey, RegionEndpoint.EUCentral1);

                // Create a transfer utility to upload the backup file to the S3 bucket
                TransferUtility transferUtility = new TransferUtility(s3Client);

                // Create a transfer request to upload the encrypted backup data to the S3 bucket
                TransferUtilityUploadRequest transferRequest = new TransferUtilityUploadRequest
                {
                    BucketName = s3Item.BucketName,
                    Key = backupFileName,
                    InputStream = new MemoryStream(Encoding.UTF8.GetBytes(encryptedBackupData)),
                    CannedACL = S3CannedACL.Private // Set the access control to private
                };

                // Upload the encrypted backup data to the S3 bucket
                transferUtility.Upload(transferRequest);

                // Show a message box to indicate the backup is done
                MessageBox.Show("Backup done and uploaded to S3 bucket:" + s3Item.BucketName);
            }
            catch (AmazonS3Exception s3Exception)
            {
                // Show an error message if the S3 upload fails
                MessageBox.Show("Error uploading backup to S3: \n\n" + s3Exception.Message);
            }
            catch (Exception ex)
            {
                // Show an error message if the backup process fails
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

        // The GenerateBackup method that creates a DEK, encrypts the backup, and saves the DEK and the backup name in the backupInfo table
        public void GenerateEncryptedBackup(string databaseName)
        {
            // Generate a unique backup file name
            string backupFileName = GenerateBackupFileName(databaseName);

            // Get the full database backup
            string backupData = GetFullDatabaseBackup(databaseName);

            // Create a KeyGenerator for the user operations passing the user id,
            // this constructor creates a random salt to be used in the operations
            KeyGenerator keyGenerator = new KeyGenerator(App.UserId.ToString());

            // Create a DEK
            string dataEncryptionKey = keyGenerator.CreateDataEncryptionKey(backupFileName);

            // gets the user KEK
            string keyEncryptionKey = keyGenerator.GetUserKeyEncryptionKeyReg();

            // Initialize a Cryptograpy object
            Cryptograpy cryptograpy = new Cryptograpy();


            // generate an IV to be used for encrypting the data
            byte[] dataIV = cryptograpy.GenerateSecureIV();
            // Encrypt the backup data using the DEK
            string encryptedBackupData = cryptograpy.EncryptStringAES(backupData, dataEncryptionKey, dataIV);


            // Generate a random IV for encrypting the DEK with the KEK
            byte[] dekIv = cryptograpy.GenerateSecureIV();
            // Encrypt the DEK using the KEK and the generated IV
            string encryptedDEK = cryptograpy.EncryptStringAES(dataEncryptionKey, keyEncryptionKey, dekIv);


            // Save the encrypted DEK, the IV used to encrypt the DEK, and the backup file name in the backupInfo table
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<BackupInfo>();
                BackupInfo newBackupInfo = new BackupInfo
                {
                    BackupName = backupFileName,
                    EncryptedDEK = encryptedDEK,
                    dekIV = Convert.ToBase64String(dekIv),
                    dataIV = Convert.ToBase64String(dataIV)
                };
                conn.Insert(newBackupInfo);
            }

            // Save the encrypted backup data either locally or in the S3 bucket
            if (SelectedItem.location_type.Equals(App.Locations.LocalLocation.ToString()))
            {
                BackupToLocalLocation(backupFileName, encryptedBackupData);
            }
            else if (SelectedItem.location_type.Equals(App.Locations.S3Location.ToString()))
            {
                BackupToS3Bucket(backupFileName, encryptedBackupData);
            }
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
