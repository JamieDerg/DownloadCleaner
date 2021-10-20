using System.IO;
using System.Linq;

namespace DownloadCleaner.Tasks
{
    public class EmptyFolderTask : Task
    {
        public override void RunTask()
        {
            var emptyFolders = Directory.EnumerateDirectories(Settings.GetInstance().downloadPath)
                .Where(d => Directory.GetFiles(d,"*.*", SearchOption.AllDirectories).Length == 0).ToArray();

            if (emptyFolders.Length is 0)
            {
                Information("no process needed");
                return;
            }
            
            Information("found {folderCount} empty folders. Deleting....",emptyFolders.Length);
            foreach (var folder in emptyFolders)
            {
               Directory.Delete(folder,true);
             
            }
            Information("Deletion complete",emptyFolders.Length);
        }

        public override string GetTaskName()
        {
            return "Empty folder Task";
        }
    }
}