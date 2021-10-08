using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BCSTech.Services.Rest
{
    public interface IRestService
    {
        void SetDelegate(IRestConnector weakReference);
        Task GetRequest(string url, CancellationToken ct, int wsType, string authHeader = null, int timeout = 100);
        Task PostRequestAsync(string url, object dictionary, CancellationToken ct, int wsType, string authHeader = null, int timeout = 100);
        // PUT and DELETE
    }
}
