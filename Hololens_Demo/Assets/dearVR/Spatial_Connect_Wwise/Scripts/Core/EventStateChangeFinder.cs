using System.Collections.Generic;
using System.Linq;

namespace SpatialConnect.Wwise.Core
{
    public interface IEventStateChangeFinder
    {
        public class Result
        {
            public readonly IEnumerable<IEvent> NewEvents;
            public readonly IEnumerable<IEventRepresentation> DeadRepresentations;

            public Result(IEnumerable<IEvent> newEvents, IEnumerable<IEventRepresentation> deadRepresentationses)
            {
                NewEvents = newEvents;
                DeadRepresentations = deadRepresentationses;
            }
        }

        Result Find(IEnumerable<IEventRepresentation> representations, IEnumerable<IEvent> events);
    }
    
    public class EventStateChangeFinder : IEventStateChangeFinder
    {
        public IEventStateChangeFinder.Result Find(IEnumerable<IEventRepresentation> representations, IEnumerable<IEvent> events)
        {
            var playingIdsInScene = representations.Select(representation => representation.PlayingId).ToList();
            var activePlayingIds = events.Select(@event => @event.PlayingId).ToList();

            var newEvents = events.Where(@event => !playingIdsInScene.Contains(@event.PlayingId));
            var deadRepresentations = representations.Where(representation => !activePlayingIds.Contains(representation.PlayingId));

            return new IEventStateChangeFinder.Result(newEvents, deadRepresentations);
        }
    }
}
