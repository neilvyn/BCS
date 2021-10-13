﻿using System;
using Newtonsoft.Json.Linq;

namespace BCSTech.Services.Rest
{
    public interface IResponseConnector
    {
        void ReceiveJSONData(string jsonString, string ws_query);
        void ReceiveError(string title, string error, string ws_query);
    }
}
