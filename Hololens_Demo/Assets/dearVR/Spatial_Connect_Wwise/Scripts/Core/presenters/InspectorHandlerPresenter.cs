using System;

namespace SpatialConnect.Wwise.Core
{
    public class InspectorHandlerPresenter : IDisposable
    {
        private readonly IInspectorBehaviour inspectorBehaviour_;
        private readonly IMixingSessionManager mixingSessionManager_;
        private readonly IEventManager eventManager_;
        private readonly IProjectPropertySet projectPropertySet_;
        
        private IMixingSession previousMixingSession_;
        
        public InspectorHandlerPresenter(IInspectorBehaviour inspectorBehaviour, 
            IMixingSessionManager mixingSessionManager, IEventManager eventManager, IProjectPropertySet projectPropertySet)
        {
            inspectorBehaviour_ = inspectorBehaviour;
            mixingSessionManager_ = mixingSessionManager;
            eventManager_ = eventManager;
            projectPropertySet_ = projectPropertySet;

            mixingSessionManager_.MixingSessionSelectionChanged += OnMixingSelectionChanged;
            eventManager_.AudioObjectSelected += OnAudioObjectSelectedInEvent;
        }

        private void OnMixingSelectionChanged(int selection, IMixingSession mixingSession)
        {
            if (previousMixingSession_ != null)
                previousMixingSession_.AudioObjectSelectionChanged -= OnAudioObjectSelectionChangedInMixer;

            mixingSession.AudioObjectSelectionChanged += OnAudioObjectSelectionChangedInMixer;
            previousMixingSession_ = mixingSession;
        }
        
        private void OnAudioObjectSelectionChangedInMixer(int? selection, IAudioObject audioObject)
        {
            if (!selection.HasValue) 
                return;
            
            inspectorBehaviour_.UpdateInspector(audioObject, projectPropertySet_, null);
            eventManager_.ClearSelection();
        }
        
        private void OnAudioObjectSelectedInEvent(IAudioObject audioObject, IEvent @event)
        {
            inspectorBehaviour_.UpdateInspector(audioObject, projectPropertySet_, @event);
            var selectedMixingSession = mixingSessionManager_.SelectedMixingSession;
            if (selectedMixingSession != null)
                selectedMixingSession.AudioObjectSelection = null;
        }

        public void Dispose()
        {
            if (previousMixingSession_ != null)
                previousMixingSession_.AudioObjectSelectionChanged -= OnAudioObjectSelectionChangedInMixer;
            
            mixingSessionManager_.MixingSessionSelectionChanged -= OnMixingSelectionChanged;
            eventManager_.AudioObjectSelected -= OnAudioObjectSelectedInEvent;
        }
    }
}
