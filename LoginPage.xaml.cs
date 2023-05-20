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

            // create an instance of the crypto class
            Cryptograpy cryptograpy = new Cryptograpy();

            // Get the user's inputted username
            string usernameInput = cryptograpy.hashText(usernameTextBox.Text); 

            // Validate the user-inputted PIN and username
            if (ValidateLoginInfo()) 
            {
                using (SQLiteConnection sqliteConnection = new SQLiteConnection(App.databasePath))
                {
                    // Create the table for UserItem if it does not exist
                    sqliteConnection.CreateTable<UserItem>();
                    // Fetch the specific user based on the provided username
                    UserItem user = sqliteConnection.Table<UserItem>().FirstOrDefault(u => u.username == usernameInput);

                    // If user with the provided username exists
                    if (user != null)
                    {
                        // Get the user's inputted PIN
                        string userInput = PINTextBox.Password;

                        // Combine the salt and user inputted PIN
                        string saltedPIN = userInput + user.salt;
                        // Hash the salted PIN
                        string hashedSaltedPIN = cryptograpy.hashText(saltedPIN);
                        // Check if the hashed salted PIN matches the stored ciphertext
                        if (hashedSaltedPIN.Equals(user.ciphertext))
                        {
                            // If a match is found, set loggedIn to true
                            loggedIn = true;
                            // set the user id and email of the user to be used in the application
                            App.UserId = user.id;

                            KeyGenerator keyGenerator = new KeyGenerator(user.id.ToString());
                            Debug.Write(user.email + " " + keyGenerator.getUserKeyReg() + " " + keyGenerator.getUserIVReg());

                            // start the schedules of the application
                            StartSchedules();

                            // Open the ConnectionsPage and close the login page
                            ConnectionsPage connectionsPage = new ConnectionsPage();
                            connectionsPage.Show();
                            this.Close();
                        }
                    }

                    // If no matching user is found, show an error message
                    if (!loggedIn)
                    {
                        MessageBox.Show("Incorrect username or PIN. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private bool ValidateLoginInfo()
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

            if (string.IsNullOrEmpty(usernameTextBox.Text))
            {
                MessageBox.Show("Username and Email cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void registerPageLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RegisterPage registerPage = new RegisterPage();
            registerPage.Show();
            this.Close();
        }

        private void resetPasswordLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //navigate to reset password page
            ResetPasswordPage resetPasswordPage = new ResetPasswordPage();
            resetPasswordPage.Show();
            this.Close();
        }
    }
}
