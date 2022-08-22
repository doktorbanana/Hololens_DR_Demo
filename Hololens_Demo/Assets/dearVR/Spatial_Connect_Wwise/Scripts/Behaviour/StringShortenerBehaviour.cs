using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class StringShortenerBehaviour : MonoBehaviour
    {
        [SerializeField] private ToggleBehaviour toggleBehaviour = default;
        
        public void Init(IStringShortenerToggle stringShortenerToggle)
        {
            toggleBehaviour.Init(stringShortenerToggle);
        }
    }
}
