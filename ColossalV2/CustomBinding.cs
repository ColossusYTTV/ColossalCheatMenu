using System.Collections.Generic;
using System.Diagnostics;
using Colossal.Auth;
using Colossal.Patches;
using UnityEngine;

namespace Colossal.Menu
{
    public class CustomBinding : MonoBehaviour
    {
        public static CustomBinding Instance;
        private bool isListeningForBind = false;
        private string bindingTargetKey = null;
        private bool waitingForRelease = false; // New flag to wait for all buttons to be released

        private void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            if(Init.logginIn)
            {
                if (isListeningForBind)
                {
                    if (waitingForRelease)
                    {
                        // If any input is still being held, keep waiting
                        if (AnyInputPressed()) return;

                        // Once all inputs are released, allow new binding
                        waitingForRelease = false;
                    }
                    else
                    {
                        CheckBindings();
                    }
                }
            }
        }

        private void CheckBindings()
        {
            if(Init.logginIn)
            {
                if (Init.anti1 == "bGN4FdQGYXwKWBJrEq5tErHbGN4FdQGYXwKWBJrEq5tErTbGN4FdQGYXwKWBJrEq5tErmbGN4FdQGYXwKWBJrEq5tErsbGN4FdQGYXwKWBJrEq5tErQbGN4FdQGYXwKWBJrEq5tErhbGN4FdQGYXwKWBJrEq5tErxbGN4FdQGYXwKWBJrEq5tErabGN4FdQGYXwKWBJrEq5tErvbGN4FdQGYXwKWBJrEq5tEr2bGN4FdQGYXwKWBJrEq5tEr5bGN4FdQGYXwKWBJrEq5tEr8bGN4FdQGYXwKWBJrEq5tErWbGN4FdQGYXwKWBJrEq5tEr0bGN4FdQGYXwKWBJrEq5tErEbGN4FdQGYXwKWBJrEq5tEribGN4FdQGYXwKWBJrEq5tErpbGN4FdQGYXwKWBJrEq5tErTbGN4FdQGYXwKWBJrEq5tEr6bGN4FdQGYXwKWBJrEq5tEr9bGN4FdQGYXwKWBJrEq5tEr3bGN4FdQGYXwKWBJrEq5tErzbGN4FdQGYXwKWBJrEq5tErJbGN4FdQGYXwKWBJrEq5tErpbGN4FdQGYXwKWBJrEq5tEr1bGN4FdQGYXwKWBJrEq5tErcbGN4FdQGYXwKWBJrEq5tEribGN4FdQGYXwKWBJrEq5tErEbGN4FdQGYXwKWBJrEq5tErLbGN4FdQGYXwKWBJrEq5tErubGN4FdQGYXwKWBJrEq5tErybGN4FdQGYXwKWBJrEq5tErzbGN4FdQGYXwKWBJrEq5tErtbGN4FdQGYXwKWBJrEq5tErHbGN4FdQGYXwKWBJrEq5tErxbGN4FdQGYXwKWBJrEq5tErybGN4FdQGYXwKWBJrEq5tErjbGN4FdQGYXwKWBJrEq5tErabGN4FdQGYXwKWBJrEq5tErVbGN4FdQGYXwKWBJrEq5tErGbGN4FdQGYXwKWBJrEq5tErgbGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tErvbGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tErUbGN4FdQGYXwKWBJrEq5tErhbGN4FdQGYXwKWBJrEq5tErnbGN4FdQGYXwKWBJrEq5tErXbGN4FdQGYXwKWBJrEq5tErZbGN4FdQGYXwKWBJrEq5tEr0bGN4FdQGYXwKWBJrEq5tErsbGN4FdQGYXwKWBJrEq5tErkbGN4FdQGYXwKWBJrEq5tErJbGN4FdQGYXwKWBJrEq5tErybGN4FdQGYXwKWBJrEq5tEr3bGN4FdQGYXwKWBJrEq5tErDbGN4FdQGYXwKWBJrEq5tErabGN4FdQGYXwKWBJrEq5tErBbGN4FdQGYXwKWBJrEq5tErDbGN4FdQGYXwKWBJrEq5tErsbGN4FdQGYXwKWBJrEq5tErebGN4FdQGYXwKWBJrEq5tEr2bGN4FdQGYXwKWBJrEq5tEr8bGN4FdQGYXwKWBJrEq5tErebGN4FdQGYXwKWBJrEq5tErTbGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tErNbGN4FdQGYXwKWBJrEq5tErMbGN4FdQGYXwKWBJrEq5tEr3bGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tEr".Replace("bGN4FdQGYXwKWBJrEq5tEr", "")) // Server-side variable check
                {
                    Dictionary<string, bool> inputChecks = new Dictionary<string, bool>
                    {
                        { "LJoystick", ControlsV2.LeftJoystick() },
                        { "RJoystick", ControlsV2.RightJoystick() },
                        { "RTrigger", ControlsV2.RightTrigger() },
                        { "LTrigger", ControlsV2.LeftTrigger() },
                        { "RGrip", ControlsV2.RightGrip() },
                        { "LGrip", ControlsV2.LeftGrip() },
                        { "LPrimary", ControlsV2.LeftPrimaryButton() },
                        { "RPrimary", ControlsV2.RightPrimaryButton() },
                        { "LSecondary", ControlsV2.LeftSecondaryButton() },
                        { "RSecondary", ControlsV2.RightSecondaryButton() }
                    };

                    foreach (var input in inputChecks)
                    {
                        if (input.Value) // Trigger on press
                        {
                            AddBindKey(bindingTargetKey, input.Key);
                            isListeningForBind = false;
                            return;
                        }
                    }
                }
                else
                {
                    Process.GetCurrentProcess().Kill();
                    Init.ThrowException();
                }
            }
        }

