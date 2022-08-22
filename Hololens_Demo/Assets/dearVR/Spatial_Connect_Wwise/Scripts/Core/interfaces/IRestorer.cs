namespace SpatialConnect.Wwise.Core
{
    public interface IRestorer
    {
        void Store();
        
        void Restore();
    }
}
