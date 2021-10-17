
using System;

namespace DownloadCleaner.Tasks
{
    public abstract class Task
    {

        public string taskName = "Default Task";

        protected string Prefix => $"[{GetTaskName()}] ";
        protected readonly Serilog.Core.Logger logger;
        protected readonly Settings settings;
        public abstract void RunTask();

        public abstract string GetTaskName();

        protected void Information(string message,params object[] args)
        {
            logger.Information(Prefix+message,args);
        }

        protected void Error(string message, params object[] args)
        {
            logger.Error(Prefix+message,args);
        }
        
        protected Task()
        {
            settings = Settings.GetInstance();
            logger = Logger.getInstance();
        }
        
        

    }
}