using System;

namespace SpatialConnect.Wwise.Core
{
    public interface ISourcePosition
    {
        bool IsSourceValid { get; }
        
        uint PlayingId { get; }
        
        void Update();

        event Action<Position> PositionUpdated;
    }
}
