﻿using BepInEx;
using Colossal.Patches;
using Colossal.Auth;
using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static GorillaTagCompetitiveServerApi;
using Colossal.Mods;

namespace Colossal.Menu
{
    internal class ToolTips : MonoBehaviour
    {
        public static string[] MainMenutips = new string[]
        {
            $"<color={Menu.MenuColour}>Submenu</color>\nMovement mods",
            $"<color={Menu.MenuColour}>Submenu</color>\nVisual mods",
            $"<color={Menu.MenuColour}>Submenu</color>\nPlayer mods",
            $"<color={Menu.MenuColour}>Submenu</color>\nComputer mods",
            $"<color={Menu.MenuColour}>Submenu</color>\nExploit mods",
            $"<color={Menu.MenuColour}>Submenu</color>\nSafety mods",
            $"<color={Menu.MenuColour}>Submenu</color>\nMusic Player",
            $"<color={Menu.MenuColour}>Submenu</color>\nSettings",
            $"<color={Menu.MenuColour}>Submenu</color>\nInformation",
            $"<color={Menu.MenuColour}>Submenu</color>\nComing Soon",
            $"<color={Menu.MenuColour}>Passive</color>\nToggles noti",
            $"<color={Menu.MenuColour}>Passive</color>\nToggles overlay",
            $"<color={Menu.MenuColour}>Passive</color>\nToggles tooltips",
        };

        public static string[] Movementtips = new string[]
        {
            $"<color={Menu.MenuColour}>Custom</color>\nFly Like IronMan",
            $"<color={Menu.MenuColour}>L Secondary & Custom</color>\nFly in your right hands direction",
            $"<color={Menu.MenuColour}>Custom</color>\nPoint palms towards walls to stick",
            $"<color={Menu.MenuColour}>Submenu</color>\nDisplays Speed Options",
            $"<color={Menu.MenuColour}>Custom</color>\nJump on air",
            $"<color={Menu.MenuColour}>Passive</color>\nFlip upside down",
            $"<color={Menu.MenuColour}>Custom</color>\nSwim in air",
            $"<color={Menu.MenuColour}>R Joystick > L Trigger & R Trigger</color>\nScale the world",
            $"<color={Menu.MenuColour}>Passive</color>\nConstantly spins your ss rig",
            $"<color={Menu.MenuColour}>W & A & S & D</color>\nFly when not in vr",
        };
        public static string[] Movement2tips = new string[]
        {
            $"<color={Menu.MenuColour}>Passive</color>\nSpeed up time",
            $"<color={Menu.MenuColour}>Custom</color>\nScale gravity",
            $"<color={Menu.MenuColour}>L Or R Grip</color>\nClimb Gorillas",
            $"<color={Menu.MenuColour}>Passive</color>\nFly away from tagged players",
            $"<color={Menu.MenuColour}>R Joystick > L Trigger & R Trigger</color>\nScale yourself",
            $"<color={Menu.MenuColour}>Custom</color>\nPhase through walls",
            $"<color={Menu.MenuColour}>Passive</color>\nMakes you unable to move",
            $"<color={Menu.MenuColour}>Button</color>\nTeleports to random player",
            $"<color={Menu.MenuColour}>Passive</color>\nChanges your movement like how different hz would",
            $"<color={Menu.MenuColour}>Custom</color>\nThrow yourself by swinging your arms",
            $"<color={Menu.MenuColour}>Submenu</color>\nStrafe Options",
            $"<color={Menu.MenuColour}>L Joystick & R Joystick</color>\nFly with your joystick movements",
        };
        public static string[] Speedtips = new string[]
        {
            $"<color={Menu.MenuColour}>Passive</color>\nAdds a speed boost",
            $"<color={Menu.MenuColour}>Custom</color>\nAdds a speed boost",
            $"<color={Menu.MenuColour}>Custom</color>\nAdds a speed boost when near infected",
            $"<color={Menu.MenuColour}>Passive</color>\nAdds a speed boost when near infected",
        };
        public static string[] Strafetips = new string[]
        {
            $"<color={Menu.MenuColour}>Custom</color>\nDifferent strafe modes",
            $"<color={Menu.MenuColour}>Setting</color>\nStrafe speed amount",
            $"<color={Menu.MenuColour}>Setting</color>\nStrafe jump amount",
        };

