using BackupSystemTool.DatabaseClasses;
using Gu.Wpf.Adorners;
using Microsoft.VisualBasic.ApplicationServices;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BackupSystemTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            // variable to track whether the login was successful
            bool loggedIn = false;

            // Validate the user-inputted PIN
            if (ValidatePin())
            {
                using (SQLiteConnection sqliteConnection = new SQLiteConnection(App.databasePath))
                {
                    Cryptograpy cryptograpy = new Cryptograpy();
                    // Create the table for UserItem if it does not exist
                    sqliteConnection.CreateTable<UserItem>();
                    // Get a list of all UserItem objects stored in the database
                    List<UserItem> users = sqliteConnection.Table<UserItem>().ToList();

                    // Get the user's inputted PIN
                    string userInput = PINTextBox.Password;

                    // Loop through each user in the database
                    foreach (UserItem user in users)
                    {
                        // Combine the salt and user inputted PIN
                        string saltedPIN = userInput + user.salt;
                        // Hash the salted PIN
                        string hashedSaltedPIN = cryptograpy.hashText(saltedPIN);
                        // Check if the hashed salted PIN matches the stored ciphertext
                        if (hashedSaltedPIN.Equals(user.ciphertext))
                        {
                            // If a match is found, set loggedIn to true
                            loggedIn = true;

                            App.UserId = user.id;

                            // start the schedules of the application
                            StartSchedules();

                            // Open the ConnectionsPage and close the login page
                            ConnectionsPage connectionsPage = new ConnectionsPage();
                            connectionsPage.Show();
                            this.Close();
                            // Break out of the loop since the user has been found
                            break;
                        }
                    }
                }
                // If no matching user is found, show an error message
                if (!loggedIn)
                {
                    MessageBox.Show("Incorrect PIN. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private bool ValidatePin()
        {
            if (string.IsNullOrEmpty(PINTextBox.Password))
            {
                MessageBox.Show("Please enter PIN", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (!Regex.IsMatch(PINTextBox.Password, @"^[a-zA-Z0-9!@#\$%\^&\*()_+-=\{\}\[\];':\"",\.<>\/\?]+$"))
            {
                MessageBox.Show("PIN can only contain letters, numbers, and special characters", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            if (PINTextBox.Password.Length < 6 || PINTextBox.Password.Length > 12)
            {
                MessageBox.Show("PIN must be between 6 and 12 characters long", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            return true;
        }

        private void StartSchedules()
        {
            // get a list of all the job items
            List<JobItem> jobItems;
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<JobItem>();
                jobItems = conn.Table<JobItem>().Where(jb => jb.userId == App.UserId).ToList();
            }

            // for each job item start select the schedules related to them and start scheduling them
            foreach (JobItem jobItem in jobItems)
            {
                // create a schedule manager to add the schedules
                BackupScheduleManager scheduleManager = new BackupScheduleManager(null, jobItem);

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
    }
}
