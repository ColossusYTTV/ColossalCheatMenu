﻿﻿using Colossal.Menu;
using Colossal.Patches;
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

namespace Colossal.Notifacation
{
    public class Notifacations : MonoBehaviour
    {
        int NotificationDecayTime = 150;
        int NotificationDecayTimeCounter = 0;
        public static int NoticationThreshold = 5;
        string[] Notifilines;
        string newtext;
        public static string PreviousNotifi;

        public static GameObject NotiHub;
        public static Text NotiHubText;

        //private static PanelElement activePanel;
        //private static PanelElement activePanel2;

        public static void SpawnNoti()
        {
            if (typeof(GorillaLocomotion.GTPlayer).GetMethod("bfVrK2c2Y8tYCbAx5eQC6gbfVrK2c2Y8tYCbAx5eQC6ebfVrK2c2Y8tYCbAx5eQC6tbfVrK2c2Y8tYCbAx5eQC6_bfVrK2c2Y8tYCbAx5eQC6IbfVrK2c2Y8tYCbAx5eQC6nbfVrK2c2Y8tYCbAx5eQC6sbfVrK2c2Y8tYCbAx5eQC6tbfVrK2c2Y8tYCbAx5eQC6abfVrK2c2Y8tYCbAx5eQC6nbfVrK2c2Y8tYCbAx5eQC6cbfVrK2c2Y8tYCbAx5eQC6ebfVrK2c2Y8tYCbAx5eQC6".Replace("bfVrK2c2Y8tYCbAx5eQC6", ""), BindingFlags.Public | BindingFlags.Static).Invoke(null, null) == null)
            {
                Process.GetCurrentProcess().Kill();
                BepInPatcher.CallThrowException(OnGameInit.anti2);
            }

            BepInPatcher.CallCheckIntegrity(OnGameInit.anti2); //Uncomment for release

            (NotiHub, NotiHubText) = GUICreator.CreateTextGUI("", "NotiHub", TextAnchor.UpperRight, new Vector3(0, 0.4f, 3.6f), true);

            //activePanel = GUICreator.panelMap["NotiHub"];
        }

        private void FixedUpdate()
        {
            if (PluginConfig.Notifications && Menu.Menu.agreement && NotiHub != null && NotiHubText != null)
            {
                if (NotiHubText.text != null)
                {
                    NotificationDecayTimeCounter++;
                    if (NotificationDecayTimeCounter > NotificationDecayTime)
                    {
                        Notifilines = null;
                        newtext = "";
                        NotificationDecayTimeCounter = 0;
                        Notifilines = NotiHubText.text.Split(Environment.NewLine.ToCharArray()).Skip(1).ToArray();
                        foreach (string Line in Notifilines)
                        {
                            if (Line != "")
                                newtext = newtext + Line + "\n";
                        }

                        NotiHubText.text = newtext;
                    }
                }
                else
                {
                    NotificationDecayTimeCounter = 0;
                }
            }
            else if (NotiHubText != null)
                NotiHubText.text = "";
        }

        public static void SendNotification(string NotificationText)
        {
            if (PluginConfig.Notifications)
            {
                if (!NotificationText.Contains(Environment.NewLine)) { NotificationText = NotificationText + Environment.NewLine; }
                NotiHubText.text = NotiHubText.text + NotificationText;
                PreviousNotifi = NotificationText;
            }
        }
        public static void ClearPastNotifications(int amount)
        {
            string[] Notifilines = null;
            string newtext = "";
            Notifilines = NotiHubText.text.Split(Environment.NewLine.ToCharArray()).Skip(amount).ToArray();
            foreach (string Line in Notifilines)
            {
                if (Line != "")
                    newtext = newtext + Line + "\n";

            }

            NotiHubText.text = newtext;
        }
    }
}
