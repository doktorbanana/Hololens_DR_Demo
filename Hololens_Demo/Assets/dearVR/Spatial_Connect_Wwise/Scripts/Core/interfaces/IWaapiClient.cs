using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IWaapi
    {
        string Call(string functionUri, string argument, string option);
        
        ulong Subscribe(string topic, string option, Action<ulong, string> callback);
        
        void Unsubscribe(ulong subscriptionId);
    }
    
    public interface IWaapiClient : IWaapi
    {
        bool Connect();

        void Disconnect();
    }
}