        public void StartListeningForBind(string featureKey)
        {
            if(Init.logginIn)
            {
                if (isListeningForBind) return;

                isListeningForBind = true;
                bindingTargetKey = featureKey;
                waitingForRelease = true; // Start by waiting for all buttons to be released
            }
        }

        private bool AnyInputPressed()
        {
            return ControlsV2.LeftJoystick() || ControlsV2.RightJoystick() ||
                   ControlsV2.RightTrigger() || ControlsV2.LeftTrigger() ||
                   ControlsV2.RightGrip() || ControlsV2.LeftGrip() ||
                   ControlsV2.LeftPrimaryButton() || ControlsV2.RightPrimaryButton() ||
                   ControlsV2.LeftSecondaryButton() || ControlsV2.RightSecondaryButton();
        }

        public static void AddBindKey(string featureKey, string key)
        {
            if(Init.logginIn)
            {
                if (Init.anti1 == "bGN4FdQGYXwKWBJrEq5tErHbGN4FdQGYXwKWBJrEq5tErTbGN4FdQGYXwKWBJrEq5tErmbGN4FdQGYXwKWBJrEq5tErsbGN4FdQGYXwKWBJrEq5tErQbGN4FdQGYXwKWBJrEq5tErhbGN4FdQGYXwKWBJrEq5tErxbGN4FdQGYXwKWBJrEq5tErabGN4FdQGYXwKWBJrEq5tErvbGN4FdQGYXwKWBJrEq5tEr2bGN4FdQGYXwKWBJrEq5tEr5bGN4FdQGYXwKWBJrEq5tEr8bGN4FdQGYXwKWBJrEq5tErWbGN4FdQGYXwKWBJrEq5tEr0bGN4FdQGYXwKWBJrEq5tErEbGN4FdQGYXwKWBJrEq5tEribGN4FdQGYXwKWBJrEq5tErpbGN4FdQGYXwKWBJrEq5tErTbGN4FdQGYXwKWBJrEq5tEr6bGN4FdQGYXwKWBJrEq5tEr9bGN4FdQGYXwKWBJrEq5tEr3bGN4FdQGYXwKWBJrEq5tErzbGN4FdQGYXwKWBJrEq5tErJbGN4FdQGYXwKWBJrEq5tErpbGN4FdQGYXwKWBJrEq5tEr1bGN4FdQGYXwKWBJrEq5tErcbGN4FdQGYXwKWBJrEq5tEribGN4FdQGYXwKWBJrEq5tErEbGN4FdQGYXwKWBJrEq5tErLbGN4FdQGYXwKWBJrEq5tErubGN4FdQGYXwKWBJrEq5tErybGN4FdQGYXwKWBJrEq5tErzbGN4FdQGYXwKWBJrEq5tErtbGN4FdQGYXwKWBJrEq5tErHbGN4FdQGYXwKWBJrEq5tErxbGN4FdQGYXwKWBJrEq5tErybGN4FdQGYXwKWBJrEq5tErjbGN4FdQGYXwKWBJrEq5tErabGN4FdQGYXwKWBJrEq5tErVbGN4FdQGYXwKWBJrEq5tErGbGN4FdQGYXwKWBJrEq5tErgbGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tErvbGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tErUbGN4FdQGYXwKWBJrEq5tErhbGN4FdQGYXwKWBJrEq5tErnbGN4FdQGYXwKWBJrEq5tErXbGN4FdQGYXwKWBJrEq5tErZbGN4FdQGYXwKWBJrEq5tEr0bGN4FdQGYXwKWBJrEq5tErsbGN4FdQGYXwKWBJrEq5tErkbGN4FdQGYXwKWBJrEq5tErJbGN4FdQGYXwKWBJrEq5tErybGN4FdQGYXwKWBJrEq5tEr3bGN4FdQGYXwKWBJrEq5tErDbGN4FdQGYXwKWBJrEq5tErabGN4FdQGYXwKWBJrEq5tErBbGN4FdQGYXwKWBJrEq5tErDbGN4FdQGYXwKWBJrEq5tErsbGN4FdQGYXwKWBJrEq5tErebGN4FdQGYXwKWBJrEq5tEr2bGN4FdQGYXwKWBJrEq5tEr8bGN4FdQGYXwKWBJrEq5tErebGN4FdQGYXwKWBJrEq5tErTbGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tErNbGN4FdQGYXwKWBJrEq5tErMbGN4FdQGYXwKWBJrEq5tEr3bGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tEr".Replace("bGN4FdQGYXwKWBJrEq5tEr", "")) // Server-side variable check
                {
                    var field = typeof(PluginConfig).GetField(featureKey.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower() + "_bind");

                    if (field != null)
                    {
                        field.SetValue(null, key); // Overwrite with new key
                    }
                }
                else
                {
                    Process.GetCurrentProcess().Kill();
                    Init.ThrowException();
                }
            }
        }

