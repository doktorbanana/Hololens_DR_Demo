using OVRSimpleJSON;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class MessageBroadcaster : IMessageBroadcaster
    {
        private readonly IWaapi waapi_;

        public IToggleMessageChannel SoloToggleMessageChannel { get; }
        public IToggleMessageChannel MuteToggleMessageChannel { get; }
        public IReferenceChangedMessageChannel AttenuationReferenceChangedMessageChannel { get; }
        public IAttenuationCurveChangedMessageChannel AttenuationCurveChangedMessageChannel { get; }
        
        public MessageBroadcaster(IWaapi waapi, IFactory factory = null)
        {
            factory = factory ?? new Factory();
            SoloToggleMessageChannel = factory.CreateSoloToggleMessageChannel();
            MuteToggleMessageChannel = factory.CreateMuteToggleMessageChannel();
            AttenuationReferenceChangedMessageChannel = factory.CreateAttenuationReferenceChangedMessageChannel();
            AttenuationCurveChangedMessageChannel = factory.CreateAttenuationCurveChangedMessageChannel();
            
            waapi_ = waapi;
        }

        public void Subscribe()
        {
            waapi_.Subscribe("ak.wwise.ui.commands.executed", 
                new JSONObject {["return"] = WaapiUtility.ToJSONArray("id")}.ToString(),
                OnUiCommandMessageReceived);

            waapi_.Subscribe("ak.wwise.core.object.referenceChanged", 
                new JSONObject {["return"] = WaapiUtility.ToJSONArray("id", "name")}.ToString(),
                OnReferenceChangedMessageReceived);
            
            waapi_.Subscribe("ak.wwise.core.object.attenuationCurveChanged", "{}", OnAttenuationCurveChanged);
            
            void OnUiCommandMessageReceived(ulong subscriptionId, string contents)
            {
                SoloToggleMessageChannel.Stream(contents);
                MuteToggleMessageChannel.Stream(contents);
            }

            void OnReferenceChangedMessageReceived(ulong subscriptionId, string contents)
            {
                AttenuationReferenceChangedMessageChannel.Stream(contents);
            }

            void OnAttenuationCurveChanged(ulong subscriptionId, string contents)
            {
                AttenuationCurveChangedMessageChannel.Stream(contents);
            }
        }
    }
}
