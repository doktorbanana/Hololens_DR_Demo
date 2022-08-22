namespace SpatialConnect.Wwise.Core
{
    public class OverlayVisibility : IShowable
    {
        private readonly IShowable[] showables_;

        public OverlayVisibility(IShowable[] showables)
        {
            showables_ = showables;
        }

        public void Show()
        {
            foreach (var showable in showables_) 
                showable.Show();
        }

        public void Hide()
        {
            foreach (var showable in showables_) 
                showable.Hide();
        }
    }
}
