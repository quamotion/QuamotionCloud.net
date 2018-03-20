using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quamotion.Cloud.Client.Models
{
    public class TestRun
    {
        [JsonProperty("testRunId")]
        public Guid TestRunId;

        [JsonProperty("deviceGroup")]
        public Guid DeviceGroupId;

        [JsonProperty("operatingSystem")]
        public int OperatingSystem;

        [JsonProperty("appId")]
        public string ApplicationId;

        [JsonProperty("appVersion")]
        public string ApplicationVersion;

        [JsonProperty("testPackageName")]
        public string TestPackageName;

        [JsonProperty("testPackageVersion")]
        public string TestPackageVersion;

        [JsonProperty("scheduled")]
        public DateTime Scheduled;

        [JsonProperty("commitId")]
        private string CommitId;

        [JsonProperty("branch")]
        public string Branch;

        [JsonProperty("scheduleId")]
        public int? ScheduleId;

        [JsonProperty("cronSchedule")]
        public string CronSchedule;

        [JsonProperty("jobs")]
        public TestJob[] Jobs;
    }
}
