using System;

namespace SpatialConnect.Wwise.Core
{
    public class MixingSessionSelectionRestorer : IRestorer
    {
        private readonly IMixingSessionManager mixingSessionManager_;
        private readonly IPersistentPropertySet persistentPropertySet_;
        
        public MixingSessionSelectionRestorer(IMixingSessionManager mixingSessionManager,
            IPersistentPropertySet persistentPropertySet)
        {
            mixingSessionManager_ = mixingSessionManager;
            persistentPropertySet_ = persistentPropertySet;
        }

        public void Store()
        {
            if (mixingSessionManager_.MixingSessionPropertySets == null) 
                return;
            
            var selectedMixingSession = mixingSessionManager_.SelectedMixingSession;
            persistentPropertySet_.SelectedMixingSessionId = selectedMixingSession.Id;

            var selectedAudioObject = selectedMixingSession.SelectedAudioObject;
            if (selectedMixingSession.AudioObjects.Length == 0 || selectedAudioObject == null)
            {
                persistentPropertySet_.SelectedAudioObjectId = null;
                return;
            }
            persistentPropertySet_.SelectedAudioObjectId = selectedAudioObject.Id;
        }

        public void Restore()
        {
            if (mixingSessionManager_.MixingSessionPropertySets == null) 
                return;
            
            RestorePreviouslySelectedMixingSession();
            RestorePreviouslySelectedAudioObject();
            
            void RestorePreviouslySelectedMixingSession()
            {
                var previouslySelectedMixingSessionId = persistentPropertySet_.SelectedMixingSessionId;
                if (string.IsNullOrEmpty(previouslySelectedMixingSessionId))
                {
                    mixingSessionManager_.MixingSessionSelection = 0;
                    return;
                }
                
                var index = Array.FindIndex(mixingSessionManager_.MixingSessionPropertySets,
                    mixingSessionPropertySet => mixingSessionPropertySet.Id == previouslySelectedMixingSessionId);
                mixingSessionManager_.MixingSessionSelection = index < 0 ? 0 : index;
            }

            void RestorePreviouslySelectedAudioObject()
            {
                if(mixingSessionManager_.SelectedMixingSession.AudioObjects.Length == 0)
                    return;
                
                var previouslySelectedAudioObjectId = persistentPropertySet_.SelectedAudioObjectId;
                if (string.IsNullOrEmpty(previouslySelectedAudioObjectId))
                    mixingSessionManager_.SelectedMixingSession.AudioObjectSelection = 0;
                else
                {
                    var mixingSession = mixingSessionManager_.SelectedMixingSession;
                    var audioObjects = mixingSession.AudioObjects;
                    var index = Array.FindIndex(audioObjects, audioObject => audioObject.Id == previouslySelectedAudioObjectId);
                    mixingSession.AudioObjectSelection = index < 0 ? 0 : index;
                }
            }
        }
    }
}
