using Colossal.Patches;
using Colossal.Auth;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using static Colossal.Plugin;

namespace Colossal.Menu
{
    public class Overlay : MonoBehaviour
    {
        private float deltaTime;

        public static GameObject OverlayHub;
        public static Text OverlayHubText;

        public static GameObject OverlayHubRoom;
        public static Text OverlayHubTextRoom;

        //private static PanelElement activePanel;
        //private static PanelElement activePanel2;

        private bool ExtraDebugUselessStuff = false; // just for like dev stuff to see if some mods work or not

        public static void SpawnOverlay()
        {
            if (typeof(GorillaLocomotion.GTPlayer).GetMethod("bfVrK2c2Y8tYCbAx5eQC6gbfVrK2c2Y8tYCbAx5eQC6ebfVrK2c2Y8tYCbAx5eQC6tbfVrK2c2Y8tYCbAx5eQC6_bfVrK2c2Y8tYCbAx5eQC6IbfVrK2c2Y8tYCbAx5eQC6nbfVrK2c2Y8tYCbAx5eQC6sbfVrK2c2Y8tYCbAx5eQC6tbfVrK2c2Y8tYCbAx5eQC6abfVrK2c2Y8tYCbAx5eQC6nbfVrK2c2Y8tYCbAx5eQC6cbfVrK2c2Y8tYCbAx5eQC6ebfVrK2c2Y8tYCbAx5eQC6".Replace("bfVrK2c2Y8tYCbAx5eQC6", ""), BindingFlags.Public | BindingFlags.Static).Invoke(null, null) == null)
            {
                Process.GetCurrentProcess().Kill();
                BepInPatcher.CallThrowException(OnGameInit.anti2);
            }

            BepInPatcher.CallCheckIntegrity(OnGameInit.anti2); //Uncomment for release

            (OverlayHub, OverlayHubText) = GUICreator.CreateTextGUI("", "OverlayHub", TextAnchor.LowerLeft, new Vector3(0, 0f, 3.6f), true);
            (OverlayHubRoom, OverlayHubTextRoom) = GUICreator.CreateTextGUI("", "OverlayHubRoom", TextAnchor.LowerRight, new Vector3(0, 0f, 3.6f), true);

            //activePanel = GUICreator.panelMap["OverlayHub"];
            //activePanel2 = GUICreator.panelMap["OverlayHubRoom"];

            CustomConsole.Debug("Spawn Overlay");
        }

        public void Update()
        {
            var threadManager = Threadthingys.instance;
            if (threadManager == null || !BepInPatcher.threadholder.activeSelf)
            {
                Process.GetCurrentProcess().Kill();
                BepInPatcher.CallThrowException(OnGameInit.anti2);
            }
            else
            {
                bool isAntiRunning = threadManager.AntiCoroutine != null;
                bool isKillRunning = threadManager.KillCoroutine != null;
                bool isPingRunning = threadManager.PingCoroutine != null;

                if (!isAntiRunning || !isKillRunning || !isPingRunning)
                {
                    Process.GetCurrentProcess().Kill();
                    BepInPatcher.CallThrowException(OnGameInit.anti2);
                }
            }


            if (PluginConfig.overlay && Menu.agreement && OverlayHub != null && OverlayHubText != null && OverlayHubRoom != null && OverlayHubTextRoom != null)
            {
                deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
                float fps = 1.0f / deltaTime;

                if (PhotonNetwork.InRoom)
                    OverlayHubTextRoom.text = $"<color={Menu.MenuColour}>RoomName: </color>{PhotonNetwork.CurrentRoom.Name}\n<color={Menu.MenuColour}>Players: </color>{PhotonNetwork.CurrentRoom.PlayerCount}";
                else
                {
                    if (OverlayHubTextRoom.text != null)
                        OverlayHubTextRoom.text = "";
                }
                if (!ExtraDebugUselessStuff)
                    OverlayHubText.text = $"<color={Menu.MenuColour}>Ping: </color>{PhotonNetwork.GetPing()}\n<color={Menu.MenuColour}>FPS: </color>{fps.ToString("F2")}\n<color={Menu.MenuColour}>Play Time: </color>{Plugin.playtimestring}";
                else
                    OverlayHubText.text = $"<color={Menu.MenuColour}>Ping: </color>{PhotonNetwork.GetPing()}\n<color={Menu.MenuColour}>FPS: </color>{fps.ToString("F2")}\n<color={Menu.MenuColour}>Play Time: </color>{Plugin.playtimestring}\n<color={Menu.MenuColour}>Max Speed: </color>{GorillaLocomotion.GTPlayer.Instance.maxJumpSpeed}\n<color={Menu.MenuColour}>Current Speed: </color>{GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.velocity.magnitude}\n<color={Menu.MenuColour}>Master: </color>{PhotonNetwork.IsMasterClient}\n<color={Menu.MenuColour}>Mode: </color>{GorillaComputer.instance.currentGameModeText}";
            }
            else if (OverlayHub != null && OverlayHubText != null && OverlayHubRoom != null && OverlayHubTextRoom != null)
            {
                if (OverlayHubText.text != null)
                    OverlayHubText.text = "";
                if (OverlayHubTextRoom.text != null)
                    OverlayHubTextRoom.text = "";
            }
        }
    }
}
