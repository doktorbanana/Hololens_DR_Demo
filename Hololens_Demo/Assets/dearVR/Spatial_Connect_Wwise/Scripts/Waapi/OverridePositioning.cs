using System;
using System.Linq;
using OVRSimpleJSON;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class OverridePositioning : IOverrideParent
    {
        private readonly string id_;
        private readonly IWaapi waapi_;
        
        public bool Enabled
        {
            get
            {
                if (IsOverridePositioningEnabled())
                    return true;
                return !DoesParentExist();
                    
                bool DoesParentExist()
                {
                    var idArray = WaapiUtility.ToJSONArray(id_);
                    var selectArray = WaapiUtility.ToJSONArray("ancestors");
                    var transformArray = new JSONArray();
                    transformArray.Add(new JSONObject {["select"] = selectArray});
                    var args = new JSONObject {["from"] = new JSONObject {["id"] = idArray}, ["transform"] = transformArray};
                    var returnArray = WaapiUtility.ToJSONArray("type");
                    var options = new JSONObject {["return"] = returnArray};
                    var result = waapi_.Call("ak.wwise.core.object.get", args.ToString(), options.ToString());
                    return JSON.Parse(result)["return"].Linq
                        .Any(item => item.Value["type"] != "WorkUnit");
                }

                bool IsOverridePositioningEnabled()
                {
                    var idArray = WaapiUtility.ToJSONArray(id_);
                    var args = new JSONObject {["from"] = new JSONObject {["id"] = idArray}};
                    var returnArray = WaapiUtility.ToJSONArray("@OverridePositioning");
                    var options = new JSONObject {["return"] = returnArray};
                    var result = waapi_.Call("ak.wwise.core.object.get", args.ToString(), options.ToString());
                    return (bool)JSON.Parse(result)["return"][0]["@OverridePositioning"];
                }
            }
        }

        public OverridePositioning(string id, IFactory factory = null)
        {
            factory = factory ?? new Factory();
            waapi_ = factory.CreateWaapi();
            
            id_ = id;
        }
    }
}
