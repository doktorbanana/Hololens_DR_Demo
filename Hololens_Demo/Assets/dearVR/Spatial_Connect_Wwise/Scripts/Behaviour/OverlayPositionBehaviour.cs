using UnityEngine;

namespace SpatialConnect.Wwise
{
    public class OverlayPositionBehaviour : MonoBehaviour
    {
        [SerializeField] private ToggleBehaviour lockToggleBehavior;

        private IOverlayPositioner overlayPositioner_;

        public void Init(IOverlayPositioner overlayPositioner)
        {
            overlayPositioner_ = overlayPositioner;
            lockToggleBehavior.Init(overlayPositioner.LockToggle);
        }

        private void Update()
        {
            overlayPositioner_.Place();
        }

        private void OnDestroy()
        {
            overlayPositioner_.Dispose();
        }
    }
}
