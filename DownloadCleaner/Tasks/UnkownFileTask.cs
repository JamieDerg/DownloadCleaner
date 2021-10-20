using System.Collections.Generic;
using System.IO;
using System.Linq;
using DownloadCleaner.FileType;

namespace DownloadCleaner.Tasks
{
    public class UnknownFileTask : Task
    {
       
        
        public override void RunTask()
        {
            Information("scanning Files...");
            var unknownFilePath = Path.Combine(settings.downloadPath, settings.unknownExtensionFolderName);

            if (!Directory.Exists(unknownFilePath))
            {
                Information("No process needed");
                return;
            }
            
            var fileTypeHelper = new FileTypeHelper();
            
            var files = Directory.GetFiles(unknownFilePath).Where(file => fileTypeHelper.FileHasEntry(file)).ToArray() ;

            if (files.Length is 0)
            {
                Information("No process needed");
                return;
            }
            
            Information("Found {fileCount} files that have a known extension", files.Length);
            Information("Moving files to Download folder");

            var extensionsToRemove = new List<string>();
            
            foreach (var file in files)
            {
                var extension = Path.GetExtension(file);
                var fileName = Path.GetFileName(file);
                var movePath = Path.Combine(settings.downloadPath, fileName);
                
                if (!extensionsToRemove.Exists(e => e == extension))
                {
                    extensionsToRemove.Add(extension);
                }
                File.Move(file, movePath);
            }
            Information("Files Moved");
            fileTypeHelper.RemoveUnkownExtensions(extensionsToRemove);
        }

        public override string GetTaskName()
        {
            return "Unknown File Clean Task";
        }
    }
}