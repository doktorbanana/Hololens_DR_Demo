using System;

namespace SpatialConnect.Wwise.Core
{
    public interface IDropDownBehaviour
    {
        string[] Items { set; }
        int Selection { set; }

        void EnableDropdown();
        void DisableDropdown();

        event Action<int> SelectionChanged;
    }
}
