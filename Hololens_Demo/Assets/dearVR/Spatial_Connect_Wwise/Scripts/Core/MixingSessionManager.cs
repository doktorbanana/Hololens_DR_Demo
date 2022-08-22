using System;
using SpatialConnect.Wwise.Core.Wwu;

namespace SpatialConnect.Wwise.Core
{
    public interface IMixingSessionManager : IDisposable
    {
        IMixingSession SelectedMixingSession { get; }
        MixingSessionPropertySet[] MixingSessionPropertySets { get; }

        int MixingSessionSelection { set; }

        void Load(string path);

        event Action<WwuFileLoadState> WwuFileLoaded;
        event Action<int, IMixingSession> MixingSessionSelectionChanged;
    }

    public class MixingSessionManager : IMixingSessionManager
    {
        private readonly IFactory factory_;
        private readonly IMessageBroadcaster messageBroadcaster_;
        private int mixingSessionSelection_;
        public IMixingSession SelectedMixingSession { get; private set; }
        public MixingSessionPropertySet[] MixingSessionPropertySets { get; private set; }

        public int MixingSessionSelection
        {
            set
            {
                mixingSessionSelection_ = value;
                SelectedMixingSession?.Dispose();
                SelectedMixingSession = factory_.CreateMixingSession(
                    MixingSessionPropertySets[mixingSessionSelection_], messageBroadcaster_);
                MixingSessionSelectionChanged?.Invoke(mixingSessionSelection_, SelectedMixingSession);
                factory_.CreateSoloMuteReset();
            }
        }

        public MixingSessionManager(IMessageBroadcaster messageBroadcaster, IFactory factory)
        {
            factory_ = factory;
            messageBroadcaster_ = messageBroadcaster;
        }

        public void Load(string path)
        {
            var result = factory_.CreateParser(path, factory_.CreateLoader()).Parse();
            MixingSessionPropertySets = result.MixingSessionPropertySet;
            WwuFileLoaded?.Invoke(result.WwuFileLoadState);
        }

        public void Dispose()
        {
            SelectedMixingSession?.Dispose();
        }

        public event Action<WwuFileLoadState> WwuFileLoaded;
        public event Action<int, IMixingSession> MixingSessionSelectionChanged;
    }
}
