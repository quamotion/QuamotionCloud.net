using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quamotion.Cloud.Client.Models
{
    public class LoginResponse
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int? ExpiresIn { get; set; }

        [JsonProperty("ext_expires_in")]
        public int? ExtExpiresIn { get; set; }

        [JsonProperty("expires_on")]
        public int? ExpiresOn { get; set; }

        [JsonProperty("not_before")]
        public int? NotBefore { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
