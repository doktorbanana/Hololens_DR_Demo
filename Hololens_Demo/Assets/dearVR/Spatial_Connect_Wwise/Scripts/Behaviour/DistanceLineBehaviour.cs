using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    [RequireComponent(typeof(RectTransform))]
    public class DistanceLineBehaviour : MonoBehaviour, IDistanceLineBehaviour
    {
        [SerializeField] private Text text = default;
        [SerializeField] private GameObject line = default;
        [SerializeField] private RectTransform lineRectTransform = default;

        private Transform centerEyeAnchorTransform_;
        private GameObject sourceGameObject_;
        private readonly IAutoDisposer distanceLinePresenter_ = new AutoDisposer();

        public string Label
        {
            set => text.text = value;
        }
        
        public void Init(IStringShortener stringShortener, IDistanceLinePropertySet distanceLinePropertySet, IPositioningPropertySet positioningPropertySet,  IFactory factory = null)
        {
            factory ??= new Factory();
            centerEyeAnchorTransform_ = distanceLinePropertySet.CenterEyeAnchorTransform;
            
            if (distanceLinePropertySet.Event != null)
                sourceGameObject_ = (GameObject)EditorUtility.InstanceIDToObject(distanceLinePropertySet.Event.GameObjectId);
            distanceLinePresenter_.Update(factory.CreateDistanceLinePresenter(this, stringShortener,
                positioningPropertySet, distanceLinePropertySet.Event));
        }
        
        private void Update()
        {
            if (!sourceGameObject_)
            {
                Disable();
                return;
            }

            var distance = Vector3.Distance(centerEyeAnchorTransform_.position, sourceGameObject_.transform.position);
            DistanceChanged?.Invoke(distance);
        }

        public void Move(float position)
        {
            lineRectTransform.anchoredPosition3D = new Vector3(position, 0f, -1f);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
        
        public void Show()
        {
            line.SetActive(true);
        }
        
        public void Hide()
        {
            line.SetActive(false);
        }

        private void OnDestroy()
        {
            distanceLinePresenter_.Dispose();
        }
        
        public event Action<float> DistanceChanged;
    }
}
