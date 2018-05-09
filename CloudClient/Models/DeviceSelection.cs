using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quamotion.Cloud.Client.Models
{
    public class DeviceSelection
    {
        [JsonProperty("deviceSelectionId")]
        public string DeviceSelectionId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("variables")]
        public Dictionary<string, string> Variables { get; set; }
    }
}
