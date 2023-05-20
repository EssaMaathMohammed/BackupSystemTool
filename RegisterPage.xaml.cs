using BackupSystemTool.DatabaseClasses;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;

namespace BackupSystemTool
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {

            // validate if the username is taken

            // Check if the username already exists in the database
            using (SQLiteConnection sqliteConnection = new SQLiteConnection(App.databasePath))
            {
                sqliteConnection.CreateTable<UserItem>();
                var existingUser = sqliteConnection.Find<UserItem>(u => u.username == username);
                if (existingUser != null)
                {
                    MessageBox.Show("This username is already taken.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }

            // Validate PIN and Password Match
            if (ValidatePin() && ValidatePasswordMatch())
            {
                Cryptograpy cryptograpy = new Cryptograpy();

                // Get the user's inputted username
                string username = cryptograpy.hashText(usernameTextBox.Text);
                string email = emailTextBox.Text;

                if (!ValidateEmail(email))
                {
                    return;
                }

                string userSalt = cryptograpy.generateSalt();
                string pin = PINTextBox.Password;
                string saltedPIN = pin + userSalt;

                // hash the combination of PIN and Salt
                string userCiphertext = cryptograpy.hashText(saltedPIN);

                // select the number of users in the database
                int numUsers = 0;
                using (SQLiteConnection sqliteConnection = new SQLiteConnection(App.databasePath))
                {
                    sqliteConnection.CreateTable<UserItem>();
                    numUsers = sqliteConnection.Table<UserItem>().Count();
                }

                // creates a new UserItem which includes ID (auto), username, email, ciphertext, salt
                UserItem user = new UserItem()
                {
                    id = numUsers + 1,
                    username = username,
                    ciphertext = userCiphertext,
                    salt = userSalt
                };

                // generate the key for the user and set the value of the key in the registry
                KeyGenerator keyGenerator = new KeyGenerator(user.id.ToString(), user.salt);
                keyGenerator.setUserKeyIVReg();

                // generate the KEK for the user and set the value of the KEK in the registry
                keyGenerator.SetUserKeyEncryptionKeyReg();

                // set the email to the encrypted email using the user's key and user IV
                user.email = cryptograpy.EncryptStringAES(email, keyGenerator.getUserKeyReg(), Convert.FromBase64String(keyGenerator.getUserIVReg()));
                
                // uses the Sqlite connection to insert the new user
                using (SQLiteConnection sqliteConnection = new SQLiteConnection(App.databasePath))
                {
                    sqliteConnection.CreateTable<UserItem>();
                    sqliteConnection.Insert(user);
                }
               
                // Show a message box to indicate that registration was completed successfully
                MessageBox.Show("Registration Completed.", "Registration Information", MessageBoxButton.OK, MessageBoxImage.Information);
                MessageBox.Show(user.email + " " + keyGenerator.getUserKeyReg() + " " + keyGenerator.getUserIVReg(), "Registration Information", MessageBoxButton.OK, MessageBoxImage.Information);

                // Create a new instance of the MainWindow (login page) and opens it
                MainWindow loginPage = new MainWindow();
                loginPage.Show();
                // Close the current window
                this.Close();
            }
        }


        private bool ValidatePasswordMatch()
        {
            // Check if passwords match
            if (PINTextBox.Password != repeatPINTextBox.Password)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            return true;
        }
        private bool ValidatePin()
        {

            if (string.IsNullOrEmpty(usernameTextBox.Text) || string.IsNullOrEmpty(emailTextBox.Text))
            {
                MessageBox.Show("Username and Email cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

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

        private bool ValidateEmail(string email)
        {
            // Define a regular expression for email format
            string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            // Check if email matches the regular expression
            if (!Regex.IsMatch(email, emailRegex))
            {
                MessageBox.Show("Email is not in a valid format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            return true;
        }

        private void loginPageLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow loginPage = new MainWindow();
            loginPage.Show();
            this.Close();
        }
    }
}
