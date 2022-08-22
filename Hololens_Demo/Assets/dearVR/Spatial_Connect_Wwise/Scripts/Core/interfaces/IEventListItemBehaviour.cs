using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IEventListItemBehaviour : IEventRepresentation, IHighlightable
    {
        string EventName { set; }
        string GameObjectName { set; }
        event Action Selected;
    }
}
