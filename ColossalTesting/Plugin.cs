﻿using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using GorillaTagScripts;
using GorillaNetworking;
using ColossalTesting.Patches;
using HarmonyLib;
using System;
using Photon.Realtime;
using UnityEngine.UIElements;
using PlayFab.ClientModels;
using PlayFab;
using System.Reflection;
using Oculus.Interaction.PoseDetection;
using Valve.VR.InteractionSystem;
using UnityEngine.AI;
using GorillaGameModes;
using ColossalTesting.Util;
using UnityEngine.UI;
using static GorillaTagCompetitiveServerApi;

namespace ColossalTesting
{
    public static class ToggleManager
    {
        public static bool LagAll { get; set; } = false;
    }

    public class ToggleData
    {
        public string Label { get; private set; }
        public bool Value { get => getter(); set => setter(value); }
        private System.Func<bool> getter;
        private System.Action<bool> setter;

        public ToggleData(string label, System.Func<bool> getter, System.Action<bool> setter)
        {
            Label = label;
            this.getter = getter;
            this.setter = setter;
        }
    }

    public class ButtonData
    {
        public string Label { get; private set; }
        public System.Action OnClick { get; private set; }

        public ButtonData(string label, System.Action onClick)
        {
            Label = label;
            OnClick = onClick;
        }
    }

    public class Plugin : MonoBehaviourPunCallbacks
    {
        private Vector2 scrollPosition = Vector2.zero;
        private List<ToggleData> toggles;
        private List<ButtonData> buttons;
        private Rect windowRect = new Rect(10, 10, 250, 300);

        private GUIStyle titleStyle;
        private GUIStyle toggleStyle;
        private GUIStyle buttonStyle;
        private GUIStyle verticalScrollStyle;
        private GUIStyle verticalScrollThumbStyle;
        private bool stylesInitialized = false;

        private readonly Color32 purpleColor = new Color32(202, 2, 247, 255);

