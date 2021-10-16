
namespace DownloadCleaner.Tasks
{
    public abstract class Task
    {

        public string taskName = "Default Task";

        protected string Prefix => $"[{taskName}] ";
        protected readonly Serilog.Core.Logger logger;
        protected readonly Settings settings;
        public abstract void RunTask();
        
        protected Task()
        {
            settings = Settings.GetInstance();
            logger = Logger.getInstance();
        }
        
        

    }
}