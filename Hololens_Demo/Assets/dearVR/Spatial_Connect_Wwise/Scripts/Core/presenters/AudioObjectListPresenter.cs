using System;

namespace SpatialConnect.Wwise.Core
{
    public class AudioObjectListPresenter : IDisposable
    {
        private readonly IAudioObjectListBehaviour audioObjectListBehaviour_;
        private readonly IEvent event_;

        public AudioObjectListPresenter(IAudioObjectListBehaviour audioObjectListBehaviour, IEvent @event)
        {
            audioObjectListBehaviour_ = audioObjectListBehaviour;
            event_ = @event;

            audioObjectListBehaviour_.AudioObjectListSelectionChanged += OnAudioObjectListSelectionChanged;
            event_.AudioObjectSelectionChanged += OnAudioObjectSelectionChanged;
        }

        private void OnAudioObjectListSelectionChanged(int[] index)
        {
            event_.AudioObjectSelection = index;
        }

        private void OnAudioObjectSelectionChanged(IAudioObjectPropertySet audioObjectPropertySet, int[] index)
        {
            audioObjectListBehaviour_.Select(index);
        }
        
        public void Dispose()
        {
            audioObjectListBehaviour_.AudioObjectListSelectionChanged -= OnAudioObjectListSelectionChanged;
            event_.AudioObjectSelectionChanged -= OnAudioObjectSelectionChanged;
        }
    }
}
