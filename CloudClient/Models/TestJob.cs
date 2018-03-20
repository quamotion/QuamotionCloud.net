using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quamotion.Cloud.Client.Models
{
    public class TestJob
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("started")]
        public DateTime Started { get; set; }

        [JsonProperty("commitId")]
        public string CommitId { get; set; }

        [JsonProperty("hasArtifacts")]
        public Boolean HasArtifacts { get; set; }
    }
}
