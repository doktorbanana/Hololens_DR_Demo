using System;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class ChannelStripBehaviour : MonoBehaviour, IChannelStripBehaviour
    {
        [SerializeField] private TextBehaviour nameTextBehaviour = default;
        [SerializeField] private ToggleBehaviour soloToggleBehaviour = default;
        [SerializeField] private ToggleBehaviour muteToggleBehaviour = default;
        [SerializeField] private SliderBehaviour voiceVolumeSliderBehaviour = default;
        [SerializeField] private GameObject outline = default;

        private int index_;
        
        public bool OutlineActive
        {
            set => outline.SetActive(value);
        }

        public void Init(IAudioObject audioObject, int index, IStringShortenerToggle stringShortenerToggle, IFactory factory = null)
        {
            index_ = index;
            factory ??= new Factory();

            nameTextBehaviour.Init(factory.CreateTextPresenter(nameTextBehaviour, stringShortenerToggle, audioObject.NameText));
            soloToggleBehaviour.Init(audioObject.SoloToggle);
            muteToggleBehaviour.Init(audioObject.MuteToggle);
            voiceVolumeSliderBehaviour.Init(factory.CreateSliderPresenter(voiceVolumeSliderBehaviour, audioObject.VoiceVolumeValue));
        }
        
        public void OnSelect()
        {
            ChannelStripSelected?.Invoke(index_);
        }

        public event Action<int> ChannelStripSelected;
    }
}
