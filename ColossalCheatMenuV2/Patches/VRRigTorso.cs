﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Colossal.Menu;
using Colossal.Mods;
using HarmonyLib;
using UnityEngine;

namespace Colossal.Patches
{
    [HarmonyPatch(typeof(VRRig), "LateUpdate")]
    class VRRRigTorsoPatch
    {
        static void Postfix(VRRig __instance)
        {
            if (!PluginConfig.decapitation)
                return;

            if (__instance.isOfflineVRRig)
            {
                __instance.transform.rotation = Quaternion.Euler(0f, Decapitation.yRotation, 0f);
                __instance.head.MapMine(__instance.scaleFactor, __instance.playerOffsetTransform);
                __instance.rightHand.MapMine(__instance.scaleFactor, __instance.playerOffsetTransform);
                __instance.leftHand.MapMine(__instance.scaleFactor, __instance.playerOffsetTransform);
            }
        }
    }
}
