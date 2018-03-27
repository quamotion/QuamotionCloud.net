using Quamotion.Cloud.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Quamotion.Cloud.Client
{
    internal class CloudConnection : ICloudConnection
    {
        private HttpClient client = new HttpClient();

        private string Host { get; set; }
        private string AccessToken { get; set; }

        public CloudConnection(string host)
        {
            this.Host = host;
        }

        public async Task Login(string apiKey, CancellationToken cancellationToken)
        {
            Dictionary<string, string> loginData = new Dictionary<string, string>();
            loginData.Add("ApiKey", apiKey);

            string response = await this.PostFormRequest("/api/login", loginData, cancellationToken).ConfigureAwait(false);
            LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response);
            this.AccessToken = loginResponse.AccessToken;
        }

        public async Task<string> SendFile(string relativeUrl, string filePath, CancellationToken cancellationToken)
        {
            this.ensureLogin();

            string url = this.Host + relativeUrl;

            using (var fileStream = File.OpenRead(filePath))
            using (var fileStreamContent = new StreamContent(fileStream))
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                formData.Add(fileStreamContent, "files", Path.GetFileName(filePath));

                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.AccessToken);
                requestMessage.Content = formData;
                var response = await client.SendAsync(requestMessage).ConfigureAwait(false);

                this.ensureSuccess(response);
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        public async Task<string> GetRequest(string relativeUrl, CancellationToken cancellationToken)
        {
            return await SendRequest(relativeUrl, HttpMethod.Get, cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> DeleteRequest(string relativeUrl, CancellationToken cancellationToken)
        {
            return await SendRequest(relativeUrl, HttpMethod.Delete, cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> SendRequest(string relativeUrl, HttpMethod httpMethod, CancellationToken cancellationToken)
        {
            this.ensureLogin();

            string url = this.Host + relativeUrl;

            var requestMessage = new HttpRequestMessage(httpMethod, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.AccessToken);
            var response = await client.SendAsync(requestMessage).ConfigureAwait(false);
            this.ensureSuccess(response);

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public async Task<string> PostFormRequest(string relativeUrl, Dictionary<string, string> form, CancellationToken cancellationToken)
        {

            string url = this.Host + relativeUrl;

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.AccessToken);
            requestMessage.Content = new FormUrlEncodedContent(form);

            var response = await this.client.SendAsync(requestMessage).ConfigureAwait(false);
            this.ensureSuccess(response);

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public async Task<string> PostJsonRequest(string relativeUrl, Object content, CancellationToken cancellationToken)
        {
            this.ensureLogin();

            string url = this.Host + relativeUrl;

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.AccessToken);
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            var response = await this.client.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            this.ensureSuccess(response);

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        private void ensureLogin()
        {
            if (this.AccessToken == null)
            {
                throw new InvalidOperationException("You are not logged in. Please log in and try again");
            }
        }

        private void ensureSuccess(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) throw new InvalidOperationException(response.Content.ReadAsStringAsync().Result);
        }
    }
}
