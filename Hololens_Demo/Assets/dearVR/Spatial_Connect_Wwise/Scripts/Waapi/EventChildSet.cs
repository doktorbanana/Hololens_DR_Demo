using System;
using System.Linq;
using OVRSimpleJSON;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class EventChildSet : IChildSet
    {
        private readonly IFactory factory_;
        private readonly string eventId_;
        private readonly Func<string, IAudioObjectPropertySet[]> postProcess_;

        public IAudioObjectPropertySet[] Children
            => factory_.CreateChildSet(eventId_, new[] { "id", "@ActionType", "@Target" }, postProcess_).Children;

        public EventChildSet(string eventId, int maxNumDescendants, IFactory factory = null)
        {
            factory_ = factory ?? new Factory();
            eventId_ = eventId;
            postProcess_ = result =>
            {
                var childCount = 0;
                var node = JSON.Parse(result)["return"];
                return node.Linq
                    .Where(action => action.Value["@ActionType"] == 1 && childCount < maxNumDescendants)
                    .Select(action =>
                    {
                        var target = action.Value["@Target"];
                        var audioObjectPropertySet = factory_.CreateAudioObjectPropertySet(target["id"],
                            target["name"], maxNumDescendants - ++childCount);
                        childCount += audioObjectPropertySet.ChildCount;

                        return audioObjectPropertySet;
                    })
                    .ToArray();
            };
        }
    }
}
