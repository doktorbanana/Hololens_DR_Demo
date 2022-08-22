using System;
using System.Linq;
using SpatialConnect.Wwise.Core.Wwu;

namespace SpatialConnect.Wwise.Core
{
    public interface IMixingSession : IDisposable
    {
        string Id { get; }
        
        IAudioObject SelectedAudioObject { get; }
        
        int? AudioObjectSelection { set; }
        
        IAudioObject[] AudioObjects { get; }
        
        event Action<int?, IAudioObject> AudioObjectSelectionChanged;
    }
    
    public class MixingSession : IMixingSession, IDisposable
    {
        public string Id { get; }

        private int? audioObjectSelection_;
        public int? AudioObjectSelection
        {
            set
            {
                audioObjectSelection_ = value;
                AudioObjectSelectionChanged?.Invoke(audioObjectSelection_, SelectedAudioObject);
            }
        }
        
        public IAudioObject[] AudioObjects { get; }

        public IAudioObject SelectedAudioObject => audioObjectSelection_.HasValue ? AudioObjects[audioObjectSelection_.Value] : null;

        public MixingSession(MixingSessionPropertySet mixingSessionPropertySet, IMessageBroadcaster messageBroadcaster, IFactory factory)
        {
            Id = mixingSessionPropertySet.Id;
            
            AudioObjects = mixingSessionPropertySet.AudioObjectIds.Select(audioObjectId => 
                    factory.CreateMixerAudioObject(audioObjectId, messageBroadcaster))
                .ToArray();
        }

        public void Dispose()
        {
            foreach (var audioObject in AudioObjects)
                if (audioObject != SelectedAudioObject)
                    audioObject.Dispose();
        }
        
        public event Action<int?, IAudioObject> AudioObjectSelectionChanged;
    }
}
