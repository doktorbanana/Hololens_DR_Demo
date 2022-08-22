using UnityEngine;

namespace SpatialConnect.Wwise
{
    public enum DominantHand
    {
        Left,
        Right
    }
    
    public interface IInputPreferenceSet
    {
        Transform ControllerAnchorTransform { get; }
        (Button, Button) StandardInteraction { get; }
        (Button, Button) OverlayPositionInteraction { get; }
        (Button, Button) AddNodeInteraction { get; }
        (Button, Button) RemoveNodeInteraction { get; }
    }
    
    public class InputPreferenceSet : IInputPreferenceSet
    {
        public Transform ControllerAnchorTransform { get; }
        public (Button, Button) StandardInteraction { get; }
        public (Button, Button) OverlayPositionInteraction { get; }
        public (Button, Button) AddNodeInteraction { get; }
        public (Button, Button) RemoveNodeInteraction { get; }

        public InputPreferenceSet(DominantHand hand, Transform leftControllerAnchorTransform,
            Transform rightControllerAnchorTransform, (Button, Button) standardInteraction,
            (Button, Button) overlayPositionInteraction, (Button, Button) addNodeInteraction,
            (Button, Button) removeNodeInteraction)
        {
            ControllerAnchorTransform = hand switch
            {
                DominantHand.Left => leftControllerAnchorTransform,
                DominantHand.Right => rightControllerAnchorTransform,
                _ => rightControllerAnchorTransform
            };

            StandardInteraction = standardInteraction;
            OverlayPositionInteraction = overlayPositionInteraction;
            AddNodeInteraction = addNodeInteraction;
            RemoveNodeInteraction = removeNodeInteraction;
        }
    }
}
