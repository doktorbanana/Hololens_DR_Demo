using System;
using System.Linq;
using SpatialConnect.Wwise.Core.Wwu;

namespace SpatialConnect.Wwise.Core
{
    public class MixingSessionDropDownPresenter : IDisposable
    {
        private const int MAX_NUM_CHARACTERS = 28;
        
        private readonly IDropDownBehaviour dropDownBehaviour_;
        private readonly IMixingSessionManager mixingSessionManager_;
        
        public MixingSessionDropDownPresenter(IDropDownBehaviour dropDownBehaviour, IMixingSessionManager mixingSessionManager)
        {
            dropDownBehaviour_ = dropDownBehaviour;
            mixingSessionManager_ = mixingSessionManager;
            
            dropDownBehaviour_.SelectionChanged += OnSelectionChanged;
            mixingSessionManager_.WwuFileLoaded += OnWwuFileLoaded;
            mixingSessionManager_.MixingSessionSelectionChanged += OnMixingSessionSelectionChanged;
            
            dropDownBehaviour_.DisableDropdown();
        }
        
        private void OnSelectionChanged(int index)
        {
            mixingSessionManager_.MixingSessionSelection = index;
        }

        private void OnWwuFileLoaded(WwuFileLoadState state)
        {
            if (state != WwuFileLoadState.Success) 
                return;
            
            dropDownBehaviour_.EnableDropdown();
            dropDownBehaviour_.Items = mixingSessionManager_
                .MixingSessionPropertySets.Select(mixingSessionPropertySet =>
                {
                    var name = mixingSessionPropertySet.Name;
                    return name.Length > MAX_NUM_CHARACTERS ? name.Substring(0, MAX_NUM_CHARACTERS - 1) + "…" : name;
                }).ToArray();
        }
        
        private void OnMixingSessionSelectionChanged(int index, IMixingSession mixingSession)
        {
            dropDownBehaviour_.Selection = index;
        }
        
        public void Dispose()
        {
            dropDownBehaviour_.SelectionChanged -= OnSelectionChanged;
            mixingSessionManager_.WwuFileLoaded -= OnWwuFileLoaded;
            mixingSessionManager_.MixingSessionSelectionChanged -= OnMixingSessionSelectionChanged;
        }
    }
}
