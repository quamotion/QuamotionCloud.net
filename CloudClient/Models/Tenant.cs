using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quamotion.Cloud.Client.Models
{
    public class Tenant
    {
        /// <summary>
        /// Gets or sets the name of the tenant.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the unique ID of this tenant.
        /// </summary>
        [JsonProperty("tenantId")]
        public int TenantId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the GitLab project that backs this tenant.
        /// </summary>
        [JsonProperty("gitLabId")]
        public int? GitLabId { get; set; }

        /// <summary>
        /// Gets or sets flags for this tenant.
        /// </summary>
        [JsonProperty("flags")]
        public int Flags { get; set; }

        /// <summary>
        /// Gets the URL of the tenant.
        /// </summary>
        [JsonProperty("relativeUrl")]
        public string RelativeUrl { get; set; }
    }
}