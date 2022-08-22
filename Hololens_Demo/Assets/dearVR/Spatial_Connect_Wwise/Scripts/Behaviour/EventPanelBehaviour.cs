using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class EventPanelBehaviour : MonoBehaviour
    {
        [SerializeField] private EventListBehaviour eventListBehaviour = default;
        [SerializeField] private EventInspectorBehaviour eventInspectorBehaviour = default;
        [SerializeField] private EventFilterBehaviour eventFilterBehaviour = default;
        [SerializeField] private IconToggleBehaviour pauseToggleBehaviour = default;
        [SerializeField] private ToggleBehaviour filterEventSpheresToggleBehaviour = default;
        
        private readonly IFactory factory_ = new Factory();

        public void Init(IEventManager eventManager, IUserPreferenceSet userPreferenceSet)
        {
            eventListBehaviour.Init(eventManager, factory_.CreateEventFilter(userPreferenceSet.ToggleableFilterKeywordSet), userPreferenceSet.StringShortenerToggle);
            eventInspectorBehaviour.Init(eventManager, userPreferenceSet.StringShortenerToggle);
            eventFilterBehaviour.Init(userPreferenceSet.ToggleableFilterKeywordSet);
            pauseToggleBehaviour.Init(eventManager.PauseToggle);
            filterEventSpheresToggleBehaviour.Init(userPreferenceSet.FilterEventSpheresToggleable);
        }
    }
}
