using System;
using OVRSimpleJSON;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class Spatialization : ISpatialization
    {
        public bool Enabled => IsSpatializationEnabled();

        private string id_;

        private IWaapi waapi_;

        public Spatialization(string id, IFactory factory = null)
        {
            factory = factory ?? new Factory();
            waapi_ = factory.CreateWaapi();

            id_ = id;
        }

        private bool IsSpatializationEnabled()
        {
            var idArray = WaapiUtility.ToJSONArray(id_);
            var args = new JSONObject {["from"] = new JSONObject {["id"] = idArray}};
            var returnArray = WaapiUtility.ToJSONArray("@3DSpatialization");
            var options = new JSONObject {["return"] = returnArray};
            var result = waapi_.Call("ak.wwise.core.object.get", args.ToString(), options.ToString());

            return Convert.ToBoolean((int)JSON.Parse(result)["return"][0]["@3DSpatialization"]);
        }
    }
}