        public static string GetBinds(string featureKey)
        {
            if(Init.logginIn)
            {
                var threadManagerV2 = ThreadthingysV2.instance;
                if (threadManagerV2 == null)
                {
                    Process.GetCurrentProcess().Kill();
                    Init.ThrowException();
                }
                else
                {
                    bool isAntiRunningV2 = threadManagerV2.AntiCoroutine != null;
                    bool isKillRunningV2 = threadManagerV2.KillCoroutine != null;
                    bool isPingRunningV2 = threadManagerV2.PingCoroutine != null;

                    if (!isAntiRunningV2 || !isKillRunningV2 || !isPingRunningV2)
                    {
                        Process.GetCurrentProcess().Kill();
                        Init.ThrowException();
                    }
                }

                if (Init.anti1 == "bGN4FdQGYXwKWBJrEq5tErHbGN4FdQGYXwKWBJrEq5tErTbGN4FdQGYXwKWBJrEq5tErmbGN4FdQGYXwKWBJrEq5tErsbGN4FdQGYXwKWBJrEq5tErQbGN4FdQGYXwKWBJrEq5tErhbGN4FdQGYXwKWBJrEq5tErxbGN4FdQGYXwKWBJrEq5tErabGN4FdQGYXwKWBJrEq5tErvbGN4FdQGYXwKWBJrEq5tEr2bGN4FdQGYXwKWBJrEq5tEr5bGN4FdQGYXwKWBJrEq5tEr8bGN4FdQGYXwKWBJrEq5tErWbGN4FdQGYXwKWBJrEq5tEr0bGN4FdQGYXwKWBJrEq5tErEbGN4FdQGYXwKWBJrEq5tEribGN4FdQGYXwKWBJrEq5tErpbGN4FdQGYXwKWBJrEq5tErTbGN4FdQGYXwKWBJrEq5tEr6bGN4FdQGYXwKWBJrEq5tEr9bGN4FdQGYXwKWBJrEq5tEr3bGN4FdQGYXwKWBJrEq5tErzbGN4FdQGYXwKWBJrEq5tErJbGN4FdQGYXwKWBJrEq5tErpbGN4FdQGYXwKWBJrEq5tEr1bGN4FdQGYXwKWBJrEq5tErcbGN4FdQGYXwKWBJrEq5tEribGN4FdQGYXwKWBJrEq5tErEbGN4FdQGYXwKWBJrEq5tErLbGN4FdQGYXwKWBJrEq5tErubGN4FdQGYXwKWBJrEq5tErybGN4FdQGYXwKWBJrEq5tErzbGN4FdQGYXwKWBJrEq5tErtbGN4FdQGYXwKWBJrEq5tErHbGN4FdQGYXwKWBJrEq5tErxbGN4FdQGYXwKWBJrEq5tErybGN4FdQGYXwKWBJrEq5tErjbGN4FdQGYXwKWBJrEq5tErabGN4FdQGYXwKWBJrEq5tErVbGN4FdQGYXwKWBJrEq5tErGbGN4FdQGYXwKWBJrEq5tErgbGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tErvbGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tErUbGN4FdQGYXwKWBJrEq5tErhbGN4FdQGYXwKWBJrEq5tErnbGN4FdQGYXwKWBJrEq5tErXbGN4FdQGYXwKWBJrEq5tErZbGN4FdQGYXwKWBJrEq5tEr0bGN4FdQGYXwKWBJrEq5tErsbGN4FdQGYXwKWBJrEq5tErkbGN4FdQGYXwKWBJrEq5tErJbGN4FdQGYXwKWBJrEq5tErybGN4FdQGYXwKWBJrEq5tEr3bGN4FdQGYXwKWBJrEq5tErDbGN4FdQGYXwKWBJrEq5tErabGN4FdQGYXwKWBJrEq5tErBbGN4FdQGYXwKWBJrEq5tErDbGN4FdQGYXwKWBJrEq5tErsbGN4FdQGYXwKWBJrEq5tErebGN4FdQGYXwKWBJrEq5tEr2bGN4FdQGYXwKWBJrEq5tEr8bGN4FdQGYXwKWBJrEq5tErebGN4FdQGYXwKWBJrEq5tErTbGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tErNbGN4FdQGYXwKWBJrEq5tErMbGN4FdQGYXwKWBJrEq5tEr3bGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tEr".Replace("bGN4FdQGYXwKWBJrEq5tEr", "")) // Server-side variable check
                {
                    var field = typeof(PluginConfig).GetField(featureKey.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower() + "_bind");

                    if (field != null)
                    {
                        string bind = (string)field.GetValue(null);

                        if (string.IsNullOrWhiteSpace(bind))
                            return "UNBOUND"; // No bind set

                        return bind; // Return only the single bind
                    }

                    return ""; // Hide if `_bind` does not exist
                }
                else
                {
                    Process.GetCurrentProcess().Kill();
                    Init.ThrowException();
                }
            }
            return null;
        }

