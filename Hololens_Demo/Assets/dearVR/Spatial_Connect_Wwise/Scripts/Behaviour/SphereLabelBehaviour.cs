using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class SphereLabelBehaviour : MonoBehaviour, ITextBehaviour, IOutlinable
    {
        [SerializeField] private GameObject selectionOutline = default;
        [SerializeField] private Canvas contentCanvas = default;
        [SerializeField] private Text nameText = default;

        private Transform centerEyeAnchorTransform_;
        private Transform sphereTransform_;
        private readonly IAutoDisposer sphereLabelPresenter_ = new AutoDisposer();

        public bool OutlineActive
        {
            set => selectionOutline.SetActive(value);
        }

        public int FontSize
        {
            set => nameText.fontSize = value;
        }

        public string Text
        {
            set => nameText.text = value;
        }
        
        public void Init(IEvent @event, IStringShortenerToggle stringShortenerToggle, Transform centerEyeAnchorTransform, Transform sphereTransform, IFactory factory = null)
        {
            factory ??= new Factory();
            sphereLabelPresenter_.Update(factory.CreateSphereLabelPresenter(this, stringShortenerToggle, @event));
            centerEyeAnchorTransform_ = centerEyeAnchorTransform;
            sphereTransform_ = sphereTransform;
            contentCanvas.enabled = false;
        }

        private void OnEnable()
        {
            StartCoroutine(EnableLabel());
            
            IEnumerator EnableLabel()
            {
                yield return 0;
                contentCanvas.enabled = true;
            }
        }

        private void Update()
        {
            var (labelPosition, labelScale) =LabelPositioner.Place(centerEyeAnchorTransform_.position, sphereTransform_.position);
            transform.position = labelPosition;
            transform.localScale = labelScale;
            transform.eulerAngles = new Vector3(0f, centerEyeAnchorTransform_.rotation.eulerAngles.y, 0f);
        }

        private void OnDestroy()
        {
            sphereLabelPresenter_.Dispose();
        }
    }
}
