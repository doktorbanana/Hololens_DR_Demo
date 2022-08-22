namespace SpatialConnect.Wwise.Core
{
    public class EventAudioObject : IAudioObject
    {
        public string Id { get; }
        public IText NameText { get; }
        public IToggle SoloToggle => null;
        public IToggle MuteToggle => null;
        public IValue VoiceVolumeValue { get; }
        public IPositioningPropertySet PositioningPropertySet { get; }
        
        public EventAudioObject(string id, IMessageBroadcaster messageBroadcaster, IFactory factory)
        {
            Id = id;
            NameText = factory.CreateNameText(id);
            VoiceVolumeValue = factory.CreateVoiceVolumeValue(id);
            PositioningPropertySet = factory.CreatePositioningPropertySet(id, messageBroadcaster);
        }
        
        public void Dispose()
        {
            VoiceVolumeValue.Dispose();
            PositioningPropertySet.Dispose();
        }
    }
}
