using OVRSimpleJSON;
using SpatialConnect.Wwise;
using SpatialConnect.Wwise.Core;

public class RemoteConnector
{
    private IWaapi waapi_ = new WaapiClient();
    public void Toggle()
    {
        if (IsRemoteConnected())
            Disconnect();
        else
            Connect();
        
        bool IsRemoteConnected()
        {
            var result = waapi_.Call("ak.wwise.core.remote.getConnectionStatus", "{}", "{}");
            return (bool) JSON.Parse(result)["isConnected"];
        }

        void Connect()
        {
            waapi_.Call("ak.wwise.core.remote.connect", "{\"host\":\"127.0.0.1\"}", "{}");
        }
        
        void Disconnect()
        {
            waapi_.Call("ak.wwise.core.remote.disconnect", "{}", "{}");
        }
    }
}
