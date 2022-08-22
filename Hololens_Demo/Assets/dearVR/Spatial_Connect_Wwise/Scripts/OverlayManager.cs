using System;
using SpatialConnect.Wwise.Core;
using UnityEngine;

namespace SpatialConnect.Wwise
{
    public class OverlayManager : IDisposable, IShowable
    {
        private readonly Transform overlayAnchorTransform_;
        private readonly GameObject overlay_;
        private readonly IShowable overlayVisibility_;
        private readonly IVrInteraction vrInteraction_;

        public OverlayManager(IVrInteraction vrInteraction, IUserPreferenceSet userPreferenceSet, GameObject spatialConnectOverlayPrefab)
        {
            vrInteraction_ = vrInteraction;
            
            overlayAnchorTransform_ = new GameObject("SpatialConnectAnchor").transform;
            var trackingSpaceTransform = vrInteraction.CenterEyeAnchorTransform.parent;
            overlayAnchorTransform_.SetParent(trackingSpaceTransform);
            
            overlay_ = UnityEngine.Object.Instantiate(spatialConnectOverlayPrefab);
            overlay_.name = "SpatialConnectWwise";
            
            IOverlayPositioner overlayPositioner = new OverlayPositioner(vrInteraction, overlay_.transform, overlayAnchorTransform_); 
            overlayPositioner.FaceUser();
            
            overlay_.GetComponent<OverlayPositionBehaviour>().Init(overlayPositioner);
            
            var spatialConnectBehaviour = overlay_.GetComponent<SpatialConnectBehaviour>();
            spatialConnectBehaviour.Init(vrInteraction, userPreferenceSet);
            
            overlayVisibility_ = new OverlayVisibility(new IShowable[]
                {spatialConnectBehaviour, vrInteraction, overlayPositioner});
            Hide();
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(overlay_);
            UnityEngine.Object.Destroy(overlayAnchorTransform_.gameObject);
            vrInteraction_.Dispose();
        }

        public void Show()
        {
            overlayVisibility_.Show();
        }

        public void Hide()
        {
            overlayVisibility_.Hide();
        }
    }
}
