using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quamotion.Cloud.Client
{
    public interface ICloudConnection
    {
        Task Login(string apiKey);

        Task<string> SendFile(string relativeUrl, string filePath);

        Task<string> GetRequest(string relativeUrl);

        Task<string> PostFormRequest(string relativeUrl, Dictionary<string, string> form);

        Task<string> PostJsonRequest(string relativeUrl, Object content);
    }
}
