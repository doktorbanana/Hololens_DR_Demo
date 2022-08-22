using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IEvent : IDisposable
    {
        string Id { get; }

        string Name { get; }
        
        uint PlayingId { get; }

        int GameObjectId { get; }
        
        Position SourcePosition { get; set; }

        string GameObjectName { get; }
        
        bool Selected { get; }

        bool Ghosting { get; }

        int[] AudioObjectSelection { set; }
        
        IAudioObjectPropertySet TreeRoot { get; }
        
        void Select();

        void Deselect();
        
        void SetToGhostState();

        event Action EventEnded;
        event Action<IEvent> GhostingEnded;
        event Action<bool, IEvent> EventSelectionStateChanged;
        event Action<IAudioObjectPropertySet, int[]> AudioObjectSelectionChanged;
        event Action<Position> SourceMoved;
    }
}
