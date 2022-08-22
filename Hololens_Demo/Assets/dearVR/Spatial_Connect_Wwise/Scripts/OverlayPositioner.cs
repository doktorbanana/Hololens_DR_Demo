using System;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public interface IOverlayPositioner : IShowable, IDisposable
    {
        IToggleable LockToggle { get; }
        void FaceUser();
        void Place();
    }

    public class OverlayPositioner : IOverlayPositioner
    {
        private readonly Transform selectionSphereTransform_;
        private readonly Transform centerEyeAnchorTransform_;
        private readonly Transform overlayTransform_;
        private readonly Transform overlayAnchorTransform_;
        private readonly Vector3 hidingPosition_ = new Vector3(0, 10000f, 0f);

        private Vector3 directionOverlayToHmd_;
        private Vector3 directionSphereToOverlay_;
        private Vector3 angleSphereToOverlay_;
        private Vector3 lastOverlayPos_;

        private bool placementButtonDown_;
        private bool overlayVisible_;

        private readonly IInputBehaviour inputBehaviour_;
        
        public IToggleable LockToggle { get; }

        public OverlayPositioner(IVrInteraction vrInteraction, Transform overlayTransform, Transform overlayAnchorTransform,
            IFactory factory = null)
        {
            factory ??= new Factory();

            selectionSphereTransform_ = vrInteraction.SelectionSphereTransform;
            centerEyeAnchorTransform_ = vrInteraction.CenterEyeAnchorTransform;
            inputBehaviour_ = vrInteraction.InputBehaviour;
            overlayTransform_ = overlayTransform;
            overlayAnchorTransform_ = overlayAnchorTransform;

            inputBehaviour_.OverlayPositionInteractionStateChanged += OnOverlayPositionInteractionStateChanged;

            LockToggle = factory.CreateToggleValue();
        }

        public void FaceUser()
        {
            overlayAnchorTransform_.position = centerEyeAnchorTransform_.position;
            overlayTransform_.eulerAngles = new Vector3(0f, centerEyeAnchorTransform_.rotation.eulerAngles.y, 0f);
        }

        public void Place()
        {
            if (!overlayVisible_ || LockToggle.State)
                return;
            
            var newPos = overlayAnchorTransform_.position - directionOverlayToHmd_;
            var overlayRotation = overlayTransform_.rotation.eulerAngles;
            
            if (placementButtonDown_)
            {
                newPos.y = selectionSphereTransform_.position.y - directionSphereToOverlay_.y;
                overlayRotation.y = selectionSphereTransform_.rotation.eulerAngles.y - angleSphereToOverlay_.y;
            }

            overlayTransform_.SetPositionAndRotation(newPos, Quaternion.Euler(overlayRotation));
        }

        public void Show()
        {
            overlayVisible_ = true;
        }

        public void Hide()
        {
            if (LockToggle.State)
                LockToggle.SwitchState();

            overlayVisible_ = false;
            overlayTransform_.position = hidingPosition_;
        }

        private void OnOverlayPositionInteractionStateChanged(bool state)
        {
            if (state) 
                overlayAnchorTransform_.position = centerEyeAnchorTransform_.position;

            placementButtonDown_ = state;
            directionSphereToOverlay_ = selectionSphereTransform_.position - overlayTransform_.position;
            angleSphereToOverlay_ = selectionSphereTransform_.rotation.eulerAngles - overlayTransform_.rotation.eulerAngles;

            if (placementButtonDown_)
            {
                RecenterOverlay();
                lastOverlayPos_ = overlayTransform_.position;
            }
            else
            {
                var delta = overlayTransform_.position.y - lastOverlayPos_.y;
                directionOverlayToHmd_.y -= delta;
            }

            void RecenterOverlay()
            {
                directionOverlayToHmd_.x = overlayAnchorTransform_.position.x - centerEyeAnchorTransform_.position.x;
                directionOverlayToHmd_.z = overlayAnchorTransform_.position.z - centerEyeAnchorTransform_.position.z;
            }
        }

        public void Dispose()
        {
            inputBehaviour_.OverlayPositionInteractionStateChanged -= OnOverlayPositionInteractionStateChanged;
        }
    }
}
