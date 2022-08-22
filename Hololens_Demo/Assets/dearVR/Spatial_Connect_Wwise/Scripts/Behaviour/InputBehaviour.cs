using System;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public interface IInputBehaviour
    {
        event Action<bool> StandardInteractionStateChanged;
        event Action<bool> OverlayPositionInteractionStateChanged;
        event Action<bool> AddNodeInteractionStateChanged;
        event Action<bool> RemoveNodeInteractionStateChanged;
    }
    
    public class InputBehaviour : MonoBehaviour, IInputBehaviour
    {
        private readonly IFactory factory_ = new Factory();

        private IInputPreferenceSet inputPreferenceSet_;
        private IInput input_;
        private IStateChangeNotifier<bool> standardInteractionStateChangeNotifier_;
        private IStateChangeNotifier<bool> overlayPositionInteractionStateChangeNotifier_;
        private IStateChangeNotifier<bool> addNodeInteractionStateChangeNotifier_;
        private IStateChangeNotifier<bool> removeNodeInteractionStateChangeNotifier_;

        public void Init(IInputPreferenceSet inputPreferenceSet)
        {
            inputPreferenceSet_ = inputPreferenceSet;
            input_ = factory_.CreateInput();

            standardInteractionStateChangeNotifier_ = factory_.CreateStateChangeNotifier<bool>();
            overlayPositionInteractionStateChangeNotifier_ = factory_.CreateStateChangeNotifier<bool>();
            addNodeInteractionStateChangeNotifier_ = factory_.CreateStateChangeNotifier<bool>();
            removeNodeInteractionStateChangeNotifier_ = factory_.CreateStateChangeNotifier<bool>();

            standardInteractionStateChangeNotifier_.StateChanged += state => StandardInteractionStateChanged?.Invoke(state);
            overlayPositionInteractionStateChangeNotifier_.StateChanged += state => OverlayPositionInteractionStateChanged?.Invoke(state);
            addNodeInteractionStateChangeNotifier_.StateChanged += state => AddNodeInteractionStateChanged?.Invoke(state);
            removeNodeInteractionStateChangeNotifier_.StateChanged += state => RemoveNodeInteractionStateChanged?.Invoke(state);
        }
        
        private void Update()
        {
            ValidateAssignedInput(standardInteractionStateChangeNotifier_, inputPreferenceSet_.StandardInteraction);
            ValidateAssignedInput(overlayPositionInteractionStateChangeNotifier_, inputPreferenceSet_.OverlayPositionInteraction);
            ValidateAssignedInput(addNodeInteractionStateChangeNotifier_, inputPreferenceSet_.AddNodeInteraction);
            ValidateAssignedInput(removeNodeInteractionStateChangeNotifier_, inputPreferenceSet_.RemoveNodeInteraction);
        }
        
        private bool? ToInput(Button button) => button switch
        {
            Button.Unassigned => null,
            Button.LeftIndexTrigger => input_.LeftIndexTrigger,
            Button.RightIndexTrigger => input_.RightIndexTrigger,
            Button.LeftHandTrigger => input_.LeftHandTrigger,
            Button.RightHandTrigger => input_.RightHandTrigger,
            Button.LeftThumbstickClick => input_.LeftThumbstickClick,
            Button.RightThumbstickClick => input_.RightThumbstickClick,
            Button.AButton => input_.AButton,
            Button.BButton => input_.BButton,
            Button.XButton => input_.XButton,
            Button.YButton => input_.YButton,
            _ => throw new ArgumentOutOfRangeException(nameof(button), button, $"Not expected input button: {button}")
        };
        
        private void ValidateAssignedInput(IStateChangeNotifier<bool> stateChangeNotifier, (Button, Button) interaction)
        {
            var (assignedButton, assignedComboButton) = interaction;
            var buttonInput = ToInput(assignedButton);
            var buttonInputCombo = ToInput(assignedComboButton);
            
            if (buttonInput == null) return;
            stateChangeNotifier.Value = buttonInputCombo != null ? buttonInput.Value && buttonInputCombo.Value : buttonInput.Value;
        }

        private void OnDestroy()
        {
            input_.Dispose();
        }

        public event Action<bool> StandardInteractionStateChanged;
        public event Action<bool> OverlayPositionInteractionStateChanged;
        public event Action<bool> AddNodeInteractionStateChanged;
        public event Action<bool> RemoveNodeInteractionStateChanged;
    }
}
