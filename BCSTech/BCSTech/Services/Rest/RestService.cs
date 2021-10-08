using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BCSTech.Services.Network;
using BCSTech.Services.Predefined;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace BCSTech.Services.Rest
{
    public class RestService : IRestService
    {
        NetworkHelper networkHelper = NetworkHelper.GetInstance;
        static RestService _restService;
        WeakReference<IRestConnector> _restServiceDelegate;

        public IRestConnector RestServiceDelegate
        {
            get
            {
                IRestConnector restServiceDelegate;
                return _restServiceDelegate.TryGetTarget(out restServiceDelegate) ? restServiceDelegate : null;
            }
            set
            {
                _restServiceDelegate = new WeakReference<IRestConnector>(value);
            }
        }

        public void SetDelegate(IRestConnector weakReference)
        {
            RestServiceDelegate = weakReference;
        }

        public static RestService GetInstance
        {
            get { if (_restService == null) _restService = new RestService(); return _restService; }
        }

        public RestService()
        {

        }

        public async Task GetRequest(string url, CancellationToken ct, int wsType, string authHeader = null, int timeout = 100)
        {
            LogConsole.AsyncOutput(this, "Request URL: " + url);

            if (NetworkHelper.GetInstance.HasInternet())
            {
                if (await NetworkHelper.GetInstance.IsHostReachable() == true)
                {
                    var handler = new TimeoutHandler
                    {
                        DefaultTimeout = TimeSpan.FromSeconds(100),
                        InnerHandler = new HttpClientHandler()
                    };

                    using (var client = new HttpClient(handler))
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.MaxResponseContentBufferSize = 256000;

                        if (!string.IsNullOrEmpty(authHeader))
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authHeader);
                        }

                        client.Timeout = Timeout.InfiniteTimeSpan;

                        var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url));

                        request.SetTimeout(TimeSpan.FromSeconds(timeout));

                        using (var response = await client.SendAsync(request, ct))
                        {
                            await RequestAsync(response, ct, wsType);
                        }
                    }
                }
                else
                {
                    RestServiceDelegate?.ReceiveError(Constants.HOST_UNREACHABLE.Title, Constants.HOST_UNREACHABLE.Description, wsType);
                }
            }
            else
            {
                RestServiceDelegate?.ReceiveError(Constants.NO_CONNECTION.Title, Constants.NO_CONNECTION.Description, wsType);
            }
        }

        public async Task PostRequestAsync(string url, object dictionary, CancellationToken ct, int wsType, string authHeader = null, int timeout = 100)
        {
            LogConsole.AsyncOutput(this, "Request URL: " + url);
            LogConsole.AsyncOutput(this, "=========================\n" + JToken.Parse(dictionary.ToString()));

            if (NetworkHelper.GetInstance.HasInternet())
            {
                if (await NetworkHelper.GetInstance.IsHostReachable() == true)
                {
                    var handler = new TimeoutHandler
                    {
                        DefaultTimeout = TimeSpan.FromSeconds(100),
                        InnerHandler = new HttpClientHandler()
                    };

                    using (var client = new HttpClient(handler))
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.MaxResponseContentBufferSize = 256000;

                        if (!string.IsNullOrEmpty(authHeader))
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authHeader);
                        }

                        client.Timeout = Timeout.InfiniteTimeSpan;

                        var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url));

                        request.SetTimeout(TimeSpan.FromSeconds(timeout));

                        request.Content = new StringContent((string)dictionary, Encoding.UTF8, "application/json");

                        using (var response = await client.SendAsync(request, ct))
                        {
                            await RequestAsync(response, ct, wsType);
                        }
                    }
                }
                else
                {
                    RestServiceDelegate?.ReceiveError(Constants.HOST_UNREACHABLE.Title, Constants.HOST_UNREACHABLE.Description, wsType);
                }
            }
            else
            {
                RestServiceDelegate?.ReceiveError(Constants.NO_CONNECTION.Title, Constants.NO_CONNECTION.Description, wsType);
            }
        }

        async Task RequestAsync(HttpResponseMessage response, CancellationToken ct, int wsType)
        {
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
                RestServiceDelegate?.ReceiveJSONData(result, wsType);
            else
                RestServiceDelegate?.ReceiveError(Constants.CriticalTitleAlert, "Something went wrong.", wsType);
        }
    }
}
