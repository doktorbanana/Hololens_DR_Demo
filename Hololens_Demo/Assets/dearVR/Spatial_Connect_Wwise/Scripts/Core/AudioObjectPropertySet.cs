using System.Linq;

namespace SpatialConnect.Wwise.Core
{
    public class AudioObjectPropertySet : IAudioObjectPropertySet
    {
        public string Id { get; }
        public string Name { get; }
        public IAudioObjectPropertySet[] Children { get; }

        public int ChildCount
        {
            get
            {
                var childCount = Children.Sum(child => child.ChildCount);
                return Children.Length + childCount;
            }
        }

        public AudioObjectPropertySet(string id, string name, int maxNumDescendants, IFactory factory)
        {
            Id = id;
            Name = name;
            Children = factory.CreateAudioObjectChildSet(Id, maxNumDescendants).Children;
        }

        public AudioObjectPropertySet(IAudioObjectPropertySet[] children)
        {
            Children = children;
        }
    }
}
