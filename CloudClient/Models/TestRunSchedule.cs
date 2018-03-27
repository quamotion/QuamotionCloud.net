using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quamotion.Cloud.Client.Models
{
    public class TestRunSchedule
    {
        /// <summary>
        /// Gets or sets the ID of the pipeline schedule.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description of the pipeline schedule.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the branch which the pipeline executes.
        /// </summary>
        [JsonProperty("ref", NullValueHandling = NullValueHandling.Ignore)]
        public string Ref { get; set; }

        /// <summary>
        /// Gets or sets the cron schedule.
        /// </summary>
        [JsonProperty("cron", NullValueHandling = NullValueHandling.Ignore)]
        public string Cron { get; set; }

        /// <summary>
        /// Gets or sets the timezone of the pipeline schedule.
        /// </summary>
        [JsonProperty("cron_timezone", NullValueHandling = NullValueHandling.Ignore)]
        public string CronTimezone { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the next run.
        /// </summary>
        [JsonProperty("next_run_at")]
        public DateTime NextRunAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the pipeline schedule is active.
        /// </summary>
        [JsonProperty("active")]
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the date and time at which the pipeline schedule has been created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time at which the pipeline schedule has been updated.
        /// </summary>
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
