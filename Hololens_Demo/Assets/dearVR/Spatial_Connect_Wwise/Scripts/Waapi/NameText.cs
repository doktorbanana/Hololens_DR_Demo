using OVRSimpleJSON;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class NameText : IText
    {
        public string Text => GetText();

        private readonly IWaapi waapi_;
        private readonly string id_;

        public NameText(string id, IFactory factory = null)
        {
            factory = factory ?? new Factory();
            waapi_ = factory.CreateWaapi();
            id_ = id;
        }

        private string GetText()
        {
            var idArray = new JSONArray();
            idArray.Add(id_);
            var args = new JSONObject { ["from"] = new JSONObject { ["id"] = idArray }};

            var returnJsonArray = new JSONArray();
            returnJsonArray.Add("name");
            var options = new JSONObject { ["return"] = returnJsonArray };

            var result = waapi_.Call("ak.wwise.core.object.get", args.ToString(), options.ToString());
            return JSON.Parse(result)["return"][0]["name"];
        }
    }
}
