using System;
using OVRSimpleJSON;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class PositioningPropertySet : IPositioningPropertySet
    {

        private readonly IFactory factory_;
        private readonly IWaapi waapi_;
        private readonly string audioObjectId_;
        private readonly IMessageBroadcaster messageBroadcaster_;

        
        public bool IsShareSetSelectable => IsAttenuationEnabled && OverridePositioning.Enabled;
        public bool IsEditable => IsAttenuationEnabled && AttenuationOption.Type == AttenuationType.ShareSet && OverridePositioning.Enabled;

        public IOverrideParent OverridePositioning { get; }

        public IAttenuationOption AttenuationOption { get; private set; }

        public bool IsAttenuationEnabled
        {
            get
            {
                var idArray = WaapiUtility.ToJSONArray(audioObjectId_);
                var args = new JSONObject {["from"] = new JSONObject {["id"] = idArray}};
                var returnArray = WaapiUtility.ToJSONArray("@EnableAttenuation");
                var options = new JSONObject {["return"] = returnArray};
                var resultEnabled = waapi_.Call("ak.wwise.core.object.get", args.ToString(), options.ToString());
            
                return (bool) JSON.Parse(resultEnabled)["return"][0]["@EnableAttenuation"];
            }
        }

        public PositioningPropertySet(string audioObjectId, IMessageBroadcaster messageBroadcaster, IFactory factory = null)
        {
            factory_ = factory?? new Factory();
            
            waapi_ = factory_.CreateWaapi();
            audioObjectId_ = audioObjectId;
            OverridePositioning = factory_.CreateOverridePositioning(audioObjectId_);
            messageBroadcaster_ = messageBroadcaster;
            messageBroadcaster_.AttenuationReferenceChangedMessageChannel.ReferenceChanged += OnAttenuationReferenceChanged;

            if (IsNone())
                AttenuationOption = factory_.CreateAttenuationOption("{00000000-0000-0000-0000-000000000000}", "None",
                    messageBroadcaster_.AttenuationCurveChangedMessageChannel);
            else
            {
                var idArray = WaapiUtility.ToJSONArray(GetAttenuationId());
                var args = new JSONObject {["from"] = new JSONObject {["id"] = idArray}};
                var returnArray = WaapiUtility.ToJSONArray("name");
                var options = new JSONObject {["return"] = returnArray};
                var result = waapi_.Call("ak.wwise.core.object.get", args.ToString(), options.ToString());
                string name = JSON.Parse(result)["return"][0]["name"];
                var id = GetAttenuationId();

                AttenuationOption = factory_.CreateAttenuationOption(id, name, messageBroadcaster_.AttenuationCurveChangedMessageChannel);
            }
            
            string GetAttenuationId()
            {
                var idArray = WaapiUtility.ToJSONArray(audioObjectId_);
                var args = new JSONObject {["from"] = new JSONObject {["id"] = idArray}};
                var returnArray = WaapiUtility.ToJSONArray("@Attenuation");
                var options = new JSONObject {["return"] = returnArray};
                var result = waapi_.Call("ak.wwise.core.object.get", args.ToString(), options.ToString());

                return JSON.Parse(result)["return"][0]["@Attenuation"]["id"];
            }
            
            bool IsNone()
            {
                return GetAttenuationId() == "{00000000-0000-0000-0000-000000000000}";
            }
        }

        private void OnAttenuationReferenceChanged(ReferenceChangedMessage message)
        {
            if(message.targetAudioObjectId != audioObjectId_)
                return;
            
            AttenuationOption.Dispose();
            AttenuationOption = factory_.CreateAttenuationOption(message.attenuationId, message.attenuationName, messageBroadcaster_.AttenuationCurveChangedMessageChannel);
            AttenuationOptionChanged?.Invoke();
        }
        
        public void Dispose()
        {
            messageBroadcaster_.AttenuationReferenceChangedMessageChannel.ReferenceChanged -= OnAttenuationReferenceChanged;
            AttenuationOption.Dispose();
        }

        public void SetAttenuationOption(string attenuationId)
        {
            var args = new JSONObject
            {
                ["object"] = audioObjectId_,
                ["reference"] = "Attenuation",
                ["value"] = attenuationId
            };
            waapi_.Call("ak.wwise.core.object.setReference", args.ToString(), "{}");
        }

        public event Action AttenuationOptionChanged;
    }
}
