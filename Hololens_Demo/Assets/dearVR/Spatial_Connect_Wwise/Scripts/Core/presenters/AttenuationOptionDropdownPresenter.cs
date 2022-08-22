using System;
using System.Linq;

namespace SpatialConnect.Wwise.Core
{
    public class AttenuationOptionDropdownPresenter : IDisposable
    {
        private const int MAX_NUM_CHARACTERS = 14;
        
        private readonly IDropDownBehaviour dropDownBehaviour_;
        private readonly IPositioningPropertySet positioningPropertySet_;
        private readonly IProjectPropertySet projectPropertySet_;

        public AttenuationOptionDropdownPresenter(IDropDownBehaviour dropDownBehaviour,
            IPositioningPropertySet positioningPropertySet, IProjectPropertySet projectPropertySet)
        {
            dropDownBehaviour_ = dropDownBehaviour;
            positioningPropertySet_ = positioningPropertySet;
            projectPropertySet_ = projectPropertySet;

            dropDownBehaviour_.SelectionChanged += OnSelectionChanged;
            positioningPropertySet_.AttenuationOptionChanged += OnAttenuationOptionChanged;

            if (positioningPropertySet_.IsShareSetSelectable)
            {
                dropDownBehaviour_.Items = projectPropertySet_.AttenuationOptionPropertySets.Select(option =>
                    option.Name.Length > MAX_NUM_CHARACTERS
                        ? option.Name.Substring(0, MAX_NUM_CHARACTERS - 1) + "…"
                        : option.Name).ToArray();

                OnAttenuationOptionChanged();
                
                dropDownBehaviour_.EnableDropdown();
            }
            else
                dropDownBehaviour_.DisableDropdown();
        }

        private void OnSelectionChanged(int index)
        {
            positioningPropertySet_.SetAttenuationOption(projectPropertySet_.AttenuationOptionPropertySets[index].Id);
        }

        private void OnAttenuationOptionChanged()
        {
            var availableShareSets = projectPropertySet_.AttenuationOptionPropertySets;
            dropDownBehaviour_.Selection = 
                Array.FindIndex(availableShareSets, attenuationOption 
                    => positioningPropertySet_.AttenuationOption.Id.Equals(attenuationOption.Id));
        }

        public void Dispose()
        {
            dropDownBehaviour_.SelectionChanged -= OnSelectionChanged;
            positioningPropertySet_.AttenuationOptionChanged -= OnAttenuationOptionChanged;
        }
    }
}
