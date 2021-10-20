using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace DownloadCleaner
{

    public class Settings
    {
        public const string JSON_PATH = "settings.json";

        public  string downloadPath;

        public  int minimumMoveTime;

        public  List<FileType.FileType> fileTypes;

        public  string unknownExtensionFolderName;
        
        private static Settings instance;

        public static Settings GetInstance()
        {
            return instance ??= new Settings();
        }
        
        private Settings()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            var logger = Logger.GetInstance();
            try
            {
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                var wrapper = JsonConvert.DeserializeObject<Wrapper>(File.ReadAllText(JSON_PATH),settings);

                CheckForErrors(wrapper);

                if (wrapper == null) return;
                
                fileTypes = wrapper.fileTypes;
                downloadPath = wrapper.downloadPath;
                unknownExtensionFolderName = wrapper.unknownExtensionFolderName;
                minimumMoveTime = wrapper.minimumMoveTime;

            }
            catch (IOException)
            {
                logger.Error("The Settings file: {settingsPath} could not be found", JSON_PATH);
                Process.GetCurrentProcess().Kill();
            }
            catch (NullReferenceException exception)
            {
                logger.Error(exception.Message);
                Process.GetCurrentProcess().Kill();
            }
        }
        
        private static void CheckForErrors(Wrapper wrapper)
        {
            if (wrapper.downloadPath is null)
            {
                throw new NullReferenceException("The attribute \"DownloadPath\" is not set");
            }
            
            if (wrapper.fileTypes is null)
            {
                throw new NullReferenceException("The attribute \"FileTypes\" is not set");
            }

            if (wrapper.unknownExtensionFolderName is null)
            {
                throw new NullReferenceException("The attribute \"UnknownExtensionFolderName\" is not set");
            }
            
            if (wrapper.minimumMoveTime is 0)
            {
                throw new NullReferenceException("The attribute \"MinimumMoveTime\" is not set or 0");
            }
        }


        public void Reload()
        {
            LoadSettings();
        }
    }
    
    //Values get set by Deserializer
    #pragma warning disable 0649
    internal class Wrapper
    {
        [JsonProperty]
        public string downloadPath;
        
        [JsonProperty]
        public int minimumMoveTime;
        
        [JsonProperty]
        public List<FileType.FileType> fileTypes;
        
        [JsonProperty]
        public string unknownExtensionFolderName;

    }
    #pragma warning restore 0649
    
}