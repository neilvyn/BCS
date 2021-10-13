using System;
using System.Threading.Tasks;
using Plugin.Connectivity;

namespace BCSTech.Services.Network
{
    public class NetworkHelper : INetworkHelper
    {
        public bool HasInternet()
        {
            if (!CrossConnectivity.IsSupported)
            {
                return true;
            }

            return CrossConnectivity.Current.IsConnected;
        }

        public async Task<bool> IsHostReachable()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return false;
            }
            return true;
        }
    }
}