        public static string[] Visualtips = new string[]
        {
            $"<color={Menu.MenuColour}>Passive</color>\nHighlight monkies and ghosts through walls",
            $"<color={Menu.MenuColour}>Passive</color>\nA filled box you can see through walls",
            $"<color={Menu.MenuColour}>Passive</color>\nA box you can see through walls",
            $"<color={Menu.MenuColour}>Passive</color>\nShows the skeleton of monkies through walls",
            $"<color={Menu.MenuColour}>Passive</color>\nDraws lines towards monkies",
            $"<color={Menu.MenuColour}>Submenu</color>\nShow nametag settings",
            $"<color={Menu.MenuColour}>Passive</color>\nShows how far away the nearest infected is",
            $"<color={Menu.MenuColour}>Passive</color>\nMakes everything max brightness",
            $"<color={Menu.MenuColour}>Passive</color>\nChange the sky colour",
            $"<color={Menu.MenuColour}>Passive</color>\nMake everyone look at you",
        };
        public static string[] Visual2tips = new string[]
        {
            $"<color={Menu.MenuColour}>Passive</color>\nPlays splashing effects",
            $"<color={Menu.MenuColour}>Passive</color>\nRemoves all leaves in forest",
            $"<color={Menu.MenuColour}>Passive</color>\nDisplays a cool comic visual when tagging someone",
            $"<color={Menu.MenuColour}>Passive</color>\nHides all traces on the select view",
            $"<color={Menu.MenuColour}>Passive</color>\nAllows you to see other CCM users",
            $"<color={Menu.MenuColour}>Passive</color>\nShows the custom boards",
        };
        public static string[] Tracers = new string[]
        {
            $"<color={Menu.MenuColour}>Passive</color>\nPosition of tracers",
            $"<color={Menu.MenuColour}>Passive</color>\nSize of tracers",
        };
        public static string[] Nametags = new string[]
        {
            $"<color={Menu.MenuColour}>Passive</color>\nTurn nametags on and off",
            $"<color={Menu.MenuColour}>Passive</color>\nShow account creation date of other players",
            $"<color={Menu.MenuColour}>Passive</color>\nShow other players colour code",
            $"<color={Menu.MenuColour}>Passive</color>\nShow distance to other players",
            $"<color={Menu.MenuColour}>Passive</color>\nShow nametags through walls",
            $"<color={Menu.MenuColour}>Passive</color>\nShow current fps of other players",
            $"<color={Menu.MenuColour}>Setting</color>\nThe height the nametag should be",
            $"<color={Menu.MenuColour}>Setting</color>\nThe size the nametag should be",
            $"<color={Menu.MenuColour}>Setting</color>\nThe colour the nametag should be",
        };

        public static string[] Playertips = new string[]
        {
            $"<color={Menu.MenuColour}>Passive</color>\nRemoves hand animations",
            $"<color={Menu.MenuColour}>Custom</color>\nTag with a gun",
            $"<color={Menu.MenuColour}>L Trigger & R Trigger</color>\nPoints and looks at monkies",
            $"<color={Menu.MenuColour}>Custom</color>\nLocks rig in place",
            $"<color={Menu.MenuColour}>Custom</color>\nGo invis",
            $"<color={Menu.MenuColour}>Passive</color>\nAutomatically tags nearest monkey",
            $"<color={Menu.MenuColour}>Passive</color>\nTags all monkies",
            $"<color={Menu.MenuColour}>Passive</color>\nDesyncs hitbox and visual position on server",
            $"<color={Menu.MenuColour}>Passive</color>\nIncreases how far you can tag from",
            $"<color={Menu.MenuColour}>Passive</color>\nRemoves wind guards",
            $"<color={Menu.MenuColour}>Passive</color>\nMakes guardian unable to pick you up",
            $"<color={Menu.MenuColour}>Passive</color>\nChanges your name to names from a file",
        };
        public static string[] Playertips2 = new string[]
        {
            $"<color={Menu.MenuColour}>Passive</color>\nDesyncs your head and body rotations",
            $"<color={Menu.MenuColour}>Passive</color>\nMakes your colour rainbow for everyone",
            $"<color={Menu.MenuColour}>Passive</color>\nPlays bad apple through your mic & sets your colour to the video",
            $"<color={Menu.MenuColour}>Passive</color>\nAutomatically aims at players in paintbrawl",
            $"<color={Menu.MenuColour}>Passive</color>\nMakes you impossible to tag",
            $"<color={Menu.MenuColour}>Passive</color>\nFakes lag",
            $"<color={Menu.MenuColour}>Passive</color>\nDisables the doors in ghost reactor",
            $"<color={Menu.MenuColour}>Passive</color>\nGives you a braclet that changes colours for everyone",
            $"<color={Menu.MenuColour}>Passive</color>\nKills yourself and turns you into a ghost",
            $"<color={Menu.MenuColour}>Passive</color>\nRevives you from being a ghost",
        };

