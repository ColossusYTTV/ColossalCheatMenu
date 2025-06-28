//using Colossal.Menu;
//using UnityEngine;
//using HarmonyLib;
//using static Colossal.Plugin;
//using System;
//using System.Collections.Generic;
//using Photon.Pun;

//namespace Colossal.Mods
//{
//    public class Aimbot : MonoBehaviour
//    {
//        // copy pasted from hitboxes!!!!!!! - starry
//        public void Update()
//        {
//            if (PluginConfig.Aimbot == 0)
//            {
//                Destroy(holder.GetComponent<Aimbot>());
//                return;
//            }
//        }
//    }

//    [HarmonyPatch(typeof(Slingshot), "GetLaunchVelocity", MethodType.Normal)]
//    public class AimbotPatch
//    {
//        static bool Prefix(Slingshot __instance, ref Vector3 __result)
//        {
//            switch (PluginConfig.Aimbot)
//            {
//                case 1:
//                    float num = float.PositiveInfinity;
//                    VRRig closestRig = null;

//                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
//                    {
//                        if (!vrrig.isOfflineVRRig)
//                        {
//                            float sqrMagnitude = (vrrig.headMesh.transform.position - __instance.transform.position).sqrMagnitude;
//                            if (sqrMagnitude < num)
//                            {
//                                if (!WhatAmI.IsOnSameTeam(PhotonNetwork.LocalPlayer, vrrig.Creator))
//                                {
//                                    num = sqrMagnitude;
//                                    closestRig = vrrig;
//                                }
//                            }
//                        }
//                    }

//                    if (closestRig != null)
//                    {
//                        Vector3 dir = closestRig.headMesh.transform.position - __instance.transform.position;
//                        __result = dir * 6;
//                        return false;
//                    }
//                    break;

//                case 2:
//                    float num2 = float.PositiveInfinity;
//                    VRRig closestRig2 = null;

//                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
//                    {
//                        if (!vrrig.isOfflineVRRig)
//                        {
//                            float sqrMagnitude = (vrrig.headMesh.transform.position - __instance.transform.position).sqrMagnitude;
//                            if (sqrMagnitude < num2)
//                            {
//                                if (!WhatAmI.IsOnSameTeam(PhotonNetwork.LocalPlayer, vrrig.Creator))
//                                {
//                                    num2 = sqrMagnitude;
//                                    closestRig2 = vrrig;
//                                }
//                            }
//                        }
//                    }

//                    if (closestRig2 != null)
//                    {
//                        Vector3 currentPosition = closestRig2.headMesh.transform.position;
//                        Vector3 currentVelocity = closestRig2.GetComponent<Rigidbody>().velocity;

//                        Vector3 futurePosition = currentPosition + (currentVelocity * 5);
//                        Vector3 dir = futurePosition - __instance.transform.position;

//                        __result = dir * 6;
//                        return false;
//                    }
//                    break;
//            }
//            return true;
//        }
//    }
//}
