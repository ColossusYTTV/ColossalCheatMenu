
using HarmonyLib;
using Photon.Pun;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ColossalTesting.Patches
{
    [HarmonyPatch(typeof(VRRig), "OnDisable")]
    internal class DisableRig
    {
        public static bool Prefix(VRRig __instance)
        {
            return !(__instance == VRRig.LocalRig);
        }
    }

    [HarmonyPatch(typeof(VRRigJobManager), "DeregisterVRRig")]
    public static class DisableRigBypass
    {
        public static bool Prefix(VRRigJobManager __instance, VRRig rig)
        {
            if (rig == GorillaTagger.Instance?.offlineVRRig)
            {
                return false; // Skip for local rig
            }
            return true; // Allow deregistration for other rigs
        }
    }
}