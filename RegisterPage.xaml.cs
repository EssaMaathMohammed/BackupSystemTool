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
            if (ValidatePin()) {
                Cryptograpy cryptograpy = new Cryptograpy();

                string pin = PINTextBox.Password;
                string userSalt = cryptograpy.generateSalt();
                string saltedPIN = pin + userSalt;
                // hash the combination of PIN and Salt
                string userCiphertext = cryptograpy.hashText(saltedPIN);

                // creates a new UserItem which includes ID (auto),
                UserItem user = new UserItem()
                {
                    ciphertext = userCiphertext,
                    salt = userSalt,
                };
                
                // uses the Sqlite connection to insert the new user
                using (SQLiteConnection sqliteConnection = new SQLiteConnection(App.databasePath)) {
                    sqliteConnection.CreateTable<UserItem>();
                    sqliteConnection.Insert(user);
                }

                // generate the key for the user and set the value of the key in the registry
                KeyGenerator keyGenerator = new KeyGenerator(user.id.ToString(),user.salt);
                keyGenerator.setKey();


                // Show a message box to indicate that registration was completed successfully
                MessageBox.Show("Registration Completed.", "Registration Information", MessageBoxButton.OK, MessageBoxImage.Information);

                // Create a new instance of the MainWindow (login page) and opens it
                MainWindow loginPage = new MainWindow();
                loginPage.Show();
                // Close the current window
                this.Close();
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
    }
}
