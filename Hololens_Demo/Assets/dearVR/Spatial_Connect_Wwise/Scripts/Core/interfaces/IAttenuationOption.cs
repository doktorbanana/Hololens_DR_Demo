using System;
using System.Collections.Generic;

namespace SpatialConnect.Wwise.Core
{
    public readonly struct Point
    {
        public readonly float x;
        public readonly float y;

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
    
    public enum AttenuationType
    {
        ShareSet,
        Custom,
        None
    }
    
    public interface IAttenuationOption : IDisposable
    {
        string Name { get; }
        string Id { get; }
        AttenuationType Type { get; }
        List<Point> Points { get; set; }
        uint MaxDistance { get; set; }
        
        event Action<List<Point>> CurveChanged;
        event Action<uint> MaxDistanceChanged;
    }
}
