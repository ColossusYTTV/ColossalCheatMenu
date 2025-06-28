﻿using Colossal.Menu;
using Colossal.Patches;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Colossal.Mods
{
    public class ExcelFly : MonoBehaviour
    {
        private static readonly float[] speeds = { 0f, 8f, 6f, 4f, 2f, 1f };
        private float speed;

        public void FixedUpdate()
        {
            int flySetting = PluginConfig.excelfly;

            if (flySetting == 0)
            {
                Destroy(this);
                return;
            }

            speed = speeds[Math.Min(flySetting, speeds.Length - 1)];

            string bind = CustomBinding.GetBinds("excelfly"); // Get the bind
            if (!string.IsNullOrEmpty(bind) && bind != "UNBOUND")
            {
                // Mirror the bind correctly
                string leftBind = CustomBinding.MirrorBind(bind, true);
                string rightBind = CustomBinding.MirrorBind(bind, false);

                if (ControlsV2.GetControl(leftBind))
                {
                    GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.velocity +=
                        -GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.right / speed;
                }

                if (ControlsV2.GetControl(rightBind))
                {
                    GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.velocity +=
                        GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.right / speed;
                }
            }
        }
    }
}
