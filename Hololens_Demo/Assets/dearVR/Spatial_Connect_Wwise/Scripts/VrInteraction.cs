using System;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public interface IVrInteraction : IShowable, IDisposable
    {
        IInputBehaviour InputBehaviour { get; }
        ILaserPointerBehaviour LaserPointerBehaviour { get; }
        Transform ControllerAnchorTransform { get; }
        Transform CenterEyeAnchorTransform { get; }
        Transform SelectionSphereTransform { get; }
    }
    
    public class VrInteraction : IVrInteraction
    {
        private readonly GameObject selectionSphere_;
        private readonly GameObject laserPointer_;

        public IInputBehaviour InputBehaviour => selectionSphere_.GetComponent<InputBehaviour>();
        public ILaserPointerBehaviour LaserPointerBehaviour => laserPointer_.GetComponent<LaserPointerBehaviour>();
        public Transform CenterEyeAnchorTransform { get; }
        public Transform ControllerAnchorTransform { get; }
        public Transform SelectionSphereTransform => selectionSphere_.transform;

        public VrInteraction(Transform centerEyeAnchorTransform, GameObject selectionSpherePrefab, 
            GameObject laserPointerPrefab, Vector3 sphereOffsetPos, IInputPreferenceSet inputPreferenceSet)
        {
            CenterEyeAnchorTransform = centerEyeAnchorTransform;
            ControllerAnchorTransform = inputPreferenceSet.ControllerAnchorTransform;

            selectionSphere_ = UnityEngine.Object.Instantiate(selectionSpherePrefab, ControllerAnchorTransform, false);
            selectionSphere_.name = "SelectionSphere";
            selectionSphere_.transform.localPosition = sphereOffsetPos;
            selectionSphere_.GetComponent<InputBehaviour>().Init(inputPreferenceSet);
            
            laserPointer_ = UnityEngine.Object.Instantiate(laserPointerPrefab, ControllerAnchorTransform, false);
            laserPointer_.name = "LaserPointer";
        }

        public void Show()
        {
            selectionSphere_.SetActive(true);
            laserPointer_.SetActive(true);
        }

        public void Hide()
        {
            selectionSphere_.SetActive(false);
            laserPointer_.SetActive(false);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(laserPointer_);
            UnityEngine.Object.Destroy(selectionSphere_);
        }
    }
}