        public static string[] Exploittips = new string[]
        {
            $"<color={Menu.MenuColour}>Passive</color>\nBreaks nametag mods",
            $"<color={Menu.MenuColour}>R Trigger (Other Player: Fist)</color>\nGives selected players platforms",
            $"<color={Menu.MenuColour}>Passive</color>\nLags everyone",
            $"<color={Menu.MenuColour}>SubMenu</color>\nDifferent cosmetic options",
            $"<color={Menu.MenuColour}>Passive</color>\nFreezes everyone (takes a while)",
            $"<color={Menu.MenuColour}>Passive</color>\nForces guardian",
            $"<color={Menu.MenuColour}>Passive</color>\nTeleports you to attempt to get guardian",
            $"<color={Menu.MenuColour}>Passive</color>\nGrabs everyone",
            $"<color={Menu.MenuColour}>Passive</color>\nKills everyones game",
        };
        public static string[] Exploit2tips = new string[]
        {
            $"<color={Menu.MenuColour}>Custom</color>\nA gun that launches giant snowballs",
            $"<color={Menu.MenuColour}>Passive</color>\nGives quest badge 99999",
            $"<color={Menu.MenuColour}>Passive</color>\nLags everyone",
            $"<color={Menu.MenuColour}>Passive</color>\nMakes room private (fixes kick all)",
            $"<color={Menu.MenuColour}>Passive</color>\nMakes room public (kicks people when they enter join triggers)",
            $"<color={Menu.MenuColour}>Passive</color>\nPrevents anyone from joining the room by instantly kicking them",
            $"<color={Menu.MenuColour}>Passive</color>\nBroadcasts the room",
            $"<color={Menu.MenuColour}>Passive</color>\nPermently breaks the room until everyone leaves",
            $"<color={Menu.MenuColour}>Passive</color>\nAttempts to kick everyone",
            $"<color={Menu.MenuColour}>Passive</color>\nMakes your name KKK on the leaderboard",
            $"<color={Menu.MenuColour}>Passive</color>\nSpams particles",
            $"<color={Menu.MenuColour}>Passive</color>\nSpams all rope positions",
        };

        public static string[] Computertips = new string[]
        {
            $"<color={Menu.MenuColour}>Passive</color>\nDisconnects from room",
            $"<color={Menu.MenuColour}>Passive</color>\nJoins code GTC",
            $"<color={Menu.MenuColour}>Passive</color>\nJoins code TTT",
            $"<color={Menu.MenuColour}>Passive</color>\nJoins code YTTV",
            $"<color={Menu.MenuColour}>Passive</color>\nJoins code MODS",
            $"<color={Menu.MenuColour}>Passive</color>\nJoins code MOD",
            $"<color={Menu.MenuColour}>Passive</color>\nJoins custom code",
            $"<color={Menu.MenuColour}>Passive</color>\nJoins a CCMV2 only code",
        };

        public static string[] Safetytips = new string[]
        {
            $"<color={Menu.MenuColour}>All Face Buttons</color>\nDisables Everything",
            $"<color={Menu.MenuColour}>Passive</color>\nAnti Report",
            $"<color={Menu.MenuColour}>Passive</color>\nRandomly changes name",
            $"<color={Menu.MenuColour}>Passive</color>\nDisables Igloo to pass a PC check",
            $"<color={Menu.MenuColour}>Passive</color>\nFakes having your quest menu open",
            $"<color={Menu.MenuColour}>Passive</color>\nFakes having your report menu open",
        };

