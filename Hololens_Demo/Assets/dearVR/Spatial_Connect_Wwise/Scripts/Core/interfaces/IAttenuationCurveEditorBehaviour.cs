using System;
using System.Collections.Generic;

namespace SpatialConnect.Wwise.Core
{
    public interface IAttenuationCurveEditorBehaviour
    {
        List<Node> Nodes { set; }

        string NodeHoverText { set; }

        event Action<List<Node>> EditorChanged;
        event Action<Node?> HoverStateUpdated;
    }
}
