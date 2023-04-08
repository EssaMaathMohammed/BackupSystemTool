using BackupSystemTool.DatabaseClasses;
using Microsoft.Win32;
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
    /// Interaction logic for DecryptionPage.xaml
    /// </summary>
    public partial class DecryptionPage : Window
    {
        public string FileName { get; set; }
        public string backupFullPath { get; set; }
        string originalBackupFile { get; set; }
        public DecryptionPage()
        {
            InitializeComponent();
        }

        // sets the Filename instance prop to the selected filepath filename, and sets the full path of the original file
        private void BrowseBackupLocationButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                originalBackupFile = openFileDialog.FileName;
                // gets the filename from the path
                FileName = System.IO.Path.GetFileName(originalBackupFile);
            }
        }

        // sets the destination location to the selectedlocation + decrypted_ + filename 
        private void BrowseOutputLocationButton_Click(object sender, RoutedEventArgs e)
        {
            if (FileName != null)
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
                        backupFullPath = System.IO.Path.Combine(dialog.SelectedPath, "decrypted_"+FileName);
                    }
                }
            }
            else {
                MessageBox.Show("PLease Selected a backup file to be decrypted first");
            }
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            BackupInfo backup = null;

            // Save the encrypted DEK, the IV used to encrypt the DEK, and the backup file name in the backupInfo table
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<BackupInfo>();
                List<BackupInfo> items = conn.Table<BackupInfo>().Where(item => item.BackupName.Equals(FileName)).ToList();
                if (items.Count > 0)
                {
                    backup = items[0];
                }
            }

            if (backup != null)
            {
                // Gets the encrypted DEK and DataIV from the database
                string encryptedDEK = backup.EncryptedDEK; // Fetch from the database
                string dataIV = backup.dataIV; // Fetch from the database
                string dekIv = backup.dekIV;

                // Convert the DataIV to a byte array
                byte[] dataIVBytes = Convert.FromBase64String(dataIV);

                // Convert the DEKIV to a byte array (if necessary)
                byte[] dekIVBytes = Convert.FromBase64String(dekIv);

                // creates a key generator using the user id 
                KeyGenerator keyGenerator = new KeyGenerator(App.UserId.ToString());

                // Gets the user KEK
                string userKek = keyGenerator.GetUserKeyEncryptionKeyReg(); // Fetch the user Kek

                // Initialize the Cryptography class
                Cryptograpy cryptography = new Cryptograpy();

                // Uses the DEKIV (if necessary) and user KEK to decrypt the DEK
                string decryptedDEK = cryptography.DecryptStringAES(encryptedDEK, userKek, dekIVBytes); // Use DEKIV if necessary


                // Reads the encrypted backup file content
                string encryptedBackupData = System.IO.File.ReadAllText(originalBackupFile);

                // Uses the decrypted DEK and DataIV to decrypt the backup file
                string decryptedBackupData = cryptography.DecryptStringAES(encryptedBackupData, decryptedDEK, dataIVBytes);

                // Stores the decrypted backup file in the backupFullPath path
                System.IO.File.WriteAllText(backupFullPath, decryptedBackupData);
            }
            else {
                MessageBox.Show("The Following Item doesnt exist in the database");
            }
            
        }

    }
}
