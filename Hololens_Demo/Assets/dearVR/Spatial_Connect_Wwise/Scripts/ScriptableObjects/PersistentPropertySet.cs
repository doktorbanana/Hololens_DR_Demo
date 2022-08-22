using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    [CreateAssetMenu(menuName = "SpatialConnect/PersistentPropertySet")]
    public class PersistentPropertySet : ScriptableObject, IPersistentPropertySet
    {
        private void OnEnable()
        {
            SelectedAudioObjectId = string.Empty;
            SelectedMixingSessionId = string.Empty;
            ShortenNameLabel = false;
        }

        public string SelectedAudioObjectId { get; set; }
        public string SelectedMixingSessionId { get; set; }
        public bool ShortenNameLabel { get; set; }
    }
}
