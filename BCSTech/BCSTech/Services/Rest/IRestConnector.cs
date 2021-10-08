using System;
using Newtonsoft.Json.Linq;

namespace BCSTech.Services.Rest
{
    public interface IRestConnector
    {
        void ReceiveJSONData(string jsonString, int wsType);
        void ReceiveError(string title, string error, int wsType);
    }
}
