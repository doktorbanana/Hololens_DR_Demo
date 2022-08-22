using System;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    [CreateAssetMenu(menuName = "SpatialConnect/SpatialConnectHandler")]
    public class SpatialConnectHandler : ScriptableObject
    {
        [Header("Spatial Connect Overlay:")]
        [SerializeField] private GameObject spatialConnectOverlayPrefab = default;
        [SerializeField] private Vector3 overlayOffsetPos = new Vector3(0f, 0.75f, 0f);

        [Header("Selection Sphere / Laser Pointer:")]
        [SerializeField] private GameObject laserPointerPrefab = default;
        [SerializeField] private GameObject selectionSpherePrefab = default;
        [SerializeField] private Vector3 sphereOffsetPos = new Vector3(0f, 0f, 0.05f);

        [Header("Event List Filter Keywords:")]
        [Tooltip("Only show events containing those keywords in event list, toggleable in event list.")]
        [SerializeField] private string keyword1 = "change";
        [SerializeField] private string keyword2 = "up to 6";
        [SerializeField] private string keyword3 = "keywords";
        [SerializeField] private string keyword4 = "in the";
        [SerializeField] private string keyword5 = "SpatialConnect";
        [SerializeField] private string keyword6 = "Handler";
        
        [Header("Exclude Event Spheres:")]
        [Tooltip("Permanently hide VR event spheres containing those keywords. Limited to 20.")]
        [SerializeField] private string[] keywords;

        [Header("Event Ghosting:")]
        [Range(0.1f, 30.0f), SerializeField] private float ghostingDuration = 3f;

        [Header("Event Sphere:")]
        [Range(20, 100), SerializeField] private int fontSize = 55;
        [ColorUsage(false), SerializeField] private Color sphereColor = new Color(0f, 1f, 0f, 0.6f);

        [Header("Controller:")]
        [SerializeField] private DominantHand dominantHand = DominantHand.Right;
        
        [Header("Controller Input Mapping:")]
        [SerializeField] private Button standardInteraction = Button.RightIndexTrigger;
        [SerializeField] private Button standardInteractionCombo = Button.Unassigned;
        [Space, SerializeField] private Button overlayPositionInteraction = Button.RightHandTrigger;
        [SerializeField] private Button overlayPositionInteractionCombo = Button.Unassigned;
        [Space, SerializeField] private Button addNodeInteraction = Button.AButton;
        [SerializeField] private Button addNodeInteractionCombo = Button.Unassigned;
        [Space, SerializeField] private Button removeNodeInteraction = Button.BButton;
        [SerializeField] private Button removeNodeInteractionCombo = Button.Unassigned;
        
        [Header("Shorten Name Labels:")] 
        [Range(4, 20), SerializeField]
        private int maxLastCharacters = 12; 
            
        private OverlayManager overlayManager_;

        private void OnValidate()
        {
            if (keywords.Length > 20) 
                Array.Resize(ref keywords, 20);
        }
        
        public void EnableOverlay(Transform centerEyeAnchorTransform, Transform leftControllerAnchorTransform,
            Transform rightControllerAnchorTransform)
        {
            var inputPreferenceSet = new InputPreferenceSet(dominantHand, leftControllerAnchorTransform,
                rightControllerAnchorTransform, (standardInteraction, standardInteractionCombo),
                (overlayPositionInteraction, overlayPositionInteractionCombo),
                (addNodeInteraction, addNodeInteractionCombo),
                (removeNodeInteraction, removeNodeInteractionCombo));
            
            var vrInteraction = new VrInteraction(centerEyeAnchorTransform, selectionSpherePrefab,
                laserPointerPrefab, sphereOffsetPos, inputPreferenceSet);
            
            var userPreferenceSet =
                new UserPreferenceSet(
                    ghostingDuration, fontSize, overlayOffsetPos,
                    new FilterKeywordSet(new[] {keyword1, keyword2, keyword3, keyword4, keyword5, keyword6}, new Factory()),
                    new FilterKeywordSet(keywords, new Factory()),
                    new StringShortenerToggle(maxLastCharacters), new ToggleValue(),
                    sphereColor);

            overlayManager_ = new OverlayManager(vrInteraction, userPreferenceSet, spatialConnectOverlayPrefab);
        }
        
        public void DisableOverlay()
        {
            overlayManager_.Dispose();
        }

        public void ShowOverlay()
        {
            overlayManager_.Show();
        }

        public void HideOverlay()
        {
            overlayManager_.Hide();
        }
    }
}