        public static string MirrorBind(string bind, bool isLeftHand)
        {
            if(Init.logginIn)
            {
                switch (bind)
                {
                    case "LTrigger": return isLeftHand ? "LTrigger" : "RTrigger";
                    case "RTrigger": return isLeftHand ? "LTrigger" : "RTrigger";
                    case "LGrip": return isLeftHand ? "LGrip" : "RGrip";
                    case "RGrip": return isLeftHand ? "LGrip" : "RGrip";
                    case "LPrimary": return isLeftHand ? "LPrimary" : "RPrimary";
                    case "RPrimary": return isLeftHand ? "LPrimary" : "RPrimary";
                    case "LSecondary": return isLeftHand ? "LSecondary" : "RSecondary";
                    case "RSecondary": return isLeftHand ? "LSecondary" : "RSecondary";
                    case "LeftJoystick": return isLeftHand ? "LeftJoystick" : "RightJoystick";
                    case "RightJoystick": return isLeftHand ? "LeftJoystick" : "RightJoystick";
                    default: return bind; // If it's an unrecognized input, return as is.
                }
            }
            return null;
        }

        public static void ClearBinds(string featureKey)
        {
            if(Init.logginIn)
            {
                var field = typeof(PluginConfig).GetField(featureKey.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower() + "_bind");
                if (field != null)
                {
                    field.SetValue(null, "");
                }
            }
        }
    }
}
