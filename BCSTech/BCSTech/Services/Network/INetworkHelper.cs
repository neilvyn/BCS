using System;
using System.Threading.Tasks;

namespace BCSTech.Services.Network
{
    public interface INetworkHelper
    {
        bool HasInternet();
        Task<bool> IsHostReachable();
    }
}
