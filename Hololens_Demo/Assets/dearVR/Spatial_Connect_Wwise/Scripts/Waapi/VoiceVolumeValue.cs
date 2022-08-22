using System;
using OVRSimpleJSON;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class VoiceVolumeValue : IValue
    {
        public float Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        private readonly IWaapi waapi_;
        private readonly string id_;
        private readonly ulong subscriptionId_;

        public VoiceVolumeValue(IWaapi waapi, string id)
        {
            waapi_ = waapi;
            id_ = id;

            var options = new JSONObject { ["object"] = id_, ["property"] = "Volume" };
            subscriptionId_ = waapi_.Subscribe("ak.wwise.core.object.propertyChanged", options.ToString(), OnVoiceVolumeChanged);
        }

        private void OnVoiceVolumeChanged(ulong subscriptionId, string contents)
        {
            var value = (float)JSON.Parse(contents)["new"];
            ValueChanged?.Invoke(value);
        }

        private float GetValue()
        {
            var idArray = new JSONArray();
            idArray.Add(id_);
            var args = new JSONObject { ["from"] = new JSONObject { ["id"] = idArray }};

            var returnJsonArray = new JSONArray();
            returnJsonArray.Add("@Volume");
            var options = new JSONObject { ["return"] = returnJsonArray };

            var result = waapi_.Call("ak.wwise.core.object.get", args.ToString(), options.ToString());
            return JSON.Parse(result)["return"][0]["@Volume"];
        }

        private void SetValue(float value)
        {
            var args = new JSONObject { ["object"] = id_, ["property"] = "Volume", ["value"] = value};
            waapi_.Call("ak.wwise.core.object.setProperty", args.ToString(), "{}");
        }

        public void Dispose()
        {
            waapi_.Unsubscribe(subscriptionId_);
        }

        public event Action<float> ValueChanged;

    }
}
