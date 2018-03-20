using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quamotion.Cloud.Client.Models
{
    public class Application
    {
        [JsonProperty("appId")]
        public string AppId { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("versionDisplayName")]
        public string VersionDisplayName { get; set; }

        [JsonProperty("operatingSystem")]
        public string OperatingSystem { get; set; }

        [JsonProperty("operatingSystemVersion")]
        public string OperatingSystemVersion { get; set; }

        [JsonProperty("cpuType")]
        public string CpuType { get; set; }
    }
}