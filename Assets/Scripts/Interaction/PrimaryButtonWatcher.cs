﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

namespace Kit.Oculus.Interaction
{
    [System.Serializable]

    public class PrimaryButtonWatcher : MonoBehaviour
    {
        private bool lastButtonState = false;
        private List<UnityEngine.XR.InputDevice> allDevices;
        private List<UnityEngine.XR.InputDevice> devicesWithPrimaryButton;

        void Start()
        {
            allDevices = new List<UnityEngine.XR.InputDevice>();
            devicesWithPrimaryButton = new List<UnityEngine.XR.InputDevice>();
            InputTracking.nodeAdded += InputTracking_nodeAdded;
        }

        // check for new input devices when new XRNode is added
        private void InputTracking_nodeAdded(XRNodeState obj)
        {
            updateInputDevices();
        }

        void Update()
        {
            bool tempState = false;
            bool invalidDeviceFound = false;
            foreach (var device in devicesWithPrimaryButton)
            {
                bool primaryButtonState = false;
                tempState = device.isValid // the device is still valid
                            && device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonState) // did get a value
                            && primaryButtonState // the value we got
                            || tempState; // cumulative result from other controllers
                if (!device.isValid)
                    invalidDeviceFound = true;
            }

            if (tempState != lastButtonState) // Button state changed since last frame
            {
                EventManager.TriggerEvent("ActivatePopup");
                lastButtonState = tempState;
            }

            if (invalidDeviceFound || devicesWithPrimaryButton.Count == 0) // refresh device lists
                updateInputDevices();
        }

        // find any devices supporting the desired feature usage
        void updateInputDevices()
        {
            devicesWithPrimaryButton.Clear();
            UnityEngine.XR.InputDevices.GetDevices(allDevices);
            bool discardedValue;
            foreach (var device in allDevices)
            {
                if (device.TryGetFeatureValue(CommonUsages.primaryButton, out discardedValue))
                {
                    devicesWithPrimaryButton.Add(device); // Add any devices that have a primary button.
                }
            }
        }

        void ActivatePopup()
        {
            EventManager.TriggerEvent("ActivatePopup");
        }
    }
}