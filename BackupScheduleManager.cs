using BackupSystemTool.DatabaseClasses;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace BackupSystemTool
{
    internal class BackupScheduleManager
    {
        public JobItem SelectedItem { get; set; }

        public JobPage JobPage { get; set; }

        private static List<TimerInfo> _timers = new List<TimerInfo>();
        public BackupScheduleManager(JobPage jobPage, JobItem SelectedItem) {
            this.JobPage = jobPage;
            this.SelectedItem = SelectedItem;
        }

        //// loads the schedule items from the database into a list
        //public List<BackupSchedule> LoadSchedulesFromDatabase()
        //{
        //    // Load schedules from the database and return as a list of BackupSchedule objects.
        //    List<BackupSchedule> backupSchedules = new List<BackupSchedule>();

        //    // using with resources, automatically close the connection upon reaching the end of using block
        //    using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
        //    {
        //        conn.CreateTable<BackupSchedule>();
        //        backupSchedules = conn.Table<BackupSchedule>().Where(scheduleItem => scheduleItem.job_id == SelectedItem.id).ToList();
        //    }
        //    return backupSchedules;
        //}

        // returns the intervals as milliseconds to be used in the scheculer
        public int GetIntervalInMilliseconds(int interval, string intervalType)
        {
            return intervalType.ToLower() switch
            {
                "minute" => interval * 60 * 1000,
                "hour" => interval * 60 * 60 * 1000,
                "day" => interval * 24 * 60 * 60 * 1000,
                // if the interval is none of the above throws an exception
                _ => throw new NotSupportedException($"Interval type '{intervalType}' is not supported."),
            };
        }

        // adds a new schedule based on the location type.
        public void AddSchedule(BackupSchedule schedule)
        {
            if (SelectedItem != null) {

                BackupManager backupManager = new BackupManager(SelectedItem);

                long intervalInMilliseconds = GetIntervalInMilliseconds(schedule.Interval, schedule.IntervalType);
                // checks if the job has a location/s for backup
                // gets the location of the backup and sets it as the destination
                if (SelectedItem.location_type == App.Locations.LocalLocation.ToString())
                {
                    var timer = new Timer(intervalInMilliseconds);
                    timer.Elapsed += (sender, args) => backupManager.BackupToLocalLocation(schedule.DatabaseName);
                    timer.AutoReset = true;
                    timer.Start(); 
                    _timers.Add(new TimerInfo { Timer = timer, JobId = SelectedItem.id, DatabaseName = schedule.DatabaseName });


                    if (JobPage != null) {
                        JobPage.ScheduleBackup_TextBox.Text = "Every " + schedule.Interval + " " + schedule.IntervalType;
                    }

                    MessageBox.Show("Schedule Started Succefully");
                }
                else if (SelectedItem.location_type == App.Locations.S3Location.ToString())
                {
                    var timer = new Timer(intervalInMilliseconds);
                    timer.Elapsed += (sender, args) => backupManager.BackupToS3Bucket(schedule.DatabaseName);
                    timer.AutoReset = true;
                    timer.Start(); 
                    _timers.Add(new TimerInfo { Timer = timer, JobId = SelectedItem.id, DatabaseName = schedule.DatabaseName });


                    if (JobPage != null) { 
                        JobPage.ScheduleBackup_TextBox.Text = "Every " + schedule.Interval + " " + schedule.IntervalType;
                    }

                    MessageBox.Show("Schedule Started Succefully");
                }
                else if (SelectedItem.location_type == null)
                {
                    // show a message if no location is selected for the backup
                    MessageBox.Show("Please Select a location for the backup");
                }
            }
        }


        // remove timer if the user updates the schedule
        public void removeTimer(int jobId, string databaseName)
        {
            TimerInfo timerToRemove = _timers.FirstOrDefault(t => t.JobId == jobId && t.DatabaseName == databaseName);

            if (timerToRemove != null)
            {
                timerToRemove.Timer.Stop();
                timerToRemove.Timer.Dispose();
                _timers.Remove(timerToRemove);
            }
        }

        // class to store Timer information
        public class TimerInfo
        {
            public Timer Timer { get; set; }
            public int JobId { get; set; }
            public string DatabaseName { get; set; }
        }
    }
}
