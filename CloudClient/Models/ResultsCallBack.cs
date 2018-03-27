using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quamotion.Cloud.Client.Models
{
    /// <summary>
    /// Class containing the callback info.
    /// </summary>
    public class ResultsCallBack
    {
        /// <summary>
        /// Gets or sets the API key to be used
        /// </summary>
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the end point for the callback.
        /// </summary>
        [JsonProperty("endPoint")]
        public string EndPoint { get; set; }
    }
}
