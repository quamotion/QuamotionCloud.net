using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quamotion.Cloud.Client
{
    public interface ICloudConnection
    {
        Task Login(string apiKey, CancellationToken cancellationToken);

        Task<string> SendFile(string relativeUrl, string filePath, CancellationToken cancellationToken);

        Task<string> GetRequest(string relativeUrl, CancellationToken cancellationToken);

        Task<string> DeleteRequest(string relativeUrl, CancellationToken cancellationToken);

        Task<string> PostFormRequest(string relativeUrl, Dictionary<string, string> form, CancellationToken cancellationToken);

        Task<string> PostJsonRequest(string relativeUrl, Object content, CancellationToken cancellationToken);
    }
}
