using System;
using OVRSimpleJSON;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class ChildSet: IChildSet
    {
        private readonly IWaapi waapi_;
        private readonly string id_;
        private readonly string[] returnRequests_;
        private readonly Func<string, IAudioObjectPropertySet[]> postProcess_;
            
        public IAudioObjectPropertySet[] Children
        {
            get
            {
                var idArray = WaapiUtility.ToJSONArray(id_);
                var selectArray = WaapiUtility.ToJSONArray("children");
                var transformArray = new JSONArray();
                transformArray.Add(new JSONObject {["select"] = selectArray});
                var args = new JSONObject {["from"] = new JSONObject {["id"] = idArray}, ["transform"] = transformArray};
                var returnArray = WaapiUtility.ToJSONArray(returnRequests_);
                var options = new JSONObject { ["return"] = returnArray };
                var result = waapi_.Call("ak.wwise.core.object.get", args.ToString(), options.ToString());

                return postProcess_.Invoke(result);
            }
        }

        public ChildSet(string id, string[] returnRequests, Func<string, IAudioObjectPropertySet[]> postProcess, IFactory factory = null)
        {
            factory ??= new Factory();
            waapi_ = factory.CreateWaapi();
            id_ = id;
            returnRequests_ = returnRequests;
            postProcess_ = postProcess;
        }
    }
}
