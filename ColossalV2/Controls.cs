using Colossal.Auth;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace Colossal.Patches
{
    public class ControlsV2
    {
        public static bool GetControl(string controlName)
        {
            if(Init.logginIn)
            {
                switch (controlName)
                {
                    case "LJoystick":
                        return ControlsV2.LeftJoystick();
                    case "RJoystick":
                        return ControlsV2.RightJoystick();
                    case "RTrigger":
                        return ControlsV2.RightTrigger();
                    case "LTrigger":
                        return ControlsV2.LeftTrigger();
                    case "RGrip":
                        return ControlsV2.RightGrip();
                    case "LGrip":
                        return ControlsV2.LeftGrip();
                    case "LPrimary":
                        return ControlsV2.LeftPrimaryButton();
                    case "RPrimary":
                        return ControlsV2.RightPrimaryButton();
                    case "LSecondary":
                        return ControlsV2.LeftSecondaryButton();
                    case "RSecondary":
                        return ControlsV2.RightSecondaryButton();
                    default:
                        return false;
                }
            }
            return false;
        }

        public static bool LeftJoystick()
        {
            if(Init.logginIn)
            {
                bool Value;
                if (WhatAmI.oculus)
                    InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primary2DAxisClick, out Value);
                else
                    Value = SteamVR_Actions.gorillaTag_LeftJoystickClick.GetState(SteamVR_Input_Sources.LeftHand);
                return Value;
            }
            return false;
        }

        public static Vector2 LeftJoystickAxis()
        {
            if (Init.logginIn)
            {
                Vector2 Value;
                if (WhatAmI.oculus)
                    InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out Value);
                else
                    Value = SteamVR_Actions.gorillaTag_LeftJoystick2DAxis.axis;
                return Value;
            }
            return new Vector2(0, 0);
        }

        public static bool RightJoystick()
        {
            if (Init.logginIn)
            {
                bool Value;
                if (WhatAmI.oculus)
                    InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primary2DAxisClick, out Value);
                else
                    Value = SteamVR_Actions.gorillaTag_RightJoystickClick.GetState(SteamVR_Input_Sources.RightHand);
                return Value;
            }
            return false;
        }

        public static Vector2 RightJoystickAxis()
        {
            if (Init.logginIn)
            {
                Vector2 Value;
                if (WhatAmI.oculus)
                    InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out Value);
                else
                    Value = SteamVR_Actions.gorillaTag_RightJoystick2DAxis.axis;
                return Value;
            }
            return new Vector2(0, 0);
        }

        public static bool RightTrigger()
        {
            if (Init.logginIn)
            {
                bool Value;
                if (WhatAmI.oculus)
                    InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out Value);
                else
                    Value = SteamVR_Actions.gorillaTag_RightTriggerClick.GetState(SteamVR_Input_Sources.RightHand);
                return Value;
            }
            return false;
        }

        public static bool LeftTrigger()
        {
            if (Init.logginIn)
            {
                bool Value;
                if (WhatAmI.oculus)
                    InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.triggerButton, out Value);
                else
                    Value = SteamVR_Actions.gorillaTag_LeftTriggerClick.GetState(SteamVR_Input_Sources.LeftHand);
                return Value;
            }
            return false;
        }

        public static bool RightGrip()
        {
            if (Init.logginIn)
            {
                bool Value;
                if (WhatAmI.oculus)
                    InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.gripButton, out Value);
                else
                    Value = SteamVR_Actions.gorillaTag_RightGripClick.GetState(SteamVR_Input_Sources.RightHand);
                return Value;
            }
            return false;
        }

        public static bool LeftGrip()
        {
            if (Init.logginIn)
            {
                bool Value;
                if (WhatAmI.oculus)
                    InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.gripButton, out Value);
                else
                    Value = SteamVR_Actions.gorillaTag_LeftGripClick.GetState(SteamVR_Input_Sources.LeftHand);
                return Value;
            }
            return false;
        }

        public static bool LeftPrimaryButton()
        {
            if (Init.logginIn)
            {
                bool Value;
                if (WhatAmI.oculus)
                    InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primaryButton, out Value);
                else
                    Value = SteamVR_Actions.gorillaTag_LeftPrimaryClick.GetState(SteamVR_Input_Sources.LeftHand);
                return Value;
            }
            return false;
        }

        public static bool RightPrimaryButton()
        {
            if (Init.logginIn)
            {
                bool Value;
                if (WhatAmI.oculus)
                    InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out Value);
                else
                    Value = SteamVR_Actions.gorillaTag_RightPrimaryClick.GetState(SteamVR_Input_Sources.RightHand);
                return Value;
            }
            return false;
        }

        public static bool LeftSecondaryButton()
        {
            if (Init.logginIn)
            {
                bool Value;
                if (WhatAmI.oculus)
                    InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.secondaryButton, out Value);
                else
                    Value = SteamVR_Actions.gorillaTag_LeftSecondaryClick.GetState(SteamVR_Input_Sources.LeftHand);
                return Value;
            }
            return false;
        }

        public static bool RightSecondaryButton()
        {
            if (Init.logginIn)
            {
                bool Value;
                if (WhatAmI.oculus)
                    InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.secondaryButton, out Value);
                else
                    Value = SteamVR_Actions.gorillaTag_RightSecondaryClick.GetState(SteamVR_Input_Sources.RightHand);
                return Value;
            }
            return false;
        }
    }
}
