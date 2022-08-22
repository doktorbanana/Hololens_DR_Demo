using System;
using System.Linq;
using OVRSimpleJSON;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class ProjectPropertySet : IProjectPropertySet
    {
        private readonly IWaapi waapi_;
        private readonly Lazy<IAttenuationOptionPropertySet[]> attenuationOptionPropertySets_;
        public string Name
        {
            get
            {
                var args = new JSONObject {["from"] = new JSONObject {["ofType"] = WaapiUtility.ToJSONArray("Project")}};
                var options = new JSONObject {["return"] = WaapiUtility.ToJSONArray("name")};
                var result = JSON.Parse(waapi_.Call("ak.wwise.core.object.get", args.ToString(), options.ToString()));
                return result["return"][0]["name"];
            }
        }

        public string ProjectPath
        {
            get
            {
                var args = new JSONObject {["from"] = new JSONObject {["ofType"] = WaapiUtility.ToJSONArray("Project")}};
                var options = new JSONObject {["return"] = WaapiUtility.ToJSONArray("filePath")};
                var result = JSON.Parse(waapi_.Call("ak.wwise.core.object.get", args.ToString(), options.ToString()));
                return result["return"][0]["filePath"];
            }
        }

        public IAttenuationOptionPropertySet[] AttenuationOptionPropertySets => attenuationOptionPropertySets_.Value;

        public ProjectPropertySet(IFactory factory = null)
        {
            factory ??= new Factory();
            waapi_ = factory.CreateWaapi();
            attenuationOptionPropertySets_ = new Lazy<IAttenuationOptionPropertySet[]>(() =>
            {
                var args = new JSONObject {["from"] = new JSONObject {["ofType"] = WaapiUtility.ToJSONArray("Attenuation")}};
                var options = new JSONObject {["return"] = WaapiUtility.ToJSONArray("name", "id")};
                var result = waapi_.Call("ak.wwise.core.object.get", args.ToString(), options.ToString());
                var returnArray = (JSONArray) JSONNode.Parse(result)["return"];
                var noneOption =
                    factory.CreateAttenuationOptionPropertySet("{00000000-0000-0000-0000-000000000000}", "None");
                return new[]{noneOption}.Concat(returnArray.Linq.Select(option =>
                {
                    var value = option.Value;
                    return factory.CreateAttenuationOptionPropertySet(value["id"], value["name"]);
                }).OrderBy(option => option.Name)).ToArray();
            });
        }
    }
}
