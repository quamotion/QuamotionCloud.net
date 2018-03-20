using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quamotion.Cloud.Client.Models
{
    public class TestPackage
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("testPackageType")]
        public string TestPackageType { get; set; }
    }
}
