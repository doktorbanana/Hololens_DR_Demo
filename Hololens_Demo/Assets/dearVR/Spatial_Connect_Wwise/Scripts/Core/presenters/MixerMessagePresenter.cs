using System;
using SpatialConnect.Wwise.Core.Wwu;

namespace SpatialConnect.Wwise.Core
{
    public class MixerMessagePresenter : IDisposable
    {
        private readonly ITextBehaviour mixerMessageTextBehaviour_;
        private readonly IMixingSessionManager mixingSessionManager_;

        private const string FILE_ERROR_MESSAGE = "Error while reading \"Default Work Unit.wwu\"";
        private const string PARSE_ERROR_MESSAGE = "Error while parsing \"Default Work Unit.wwu\"";
        private const string NO_AUDIO_OBJECTS_MESSAGE = "No Audio Objects found in the selected Mixing Session";
        private const string NO_MIXING_SESSION_ERROR_MESSAGE = "No Mixing Session found in \"Default Work Unit.wwu\"";
        
        public MixerMessagePresenter(ITextBehaviour mixerMessageTextBehaviour, IMixingSessionManager mixingSessionManager)
        {
            mixerMessageTextBehaviour_ = mixerMessageTextBehaviour;
            mixingSessionManager_ = mixingSessionManager;

            mixingSessionManager_.WwuFileLoaded += OnWwuFileLoaded;
            mixingSessionManager_.MixingSessionSelectionChanged += OnMixingSessionSelectionChanged;
        }

        private void OnWwuFileLoaded(WwuFileLoadState fileLoadState)
        {
            switch (fileLoadState)
            {
                case WwuFileLoadState.FileLoadError:
                    mixerMessageTextBehaviour_.Text = FILE_ERROR_MESSAGE;
                    break;
                case WwuFileLoadState.XmlParseError:
                    mixerMessageTextBehaviour_.Text = PARSE_ERROR_MESSAGE;
                    break;
                case WwuFileLoadState.NoMixingSessionError:
                    mixerMessageTextBehaviour_.Text = NO_MIXING_SESSION_ERROR_MESSAGE;
                    break;
            }
        }
        
        private void OnMixingSessionSelectionChanged(int index, IMixingSession mixingSession)
        {
            var audioObjects = mixingSession.AudioObjects;

            mixerMessageTextBehaviour_.Text = audioObjects.Length == 0 ? NO_AUDIO_OBJECTS_MESSAGE : "";
        }
        
        public void Dispose()
        {
            mixingSessionManager_.WwuFileLoaded -= OnWwuFileLoaded;
            mixingSessionManager_.MixingSessionSelectionChanged -= OnMixingSessionSelectionChanged;
        }
    }
}
