﻿using Colossal.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

namespace Colossal.Mods
{
    public class FakeQuestMenu : MonoBehaviour
    {
        public void Update()
        {
            if (PluginConfig.fakequestmenu)
            {
                if (!GorillaLocomotion.GTPlayer.Instance.inOverlay)
                    GorillaLocomotion.GTPlayer.Instance.inOverlay = true;

                PluginConfig.nofinger = true;

                GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.localPosition = new Vector3(238f, -90f, 0f);
                GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.localPosition = new Vector3(-190f, 90f, 0f);
                GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.rotation = Camera.main.transform.rotation * Quaternion.Euler(-55f, 90f, 0f);
                GorillaLocomotion.GTPlayer.Instance.rightControllerTransform.rotation = Camera.main.transform.rotation * Quaternion.Euler(-55f, -49f, 0f);

                GorillaLocomotion.GTPlayer.Instance.wasLeftHandColliding = false;
                GorillaLocomotion.GTPlayer.Instance.wasRightHandColliding = false;
            }
            else
            {
                if (GorillaLocomotion.GTPlayer.Instance.inOverlay)
                    GorillaLocomotion.GTPlayer.Instance.inOverlay = false;

                PluginConfig.nofinger = false;

                Destroy(this.GetComponent<FakeQuestMenu>());
            }
        }
    }
}
