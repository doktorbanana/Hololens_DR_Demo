using System;
using System.Linq;

namespace SpatialConnect.Wwise.Core
{
    public class Event : IEvent
    {
        private int[] audioObjectSelection_;
        private readonly EventPropertySet eventPropertySet_;
        private readonly Lazy<IAudioObjectPropertySet> treeRoot_;
        private Position sourcePosition_;
        private const int MAX_CHILDREN = 25;
        
        public bool Selected { get; private set; }
        public bool Ghosting { get; private set; }
        public string Id => eventPropertySet_.id;
        public string Name => eventPropertySet_.name;
        public uint PlayingId => eventPropertySet_.playingId;
        public int GameObjectId => eventPropertySet_.gameObjectId;

        public Position SourcePosition
        {
            get => sourcePosition_;
            set
            {
                sourcePosition_ = value;
                SourceMoved?.Invoke(sourcePosition_);
            }
        }

        public string GameObjectName => eventPropertySet_.gameObjectName;
        public IAudioObjectPropertySet TreeRoot => treeRoot_.Value;

        public int[] AudioObjectSelection
        {
            set
            {
                audioObjectSelection_ = value;
                IAudioObjectPropertySet pointer = null;

                if (audioObjectSelection_.Length != 0)
                    pointer = audioObjectSelection_.Aggregate(treeRoot_.Value, 
                        (current, index) => current.Children[index]);

                AudioObjectSelectionChanged?.Invoke(pointer, audioObjectSelection_);
            }
        }
        
        public Event(EventPropertySet eventPropertySet, IFactory factory)
        {
            eventPropertySet_ = eventPropertySet;
            treeRoot_ = new Lazy<IAudioObjectPropertySet>(
                () => factory.CreateAudioObjectPropertySetTreeRoot(factory.CreateEventChildSet(Id, MAX_CHILDREN).Children));
        }

        public void Select()
        {
            Selected = true;
            EventSelectionStateChanged?.Invoke(true, this);
            AudioObjectSelection = new [] {0};
        }

        public void Deselect()
        {
            Selected = false;
            EventSelectionStateChanged?.Invoke(false, this);
        }

        public void SetToGhostState()
        {
            Ghosting = true;
            EventEnded?.Invoke();
        }
        
        public void Dispose()
        {
            GhostingEnded?.Invoke(this);
        }

        public event Action EventEnded;
        public event Action<IEvent> GhostingEnded;
        public event Action<bool, IEvent> EventSelectionStateChanged;
        public event Action<IAudioObjectPropertySet, int[]> AudioObjectSelectionChanged;
        public event Action<Position> SourceMoved;
    }
}
