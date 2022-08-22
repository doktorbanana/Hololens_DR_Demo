using System;

namespace SpatialConnect.Wwise.Core
{
    public class SourcePositionPresenter : IDisposable
    {
        private readonly IEvent event_;
        private readonly ISourcePosition sourcePosition_;
        
        public SourcePositionPresenter(ISourcePosition sourcePosition, IEvent @event)
        {
            event_ = @event;
            sourcePosition_ = sourcePosition;

            sourcePosition_.PositionUpdated += OnPositionUpdated;
        }

        private void OnPositionUpdated(Position position)
        {
            event_.SourcePosition = position;
        }
        
        public void Dispose()
        {
            sourcePosition_.PositionUpdated -= OnPositionUpdated;
        }
    }
}