        private void OnPlayFabLoginSuccess(LoginResult result)
        {
            PhotonNetwork.AuthValues = new AuthenticationValues
            {
                AuthType = CustomAuthenticationType.Custom,
                UserId = result.PlayFabId,
            };
            PhotonNetwork.ConnectUsingSettings();
        }
        private void OnPlayFabLoginFailure(PlayFabError error)
        {
            Debug.LogError($"PlayFab Login Failed: {error.ErrorMessage}");
        }
        private void Awake()
        {
            toggles = new List<ToggleData>
            {
                new ToggleData("Lag All", () => ToggleManager.LagAll, value => ToggleManager.LagAll = value)
            };

            buttons = new List<ButtonData>
            {
                new ButtonData("Disconnect", () => PhotonNetwork.Disconnect()),


                new ButtonData("Dev Server", () =>
                {
                    GameObject.Destroy(GameObject.Find("LegalAgreementCheck"), 0f);
                    GameObject.Destroy(GameObject.Find("LegalAgreements"), 0f);
                    GameObject.Destroy(GameObject.Find("UIParent"), 0f);

                    GorillaComputer.instance.UpdateFailureText("AUTHED ONTO DEV SERVER");

                    //// LHAX
                    //PlayFabSettings.TitleId = "1D5C7";
                    //Photon.Pun.PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice = "50ca02be-3f9b-4dfd-b36c-525d941f899e";
                    //Photon.Pun.PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "50807729-dc79-4cef-9ae5-5ae8c81ca063";
                    //PhotonNetwork.AuthValues = new AuthenticationValues();
                    //PhotonNetwork.AuthValues.UserId = "F03C468C9CE0CFD8";
                    //PhotonNetwork.ConnectUsingSettings();

                    //// Colossal
                    //PlayFabSettings.TitleId = "178627";
                    PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "66a98405-3b0c-4da8-bcdf-1fb1d0b76525";
                    PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice = "0a036bf3-31d8-4192-a866-f4602453c95f";
                    PhotonNetwork.AuthValues = null; // Anonymous connection
                    PhotonNetwork.LogLevel = PunLogLevel.Full;
                    PhotonNetwork.ConnectUsingSettings();
                }),
                new ButtonData("Join Dev Room", () =>
                {
                    Hashtable hash = new Hashtable();
                    hash.Add("joinedGameMode", "publicDEFAULTInfection");
                    hash.Add("gameMode", "publicDEFAULTInfection");

                    string[] hash2 =
                    {
                        "joinedGameMode=publicDEFAULTInfection",
                        "gameMode=publicDEFAULTInfection"
                    };

                    RoomOptions options = new RoomOptions();
                    options.IsOpen = true;
                    options.IsVisible = true;
                    options.MaxPlayers = 10;
                    options.PublishUserId = true;
                    options.SuppressPlayerInfo = false;
                    options.CustomRoomProperties = hash;
                    options.CustomRoomPropertiesForLobby = hash2;
                    PhotonNetwork.JoinOrCreateRoom("YTTV", options, null, null);
                }),


                new ButtonData("Send Report", () => GorillaNot.instance.SendReport("Nigger", "Nigger", "Nigger")),
                new ButtonData("Send Raise Event", () =>
                {
                    WebFlags flags = new WebFlags(0);
                    flags.HttpForward = true;
                    RaiseEventOptions options = new RaiseEventOptions
                    {
                        Receivers = ReceiverGroup.All,
                        CachingOption = EventCaching.AddToRoomCache,
                        Flags = flags
                    };
                    PhotonNetwork.RaiseEvent(0, null, options, SendOptions.SendReliable);
                }),
                new ButtonData("Send Cloud Script", () =>
                {
                    var request = new ExecuteCloudScriptRequest
                    {
                        FunctionName = "Nigger",
                        FunctionParameter = new { Data = "Nigger" },
                        GeneratePlayStreamEvent = true // Optional: Log to PlayFab playstream
                    };

                    PlayFabClientAPI.ExecuteCloudScript(request,
                        result => Debug.Log("CloudScript executed: " + JsonUtility.ToJson(result)),
                        error => Debug.LogError("CloudScript error: " + error.ErrorMessage));
                }),
                new ButtonData("Set Room Custom", () =>
                {
                    ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
                    {
                        { "Nigger", "Nigger" }
                    };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(props, null);
                }),
                new ButtonData("Log Room Info", () =>
                {
                    if (!PhotonNetwork.InRoom)
                    {
                        Debug.LogError("Not currently in a room");
                        return;
                    }

                    Hashtable customProps = PhotonNetwork.CurrentRoom.CustomProperties;
                    if (customProps != null && customProps.Count != 0)
                    {
                        Debug.Log("Custom Properties:");
                        foreach (var key in customProps.Keys)
                        {
                            Debug.Log($"Key: {key}, Value: {customProps[key]}");
                        }
                    }

                    Debug.Log($"Gamemode: {GorillaComputer.instance.currentGameMode}");

                    foreach(Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                    {
                        Debug.Log(player.NickName + " " + player.UserId);
                    }
                }),


                new ButtonData("Unmute Self", () =>
                {
                    GorillaTagger.moderationMutedTime = -1;
                }),
                new ButtonData("Get All Comp Stats", () =>
                {
                    List<string> playfabIds = new List<string>();
                    foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
                    {
                        if (!string.IsNullOrEmpty(p.UserId))
                        {
                            playfabIds.Add(p.UserId);
                            Debug.Log($"Added player ID: {p.UserId}");
                        }
                    }

                    if (playfabIds.Count == 0)
                    {
                        Debug.LogWarning("No players found in PhotonNetwork.PlayerList");
                        return;
                    }

                    // Make a single API call with all player IDs
                    GorillaTagCompetitiveServerApi.Instance.RequestGetRankInformation(
                        playfabIds,
                        (RankedModeProgressionData data) =>
                        {
                            if (data == null || data.playerData == null)
                            {
                                Debug.LogWarning("No rank data received from server");
                                return;
                            }

                            // Log all players' data
                            Debug.Log($"Received data for {data.playerData.Count} players");
                            foreach (var player in data.playerData)
                            {
                                Debug.Log($"PlayfabID: {player.playfabID}");
                                foreach (var platform in player.platformData)
                                {
                                    Debug.Log($"Platform: {platform.platform}, Elo: {platform.elo}, MajorTier: {platform.majorTier}, MinorTier: {platform.minorTier}, RankProgress: {platform.rankProgress}");
                                }
                            }
                        });
                }),
                new ButtonData("Deregister Scoreboard", () =>
                {
                    GorillaTagCompetitiveManager.DeregisterScoreboard(null);
                }),
                new ButtonData("Deregister Scoreboard", () =>
                {
                    GorillaTagCompetitiveManager.DeregisterScoreboard(null);
                }),
                new ButtonData("Deregister Scoreboard", () =>
                {
                    
                }),
            };
        }

        private void InitializeStyles()
        {
            titleStyle = new GUIStyle
            {
                fontSize = 24,
                normal = { textColor = Color.white },
                alignment = TextAnchor.MiddleCenter,
                padding = new RectOffset(0, 0, 10, 20)
            };

            toggleStyle = new GUIStyle
            {
                normal = { textColor = Color.white },
                active = { textColor = purpleColor },
                hover = { textColor = Color.white },
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(0, 0, 5, 5)
            };

            buttonStyle = new GUIStyle(GUI.skin.button)
            {
                normal = { textColor = Color.white, background = null },
                active = { textColor = purpleColor },
                hover = { textColor = Color.white },
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(0, 0, 5, 5)
            };

            verticalScrollStyle = new GUIStyle(GUI.skin.verticalScrollbar)
            {
                normal = { background = null },
                fixedWidth = 0
            };

            verticalScrollThumbStyle = new GUIStyle(GUI.skin.verticalScrollbarThumb)
            {
                normal = { background = null },
                fixedWidth = 0
            };

            stylesInitialized = true;
        }

        private void OnGUI()
        {
            if (!stylesInitialized)
            {
                InitializeStyles();
            }

            GUI.backgroundColor = new Color32(43, 19, 56, 230);
            GUI.color = purpleColor;

            windowRect = GUI.Window(0, windowRect, DrawWindow, "", GUI.skin.box);

            windowRect.x = Mathf.Clamp(windowRect.x, 0, Screen.width - windowRect.width);
            windowRect.y = Mathf.Clamp(windowRect.y, 0, Screen.height - windowRect.height);
        }

        private void DrawWindow(int windowID)
        {
            GUI.DragWindow(new Rect(0, 0, 250, 30));

            scrollPosition = GUILayout.BeginScrollView(scrollPosition,
                false,
                true,
                GUIStyle.none,
                verticalScrollStyle,
                GUILayout.Width(250),
                GUILayout.Height(300));

            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            GUILayout.Label("Colossal", titleStyle);
            if (PhotonNetwork.InRoom)
                GUILayout.Label(PhotonNetwork.MasterClient.NickName, titleStyle);
            GUILayout.Label(PhotonNetwork.NetworkClientState.ToString(), titleStyle);

            foreach (var toggle in toggles)
            {
                GUI.backgroundColor = Color.clear;
                GUIStyle currentStyle = new GUIStyle(toggleStyle);
                currentStyle.normal.textColor = toggle.Value ? (Color)purpleColor : Color.white;
                if (GUILayout.Button(toggle.Label, currentStyle, GUILayout.ExpandWidth(true)))
                {
                    toggle.Value = !toggle.Value;
                }
            }

            foreach (var button in buttons)
            {
                GUI.backgroundColor = Color.clear;
                if (GUILayout.Button(button.Label, buttonStyle, GUILayout.ExpandWidth(true)))
                {
                    button.OnClick?.Invoke();
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            GUILayout.EndScrollView();
        }

        private byte[] giantPayload;
        private void Update()
        {
            if (ToggleManager.LagAll)
            {
            }
        }
    }
}