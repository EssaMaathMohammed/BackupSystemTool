using BackupSystemTool.DatabaseClasses;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace BackupSystemTool.Controls
{
    /// <summary>
    /// Interaction logic for JobDatabaseUserControl.xaml
    /// </summary>
    public partial class JobDatabaseUserControl : UserControl
    {
        private static JobDatabases defaultValue = new JobDatabases() { 
            job_id = -1,
            database_name = "Default Item"
        };

        public JobDatabaseUserControl()
        {
            InitializeComponent();
        }


        public JobDatabases JobDatabases
        {
            get { return (JobDatabases)GetValue(JobDatabasesProperty); }
            set { SetValue(JobDatabasesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for JobDatabases.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty JobDatabasesProperty =
            DependencyProperty.Register("JobDatabases", typeof(JobDatabases), typeof(JobDatabaseUserControl), new PropertyMetadata(defaultValue,SetDatabaseContent));

        private static void SetDatabaseContent(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            JobDatabaseUserControl jobDatabaseUserControl = d as JobDatabaseUserControl;
            if (jobDatabaseUserControl != null )
            {
                JobDatabases databaseItem = e.NewValue as JobDatabases;

                if (databaseItem != null )
                {
                    jobDatabaseUserControl.DatabaseNameTextBlock.Text = databaseItem.database_name;
                    if (BrowseDatabasesDialog.selectedDatabases.Contains(databaseItem.database_name))
                    {
                        jobDatabaseUserControl.AddDatabaseButton.Visibility = Visibility.Collapsed;
                        jobDatabaseUserControl.DeleteDatabaseButton.Visibility = Visibility.Visible;
                    }
                    else {
                        jobDatabaseUserControl.AddDatabaseButton.Visibility = Visibility.Visible;
                        jobDatabaseUserControl.DeleteDatabaseButton.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void AddDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if item already exists in the list
            if (!BrowseDatabasesDialog.selectedDatabases.Contains(JobDatabases.database_name))
            {
                // Add the item to the list
                BrowseDatabasesDialog.selectedDatabases.Add(JobDatabases.database_name);

                AddDatabaseButton.Visibility = Visibility.Collapsed;
                DeleteDatabaseButton.Visibility = Visibility.Visible;
            }
        }

        private void DeleteDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if item exists in the list
            if (BrowseDatabasesDialog.selectedDatabases.Contains(JobDatabases.database_name))
            {
                // Remove the item from the list
                BrowseDatabasesDialog.selectedDatabases.Remove(JobDatabases.database_name);

                AddDatabaseButton.Visibility = Visibility.Visible;
                DeleteDatabaseButton.Visibility = Visibility.Collapsed;
            }
        }

    }
}
