namespace SpatialConnect.Wwise.Core
{
    public class ShortenNameLabelRestorer : IRestorer
    {
        private readonly IToggleable toggleable_;
        private readonly IPersistentPropertySet persistentPropertySet_;
        
        public ShortenNameLabelRestorer(IToggleable toggleable, IPersistentPropertySet persistentPropertySet)
        {
            toggleable_ = toggleable;
            persistentPropertySet_ = persistentPropertySet;
        }
        
        public void Store()
        {
            persistentPropertySet_.ShortenNameLabel = toggleable_.State;
        }

        public void Restore()
        {
            if(persistentPropertySet_.ShortenNameLabel)
                toggleable_.SwitchState();
        }
    }
}
