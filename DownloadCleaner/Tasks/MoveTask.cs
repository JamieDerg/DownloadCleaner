using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DownloadCleaner.FileType;


namespace DownloadCleaner.Tasks
{
    public class MoveTask : Task
    {
        
        private readonly FileTypeHelper fileTypeHelper;

        private readonly List<string> unkownFileTypes;

       
        public MoveTask()
        {
            fileTypeHelper = new FileTypeHelper();
            unkownFileTypes = new List<string>();
        }
        public override void RunTask()
        {
            
            var downloadPath = settings.downloadPath;
            var excludedFolders = fileTypeHelper.GetAllFileTypePaths();
            
            Information("scanning Files...");
            
            var files = Directory.GetFiles(downloadPath , "*.*", SearchOption.AllDirectories)
                            .Where(d => excludedFolders.All(e => !d.StartsWith(e))).ToArray();

            if (files.Length == 0)
            {
                Information("No process needed");
                return;
            }
            
            foreach (var file in files)
            {
                if(file.StartsWith(Path.Combine(settings.downloadPath,
                    settings.unknownExtensionFolderName)))
                    continue;

                if (ShouldMoveFile(file))
                {
                    MoveKnownFile(file);
                }
            }
            
            fileTypeHelper.AddUnknownExtensions(unkownFileTypes);
            unkownFileTypes.Clear();
        }

        public override string GetTaskName()
        {
            return "Move Task";
        }

        private void MoveKnownFile(String file)
        {
            Information("Moving file: {fileName}", file);
            var movePath = fileTypeHelper.GetMoveFilePath(file);
            MoveFile(file, movePath);
        }
        
        private void MoveUnknownFile(String file)
        {
            Information("Moving file with unknown extension: {fileName}", file);
            var fileName = Path.GetFileName(file);
            var extension = Path.GetExtension(file);
            var movePath = Path.Join(settings.downloadPath,
                settings.unknownExtensionFolderName,
                fileName);
            unkownFileTypes.Add(extension);
            MoveFile(file,movePath);
        }

        private void MoveFile(String file, String movePath)
        {
            new System.IO.FileInfo(movePath).Directory?.Create();
            try
            {
                File.Move(file, movePath, true);
            }
            catch (Exception e)
            {
                Error("The file: {file} could not be moved: {error}",file,e.Message);
            }
            Information("File has successfully been moved. New Path: {movePath}", movePath);
        }
        bool ShouldMoveFile(string file)
        {
            var minimumMoveTime = settings.minimumMoveTime;
            
            #if DEBUG
        
                var lastUpdatedTime = File.GetLastWriteTime(file).AddMinutes(1);
            #else
                var lastUpdatedTime = File.GetLastWriteTime(file).AddHours(minimumMoveTime);
       
            #endif

         
            
            bool removeTimeReached = lastUpdatedTime < DateTime.Now;

            if (removeTimeReached && !fileTypeHelper.fileHasEntry(file))
            {
                MoveUnknownFile(file);
                return false;
            }
            
            return removeTimeReached;
        }
    }
}