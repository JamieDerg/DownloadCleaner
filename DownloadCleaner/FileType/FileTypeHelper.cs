using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace DownloadCleaner.FileType
{
    public class FileTypeHelper
    {
        private Settings settings;

        private const string UNKOWN_EXTENSION_PATH = "unkownExtensions.json";
        
        public FileTypeHelper()
        {
            settings = Settings.GetInstance();
        }
        
        public string[] GetAllFileTypePaths()
        {
            var fileTypes = settings.fileTypes;
            var downloadPath = settings.downloadPath;
            
            var strings = from j in fileTypes select $"{downloadPath}\\{j.Path}";
            return strings.ToArray();
        }

        public bool fileHasEntry(string file)
        {
            var extension = Path.GetExtension(file);

            return GetFileTypeFromExtension(extension) is not null;
        }

        private FileType GetFileTypeFromExtension(string extension)
        {
            var fileTypes = settings.fileTypes;
            
            
            bool HasEntryInSubType(List<SubType> subTypes) => subTypes.Any(e => e.Extensions.Contains(extension));
            var fileType = fileTypes.Find(e => e.Extensions.Contains(extension) || HasEntryInSubType(e.SubTypes));
            return fileType;
        }
        
        public string GetMoveFilePath(string file)
        {
            var downloadPath = settings.downloadPath;

            var fileName = Path.GetFileName(file);
            var extension = Path.GetExtension(file);

            var fileType = GetFileTypeFromExtension(extension);
            var subType = fileType.SubTypes.Find(e => e.Extensions.Contains(extension));
            var baseMovePath = Path.Combine(downloadPath, fileType.Path);

            if (subType is not null)
            {
                baseMovePath = Path.Combine(baseMovePath, subType.Path);
            }

            return Path.Combine(baseMovePath, fileName);
        }

        public void AddUnknownExtensions(List<string> unknownExtensions)
        {
            createUnkownExtensionsFile();
            List<string> existingUnknownExtensions = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(UNKOWN_EXTENSION_PATH));
            
            
            foreach (var extension in unknownExtensions)
            {
                if (existingUnknownExtensions != null && !existingUnknownExtensions.Contains(extension))
                {
                    existingUnknownExtensions.Add(extension);
                }
            }
            
            string json = JsonConvert.SerializeObject(existingUnknownExtensions, Formatting.Indented);
            File.WriteAllText(UNKOWN_EXTENSION_PATH, json);
            
        }

        public void RemoveUnkownExtensions(List<string> unknownExtensions)
        {
            createUnkownExtensionsFile();
            List<string> existingUnknownExtensions = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(UNKOWN_EXTENSION_PATH));
            
            existingUnknownExtensions?.RemoveAll(extension => unknownExtensions.Any(e => e == extension));
            
            string json = JsonConvert.SerializeObject(existingUnknownExtensions, Formatting.Indented);
            File.WriteAllText(UNKOWN_EXTENSION_PATH, json);
        }

        private void createUnkownExtensionsFile()
        {
            if (!File.Exists(UNKOWN_EXTENSION_PATH))
            {
                File.WriteAllText(UNKOWN_EXTENSION_PATH,"[]");
            }
        }
    }
}