namespace SpatialConnect.Wwise.Core
{
    public readonly struct RaycastResult
    {
        public RaycastResult(bool hit, float distance = 0f)
        {
            this.hit = hit;
            this.distance = distance;
        }
        public readonly bool hit;
        public readonly float distance;
    }
    
    public interface IRaycaster
    {
        RaycastResult Check();
    }
}