        public static string[] Settingstips = new string[]
        {
            $"<color={Menu.MenuColour}>Submenu</color>\nMenu Colour options",
            $"<color={Menu.MenuColour}>Passive</color>\nMenu position",
            $"<color={Menu.MenuColour}>Passive</color>\nConfig to load",
            $"<color={Menu.MenuColour}>Passive</color>\nLoad selected config",
            $"<color={Menu.MenuColour}>Passive</color>\nSave menu settings",
            $"<color={Menu.MenuColour}>Passive</color>\nLogs all player info in every room into a database",
            $"<color={Menu.MenuColour}>Passive</color>\nSets menu control scheme to inverted",
            $"<color={Menu.MenuColour}>Passive</color>\nLogs out of your Colossal account",
        };
        public static string[] SettingsColourtips = new string[]
        {
            $"<color={Menu.MenuColour}>Passive</color>\nGUI colour",
            $"<color={Menu.MenuColour}>Passive</color>\nExtra rig colour",
            $"<color={Menu.MenuColour}>Passive</color>\nTagging beam colour",
            $"<color={Menu.MenuColour}>Passive</color>\nESP colour",
            $"<color={Menu.MenuColour}>Passive</color>\nExtra rig opacity",
            $"<color={Menu.MenuColour}>Passive</color>\nHitboxes opacity",
            $"<color={Menu.MenuColour}>Passive</color>\nHitboxes colour",
            $"<color={Menu.MenuColour}>Passive</color>\nPlatforms colour",
            $"<color={Menu.MenuColour}>Passive</color>\nTarget Indicator colour",
        };

        public static string[] Musictips = new string[]
        {
            $"<color={Menu.MenuColour}>Passive</color>\nSelected music",
            $"<color={Menu.MenuColour}>Passive</color>\nPlays the selected music",
            $"<color={Menu.MenuColour}>Passive</color>\nStops selected music",
            $"<color={Menu.MenuColour}>Passive</color>\nLoops selected music",
            $"<color={Menu.MenuColour}>Passive</color>\nLets everyone else hear it",
            $"<color={Menu.MenuColour}>Passive</color>\nVolume of the music",
        };

        public static string[] Info
        {
            get
            {
                return new string[]
                {
                    $"<color={Menu.MenuColour}>PlayerList</color>\n{playerinfo}",
                    $"<color={Menu.MenuColour}>GTC Codes</color>\n{Threadthingys.GTCCodeInfo}",
                    $"<color={Menu.MenuColour}>IIDK Users</color>\n{Threadthingys.IIDKInfo}",
                    $"<color={Menu.MenuColour}>CCMV2 Users</color>\n{BepInPatcher.playercount}",
                };
            }
        }

        public static GameObject HUDObj;
        public static GameObject HUDObj2;
        static GameObject MainCamera;
        static Text Testtext;
        private static TextAnchor textAnchor = TextAnchor.MiddleCenter;
        static Material AlertText = new Material(Shader.Find("GUI/Text Shader"));
        static Text NotifiText;
        private static GameObject TestText;

        public static string playerinfo;

        public void Update()
        {
            var threadManager = Threadthingys.instance;
            var threadManagerV2 = ThreadthingysV2.instance;
            if (threadManager == null || threadManagerV2 == null || !BepInPatcher.threadholder.activeSelf)
            {
                Process.GetCurrentProcess().Kill();
                BepInPatcher.CallThrowException(OnGameInit.anti2);
            }
            else
            {
                bool isAntiRunning = threadManager.AntiCoroutine != null;
                bool isKillRunning = threadManager.KillCoroutine != null;
                bool isPingRunning = threadManager.PingCoroutine != null;

                bool isAntiRunningV2 = threadManagerV2.AntiCoroutine != null;
                bool isKillRunningV2 = threadManagerV2.KillCoroutine != null;
                bool isPingRunningV2 = threadManagerV2.PingCoroutine != null;

                if (!isAntiRunning || !isKillRunning || !isPingRunning || !isAntiRunningV2 || !isKillRunningV2 || !isPingRunningV2)
                {
                    Process.GetCurrentProcess().Kill();
                    BepInPatcher.CallThrowException(OnGameInit.anti2);
                }
            }

            if (HUDObj2 != null)
            {
                AntiScreenShare.SetAntiScreenShareLayer(HUDObj2);

                HUDObj2.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                HUDObj2.transform.LookAt(GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position);
            }
        }

