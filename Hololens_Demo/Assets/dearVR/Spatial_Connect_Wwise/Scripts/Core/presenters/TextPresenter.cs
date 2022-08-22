using System;

namespace SpatialConnect.Wwise.Core
{
    public class TextPresenter : IDisposable 
    {
        private readonly IText text_;
        private readonly ITextBehaviour textBehaviour_;
        private readonly IStringShortenerToggle stringShortenerToggle_;
        
        public TextPresenter(ITextBehaviour textBehaviour, IStringShortenerToggle stringShortenerToggle, IText text)
        {
            textBehaviour_ = textBehaviour;
            text_ = text;
            stringShortenerToggle_ = stringShortenerToggle;
            stringShortenerToggle_.StateChanged += OnStateChanged;
            
            textBehaviour_.Text = stringShortenerToggle_.Process(text_.Text);
        }

        private void OnStateChanged(bool state)
        {
            textBehaviour_.Text = stringShortenerToggle_.Process(text_.Text);
        }
        
        public void Dispose()
        {
            stringShortenerToggle_.StateChanged -= OnStateChanged;
        }
    }
}
