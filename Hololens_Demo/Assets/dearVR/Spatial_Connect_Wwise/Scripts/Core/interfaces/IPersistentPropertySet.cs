namespace SpatialConnect.Wwise.Core
{
    public interface IPersistentPropertySet
    {
        string SelectedAudioObjectId { get; set; }
        string SelectedMixingSessionId { get; set; }
        bool ShortenNameLabel { get; set; }
    }
}
