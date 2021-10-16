using System.Collections.Generic;
using System.IO;
using System.Linq;
using DownloadCleaner.FileType;

namespace DownloadCleaner.Tasks
{
    public class UnknownFileTask : Task
    {
        public UnknownFileTask()
        {
            taskName = "Unkown File Clean Task";
        }
        
        
        public override void RunTask()
        {
            logger.Information(Prefix+"scanning Files...");
            var unkownFilePath = Path.Combine(settings.downloadPath, settings.unknownExtensionFolderName);

            if (!Directory.Exists(unkownFilePath))
            {
                logger.Information(Prefix+"No process needed");
                return;
            }
            
            var fileTypeHelper = new FileTypeHelper();
            
            var files = Directory.GetFiles(unkownFilePath).Where(file => fileTypeHelper.fileHasEntry(file)).ToArray() ;

            if (files.Length == 0)
            {
                logger.Information(Prefix+"No process needed");
                return;
            }
            
            logger.Information(Prefix+"Found {fileCount} files that have a known extension", files.Length);
            logger.Information(Prefix+"Moving files to Download folder");

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
            logger.Information(Prefix+"Files Moved");
            fileTypeHelper.RemoveUnkownExtensions(extensionsToRemove);
        }
    }
}