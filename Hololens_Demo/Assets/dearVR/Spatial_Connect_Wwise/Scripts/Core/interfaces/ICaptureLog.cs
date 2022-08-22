using System;

namespace SpatialConnect.Wwise.Core
{
    public readonly struct EventPropertySet
    {
        public readonly string id;
        public readonly string name;
        public readonly int gameObjectId;
        public readonly string gameObjectName;
        public readonly uint playingId;

        public EventPropertySet(string id, string name, int gameObjectId, string gameObjectName, uint playingId)
        {
            this.id = id;
            this.name = name;
            this.gameObjectId = gameObjectId;
            this.gameObjectName = gameObjectName;
            this.playingId = playingId;
        }
    }

    public interface ICaptureLog : ISubscriber
    {
        event Action<EventPropertySet> EventTriggered;
        event Action<uint> EventFinished;
    }
}
