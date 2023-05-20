using BackupSystemTool.DatabaseClasses;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BackupSystemTool
{
    /// <summary>
    /// Interaction logic for ConnectionsPage.xaml
    /// </summary>
    public partial class ConnectionsPage : Window
    {
        private bool navigateClose = false;
        private ClosingDialog.UserChoice closingResult = ClosingDialog.UserChoice.Cancel;
        public ConnectionsPage()
        {
           
            InitializeComponent();
            UpdateConnectionList();
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
            // Add event handler for restoring the app on double-click
            App._notifyIcon.MouseDoubleClick += (s, e) => ShowAndActivate();
        }

        // Reading the fields of the database ConnectionItem Table based on the user id
        public static List<ConnectionItem> GetConnectionsList()
        {
            List<ConnectionItem> connectionItems = new List<ConnectionItem>();
            // using with resources, automatically close the connection upon reaching the end of using block
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<ConnectionItem>();
                connectionItems = conn.Table<ConnectionItem>().Where(conItem=> conItem.UserId == App.UserId).ToList();
            }

            return connectionItems;
        }

        public void UpdateConnectionList() {
            if (GetConnectionsList().Count > 0)
            {
                ConnectionsListView.ItemsSource = GetConnectionsList();
            }
        }

        private void addConnection_Click(object sender, RoutedEventArgs e)
        {
            AddConnectionDialog dialog = new AddConnectionDialog(this, App.UserId);
            dialog.ShowDialog();
        }

        private void LoginPageButton_Click(object sender, RoutedEventArgs e)
        {
            navigateClose = true;
            JobPage jobPage = new JobPage();
            jobPage.Show();
            this.Close();
        }
        private void decryptionPageButton_Click(object sender, RoutedEventArgs e)
        {
            navigateClose = true;
            DecryptionPage decryptionPage = new DecryptionPage();
            decryptionPage.Show();
            this.Close();
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

        private void ShowAndActivate()
        {
            Show(); // Show the main window
            WindowState = WindowState.Normal; // Set the window state to normal
            Activate(); // Activate the window to bring it to the top and give it focus
            App._notifyIcon.Visible = false; // Hide the notification icon
        }

        private void Window_Closed(object sender, EventArgs e)
        {}
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
            App.UserId = -1;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            closingResult = ClosingDialog.UserChoice.Close;
            this.Close();
        }
    }
}
