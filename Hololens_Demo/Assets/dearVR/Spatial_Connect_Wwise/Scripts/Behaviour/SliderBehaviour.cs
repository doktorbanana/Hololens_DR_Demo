using System;
using UnityEngine;
using UnityEngine.UI;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Slider))]
    [RequireComponent(typeof(BoxCollider))]
    public class SliderBehaviour : MonoBehaviour, ISliderBehaviour
    {
        [SerializeField] private GameObject notInteractablePanel = default;
        [SerializeField] private uint updateRatePerSec = 30;

        private BoxCollider boxCollider_;
        private Slider slider_;
        private Rect rect_;

        private ISliderInputTranslator sliderInputTranslator_;
        private IExecutionLimiter executionLimiter_;

        private readonly IAutoDisposer sliderPresenter_ = new AutoDisposer();
        private readonly IFactory factory_ = new Factory();

        private bool dragging_;

        private void OnValidate()
        {
            updateRatePerSec = updateRatePerSec == 0 ? 1 : updateRatePerSec;
        }

        public float Value
        {
            set => slider_.value = value;
        }

        public float MaxValue
        {
            set => slider_.maxValue = value;
        }

        public void Init(IDisposable sliderPresenter = null)
        {
            sliderPresenter_.Update(sliderPresenter);
            sliderInputTranslator_ = new SliderInputTranslator(slider_, rect_);
        }
        
        private void Awake()
        {
            slider_= GetComponent<Slider>();
            boxCollider_ = GetComponent<BoxCollider>();
            rect_ = GetComponent<RectTransform>().rect;
            executionLimiter_ = factory_.CreateExecutionLimiter(updateRatePerSec);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;
            other.gameObject.GetComponent<InputBehaviour>().StandardInteractionStateChanged += OnStandardInteractionStateChanged;
        }

        private void OnStandardInteractionStateChanged(bool pressed)
        {
            dragging_ = pressed;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!dragging_)
                return;

            if (other.gameObject.name != "SelectionSphere")
                return;

            executionLimiter_.TryExecute(() =>
            {
                var value = sliderInputTranslator_.Translate(transform, other.transform);
                SliderValueChanged?.Invoke(value);
            });
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name != "SelectionSphere")
                return;

            dragging_ = false;
            other.gameObject.GetComponent<InputBehaviour>().StandardInteractionStateChanged -= OnStandardInteractionStateChanged;
        }
        
        public void EnableSlider()
        {
            boxCollider_.enabled = true;
            notInteractablePanel.SetActive(false);
        }
        
        public void DisableSlider()
        {
            boxCollider_.enabled = false;
            notInteractablePanel.SetActive(true);
        }

        public void HideSlider()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            sliderPresenter_.Dispose();
        }

        public event Action<float> SliderValueChanged;
    }
}
