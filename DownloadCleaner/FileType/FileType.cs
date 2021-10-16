using System.Collections.Generic;
using Newtonsoft.Json;

namespace DownloadCleaner.FileType
{
    public class FileType
    {
        [JsonProperty]
        public string TypeName;
        
        [JsonProperty]
        public string Path;
        
        [JsonProperty]
        public List<string> Extensions { get; } = new List<string>();
        
        [JsonProperty]
        public List<SubType> SubTypes { get; } = new List<SubType>();
        
    }

    public class SubType
    {
        [JsonProperty]
        public string TypeName;
        
        [JsonProperty]
        public string Path;
        
        [JsonProperty]
        public List<string> Extensions;
    }
}