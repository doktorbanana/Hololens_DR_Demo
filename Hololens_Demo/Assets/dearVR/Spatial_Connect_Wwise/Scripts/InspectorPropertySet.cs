using SpatialConnect.Wwise.Core;
using UnityEngine;

namespace SpatialConnect.Wwise
{
    public interface IDistanceLinePropertySet
    {
        IEvent Event { get;  }
        Transform CenterEyeAnchorTransform { get; }
    }
    
    public interface IInspectorPropertySet : IDistanceLinePropertySet
    {
        IProjectPropertySet ProjectPropertySet { get; }
    }
    
    public class InspectorPropertySet : IInspectorPropertySet
    {
        public IProjectPropertySet ProjectPropertySet { get; }
        public IEvent Event { get;  }
        public Transform CenterEyeAnchorTransform { get; }
        
        public InspectorPropertySet(IProjectPropertySet projectPropertySet, IEvent @event,
            Transform centerEyeAnchorTransform)
        {
            ProjectPropertySet = projectPropertySet;
            Event = @event;
            CenterEyeAnchorTransform = centerEyeAnchorTransform;
        }
    } 
}
