using System;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class MixerBehaviour : MonoBehaviour, IMixerBehaviour
    {
        [SerializeField] private MixingSessionHandlerBehaviour mixingSessionHandlerBehaviour = default;
        [SerializeField] private GridDropdownBehaviour selectionDropdownBehaviour = default;
        [SerializeField] private TextBehaviour errorTextBehaviour = default;

        private const uint MAX_NUMBER_OF_INSTANCES = 20;
        private const float CHANNEL_STRIP_WIDTH = 160;
        private const float MARGIN = 40f;

        private IDisposable mixerPresenter_;
        private IDisposable dropDownPresenter_;
        
        public void Init(IMixingSessionManager mixingSessionManager, IStringShortenerToggle stringShortenerToggle, IFactory factory = null)
        {
            factory ??= new Factory();
            
            selectionDropdownBehaviour.Init(MAX_NUMBER_OF_INSTANCES);
            dropDownPresenter_ = factory.CreateMixingSessionDropDownPresenter(selectionDropdownBehaviour, mixingSessionManager);
            
            errorTextBehaviour.Init(factory.CreateMixerMessagePresenter(errorTextBehaviour, mixingSessionManager));
            mixingSessionHandlerBehaviour.Init(mixingSessionManager, stringShortenerToggle);

            mixerPresenter_ = factory.CreateMixerPresenter(this, mixingSessionManager);
        }

        public void Resize(int channelCount)
        {
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.
                SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, channelCount * CHANNEL_STRIP_WIDTH + MARGIN);
        }

        private void OnDestroy()
        {
            mixerPresenter_.Dispose();
            dropDownPresenter_.Dispose();
        }
    }
}
