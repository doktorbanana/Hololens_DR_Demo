using System;

namespace SpatialConnect.Wwise.Core
{
    public class AudioObjectNameTextPresenter : IDisposable
    {
        private readonly ITextBehaviour textBehaviour_;
        private readonly IStringShortenerToggle stringShortenerToggle_;
        private readonly string audioObjectName_;
        
        public AudioObjectNameTextPresenter(ITextBehaviour textBehaviour,
            IStringShortenerToggle stringShortenerToggle, string audioObjectName)
        {
            textBehaviour_ = textBehaviour;
            stringShortenerToggle_ = stringShortenerToggle;
            audioObjectName_ = audioObjectName;

            stringShortenerToggle_.StateChanged += OnStateChanged;
            textBehaviour_.Text = stringShortenerToggle_.Process(audioObjectName_);
        }

        private void OnStateChanged(bool state)
        {
            textBehaviour_.Text = stringShortenerToggle_.Process(audioObjectName_);
        }

        public void Dispose()
        {
            stringShortenerToggle_.StateChanged -= OnStateChanged;
        }
    }
}
