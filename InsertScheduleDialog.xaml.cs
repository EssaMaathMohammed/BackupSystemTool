using BackupSystemTool.DatabaseClasses;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for InsertScheduleDialog.xaml
    /// </summary>
    public partial class InsertScheduleDialog : Window
    {
        public JobPage JobPage { get; set; }
        public JobItem SelectedItem { get; set; }
        public InsertScheduleDialog(JobPage jobPage,JobItem SelectedItem)
        {
            this.JobPage = jobPage;
            this.SelectedItem = SelectedItem; 
            InitializeComponent();
        }

        private void addScheduleButton_Click(object sender, RoutedEventArgs e)
        {

            // select the related databases of the job 
            List<string> databases = GetJobDatabases();

            ComboBoxItem intervalTypeComboBoxItem = intervalDurationComboBox.SelectedItem as ComboBoxItem;

            if (SelectedItem != null)
            {
                // create an object of the schedule manager passing the select job item in it
                BackupScheduleManager scheduleManager = new BackupScheduleManager(JobPage, SelectedItem);

                // iterate through the databases related to the job
                foreach (string database in databases)
                {
                    bool itemExists = false;

                    // create a backup schedule out of every database name in the list
                    BackupSchedule schedule = new BackupSchedule()
                    {
                        job_id = SelectedItem.id,
                        DatabaseName = database,
                        IntervalType = intervalTypeComboBoxItem.Content.ToString(),
                        Interval = int.Parse(intervalDuration_TextBox.Text)
                    };

                    // if the new schedule already eists update the info of the original schedule then update it
                    BackupSchedule existingSchedule = null;
                    using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                    {
                        conn.CreateTable<BackupSchedule>();
                        List<BackupSchedule> scheduleList = conn.Table<BackupSchedule>().Where(item => item.DatabaseName.Equals(schedule.DatabaseName) && item.job_id == schedule.job_id).ToList();
                        if (scheduleList.Count > 0) { 
                            existingSchedule = scheduleList[0];
                            // give the schelde the new info
                            existingSchedule.IntervalType = schedule.IntervalType;
                            existingSchedule.Interval = schedule.Interval;
                        }
                    }

                    // if the item already exists
                    if (existingSchedule != null)
                    {
                        // first remove the item from the schedule timer list
                        scheduleManager.removeTimer(SelectedItem.id, database);

                        // update the existing echedule
                        using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                        {
                            conn.CreateTable<BackupSchedule>();
                            conn.Update(existingSchedule);
                        }
                    }
                    else { 
                        // insert the job schedule into the database
                        using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                        {
                            conn.CreateTable<BackupSchedule>();
                            conn.Insert(schedule);
                        }
                    }

                    // add the schedule and create a timer for it
                    scheduleManager.AddSchedule(schedule);

                }
            }

            // close the dialog
            this.Close();
        }

        // gets a list of the databases related to the job
        private List<string> GetJobDatabases() {
            List<string> databases = new List<string>();

            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<JobDatabases>();
                List<JobDatabases> jobDatabasesList = conn.Table<JobDatabases>().Where(jobDBItem => jobDBItem.job_id == SelectedItem.id).ToList();

                foreach (JobDatabases jobDatabase in jobDatabasesList) {
                    databases.Add(jobDatabase.database_name);
                }
            }
            return databases;
        }
    }
}
