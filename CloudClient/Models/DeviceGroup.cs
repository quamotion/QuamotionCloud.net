using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quamotion.Cloud.Client.Models
{
    public class DeviceGroup
    {
        [JsonProperty("deviceGroupId")]
        public Guid DeviceGroupId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("devices")]
        public List<DeviceSelection> Devices { get; set; }

        [JsonProperty("variables")]
        public Dictionary<string, string> Variables { get; set; }
    }
}
