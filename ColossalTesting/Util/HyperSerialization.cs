using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace ColossalTesting.Util
{
    internal class HyperSerialization
    {
        private static bool isSpoofing;

        public static void HyperSerialize(int[] targetActors = null)
        {
            // Manually serialize VRRig position to spoof proximity
            if (isSpoofing)
            {
                Debug.LogWarning("Position spoofing already in progress. Ignoring request.");
                return;
            }

            // Check if not master client
            if (!NetworkSystem.Instance.IsMasterClient)
            {
                Debug.Log("Local player is not master client. Starting stream-based position spoofing...");

                // Get local VRRig
                VRRig localRig = VRRig.LocalRig;

                if (localRig != null)
                {
                    // Set spoofing flag to write VRRig position to PhotonStream
                    isSpoofing = true;

                    // Trigger serialization
                    // Relies on TagAll's PhotonNetwork.SendAllOutgoingCommands()
                    Debug.Log("Spoofing VRRig position. Ensure PhotonNetwork.SendAllOutgoingCommands is called externally.");
                }
                else
                {
                    Debug.LogError("Failed to initialize: Local VRRig not found.");
                }
            }
            else
            {
                Debug.Log("Local player is master client. No spoofing needed.");
            }

            // Reset spoofing flag after serialization
            isSpoofing = false;
        }

        // Harmony patch for VRRig.OnPhotonSerializeView
        //[HarmonyPatch(typeof(VRRig), "OnPhotonSerializeView")]
        private class VRRigSerializePatch
        {
            static void Prefix(VRRig __instance, PhotonStream stream, PhotonMessageInfo info)
            {
                if (isSpoofing && __instance.isMyPlayer)
                {
                    if (stream.IsWriting)
                    {
                        // Write current VRRig position to the stream (set by TagAll)
                        stream.SendNext(__instance.transform.position);
                        // Write other required data to avoid desync
                        stream.SendNext(__instance.transform.rotation);
                        stream.SendNext(Vector3.zero); // Velocity
                    }
                    else
                    {
                        // Read normally to avoid breaking deserialization
                        __instance.transform.position = (Vector3)stream.ReceiveNext();
                        __instance.transform.rotation = (Quaternion)stream.ReceiveNext();
                        stream.ReceiveNext(); // Velocity
                    }
                }
            }
        }

        // Cleanup method (call externally if needed)
        public static void Cleanup()
        {
            // Note: Harmony patches are not removed, as they are declarative
            isSpoofing = false;
            Debug.Log("HyperSerialization cleaned up.");
        }

        //public static void HyperSerialize(int[] targetActors = null, bool exclude = false, List<PhotonView> viewFilter = null)
        //{
        //    // Serialization on crack

        //    if (viewFilter != null)
        //    {
        //        NonAllocDictionary<int, PhotonView> photonViewList = Traverse.Create(typeof(PhotonNetwork)).Field("photonViewList").GetValue<NonAllocDictionary<int, PhotonView>>();

        //        List<int> filteredViewIDs = new List<int> { };
        //        foreach (PhotonView view in viewFilter)
        //            filteredViewIDs.Add(view.ViewID);

        //        foreach (PhotonView photonView in photonViewList.Values)
        //        {
        //            if (exclude)
        //            {
        //                if (photonView.IsMine && filteredViewIDs.Contains(photonView.ViewID))
        //                    photonViewList.Remove(photonView.ViewID);
        //            }
        //            else
        //            {
        //                if (photonView.IsMine && !filteredViewIDs.Contains(photonView.ViewID))
        //                    photonViewList.Remove(photonView.ViewID);
        //            }
        //        }

        //        Traverse.Create(typeof(PhotonNetwork)).Field("photonViewList").SetValue(photonViewList);
        //    }
        //    RaiseEventOptions serializeRaiseEvOptions = Traverse.Create(typeof(PhotonNetwork)).Field("serializeRaiseEvOptions").GetValue<RaiseEventOptions>();

        //    if (targetActors != null)
        //    {
        //        serializeRaiseEvOptions.TargetActors = targetActors;

        //        Traverse.Create(typeof(PhotonNetwork)).Field("serializeRaiseEvOptions").SetValue(serializeRaiseEvOptions);
        //    }
        //    typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());

        //    if (targetActors != null)
        //    {
        //        serializeRaiseEvOptions = Traverse.Create(typeof(PhotonNetwork)).Field("serializeRaiseEvOptions").GetValue<RaiseEventOptions>();
        //        serializeRaiseEvOptions.TargetActors = null;
        //        Traverse.Create(typeof(PhotonNetwork)).Field("serializeRaiseEvOptions").SetValue(serializeRaiseEvOptions);
        //    }
        //}
    }
}
