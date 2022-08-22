namespace SpatialConnect.Wwise.Core
{
    public class MixerAudioObject : IAudioObject
    {
        public string Id { get; }
        public IText NameText { get; }
        public IToggle SoloToggle { get; }
        public IToggle MuteToggle { get; }
        public IValue VoiceVolumeValue { get; }
        public IPositioningPropertySet PositioningPropertySet { get; }
        
        public MixerAudioObject(string id, IMessageBroadcaster messageBroadcaster, IFactory factory)
        {
            Id = id;
            NameText = factory.CreateNameText(id);
            VoiceVolumeValue = factory.CreateVoiceVolumeValue(id);
            PositioningPropertySet = factory.CreatePositioningPropertySet(id, messageBroadcaster);
            SoloToggle = factory.CreateSoloToggle(id, messageBroadcaster.SoloToggleMessageChannel);
            MuteToggle = factory.CreateMuteToggle(id, messageBroadcaster.MuteToggleMessageChannel);
        }
        
        public void Dispose()
        {
            VoiceVolumeValue.Dispose();
            PositioningPropertySet.Dispose();
            SoloToggle.Dispose();
            MuteToggle.Dispose();
        }
    }
}
