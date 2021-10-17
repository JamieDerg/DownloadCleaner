using System;
using System.IO;

namespace DownloadCleaner.Tasks
{
    public class SettingsReloadTask : Task
    {
        private DateTime lastChangedDate;

        public SettingsReloadTask()
        {
            lastChangedDate = File.GetLastWriteTime(Settings.JSON_PATH);
        }
        public override void RunTask()
        {
            Information("Checking if settings have to be reloaded...");
            var currentChangedDate = File.GetLastWriteTime(Settings.JSON_PATH);
            if (currentChangedDate != lastChangedDate)
            {
                Information("reloading settings...");
                settings.Reload();
                Information("done!");
                return;
            }
            Information("No Process needed");
        }

        public override string GetTaskName()
        {
            return "Settings Reload Task";
        }
    }
}