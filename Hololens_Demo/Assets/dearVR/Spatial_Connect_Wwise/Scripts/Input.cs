using System.Collections.Generic;
using System.Linq;
using UnityEngine.XR;
using SpatialConnect.Wwise.Core;
using Microsoft.MixedReality.Toolkit.Input;
using Peter.Hololens;

namespace SpatialConnect.Wwise
{
    public enum Button
    {
        Unassigned,
        LeftIndexTrigger,
        RightIndexTrigger,
        LeftHandTrigger,
        RightHandTrigger,
        LeftThumbstickClick,
        RightThumbstickClick,
        AButton,
        BButton,
        XButton,
        YButton
    }
    
    public class Input : IInput
    {
        public bool LeftIndexTrigger => GetDeviceInput(leftInputDevices_, CommonUsages.triggerButton);
        public bool RightIndexTrigger => GetHololensInput.HololensInput(); //GetDeviceInput(rightInputDevices_, CommonUsages.triggerButton);
        public bool LeftHandTrigger => GetDeviceInput(leftInputDevices_, CommonUsages.gripButton);
        public bool RightHandTrigger => GetDeviceInput(rightInputDevices_, CommonUsages.gripButton);
        public bool LeftThumbstickClick => GetDeviceInput(leftInputDevices_, CommonUsages.primary2DAxisClick);
        public bool RightThumbstickClick => GetDeviceInput(rightInputDevices_, CommonUsages.primary2DAxisClick);
        public bool AButton => GetDeviceInput(rightInputDevices_, CommonUsages.primaryButton);
        public bool BButton => GetDeviceInput(rightInputDevices_, CommonUsages.secondaryButton);
        public bool XButton => GetDeviceInput(leftInputDevices_, CommonUsages.primaryButton);
        public bool YButton => GetDeviceInput(leftInputDevices_, CommonUsages.secondaryButton);
        
        private readonly List<InputDevice> leftInputDevices_;
        private readonly List<InputDevice> rightInputDevices_;
        
        public Input()
        {
            leftInputDevices_ = new List<InputDevice>();
            rightInputDevices_ = new List<InputDevice>();
            
            InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftInputDevices_);
            InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightInputDevices_);
            
            InputDevices.deviceConnected += OnDeviceConnected;
            InputDevices.deviceDisconnected += OnDeviceDisconnected;

            InputTracking.trackingAcquired += state =>
            {
                InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftInputDevices_);
                InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightInputDevices_);
            };
        }
        
        private void OnDeviceConnected(InputDevice device)
        { 
            switch (device.characteristics)
            {
                case InputDeviceCharacteristics.Left:
                    leftInputDevices_.Add(device);
                    break;
                case InputDeviceCharacteristics.Right:
                    rightInputDevices_.Add(device);
                    break;
            }
        }

        private void OnDeviceDisconnected(InputDevice device)
        {
            switch (device.characteristics)
            {
                case InputDeviceCharacteristics.Left:
                    if (leftInputDevices_.Contains(device))
                        leftInputDevices_.Remove(device);
                    break;
                case InputDeviceCharacteristics.Right:
                    if (rightInputDevices_.Contains(device))
                        rightInputDevices_.Remove(device);
                    break;
            }
        }
        
        private static bool GetDeviceInput(IEnumerable<InputDevice> inputDevices, InputFeatureUsage<bool> inputFeatureUsage)
        {
            return inputDevices
                .Select(device => device.TryGetFeatureValue(inputFeatureUsage, out var value) && value)
                .FirstOrDefault();
        }
        
        public void Dispose()
        {
            InputDevices.deviceConnected -= OnDeviceConnected;
            InputDevices.deviceDisconnected -= OnDeviceDisconnected;
            leftInputDevices_.Clear();
            rightInputDevices_.Clear();
        }
    }
}
