using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public interface IUserPreferenceSet
    {
        float GhostingDuration { get; }
        int EventSphereFontSize { get; }
        Vector3 OverlayOffsetPosition { get; }
        IFilterKeywordSet ToggleableFilterKeywordSet { get; }
        IFilterKeywordSet PermanentFilterKeywordSet { get; }
        IStringShortenerToggle StringShortenerToggle { get; }
        IToggleable FilterEventSpheresToggleable { get; }
        Color SphereColor { get; }
    }
    
    public class UserPreferenceSet : IUserPreferenceSet
    {
        public float GhostingDuration { get; }
        public int EventSphereFontSize { get; }
        public Vector3 OverlayOffsetPosition { get; }
        public IFilterKeywordSet ToggleableFilterKeywordSet { get; }
        public IFilterKeywordSet PermanentFilterKeywordSet { get; }
        public IStringShortenerToggle StringShortenerToggle { get; }
        public IToggleable FilterEventSpheresToggleable { get; }
        public Color SphereColor { get; }

        public UserPreferenceSet(float ghostingDuration, int eventSphereFontSize, 
            Vector3 overlayOffsetPosition, IFilterKeywordSet toggleableFilterKeywordSet, IFilterKeywordSet permanentFilterKeywordSet,
            IStringShortenerToggle stringShortenerToggle, IToggleable filterEventSpheresToggleable, Color sphereColor)
        {
            GhostingDuration = ghostingDuration;
            EventSphereFontSize = eventSphereFontSize;
            OverlayOffsetPosition = overlayOffsetPosition;
            ToggleableFilterKeywordSet = toggleableFilterKeywordSet;
            PermanentFilterKeywordSet = permanentFilterKeywordSet;
            StringShortenerToggle = stringShortenerToggle;
            FilterEventSpheresToggleable = filterEventSpheresToggleable;
            SphereColor = sphereColor;
        }
    }
}
