using System;
using System.Linq;
using OVRSimpleJSON;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class AudioObjectChildSet : IChildSet
    {
        private readonly IFactory factory_;
        private readonly string audioObjectId_;
        private readonly Func<string, IAudioObjectPropertySet[]> postProcess_;

        public IAudioObjectPropertySet[] Children
            => factory_.CreateChildSet(audioObjectId_, new[] { "id", "name", "type" }, postProcess_).Children;

        public AudioObjectChildSet(string audioObjectId, int maxNumDescendants, IFactory factory = null)
        {
            factory_ = factory ?? new Factory();
            audioObjectId_ = audioObjectId;
            postProcess_ = result =>
            {
                var childCount = 0;
                var node = JSON.Parse(result)["return"];
                return node.Linq
                    .Where(item => item.Value["type"] != "AudioFileSource" && childCount < maxNumDescendants)
                    .Select(item =>
                    {
                        var audioObjectPropertySet = factory_.CreateAudioObjectPropertySet(item.Value["id"],
                            item.Value["name"], maxNumDescendants - ++childCount);
                        childCount += audioObjectPropertySet.ChildCount;

                        return audioObjectPropertySet;
                    })
                    .ToArray();
            };
        }
    }
}
