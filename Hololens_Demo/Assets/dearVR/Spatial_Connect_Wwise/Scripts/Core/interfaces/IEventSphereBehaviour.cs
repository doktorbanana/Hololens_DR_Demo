using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IEventSphereBehaviour : IEventRepresentation
    {
        new uint PlayingId { get; }
        
        bool Pointed { get; }
        
        Position Position { set; }
        
        int FontSize { set; }
        
        event Action<object, float> Shot;

        event Action<object> Selected;

        new void Deactivate();
    }
}
