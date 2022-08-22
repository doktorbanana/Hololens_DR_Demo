using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IPositioningPropertySet : IDisposable
    {
        bool IsShareSetSelectable { get; }
        bool IsEditable { get; }
        
        IOverrideParent OverridePositioning { get; }
        bool IsAttenuationEnabled { get; }
        IAttenuationOption AttenuationOption { get;}

        void SetAttenuationOption(string attenuationId);
        
        event Action AttenuationOptionChanged;
    }
}
