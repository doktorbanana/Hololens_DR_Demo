using System;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class MixingSessionBehaviour : MonoBehaviour, IMixingSessionBehaviour
    {
        [SerializeField] private GameObject channelStripPrefab = default;
        [SerializeField] private GameObject channelStrips = default;
        [SerializeField] private SliderBehaviour sliderBehaviour = default;

        private IDisposable mixingSessionPresenter_;
        private const int MAX_CHANNEL_STRIPS = 8;
        private const float CHANNEL_STRIP_WIDTH = 160;

        public int? ChannelStripSelection
        {
            set
            {
                var childCount = channelStrips.transform.childCount;
                for (var i = 0; i < childCount; ++i)
                {
                    var child = channelStrips.transform.GetChild(i);
                    var channelStripBehaviour = child.GetComponent<ChannelStripBehaviour>();
                    channelStripBehaviour.OutlineActive = value == i;
                }
            }
        }
        
        public void Init(IMixingSession mixingSession, IStringShortenerToggle stringShortenerToggle, IFactory factory = null)
        {
            for (var i = 0; i < mixingSession.AudioObjects.Length; ++i)
            {
                var channelStrip = Instantiate(channelStripPrefab, channelStrips.transform, false);
                var channelStripBehaviour = channelStrip.GetComponent<ChannelStripBehaviour>();
                channelStripBehaviour.Init(mixingSession.AudioObjects[i], i, stringShortenerToggle);
                channelStripBehaviour.ChannelStripSelected += OnChannelStripSelected;
                var rectTransform = channelStrip.GetComponent<RectTransform>();
                rectTransform.anchoredPosition3D = new Vector3(rectTransform.rect.width * i, 0f, 0f);
            }
            
            factory = factory?? new Factory();
            mixingSessionPresenter_ = factory.CreateMixingSessionPresenter(this, sliderBehaviour, mixingSession);
            sliderBehaviour.Init();
        }

        public void ScrollTo(int offset)
        {
            var channelStripCount = channelStrips.transform.childCount;
            for (var i = 0; i < channelStripCount; ++i)
            {
                var shouldActivate = offset <= i && i < offset + MAX_CHANNEL_STRIPS;
                channelStrips.transform.GetChild(i).gameObject.SetActive(shouldActivate);
            }

            var rectTransform = channelStrips.GetComponent<RectTransform>();
            rectTransform.anchoredPosition3D = new Vector3(-CHANNEL_STRIP_WIDTH * offset, 0f, 0f);
        }

        private void OnChannelStripSelected(int index)
        {
            ChannelStripSelectionChanged?.Invoke(index);
        }

        private void OnDestroy()
        {
            mixingSessionPresenter_.Dispose();
        }

        public event Action<int> ChannelStripSelectionChanged;
    }
}
