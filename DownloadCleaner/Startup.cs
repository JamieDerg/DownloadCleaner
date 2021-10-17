using DownloadCleaner.Tasks;

namespace DownloadCleaner
{
    public class Startup
    {
        private TaskRunner taskRunner;
        
        public void Start()
        {
            taskRunner = new TaskRunner();
            taskRunner.addTask(new SettingsReloadTask());
            taskRunner.addTask(new MoveTask());
            taskRunner.addTask(new EmptyFolderTask());
            taskRunner.addTask(new UnknownFileTask());
            taskRunner.Run();
        }

        public void Stop()
        {
            taskRunner.Stop();
        }
    }
}