        private static string[] GetTooltipArray(string category)
        {
            switch (category)
            {
                case "Main":
                    return MainMenutips;
                case "Back":
                    return MainMenutips;
                case "Movement":
                    return Movementtips;
                case "Movement2":
                    return Movement2tips;
                case "Speed Options":
                    return Speedtips;
                case "Strafe Options":
                    return Strafetips;
                case "Visual":
                    return Visualtips;
                case "Visual2":
                    return Visual2tips;
                case "NameTags":
                    return Nametags;
                case "Player":
                    return Playertips;
                case "Player2":
                    return Playertips2;
                case "Exploits":
                    return Exploittips;
                case "Exploits2":
                    return Exploit2tips;
                case "Computer":
                    return Computertips;
                case "Safety":
                    return Safetytips;
                case "Settings":
                    return Settingstips;
                case "ColourSettings":
                    return SettingsColourtips;
                case "MusicPlayer":
                    return Musictips;
                case "Info":
                    return Info;
                default:
                    return null;
            }
        }

        public static async void HandToolTips(string category, int selectedIndex)
        {
            if (Menu.GUIToggled && PluginConfig.tooltips)
            {
                if (Menu.agreement)
                {
                    MainCamera = GameObject.Find("Main Camera");
                    if (HUDObj == null)
                    {
                        HUDObj = new GameObject();
                        HUDObj2 = new GameObject();
                        HUDObj2.name = "CLIENT_HUB_TOOLTIP";
                        HUDObj.name = "CLIENT_HUB_TOOLTIP";
                        HUDObj.AddComponent<Canvas>();
                        HUDObj.AddComponent<CanvasScaler>();
                        HUDObj.AddComponent<GraphicRaycaster>();
                        HUDObj.GetComponent<Canvas>().enabled = true;
                        HUDObj.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
                        HUDObj.GetComponent<Canvas>().worldCamera = MainCamera.GetComponent<Camera>();
                        HUDObj.GetComponent<RectTransform>().sizeDelta = new Vector2(5, 5);
                        HUDObj.GetComponent<RectTransform>().position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, MainCamera.transform.position.z);
                        HUDObj2.transform.position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, MainCamera.transform.position.z - 4.6f);
                        HUDObj.transform.parent = HUDObj2.transform;
                        HUDObj.GetComponent<RectTransform>().localPosition = new Vector3(0.3f, 0.2f, 2.2f);
                        var Temp = HUDObj.GetComponent<RectTransform>().rotation.eulerAngles;
                        Temp.y = -270f;
                        HUDObj.transform.localScale = new Vector3(1f, 1f, 1f);
                        HUDObj.GetComponent<RectTransform>().rotation = Quaternion.Euler(Temp);
                    }

                    string[] tooltipArray = GetTooltipArray(category);

                    if (tooltipArray != null && selectedIndex >= 0 && selectedIndex < tooltipArray.Length)
                    {
                        string tooltipText = tooltipArray[selectedIndex];

                        if (!string.IsNullOrWhiteSpace(tooltipText))
                        {
                            if (TestText == null)
                            {
                                TestText = new GameObject();
                                TestText.transform.parent = HUDObj.transform;
                                Testtext = TestText.AddComponent<Text>();
                                Testtext.fontSize = 10;
                                Testtext.font = BepInPatcher.gtagfont;
                                Testtext.rectTransform.sizeDelta = new Vector2(260, 300);
                                Testtext.rectTransform.localScale = new Vector3(0.004f, 0.004f, 0.1f);
                                Testtext.rectTransform.localPosition = new Vector3(2.2f, -0.1f, -0.2f);
                                Testtext.rectTransform.localRotation = Quaternion.Euler(0, 90, 0);
                                Testtext.material = AlertText;
                                NotifiText = Testtext;
                                Testtext.alignment = TextAnchor.MiddleCenter;
                            }

                            Testtext.text = tooltipText;
                        }
                        else
                        {
                            if (TestText != null)
                                Testtext.text = "";
                            else
                                CustomConsole.Error("ToolTip is null");
                        }
                    }
                    else
                    {
                        if (TestText != null)
                            Testtext.text = "";
                    }

                    if (PhotonNetwork.InRoom && category.ToLower().Contains("info"))
                    {
                        await RankedInfo.FetchCompetitiveDataAsync();

                        List<string> playerInfoList = new List<string>();
                        Dictionary<string, string> creationDates = new Dictionary<string, string>();

                        foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                        {
                            if (vrrig == null || vrrig.playerNameVisible == null)
                            {
                                continue;
                            }

                            string userId = vrrig.Creator.UserId;
                            string playerName = vrrig.Creator.NickName.ToUpper().Normalize();
                            if (playerName.Length > 14)
                            {
                                playerName = playerName.Substring(0, 14);
                            }

                            bool isInfected = WhatAmI.IsInfected(vrrig.Creator);
                            string nameColor = isInfected ? "red" : "white";

                            string prefix = "";
                            Dictionary<string, (string displayPrefix, string color)> prefixMapping = new Dictionary<string, (string, string)>
                            {
                                { ThisGuyIsUsingColossal.ccmprefix, ("CCM", "magenta") },
                                { "GC", ("GC", "#216300") },
                                { "BarkVersion", ("BARK", "#5c4417") },
                                { "GrateVersion", ("GRATE", "#6e6e6e") },
                                { "GS", ("GSH", "#808080") },
                                { "TicTacToe", ("TTT", "#b2b500") },
                                { "BananaOS", ("BANANOS", "#ffea00") },
                                { "genesis", ("GENESIS", "#f8ff5d") },
                                { "ORBIT", ("ORBIT", "#ba5dff") },
                                { "VioletPaidUser", ("VIOLET", "#785dff") },
                            };

                            if (vrrig.Creator.GetPlayerRef()?.CustomProperties != null)
                            {
                                foreach (var mapping in prefixMapping)
                                {
                                    if (vrrig.Creator.GetPlayerRef().CustomProperties.ContainsKey(mapping.Key))
                                    {
                                        prefix += $"<color={mapping.Value.color}>[{mapping.Value.displayPrefix}] </color>";
                                    }
                                }
                            }

                            // Fetch creation date
                            string creationDate = creationDates.ContainsKey(userId) ? creationDates[userId] : await CreationDate.GetCreationDateAsync(vrrig);
                            if (creationDate != null)
                            {
                                creationDates[userId] = creationDate;
                            }

                            // Fetch Elo data
                            string eloText = "Elo: N/A";
                            var playerTierData = RankedInfo.GetPlayerTierData();
                            if (playerTierData.TryGetValue(userId, out var tierData))
                            {
                                string pcElo = tierData.pcData != null ? $"PC({(int)tierData.pcData.elo})" : "PC(N/A)";
                                string questElo = tierData.questData != null ? $"Q({(int)tierData.questData.elo})" : "Q(N/A)";
                                eloText = $"{pcElo}{questElo}";
                            }

                            // Get player color
                            Color playerColor = vrrig.playerColor;
                            string colorText = $"{(int)(playerColor.r * 9)}{(int)(playerColor.g * 9)}{(int)(playerColor.b * 9)}";

                            // Combine player info
                            string info = $"{prefix} {creationDate} {colorText} {eloText} <color={nameColor}>{playerName}</color>";
                            playerInfoList.Add(info);
                        }

                        playerinfo = string.Join("\n", playerInfoList);
                    }
                    else
                    {
                        if (playerinfo != "Not In Room")
                        {
                            playerinfo = "Not In Room";
                        }
                    }
                }
            }
            else
            {
                if (TestText != null)
                    Testtext.text = "";
                else
                    CustomConsole.Error("ToolTip is null");
            }
        }
    }
}