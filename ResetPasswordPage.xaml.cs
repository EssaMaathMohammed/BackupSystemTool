using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using BackupSystemTool.DatabaseClasses;
using System.Diagnostics;
using Google.Api.Gax;
using static BackupSystemTool.BackupScheduleManager;
using System.Timers;

namespace BackupSystemTool
{
    /// <summary>
    /// Interaction logic for ResetPasswordPage.xaml
    /// </summary>
    public partial class ResetPasswordPage : Window
    {
        byte[] secureIV;
        string userKey;
        string username;
        string code;
        public ResetPasswordPage()
        {
            InitializeComponent();
        }

        private void sendCodeButton_Click(object sender, RoutedEventArgs e)
        {
            // check if the username text box is not empty
            if (usernameTextBox.Text != "")
            {
                Cryptograpy cryptograpy = new Cryptograpy();
                username = cryptograpy.hashText(usernameTextBox.Text);

                // check if the email exists in the database 
                using (SQLite.SQLiteConnection sqliteConnection = new SQLite.SQLiteConnection(App.databasePath))
                {
                    // Create the table for UserItem if it does not exist
                    sqliteConnection.CreateTable<UserItem>();
                    // Fetch the specific user based on the provided username
                    UserItem user = sqliteConnection.Table<UserItem>().FirstOrDefault(u => u.username == username);
                    // If user with the provided username exists
                    if (user != null)
                    {
                        // generate an 8 digit random code
                        Random random = new Random();
                        code = random.Next(10000000, 99999999).ToString();

                        // encrypt the code and store it in the database
                        KeyGenerator keyGenerator = new KeyGenerator(user.id.ToString());
                        // generate a new secure IV and user key
                        secureIV = cryptograpy.GenerateSecureIV();
                        userKey = keyGenerator.getUserKeyReg();

                        // make a timer that removes the value of code after 5 minutes
                        var timer = new Timer(1000 * 60 * 5);
                        timer.Elapsed += (sender, args) =>
                        {
                            code = cryptograpy.generateSalt() + cryptograpy.generateSalt();
                        };
                        timer.AutoReset = true;
                        timer.Start();

                        Debug.WriteLine(user.email + " " + keyGenerator.getUserKeyReg() + " " + keyGenerator.getUserIVReg());

                        // decrypt the user email
                        string decryptedEmail = cryptograpy.DecryptStringAES(user.email, userKey, Convert.FromBase64String(keyGenerator.getUserIVReg()));
                       

                        // send the code to the user's email
                        string body = "Your password reset code is: " + code;
                        EmailServices emailServices = new EmailServices();
                        emailServices.SendEmail(decryptedEmail, "Password Recovery Email", body);

                        // encrypt the code
                        code = cryptograpy.EncryptStringAES(code.ToString(), userKey, secureIV);

                        enterEmailStackPanel.Visibility = Visibility.Collapsed;
                        enterCodeStackPanel.Visibility = Visibility.Visible;    
                    }
                    else
                    {
                        MessageBox.Show("The username you entered does not exist in our database.");
                    }
                }
            }
            else
            {
                    MessageBox.Show("Please enter your username.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        
        private void resetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            // check if the password is valid
            if (!ValidatePin()) { 
                // tell the user that the password is not valid
                MessageBox.Show("The password you entered is not valid.");
                return;
            }
            // check if the passwords match
            if (!ValidatePasswordMatch()) { 
                // tell the user that the passwords do not match
                MessageBox.Show("The passwords you entered do not match.");
                return;
            }

            // update the password in the database
            using (SQLite.SQLiteConnection sqliteConnection = new SQLite.SQLiteConnection(App.databasePath))
            {
                // Create the table for UserItem if it does not exist
                sqliteConnection.CreateTable<UserItem>();
                // Fetch the specific user based on the provided username
                UserItem user = sqliteConnection.Table<UserItem>().FirstOrDefault(u => u.username == username);
                // If user with the provided username exists
                if (user != null)
                {
                    Cryptograpy cryptograpy = new Cryptograpy();
                    user.ciphertext = cryptograpy.hashText(PINTextBox.Password + user.salt);
                    sqliteConnection.Update(user);
                    // tell the user that the password has been updated
                    MessageBox.Show("Your password has been updated.");
                    // open login page
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("The email you entered does not exist in our database.");
                }
            }

        }
        private void checkCodeButton_Click(object sender, RoutedEventArgs e)
        {
            // check if the code is correct
            if (ValidateCode(codeTextBox.Password))
            {
                enterCodeStackPanel.Visibility = Visibility.Collapsed;
                resetPasswordStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("The code you entered is incorrect.");
            }
        }
        // valideate code method
        private bool ValidateCode(string userInputCode)
        {
            try {
                // decrypt the code
                Cryptograpy cryptograpy = new Cryptograpy();
                userInputCode = cryptograpy.EncryptStringAES(userInputCode, userKey, secureIV);
                // check if the code is correct
                if (code == userInputCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }catch (Exception ex)
            {
                return false;
            }
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
        private void loginPageLabel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow loginPage = new MainWindow();
            loginPage.Show();
            this.Close();
        }
    }
}
