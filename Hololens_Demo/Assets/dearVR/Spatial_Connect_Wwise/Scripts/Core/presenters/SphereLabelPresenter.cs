using System;

namespace SpatialConnect.Wwise.Core
{
    public class SphereLabelPresenter : IDisposable
    {
        private readonly ITextBehaviour textBehaviour_;
        private readonly IStringShortenerToggle stringShortenerToggle_;
        private readonly IEvent event_;
        
        public SphereLabelPresenter(ITextBehaviour textBehaviour, IStringShortenerToggle stringShortenerToggle, IEvent @event)
        {
            textBehaviour_ = textBehaviour;
            stringShortenerToggle_ = stringShortenerToggle;
            event_ = @event;

            stringShortenerToggle_.StateChanged += OnStateChanged;
            textBehaviour_.Text = stringShortenerToggle_.Process(event_.Name);
        }

        private void OnStateChanged(bool state)
        {
            textBehaviour_.Text = stringShortenerToggle_.Process(event_.Name);
        }

        public void Dispose()
        {
            stringShortenerToggle_.StateChanged -= OnStateChanged;
        }
    }
}
