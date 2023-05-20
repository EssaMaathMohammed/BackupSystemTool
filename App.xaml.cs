using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace BackupSystemTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        // Enum List for locations
        public enum Locations
        {
            LocalLocation,
            LANLocation,
            S3Location
        }

        public static System.Windows.Forms.NotifyIcon _notifyIcon;
        public static string absoluteIconPath;
        public static int UserId { get; set; }
        // Creates a path to the database directory location + Database Name
        static string ApplicationFileName = "BackupSystemTool";
        static string databaseName = "PII.db";
        static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static string databaseFolderPath = Path.Combine(folderPath, ApplicationFileName);
        public static string databasePath = Path.Combine(databaseFolderPath, databaseName);
        public App() {
            // checks if directory exists, if not creates it.
            if (!Directory.Exists(databaseFolderPath))
            {
                Directory.CreateDirectory(databaseFolderPath);
            }

            // Get the icon file path
            string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string relativeIconPath = @"..\..\..\resources\icons\icon-testing-note-book-report-testing-177786230.ico";
            absoluteIconPath = Path.GetFullPath(Path.Combine(basePath, relativeIconPath));

            // Initialize the notification icon
            _notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = new System.Drawing.Icon(absoluteIconPath),
                Visible = false,
            };
        }

    }   
}
