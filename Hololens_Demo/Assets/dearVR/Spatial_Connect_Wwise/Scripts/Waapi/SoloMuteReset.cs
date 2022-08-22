using OVRSimpleJSON;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class SoloMuteReset : IReset
    {
        public SoloMuteReset(IWaapi waapi)
        {
            var argsSolo = new JSONObject {["command"] = "ResetAllMutes"};
            waapi.Call("ak.wwise.ui.commands.execute", argsSolo.ToString(), "{}");

            var argsMute = new JSONObject {["command"] = "ResetAllSolos"};
            waapi.Call("ak.wwise.ui.commands.execute", argsMute.ToString(), "{}");
        }
    }
}
