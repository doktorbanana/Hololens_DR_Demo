using System;
using SpatialConnect.Wwise.Core;
using UnityEngine;

namespace SpatialConnect.Wwise
{
    public class WaapiClient : IWaapiClient
    {
        public bool Connect()
        {
            return AkWaapiClient.Connect("127.0.0.1", 8080);
        }

        public string Call(string functionUri, string argument, string option)
        {
            var success = AkWaapiClient.Call(functionUri, argument, option, out var result);
            if (!success)
                throw new Exception("akWaapiClient.Call unsuccessful" + result);
            return result;
        }

        public ulong Subscribe(string topic, string option, Action<ulong, string> callback)
        {
            var success = AkWaapiClient.Subscribe(topic, option, callback.Invoke, out var subscriptionID, out var detail);
            if (!success)
                throw new Exception("akWaapiClient.Subscribe unsuccessful. " + detail);
            return subscriptionID;
        }

        public void Unsubscribe(ulong subscriptionId)
        {
            var success = AkWaapiClient.Unsubscribe(subscriptionId, out var detail);
            if (!success)
                throw new Exception("akWaapiClient.Unsubscribe unsuccessful. " + detail);
        }

        public void Disconnect()
        {
            AkWaapiClient.Disconnect();
        }
    }
}
