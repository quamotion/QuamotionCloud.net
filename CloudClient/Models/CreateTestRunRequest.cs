using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quamotion.Cloud.Client.Models
{
    public class CreateTestRunRequest
    {
        [JsonProperty("app")]
        public Application App { get; set; }

        [JsonProperty("testPackage")]
        public TestPackage TestPackage { get; set; }

        [JsonProperty("deviceGroupId")]
        public Guid DeviceGroupId { get; set; }

        [JsonProperty("schedule")]
        public string Schedule { get; set; }

        [JsonProperty("testScriptParameters")]
        public string TestScriptParameters { get; set; }

        [JsonProperty("testScriptEnvironmentVariables")]
        public Dictionary<string, string> TestScriptEnvironmentVariables { get; set; }

        [JsonProperty("resultsCallBack")]
        public ResultsCallBack ResultsCallBack { get; set; }
    }
}