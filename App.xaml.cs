﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BackupSystemTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Creates a path to the database directory location + Database Name
        static string ApplicationFileName = "BackupSystemTool";
        static string databaseName = "ConnectionsDB.db";
        static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static string databaseFolderPath = Path.Combine(folderPath, ApplicationFileName);
        public static string databasePath = Path.Combine(databaseFolderPath, databaseName);
        public App() {
            // checks if directory exists, if not creates it.
            if (!Directory.Exists(databaseFolderPath))
            {
                Directory.CreateDirectory(databaseFolderPath);
            }
        }

    }
    
}
