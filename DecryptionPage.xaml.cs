using BackupSystemTool.DatabaseClasses;
using Microsoft.Win32;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        private bool navigateClose = false;
        private ClosingDialog.UserChoice closingResult = ClosingDialog.UserChoice.Cancel;
        public DecryptionPage()
        {
            InitializeComponent();

            // Get the icon file path
            string basePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string relativeIconPath = @"..\..\..\resources\icons\icon-testing-note-book-report-testing-177786230.ico";
            App.absoluteIconPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(basePath, relativeIconPath));

            // Initialize the notification icon
            App._notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = new System.Drawing.Icon(App.absoluteIconPath),
                Visible = false,
            };
            // Add event handler for restoring the app on double-click
            App._notifyIcon.MouseDoubleClick += (s, e) => ShowAndActivate();
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
            if (FileName != null && backupFullPath != null)
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

                    string decryptedDEK = "";
                    try
                    {
                        // Uses the DEKIV (if necessary) and user KEK to decrypt the DEK
                        decryptedDEK = cryptography.DecryptStringAES(encryptedDEK, userKek, dekIVBytes); // Use DEKIV if necessary
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Encryption Failed, Credentials are not correct");
                    }

                    if (!string.IsNullOrEmpty(decryptedDEK))
                    {
                        // Reads the encrypted backup file content
                        string encryptedBackupData = System.IO.File.ReadAllText(originalBackupFile);

                        // Uses the decrypted DEK and DataIV to decrypt the backup file
                        string decryptedBackupData = cryptography.DecryptStringAES(encryptedBackupData, decryptedDEK, dataIVBytes);

                        // Stores the decrypted backup file in the backupFullPath path
                        System.IO.File.WriteAllText(backupFullPath, decryptedBackupData);

                        MessageBox.Show("Backup Decrypted Successfully");
                    }
                }
                else
                {
                    MessageBox.Show("The Following Item doesnt exist in the database");
                }
            }
            else { 
                MessageBox.Show("Please Select a backup file to be decrypted and a location to save it");
            }
            
            
        }
        private void navigationButton_Click(object sender, RoutedEventArgs e)
        {
            if (navigationListView.Visibility == Visibility.Visible)
            {
                buttonBorder.BorderBrush = new SolidColorBrush(Colors.White);
                navigationListView.Visibility = Visibility.Collapsed;
                navigationButton.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                buttonBorder.BorderBrush = new SolidColorBrush(Colors.Transparent);
                navigationListView.Visibility = Visibility.Visible;
                navigationButton.Foreground = new SolidColorBrush(Color.FromRgb(65, 71, 112));
            }
        }
        private void connectionPageButton_Click(object sender, RoutedEventArgs e)
        {
            navigateClose = true;
            ConnectionsPage connectionPage = new ConnectionsPage();
            connectionPage.Show();
            this.Close();
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            List<JobItem> jobs = null;
            // select all jobs realted to user
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<JobItem>();
                jobs = conn.Table<JobItem>().Where(j => j.userId == App.UserId).ToList();
            }

            BackupScheduleManager backupScheduleManager = new BackupScheduleManager();
            if (jobs != null)
            {
                backupScheduleManager.clearAllTimers(jobs);
            }
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            closingResult = ClosingDialog.UserChoice.Close;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!navigateClose)
            {
                if (closingResult == ClosingDialog.UserChoice.Cancel)
                {
                    // Show the custom closing dialog
                    var closeDialog = new ClosingDialog();
                    closeDialog.ShowDialog();

                    // Get the user's choice from the dialog
                    closingResult = closeDialog.Choice;

                    // Perform action based on the user's choice
                    switch (closingResult)
                    {
                        case ClosingDialog.UserChoice.Close:
                            App._notifyIcon.Dispose(); // Dispose of the notification icon resources
                            break;
                        case ClosingDialog.UserChoice.RunInBackground:
                            Hide(); // Hide the main window
                            App._notifyIcon.Visible = true; // Show the notification icon
                            e.Cancel = true; // Cancel the closing event
                            break;
                        case ClosingDialog.UserChoice.Cancel:
                            e.Cancel = true; // Cancel the closing event
                            break;
                    }
                }
                else
                {
                    App._notifyIcon.Dispose();
                }
            }
        }

        private void jobPageButton_Click(object sender, RoutedEventArgs e)
        {
            navigateClose = true;
            JobPage jobPage = new JobPage();
            jobPage.Show();
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void ShowAndActivate()
        {
            Show(); // Show the main window
            WindowState = WindowState.Normal; // Set the window state to normal
            Activate(); // Activate the window to bring it to the top and give it focus
            App._notifyIcon.Visible = false; // Hide the notification icon
        }
    }
}
