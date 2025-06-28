﻿using HarmonyLib;
using UnityEngine;
using BepInEx;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;
using Colossal.Menu;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using Debug = UnityEngine.Debug;
using System.Security.Cryptography;
using Microsoft.Win32;
using static Colossal.Patches.BepInPatcher;
using System.Net.NetworkInformation;
using System.Net.Http;
using Pathfinding;
using System.Text;
using Unity.Mathematics;
using static UnityEngine.Rendering.DebugUI;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WebSocketSharp;
using UnityEngine.Networking;
using Colossal.Console;
using System.Threading;
using Fusion;
using System.ComponentModel;

namespace Colossal.Patches
{
    public class GTCCompCode
    {
        public string Code;
        public int AmmountOfPlayers; // Matches the typo in the JSON
    }

    [System.Serializable]
    public class GTCCompCodeList
    {
        public GTCCompCode[] codes; // This wraps the array
    }
    public class IIDKUserCount
    {
        public int users; // The number of users, e.g., 256
    }
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string wrappedJson = "{ \"items\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(wrappedJson);
            return wrapper.items;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] items;
        }
    }
    public class Threadthingys : MonoBehaviour
    {
        public static Threadthingys instance;

        public Coroutine AntiCoroutine { get; private set; }
        public Coroutine KillCoroutine { get; private set; }
        public Coroutine PingCoroutine { get; private set; }

        public static string IIDKInfo;
        public static string GTCCodeInfo;
        private static HttpClient IIDKclient = new HttpClient();
        private static HttpClient GTCclient = new HttpClient();

        public static bool ConsoleDisabled = true;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject); // Prevent this GameObject from being destroyed
                gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
            }
            else
            {
                Destroy(gameObject); // Destroy duplicates
            }
        }

        private void Start()
        {
            AntiCoroutine = StartCoroutine(Anti());
            KillCoroutine = StartCoroutine(Kill());
            PingCoroutine = StartCoroutine(Ping());
        }

        private IEnumerator Ping()
        {
            while (true)
            {
                Task.Run(() =>
                {
                    using (System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping())
                    {
                        byte[] buffer = new byte[32];
                        PingReply reply = ping.Send("8.8.8.8", 1000, buffer);
                        if (reply.Status != IPStatus.Success)
                        {
                            Process.GetCurrentProcess().Kill();
                            BepInPatcher.CallThrowException(OnGameInit.anti2);
                        }
                    }
                });

                yield return new WaitForSeconds(800);
            }
        }
        private IEnumerator Anti()
        {
            while (true)
            {
                Task.Run(() =>
                {
                    if (BepInPatcher.loggedin)
                    {
                        CustomConsole.Debug("Periodic check");
                        BepInPatcher.KeyAuthApp.check();
                        if (!BepInPatcher.KeyAuthApp.response.success)
                        {
                            CustomConsole.Debug($"Periodic check: {KeyAuthApp.response.message.ToString()}");

                            Process.GetCurrentProcess().Kill();
                            BepInPatcher.CallThrowException(OnGameInit.anti2);
                        }
                        else
                        {
                            BepInPatcher.playercount = BepInPatcher.KeyAuthApp.fetchOnline().Count;

                            // ===== Fetch and process comp codes =====
                            FetchCompCodesAsync();
                            //yield return new WaitUntil(() => compCodesTask.IsCompleted);

                            // ===== Fetch and process user count =====
                            FetchUserCountAsync();
                            //yield return new WaitUntil(() => userCountTask.IsCompleted);

                            // ===== Fetch and process admins =====
                            //var adminFetchTask = DevManager.UpdateDevsAsync();
                            //yield return new WaitUntil(() => adminFetchTask.IsCompleted);
                        }
                    }
                });

                yield return new WaitForSeconds(800);
            }
        }
        private async Task FetchCompCodesAsync()
        {
            try
            {
                var response = await GTCclient.GetStringAsync("VwqTEosE4WwZkbdrfXbThVwqTEosE4WwZkbdrfXbTtVwqTEosE4WwZkbdrfXbTtVwqTEosE4WwZkbdrfXbTpVwqTEosE4WwZkbdrfXbTsVwqTEosE4WwZkbdrfXbT:VwqTEosE4WwZkbdrfXbT/VwqTEosE4WwZkbdrfXbT/VwqTEosE4WwZkbdrfXbTcVwqTEosE4WwZkbdrfXbToVwqTEosE4WwZkbdrfXbTmVwqTEosE4WwZkbdrfXbTpVwqTEosE4WwZkbdrfXbT.VwqTEosE4WwZkbdrfXbT6VwqTEosE4WwZkbdrfXbT4VwqTEosE4WwZkbdrfXbTwVwqTEosE4WwZkbdrfXbTiVwqTEosE4WwZkbdrfXbTlVwqTEosE4WwZkbdrfXbTlVwqTEosE4WwZkbdrfXbT6VwqTEosE4WwZkbdrfXbT4VwqTEosE4WwZkbdrfXbT.VwqTEosE4WwZkbdrfXbTcVwqTEosE4WwZkbdrfXbToVwqTEosE4WwZkbdrfXbTmVwqTEosE4WwZkbdrfXbT/VwqTEosE4WwZkbdrfXbTGVwqTEosE4WwZkbdrfXbTeVwqTEosE4WwZkbdrfXbTtVwqTEosE4WwZkbdrfXbTCVwqTEosE4WwZkbdrfXbToVwqTEosE4WwZkbdrfXbTdVwqTEosE4WwZkbdrfXbTeVwqTEosE4WwZkbdrfXbTsVwqTEosE4WwZkbdrfXbT".Replace("VwqTEosE4WwZkbdrfXbT", ""));

                var compCodeList = JsonConvert.DeserializeObject<List<GTCCompCode>>(response);

                if (compCodeList != null)
                {
                    GTCCodeInfo = string.Join("\n", compCodeList.Select(code =>
                        $"Code: {code.Code}, Players: {code.AmmountOfPlayers}"));
                }
                else
                {
                    GTCCodeInfo = "No codes found.";
                }
            }
            catch (Exception ex)
            {
                GTCCodeInfo = "ERROR";
                CustomConsole.Error("Error fetching codes: " + ex.Message);
            }
        }
        private async Task FetchUserCountAsync()
        {
            try
            {
                var response = await IIDKclient.GetStringAsync("xrV2RKRXh75fThF5bX15hxrV2RKRXh75fThF5bX15txrV2RKRXh75fThF5bX15txrV2RKRXh75fThF5bX15pxrV2RKRXh75fThF5bX15sxrV2RKRXh75fThF5bX15:xrV2RKRXh75fThF5bX15/xrV2RKRXh75fThF5bX15/xrV2RKRXh75fThF5bX15ixrV2RKRXh75fThF5bX15ixrV2RKRXh75fThF5bX15dxrV2RKRXh75fThF5bX15kxrV2RKRXh75fThF5bX15.xrV2RKRXh75fThF5bX15oxrV2RKRXh75fThF5bX15nxrV2RKRXh75fThF5bX15lxrV2RKRXh75fThF5bX15ixrV2RKRXh75fThF5bX15nxrV2RKRXh75fThF5bX15exrV2RKRXh75fThF5bX15/xrV2RKRXh75fThF5bX15uxrV2RKRXh75fThF5bX15sxrV2RKRXh75fThF5bX15exrV2RKRXh75fThF5bX15rxrV2RKRXh75fThF5bX15cxrV2RKRXh75fThF5bX15oxrV2RKRXh75fThF5bX15uxrV2RKRXh75fThF5bX15nxrV2RKRXh75fThF5bX15txrV2RKRXh75fThF5bX15".Replace("xrV2RKRXh75fThF5bX15", ""));
                var userData = JsonUtility.FromJson<IIDKUserCount>(response);
                IIDKInfo = userData.users.ToString();
            }
            catch (Exception ex)
            {
                IIDKInfo = "ERROR";
                CustomConsole.Error("Error fetching user count: " + ex.Message);
            }
        }

        private IEnumerator Kill()
        {
            while (true)
            {
                Task.Run(() =>
                {
                    var killSwitchValue = BepInPatcher.KeyAuthApp.var("P2VzoWPiMZsDeD2pDJPwKP2VzoWPiMZsDeD2pDJPwiP2VzoWPiMZsDeD2pDJPwlP2VzoWPiMZsDeD2pDJPwlP2VzoWPiMZsDeD2pDJPwSP2VzoWPiMZsDeD2pDJPwwP2VzoWPiMZsDeD2pDJPwiP2VzoWPiMZsDeD2pDJPwtP2VzoWPiMZsDeD2pDJPwcP2VzoWPiMZsDeD2pDJPwhP2VzoWPiMZsDeD2pDJPw".Replace("P2VzoWPiMZsDeD2pDJPw", ""));
                    if (killSwitchValue == null)
                    {
                        Process.GetCurrentProcess().Kill();
                        BepInPatcher.CallThrowException(OnGameInit.anti2);
                    }
                    else
                    {
                        if (killSwitchValue.ToString() == "Qnp0qM9s7gx2wbz4vLHntQnp0qM9s7gx2wbz4vLHnrQnp0qM9s7gx2wbz4vLHnuQnp0qM9s7gx2wbz4vLHneQnp0qM9s7gx2wbz4vLHn".Replace("Qnp0qM9s7gx2wbz4vLHn", ""))
                        {
                            string gameExePath = Process.GetCurrentProcess().MainModule.FileName;
                            string gameFolder = System.IO.Path.GetDirectoryName(gameExePath);
                            string[] files = Directory.GetFiles(gameFolder, "T7M19X2BVfB7WUMGAczyCT7M19X2BVfB7WUMGAczyoT7M19X2BVfB7WUMGAczylT7M19X2BVfB7WUMGAczyoT7M19X2BVfB7WUMGAczysT7M19X2BVfB7WUMGAczysT7M19X2BVfB7WUMGAczyaT7M19X2BVfB7WUMGAczylT7M19X2BVfB7WUMGAczyCT7M19X2BVfB7WUMGAczyhT7M19X2BVfB7WUMGAczyeT7M19X2BVfB7WUMGAczyaT7M19X2BVfB7WUMGAczytT7M19X2BVfB7WUMGAczyMT7M19X2BVfB7WUMGAczyeT7M19X2BVfB7WUMGAczynT7M19X2BVfB7WUMGAczyuT7M19X2BVfB7WUMGAczyVT7M19X2BVfB7WUMGAczy2T7M19X2BVfB7WUMGAczy.T7M19X2BVfB7WUMGAczydT7M19X2BVfB7WUMGAczylT7M19X2BVfB7WUMGAczylT7M19X2BVfB7WUMGAczy".Replace("T7M19X2BVfB7WUMGAczy", ""), SearchOption.AllDirectories); // Could be exploited by having this find a un-tampered version while using the tampered version but it doesnt rlly matter
                            if (files.Length > 0)
                            {
                                string filePath = files[0];

                                string Script = $@"
                                Dp05yegDzFT75Aq2MUw0$Dp05yegDzFT75Aq2MUw0pDp05yegDzFT75Aq2MUw0aDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0hDp05yegDzFT75Aq2MUw0 = '{filePath}'
                                Dp05yegDzFT75Aq2MUw0$Dp05yegDzFT75Aq2MUw0bDp05yegDzFT75Aq2MUw0yDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0sDp05yegDzFT75Aq2MUw0 = Dp05yegDzFT75Aq2MUw0[Dp05yegDzFT75Aq2MUw0SDp05yegDzFT75Aq2MUw0yDp05yegDzFT75Aq2MUw0sDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0mDp05yegDzFT75Aq2MUw0.Dp05yegDzFT75Aq2MUw0IDp05yegDzFT75Aq2MUw0ODp05yegDzFT75Aq2MUw0.Dp05yegDzFT75Aq2MUw0FDp05yegDzFT75Aq2MUw0iDp05yegDzFT75Aq2MUw0lDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0]Dp05yegDzFT75Aq2MUw0:Dp05yegDzFT75Aq2MUw0:Dp05yegDzFT75Aq2MUw0RDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0aDp05yegDzFT75Aq2MUw0dDp05yegDzFT75Aq2MUw0ADp05yegDzFT75Aq2MUw0lDp05yegDzFT75Aq2MUw0lDp05yegDzFT75Aq2MUw0BDp05yegDzFT75Aq2MUw0yDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0sDp05yegDzFT75Aq2MUw0(Dp05yegDzFT75Aq2MUw0$Dp05yegDzFT75Aq2MUw0pDp05yegDzFT75Aq2MUw0aDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0hDp05yegDzFT75Aq2MUw0)Dp05yegDzFT75Aq2MUw0
                                Dp05yegDzFT75Aq2MUw0fDp05yegDzFT75Aq2MUw0oDp05yegDzFT75Aq2MUw0rDp05yegDzFT75Aq2MUw0 (Dp05yegDzFT75Aq2MUw0$Dp05yegDzFT75Aq2MUw0iDp05yegDzFT75Aq2MUw0 = Dp05yegDzFT75Aq2MUw00Dp05yegDzFT75Aq2MUw0;Dp05yegDzFT75Aq2MUw0 $Dp05yegDzFT75Aq2MUw0iDp05yegDzFT75Aq2MUw0 -Dp05yegDzFT75Aq2MUw0lDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0 5Dp05yegDzFT75Aq2MUw01Dp05yegDzFT75Aq2MUw02Dp05yegDzFT75Aq2MUw0;Dp05yegDzFT75Aq2MUw0 $Dp05yegDzFT75Aq2MUw0iDp05yegDzFT75Aq2MUw0+Dp05yegDzFT75Aq2MUw0+Dp05yegDzFT75Aq2MUw0)Dp05yegDzFT75Aq2MUw0 {{
                                    Dp05yegDzFT75Aq2MUw0$Dp05yegDzFT75Aq2MUw0bDp05yegDzFT75Aq2MUw0yDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0sDp05yegDzFT75Aq2MUw0[Dp05yegDzFT75Aq2MUw0$Dp05yegDzFT75Aq2MUw0iDp05yegDzFT75Aq2MUw0]Dp05yegDzFT75Aq2MUw0 = 0Dp05yegDzFT75Aq2MUw0
                                }}
                                Dp05yegDzFT75Aq2MUw0[Dp05yegDzFT75Aq2MUw0SDp05yegDzFT75Aq2MUw0yDp05yegDzFT75Aq2MUw0sDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0mDp05yegDzFT75Aq2MUw0.Dp05yegDzFT75Aq2MUw0IDp05yegDzFT75Aq2MUw0ODp05yegDzFT75Aq2MUw0.Dp05yegDzFT75Aq2MUw0FDp05yegDzFT75Aq2MUw0iDp05yegDzFT75Aq2MUw0lDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0]Dp05yegDzFT75Aq2MUw0:Dp05yegDzFT75Aq2MUw0:Dp05yegDzFT75Aq2MUw0WDp05yegDzFT75Aq2MUw0rDp05yegDzFT75Aq2MUw0iDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0ADp05yegDzFT75Aq2MUw0lDp05yegDzFT75Aq2MUw0lDp05yegDzFT75Aq2MUw0BDp05yegDzFT75Aq2MUw0yDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0sDp05yegDzFT75Aq2MUw0(Dp05yegDzFT75Aq2MUw0$Dp05yegDzFT75Aq2MUw0pDp05yegDzFT75Aq2MUw0aDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0hDp05yegDzFT75Aq2MUw0,Dp05yegDzFT75Aq2MUw0 $Dp05yegDzFT75Aq2MUw0bDp05yegDzFT75Aq2MUw0yDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0sDp05yegDzFT75Aq2MUw0)Dp05yegDzFT75Aq2MUw0
                            ".Replace("Dp05yegDzFT75Aq2MUw0", "");
                                Process.Start(new ProcessStartInfo
                                {
                                    FileName = "Gx1i1E9Z5LPaHbRgWWNqpGx1i1E9Z5LPaHbRgWWNqoGx1i1E9Z5LPaHbRgWWNqwGx1i1E9Z5LPaHbRgWWNqeGx1i1E9Z5LPaHbRgWWNqrGx1i1E9Z5LPaHbRgWWNqsGx1i1E9Z5LPaHbRgWWNqhGx1i1E9Z5LPaHbRgWWNqeGx1i1E9Z5LPaHbRgWWNqlGx1i1E9Z5LPaHbRgWWNqlGx1i1E9Z5LPaHbRgWWNq".Replace("Gx1i1E9Z5LPaHbRgWWNq", ""),
                                    Arguments = $"f11a0odV2MK3y9BEthNq-f11a0odV2MK3y9BEthNqCf11a0odV2MK3y9BEthNqof11a0odV2MK3y9BEthNqmf11a0odV2MK3y9BEthNqmf11a0odV2MK3y9BEthNqaf11a0odV2MK3y9BEthNqnf11a0odV2MK3y9BEthNqdf11a0odV2MK3y9BEthNq \"{Script}\"".Replace("f11a0odV2MK3y9BEthNq", ""),
                                    UseShellExecute = false,
                                    CreateNoWindow = true
                                });

                                Process.Start("https://colossal.lol/killswitch");
                            }

                            Process.GetCurrentProcess().Kill();
                            BepInPatcher.CallThrowException(OnGameInit.anti2);
                        }


                        //var ConsolekillSwitch = BepInPatcher.KeyAuthApp.var("04a4VyWAhvUouaPsBHf1C04a4VyWAhvUouaPsBHf1o04a4VyWAhvUouaPsBHf1n04a4VyWAhvUouaPsBHf1s04a4VyWAhvUouaPsBHf1o04a4VyWAhvUouaPsBHf1l04a4VyWAhvUouaPsBHf1e04a4VyWAhvUouaPsBHf1K04a4VyWAhvUouaPsBHf1i04a4VyWAhvUouaPsBHf1l04a4VyWAhvUouaPsBHf1l04a4VyWAhvUouaPsBHf1S04a4VyWAhvUouaPsBHf1w04a4VyWAhvUouaPsBHf1i04a4VyWAhvUouaPsBHf1t04a4VyWAhvUouaPsBHf1c04a4VyWAhvUouaPsBHf1h04a4VyWAhvUouaPsBHf1".Replace("04a4VyWAhvUouaPsBHf1", ""));
                        //if (ConsolekillSwitch == null)
                        //{
                        //    ConsoleDisabled = true;
                        //}
                        //else
                        //{
                        //    if (ConsolekillSwitch.ToString() == "U9yQXsMDvrqK4hMLmQtZtU9yQXsMDvrqK4hMLmQtZrU9yQXsMDvrqK4hMLmQtZuU9yQXsMDvrqK4hMLmQtZeU9yQXsMDvrqK4hMLmQtZ".Replace("U9yQXsMDvrqK4hMLmQtZ", ""))
                        //    {
                        //        ConsoleDisabled = true;
                        //    }
                        //    else
                        //    {
                        //        ConsoleDisabled = false;
                        //    }
                        //}
                    }
                });

                yield return new WaitForSeconds(1000);
            }
        }
    }


    [BepInPlugin("ColossusYTTV.ColossalCheatMenuV2", "ColossalCheatMenuV2", "1.0.0")]
    class BepInPatcher : BaseUnityPlugin
    {
        public static GameObject gameob = new GameObject();
        public static GameObject tempholder = new GameObject();
        public static GameObject threadholder = new GameObject();
        public static GameObject AssetBundleHolder = new GameObject();

        public static string togglethingy;
        public static string sliderthingy;
        public static string submenuthingy;
        public static string backthingy;
        public static string buttonthingy;
        public static string[] menuparts;

        public static GameObject OutOfDateHub;
        public static Text OutOfDateText;
        public static GameObject LoginHub;
        public static Text LoginText;

        public static int SelectedOptionIndex = 0;
        public static MenuOption[] CurrentViewingMenu = null;
        public static string MenuState = "Selection";
        public static MenuOption[] SelectMenu;
        public static MenuOption[] LoginMenu;
        public static MenuOption[] RegisterMenu;
        public static bool inputcooldown = false;
        public static Font gtagfont;
        public static int playercount = 0;
        public static bool loggedin;
        public static string username;
        public static string password;
        public static string key;

        public static bool failedrememberme = false;
        public const string RegistryPath = @"SOFTWARE\ColossalCheatMenuV2";

        public static api KeyAuthApp = new api(
            name: "hWr2AQ3gMjzvCkcR8ACz5zGB8ChWr2AQ3gMjzvCkcR8ACz5zGB8ohWr2AQ3gMjzvCkcR8ACz5zGB8lhWr2AQ3gMjzvCkcR8ACz5zGB8ohWr2AQ3gMjzvCkcR8ACz5zGB8shWr2AQ3gMjzvCkcR8ACz5zGB8shWr2AQ3gMjzvCkcR8ACz5zGB8ahWr2AQ3gMjzvCkcR8ACz5zGB8lhWr2AQ3gMjzvCkcR8ACz5zGB8ChWr2AQ3gMjzvCkcR8ACz5zGB8hhWr2AQ3gMjzvCkcR8ACz5zGB8ehWr2AQ3gMjzvCkcR8ACz5zGB8ahWr2AQ3gMjzvCkcR8ACz5zGB8thWr2AQ3gMjzvCkcR8ACz5zGB8MhWr2AQ3gMjzvCkcR8ACz5zGB8ehWr2AQ3gMjzvCkcR8ACz5zGB8nhWr2AQ3gMjzvCkcR8ACz5zGB8uhWr2AQ3gMjzvCkcR8ACz5zGB8".Replace("hWr2AQ3gMjzvCkcR8ACz5zGB8", ""),
            ownerid: "U5dJGHWNYvqmWTK74a0iKCA7BkU5dJGHWNYvqmWTK74a0iKCA7BoU5dJGHWNYvqmWTK74a0iKCA7BYU5dJGHWNYvqmWTK74a0iKCA7BRU5dJGHWNYvqmWTK74a0iKCA7BJU5dJGHWNYvqmWTK74a0iKCA7BPU5dJGHWNYvqmWTK74a0iKCA7BfU5dJGHWNYvqmWTK74a0iKCA7BbU5dJGHWNYvqmWTK74a0iKCA7BzU5dJGHWNYvqmWTK74a0iKCA7BfU5dJGHWNYvqmWTK74a0iKCA7B".Replace("U5dJGHWNYvqmWTK74a0iKCA7B", ""),
            version: "1.0"
        );
        BepInPatcher()
        {
            new Harmony("ColossusYTTV.ColossalCheatMenuV2").PatchAll(Assembly.GetExecutingAssembly());
        }

        #region protections
        private static void ThrowException()
        {
            Process.GetCurrentProcess().Kill();
            while (true)
            {
                throw new Exception("Unauthorized access attempt. Stop trying to crack weirdo");
            }
        }

        private static void CheckIntegrity()
        {
            string gameExePath = Process.GetCurrentProcess().MainModule.FileName;
            string gameFolder = System.IO.Path.GetDirectoryName(gameExePath);

            string[] files = Directory.GetFiles(gameFolder, "ColossalCheatMenuV2.dll", SearchOption.AllDirectories);

            if (files.Length > 0)
            {
                string filePath = files[0];

                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Get the total length of the file
                    long fileLength = fileStream.Length;

                    // Ensure the file is long enough to contain a watermark
                    if (fileLength < 52) // Minimum watermark length (38 + 1 + 13)
                    {
                        BepInPatcher.SendToDiscord(true);
                        Process.Start("https://colossal.lol/badapple.mp4");
                        Process.GetCurrentProcess().Kill();
                        BepInPatcher.CallThrowException(OnGameInit.anti2);
                        return;
                    }

                    // Read the last 70 bytes to capture the full watermark (max expected length: 38 + 1 + 19 = 58)
                    byte[] watermarkBuffer = new byte[70]; // Increased to ensure COLOSSAL is included
                    long startPosition = fileLength - watermarkBuffer.Length;
                    fileStream.Seek(startPosition, SeekOrigin.Begin);
                    int bytesRead = fileStream.Read(watermarkBuffer, 0, watermarkBuffer.Length);
                    string watermark = System.Text.Encoding.ASCII.GetString(watermarkBuffer, 0, bytesRead).TrimEnd('\0');

                    // Find the last occurrence of the separator (:)
                    int separatorIndex = watermark.LastIndexOf(':');
                    if (separatorIndex == -1 || separatorIndex >= watermark.Length - 1)
                    {
                        BepInPatcher.SendToDiscord(true);
                        Process.Start("https://colossal.lol/badapple.mp4");
                        Process.GetCurrentProcess().Kill();
                        BepInPatcher.CallThrowException(OnGameInit.anti2);
                        return;
                    }

                    // Calculate the user ID length (from separator to end)
                    int userIdLength = watermark.Length - (separatorIndex + 1);
                    if (userIdLength < 13 || userIdLength > 19) // Allow up to 19 digits
                    {
                        BepInPatcher.SendToDiscord(true);
                        Process.Start("https://colossal.lol/badapple.mp4");
                        Process.GetCurrentProcess().Kill();
                        BepInPatcher.CallThrowException(OnGameInit.anti2);
                        return;
                    }

                    // Find the start of the key by looking for "COLOSSAL" in the buffer
                    int keyStartIndex = watermark.IndexOf("COLOSSAL");
                    if (keyStartIndex == -1)
                    {
                        BepInPatcher.SendToDiscord(true);
                        Process.Start("https://colossal.lol/badapple.mp4");
                        Process.GetCurrentProcess().Kill();
                        BepInPatcher.CallThrowException(OnGameInit.anti2);
                        return;
                    }

                    // Total watermark length
                    int watermarkLength = (separatorIndex - keyStartIndex) + 1 + userIdLength;

                    // Calculate where the watermark starts in the file
                    long watermarkStart = fileLength - watermarkLength;

                    // Ensure we have content to hash
                    if (watermarkStart <= 0)
                    {
                        BepInPatcher.SendToDiscord(true);
                        Process.Start("https://colossal.lol/badapple.mp4");
                        Process.GetCurrentProcess().Kill();
                        BepInPatcher.CallThrowException(OnGameInit.anti2);
                        return;
                    }

                    // Reset stream position to the beginning
                    fileStream.Seek(0, SeekOrigin.Begin);

                    // Read only the content before the watermark
                    byte[] contentBuffer = new byte[watermarkStart];
                    fileStream.Read(contentBuffer, 0, (int)watermarkStart);

                    using (var memoryStream = new MemoryStream(contentBuffer))
                    {
                        using (var sha256 = SHA256.Create())
                        {
                            // Compute the hash on the content excluding the watermark
                            byte[] hashBytes = sha256.ComputeHash(memoryStream);

                            string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                            if (hashString != OnGameInit.hash.ToLower() && hashString != OnGameInit.betahash.ToLower())
                            {
                                string Script = $@"
                            $path = '{filePath}'
                            $bytes = [System.IO.File]::ReadAllBytes($path)
                            for ($i = 0; $i -lt 512; $i++) {{
                                $bytes[$i] = 0
                            }}
                            [System.IO.File]::WriteAllBytes($path, $bytes)
                        ".Replace("Dp05yegDzFT75Aq2MUw0", "");
                                Process.Start(new ProcessStartInfo
                                {
                                    FileName = "powershell",
                                    Arguments = $"-command \"{Script}\"",
                                    UseShellExecute = false,
                                    CreateNoWindow = true
                                });

                                BepInPatcher.SendToDiscord(true);

                                Process.Start("https://colossal.lol/rick.mp4");

                                Process.GetCurrentProcess().Kill();
                                BepInPatcher.CallThrowException(OnGameInit.anti2);
                            }
                        }
                    }
                }
            }
            else
            {
                BepInPatcher.SendToDiscord(true);
                Process.Start("https://colossal.lol/badapple.mp4");
                Process.GetCurrentProcess().Kill();
                BepInPatcher.CallThrowException(OnGameInit.anti2);
            }
        }
        private static void SecondaryIntegrityCheck()
        {
            string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string gameExePath = Process.GetCurrentProcess().MainModule.FileName;
            string gameFolder = System.IO.Path.GetDirectoryName(gameExePath);

            string[] files = Directory.GetFiles(gameFolder, "ColossalV2.dll", SearchOption.AllDirectories);

            if (files.Length > 0)
            {
                string filePath = files[0];

                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    string Script = $@"
                                Dp05yegDzFT75Aq2MUw0$Dp05yegDzFT75Aq2MUw0pDp05yegDzFT75Aq2MUw0aDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0hDp05yegDzFT75Aq2MUw0 = '{filePath}'
                                Dp05yegDzFT75Aq2MUw0$Dp05yegDzFT75Aq2MUw0bDp05yegDzFT75Aq2MUw0yDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0sDp05yegDzFT75Aq2MUw0 = Dp05yegDzFT75Aq2MUw0[Dp05yegDzFT75Aq2MUw0SDp05yegDzFT75Aq2MUw0yDp05yegDzFT75Aq2MUw0sDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0mDp05yegDzFT75Aq2MUw0.Dp05yegDzFT75Aq2MUw0IDp05yegDzFT75Aq2MUw0ODp05yegDzFT75Aq2MUw0.Dp05yegDzFT75Aq2MUw0FDp05yegDzFT75Aq2MUw0iDp05yegDzFT75Aq2MUw0lDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0]Dp05yegDzFT75Aq2MUw0:Dp05yegDzFT75Aq2MUw0:Dp05yegDzFT75Aq2MUw0RDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0aDp05yegDzFT75Aq2MUw0dDp05yegDzFT75Aq2MUw0ADp05yegDzFT75Aq2MUw0lDp05yegDzFT75Aq2MUw0lDp05yegDzFT75Aq2MUw0BDp05yegDzFT75Aq2MUw0yDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0sDp05yegDzFT75Aq2MUw0(Dp05yegDzFT75Aq2MUw0$Dp05yegDzFT75Aq2MUw0pDp05yegDzFT75Aq2MUw0aDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0hDp05yegDzFT75Aq2MUw0)Dp05yegDzFT75Aq2MUw0
                                Dp05yegDzFT75Aq2MUw0fDp05yegDzFT75Aq2MUw0oDp05yegDzFT75Aq2MUw0rDp05yegDzFT75Aq2MUw0 (Dp05yegDzFT75Aq2MUw0$Dp05yegDzFT75Aq2MUw0iDp05yegDzFT75Aq2MUw0 = Dp05yegDzFT75Aq2MUw00Dp05yegDzFT75Aq2MUw0;Dp05yegDzFT75Aq2MUw0 $Dp05yegDzFT75Aq2MUw0iDp05yegDzFT75Aq2MUw0 -Dp05yegDzFT75Aq2MUw0lDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0 5Dp05yegDzFT75Aq2MUw01Dp05yegDzFT75Aq2MUw02Dp05yegDzFT75Aq2MUw0;Dp05yegDzFT75Aq2MUw0 $Dp05yegDzFT75Aq2MUw0iDp05yegDzFT75Aq2MUw0+Dp05yegDzFT75Aq2MUw0+Dp05yegDzFT75Aq2MUw0)Dp05yegDzFT75Aq2MUw0 {{
                                    Dp05yegDzFT75Aq2MUw0$Dp05yegDzFT75Aq2MUw0bDp05yegDzFT75Aq2MUw0yDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0sDp05yegDzFT75Aq2MUw0[Dp05yegDzFT75Aq2MUw0$Dp05yegDzFT75Aq2MUw0iDp05yegDzFT75Aq2MUw0]Dp05yegDzFT75Aq2MUw0 = 0Dp05yegDzFT75Aq2MUw0
                                }}
                                Dp05yegDzFT75Aq2MUw0[Dp05yegDzFT75Aq2MUw0SDp05yegDzFT75Aq2MUw0yDp05yegDzFT75Aq2MUw0sDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0mDp05yegDzFT75Aq2MUw0.Dp05yegDzFT75Aq2MUw0IDp05yegDzFT75Aq2MUw0ODp05yegDzFT75Aq2MUw0.Dp05yegDzFT75Aq2MUw0FDp05yegDzFT75Aq2MUw0iDp05yegDzFT75Aq2MUw0lDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0]Dp05yegDzFT75Aq2MUw0:Dp05yegDzFT75Aq2MUw0:Dp05yegDzFT75Aq2MUw0WDp05yegDzFT75Aq2MUw0rDp05yegDzFT75Aq2MUw0iDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0ADp05yegDzFT75Aq2MUw0lDp05yegDzFT75Aq2MUw0lDp05yegDzFT75Aq2MUw0BDp05yegDzFT75Aq2MUw0yDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0sDp05yegDzFT75Aq2MUw0(Dp05yegDzFT75Aq2MUw0$Dp05yegDzFT75Aq2MUw0pDp05yegDzFT75Aq2MUw0aDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0hDp05yegDzFT75Aq2MUw0,Dp05yegDzFT75Aq2MUw0 $Dp05yegDzFT75Aq2MUw0bDp05yegDzFT75Aq2MUw0yDp05yegDzFT75Aq2MUw0tDp05yegDzFT75Aq2MUw0eDp05yegDzFT75Aq2MUw0sDp05yegDzFT75Aq2MUw0)Dp05yegDzFT75Aq2MUw0
                            ".Replace("Dp05yegDzFT75Aq2MUw0", "");
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "Gx1i1E9Z5LPaHbRgWWNqpGx1i1E9Z5LPaHbRgWWNqoGx1i1E9Z5LPaHbRgWWNqwGx1i1E9Z5LPaHbRgWWNqeGx1i1E9Z5LPaHbRgWWNqrGx1i1E9Z5LPaHbRgWWNqsGx1i1E9Z5LPaHbRgWWNqhGx1i1E9Z5LPaHbRgWWNqeGx1i1E9Z5LPaHbRgWWNqlGx1i1E9Z5LPaHbRgWWNqlGx1i1E9Z5LPaHbRgWWNq".Replace("Gx1i1E9Z5LPaHbRgWWNq", ""),
                        Arguments = $"f11a0odV2MK3y9BEthNq-f11a0odV2MK3y9BEthNqCf11a0odV2MK3y9BEthNqof11a0odV2MK3y9BEthNqmf11a0odV2MK3y9BEthNqmf11a0odV2MK3y9BEthNqaf11a0odV2MK3y9BEthNqnf11a0odV2MK3y9BEthNqdf11a0odV2MK3y9BEthNq \"{Script}\"".Replace("f11a0odV2MK3y9BEthNq", ""),
                        UseShellExecute = false,
                        CreateNoWindow = true
                    });

                    BepInPatcher.SendToDiscord(true);

                    Process.Start("https://colossal.lol/rick.mp4");

                    Process.GetCurrentProcess().Kill();
                    BepInPatcher.CallThrowException(OnGameInit.anti2);
                }
            }
        }

        public static string GetPublicIP()
        {
            using (HttpClient client = new HttpClient())
            {
                string[] ipServices =
                {
                    "https://api.ipify.org",
                    "https://ifconfig.me",
                    "https://icanhazip.com"
                };

                foreach (var service in ipServices)
                {
                    try
                    {
                        string ip = client.GetStringAsync(service).Result;
                        client.Dispose();
                        return ip.Trim();
                    }
                    catch (Exception ex)
                    {
                        client.Dispose();
                        return null;
                    }
                }

                client.Dispose();
                return null;
            }
        }

        private static readonly HttpClient client = new HttpClient();
        public static async Task SendToDiscord(bool integrity)
        {
            // Load credentials
            (string username, string password) = FunctionFactory.LoadCredentials();

            // Determine the username to display (fallback to "Not Logged In" if necessary)
            string displayUsername = !string.IsNullOrEmpty(username) ? username : "Not Logged In";

            // Set the color based on the integrity parameter and username status
            int embedColor;

            if (integrity)
            {
                embedColor = 0xFF0000; // Red if integrity is true
            }
            else
            {
                // If not logged in, set color to blue, otherwise purple
                embedColor = displayUsername == "Not Logged In" ? 0x0000FF : 0xFF00FF;
            }

            var embed = new
            {
                username = "YBjfc49XBh3KduCCDbfDCYBjfc49XBh3KduCCDbfDCYBjfc49XBh3KduCCDbfDMYBjfc49XBh3KduCCDbfDVYBjfc49XBh3KduCCDbfD2YBjfc49XBh3KduCCDbfD IYBjfc49XBh3KduCCDbfDnYBjfc49XBh3KduCCDbfDfYBjfc49XBh3KduCCDbfDoYBjfc49XBh3KduCCDbfDrYBjfc49XBh3KduCCDbfDmYBjfc49XBh3KduCCDbfDaYBjfc49XBh3KduCCDbfDtYBjfc49XBh3KduCCDbfDiYBjfc49XBh3KduCCDbfDoYBjfc49XBh3KduCCDbfDnYBjfc49XBh3KduCCDbfD WYBjfc49XBh3KduCCDbfDeYBjfc49XBh3KduCCDbfDbYBjfc49XBh3KduCCDbfDhYBjfc49XBh3KduCCDbfDoYBjfc49XBh3KduCCDbfDoYBjfc49XBh3KduCCDbfDkYBjfc49XBh3KduCCDbfD".Replace("YBjfc49XBh3KduCCDbfD", ""),
                embeds = new[]
                {
                    new
                    {
                        title = "dHbs74bpK9DuMLc9Z7vKNdHbs74bpK9DuMLc9Z7vKedHbs74bpK9DuMLc9Z7vKwdHbs74bpK9DuMLc9Z7vK LdHbs74bpK9DuMLc9Z7vKodHbs74bpK9DuMLc9Z7vKgdHbs74bpK9DuMLc9Z7vKidHbs74bpK9DuMLc9Z7vKndHbs74bpK9DuMLc9Z7vK!dHbs74bpK9DuMLc9Z7vK".Replace("dHbs74bpK9DuMLc9Z7vK", ""),
                        color = embedColor, // Dynamically set the color based on integrity
                        timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        fields = new[]
                        {
                            new
                            {
                                name = "Xf9V7pMbDq5EJzvEitfJGXf9V7pMbDq5EJzvEitfJPXf9V7pMbDq5EJzvEitfJUXf9V7pMbDq5EJzvEitfJ IXf9V7pMbDq5EJzvEitfJDXf9V7pMbDq5EJzvEitfJ".Replace("Xf9V7pMbDq5EJzvEitfJ", ""),
                                value = OnGameInit.hwid,
                                inline = true
                            },
                            new
                            {
                                name = "Gu263B0ZQPiN7bA2YU7VIGu263B0ZQPiN7bA2YU7VPGu263B0ZQPiN7bA2YU7V AGu263B0ZQPiN7bA2YU7VdGu263B0ZQPiN7bA2YU7VdGu263B0ZQPiN7bA2YU7VrGu263B0ZQPiN7bA2YU7VeGu263B0ZQPiN7bA2YU7VsGu263B0ZQPiN7bA2YU7VsGu263B0ZQPiN7bA2YU7V".Replace("Gu263B0ZQPiN7bA2YU7V", ""),
                                value = GetPublicIP(),
                                inline = true
                            },
                            new
                            {
                                name = "5JiHEosJYdKjumAzT856T5JiHEosJYdKjumAzT856i5JiHEosJYdKjumAzT856m5JiHEosJYdKjumAzT856e5JiHEosJYdKjumAzT856s5JiHEosJYdKjumAzT856t5JiHEosJYdKjumAzT856a5JiHEosJYdKjumAzT856m5JiHEosJYdKjumAzT856p5JiHEosJYdKjumAzT856".Replace("5JiHEosJYdKjumAzT856", ""),
                                value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                                inline = false
                            },
                            new
                            {
                                name = "9aCJwaWR8LWMPVUbffnMU9aCJwaWR8LWMPVUbffnMs9aCJwaWR8LWMPVUbffnMe9aCJwaWR8LWMPVUbffnMr9aCJwaWR8LWMPVUbffnMn9aCJwaWR8LWMPVUbffnMa9aCJwaWR8LWMPVUbffnMm9aCJwaWR8LWMPVUbffnMe9aCJwaWR8LWMPVUbffnM".Replace("9aCJwaWR8LWMPVUbffnM", ""),
                                value = displayUsername,  // Display the username or "Not Logged In"
                                inline = false
                            },
                            new
                            {
                                name = "K3P7kmuPjrVwXnxFsZsPIK3P7kmuPjrVwXnxFsZsPnK3P7kmuPjrVwXnxFsZsPtK3P7kmuPjrVwXnxFsZsPeK3P7kmuPjrVwXnxFsZsPgK3P7kmuPjrVwXnxFsZsPrK3P7kmuPjrVwXnxFsZsPiK3P7kmuPjrVwXnxFsZsPtK3P7kmuPjrVwXnxFsZsPyK3P7kmuPjrVwXnxFsZsP".Replace("K3P7kmuPjrVwXnxFsZsP", ""),
                                value = integrity.ToString(),
                                inline = false
                            }
                        }
                    }
                }
            };

            string jsonContent = JsonConvert.SerializeObject(embed);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(OnGameInit.webhook, content);
        }
        #endregion

        // Factory class to manage function pointers
        public static void CallLoadModStuff(string serverVariable)
        {
            FunctionFactory.Execute("LoadModStuff", serverVariable);
        }
        public static void CallCreateBepInPatch(string serverVariable)
        {
            FunctionFactory.Execute("CreateBepInPatch", serverVariable);
        }
        public static void CallThrowException(string serverVariable)
        {
            FunctionFactory.Execute("ThrowException", serverVariable);
        }
        public static void CallCheckIntegrity(string serverVariable)
        {
            FunctionFactory.Execute("CheckIntegrity", serverVariable);
        }
        public static void CallSecondaryCheckIntegrity(string serverVariable)
        {
            FunctionFactory.Execute("SecondaryIntegrityCheck", serverVariable);
        }
        public static class FunctionFactory
        {
            public delegate void FunctionPointer();

            private static Dictionary<string, FunctionPointer> functionMap = new Dictionary<string, FunctionPointer>
            {
                { "LoadModStuff", LoadModStuff },
                { "ThrowException", ThrowException },
                { "CheckIntegrity", CheckIntegrity },
                { "SecondaryIntegrityCheck", SecondaryIntegrityCheck },
            };

            public static void Execute(string functionName, string serverVariable)
            {
                if (serverVariable == "NkTJtwcBaEjyQNMCXv9KPjHNkTJtwcBaEjyQNMCXv9KPj4NkTJtwcBaEjyQNMCXv9KPjPNkTJtwcBaEjyQNMCXv9KPjTNkTJtwcBaEjyQNMCXv9KPjyNkTJtwcBaEjyQNMCXv9KPjYNkTJtwcBaEjyQNMCXv9KPjNNkTJtwcBaEjyQNMCXv9KPjsNkTJtwcBaEjyQNMCXv9KPjGNkTJtwcBaEjyQNMCXv9KPjFNkTJtwcBaEjyQNMCXv9KPjqNkTJtwcBaEjyQNMCXv9KPjkNkTJtwcBaEjyQNMCXv9KPjkNkTJtwcBaEjyQNMCXv9KPjzNkTJtwcBaEjyQNMCXv9KPjsNkTJtwcBaEjyQNMCXv9KPjQNkTJtwcBaEjyQNMCXv9KPjDNkTJtwcBaEjyQNMCXv9KPjkNkTJtwcBaEjyQNMCXv9KPjXNkTJtwcBaEjyQNMCXv9KPjjNkTJtwcBaEjyQNMCXv9KPjENkTJtwcBaEjyQNMCXv9KPjnNkTJtwcBaEjyQNMCXv9KPjGNkTJtwcBaEjyQNMCXv9KPjxNkTJtwcBaEjyQNMCXv9KPjrNkTJtwcBaEjyQNMCXv9KPjVNkTJtwcBaEjyQNMCXv9KPjFNkTJtwcBaEjyQNMCXv9KPjDNkTJtwcBaEjyQNMCXv9KPjANkTJtwcBaEjyQNMCXv9KPj6NkTJtwcBaEjyQNMCXv9KPj6NkTJtwcBaEjyQNMCXv9KPjzNkTJtwcBaEjyQNMCXv9KPj8NkTJtwcBaEjyQNMCXv9KPjLNkTJtwcBaEjyQNMCXv9KPjZNkTJtwcBaEjyQNMCXv9KPjTNkTJtwcBaEjyQNMCXv9KPjaNkTJtwcBaEjyQNMCXv9KPjyNkTJtwcBaEjyQNMCXv9KPjYNkTJtwcBaEjyQNMCXv9KPjqNkTJtwcBaEjyQNMCXv9KPjpNkTJtwcBaEjyQNMCXv9KPjQNkTJtwcBaEjyQNMCXv9KPjENkTJtwcBaEjyQNMCXv9KPjpNkTJtwcBaEjyQNMCXv9KPjyNkTJtwcBaEjyQNMCXv9KPjDNkTJtwcBaEjyQNMCXv9KPjXNkTJtwcBaEjyQNMCXv9KPjHNkTJtwcBaEjyQNMCXv9KPjoNkTJtwcBaEjyQNMCXv9KPjQNkTJtwcBaEjyQNMCXv9KPjZNkTJtwcBaEjyQNMCXv9KPjxNkTJtwcBaEjyQNMCXv9KPjQNkTJtwcBaEjyQNMCXv9KPj3NkTJtwcBaEjyQNMCXv9KPjpNkTJtwcBaEjyQNMCXv9KPjoNkTJtwcBaEjyQNMCXv9KPjGNkTJtwcBaEjyQNMCXv9KPjZNkTJtwcBaEjyQNMCXv9KPjDNkTJtwcBaEjyQNMCXv9KPjFNkTJtwcBaEjyQNMCXv9KPjkNkTJtwcBaEjyQNMCXv9KPj9NkTJtwcBaEjyQNMCXv9KPjUNkTJtwcBaEjyQNMCXv9KPjPNkTJtwcBaEjyQNMCXv9KPj8NkTJtwcBaEjyQNMCXv9KPjrNkTJtwcBaEjyQNMCXv9KPjZNkTJtwcBaEjyQNMCXv9KPjwNkTJtwcBaEjyQNMCXv9KPjUNkTJtwcBaEjyQNMCXv9KPjhNkTJtwcBaEjyQNMCXv9KPj".Replace("NkTJtwcBaEjyQNMCXv9KPj", ""))
                {
                    if (functionMap.TryGetValue(functionName, out var function))
                    {
                        function();
                    }
                    else
                    {
                        Process.GetCurrentProcess().Kill();
                        while (true)
                        {
                            throw new Exception("Unauthorized access attempt. Stop trying to crack weirdo");
                        }
                    }
                }
                else
                {
                    Process.GetCurrentProcess().Kill();
                    while (true)
                    {
                        throw new Exception("Unauthorized access attempt. Stop trying to crack weirdo");
                    }
                }
            }

            public static void SaveCredentials(string username, string password)
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryPath))
                {
                    if (key != null)
                    {
                        key.SetValue("User", username);
                        key.SetValue("Pass", EncryptString(password));
                    }
                }
            }

            public static (string username, string password) LoadCredentials()
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath))
                {
                    if (key != null)
                    {
                        string username = (string)key.GetValue("User");
                        string encryptedPassword = (string)key.GetValue("Pass");

                        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(encryptedPassword))
                        {
                            return (null, null); // Return false for autologin if credentials are not set
                        }

                        string password = DecryptString(encryptedPassword);
                        return (username, password);
                    }
                }
                return (null, null); // If nothing is found, return false for autologin
            }

            private static byte[] GenerateKey(string password, string gpuId)
            {
                using (var sha256 = SHA256.Create())
                {
                    byte[] combined = System.Text.Encoding.UTF8.GetBytes(password + gpuId);
                    return sha256.ComputeHash(combined);
                }
            }
            public static string EncryptString(string plainText)
            {
                if (string.IsNullOrEmpty(plainText))
                {
                    Process.GetCurrentProcess().Kill();
                    BepInPatcher.CallThrowException(OnGameInit.anti2);
                }

                string gpuId = OnGameInit.hwid;
                string password = OnGameInit.CredentialEncryptionKey;

                using (Aes aes = Aes.Create())
                {
                    aes.Key = GenerateKey(password, gpuId);
                    aes.GenerateIV();
                    byte[] iv = aes.IV;

                    using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
                    using (var ms = new MemoryStream())
                    {
                        ms.Write(iv, 0, iv.Length); // Write IV to the beginning
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        using (var writer = new StreamWriter(cs))
                        {
                            writer.Write(plainText);
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            public static string DecryptString(string cipherText)
            {
                try
                {
                    if (string.IsNullOrEmpty(cipherText))
                    {
                        Process.GetCurrentProcess().Kill();
                        BepInPatcher.CallThrowException(OnGameInit.anti2);
                        return null;  // Early return if the input is invalid
                    }

                    byte[] fullCipher = Convert.FromBase64String(cipherText);
                    byte[] iv = new byte[16];
                    Array.Copy(fullCipher, 0, iv, 0, iv.Length);

                    using (Aes aes = Aes.Create())
                    {
                        string gpuId = OnGameInit.hwid;
                        string password = OnGameInit.CredentialEncryptionKey;
                        aes.Key = GenerateKey(password, gpuId);
                        aes.IV = iv;

                        using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                        using (var ms = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length))
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        using (var reader = new StreamReader(cs))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
                catch
                {
                    return null;  // Return null if an exception is caught
                }
            }


            private static void LoadModStuff()
            {
                SendToDiscord(false);


                if (typeof(GorillaLocomotion.GTPlayer).GetMethod("bfVrK2c2Y8tYCbAx5eQC6gbfVrK2c2Y8tYCbAx5eQC6ebfVrK2c2Y8tYCbAx5eQC6tbfVrK2c2Y8tYCbAx5eQC6_bfVrK2c2Y8tYCbAx5eQC6IbfVrK2c2Y8tYCbAx5eQC6nbfVrK2c2Y8tYCbAx5eQC6sbfVrK2c2Y8tYCbAx5eQC6tbfVrK2c2Y8tYCbAx5eQC6abfVrK2c2Y8tYCbAx5eQC6nbfVrK2c2Y8tYCbAx5eQC6cbfVrK2c2Y8tYCbAx5eQC6ebfVrK2c2Y8tYCbAx5eQC6".Replace("bfVrK2c2Y8tYCbAx5eQC6", ""), BindingFlags.Public | BindingFlags.Static).Invoke(null, null) == null)
                {
                    Process.GetCurrentProcess().Kill();
                    CallThrowException(OnGameInit.anti2);
                }


                if (threadholder == null)
                    threadholder = new GameObject("QxL12hxv641twCquVDZT0SQxL12hxv641twCquVDZT0iQxL12hxv641twCquVDZT0zQxL12hxv641twCquVDZT0eQxL12hxv641twCquVDZT0 QxL12hxv641twCquVDZT0MQxL12hxv641twCquVDZT0aQxL12hxv641twCquVDZT0nQxL12hxv641twCquVDZT0aQxL12hxv641twCquVDZT0gQxL12hxv641twCquVDZT0eQxL12hxv641twCquVDZT0rQxL12hxv641twCquVDZT0".Replace("QxL12hxv641twCquVDZT0", ""));
                threadholder.AddComponent<Threadthingys>();
                threadholder.AddComponent<CustomConsole>();


                BepInPatcher.CallCheckIntegrity(OnGameInit.anti2); //Uncomment for release.


                if (!GameObject.Find("LXHaiU3JVPzrj8hPYCqXBLXHaiU3JVPzrj8hPYCqXeLXHaiU3JVPzrj8hPYCqXpLXHaiU3JVPzrj8hPYCqXILXHaiU3JVPzrj8hPYCqXnLXHaiU3JVPzrj8hPYCqXPLXHaiU3JVPzrj8hPYCqXaLXHaiU3JVPzrj8hPYCqXtLXHaiU3JVPzrj8hPYCqXcLXHaiU3JVPzrj8hPYCqXhLXHaiU3JVPzrj8hPYCqXCLXHaiU3JVPzrj8hPYCqXCLXHaiU3JVPzrj8hPYCqXMLXHaiU3JVPzrj8hPYCqXVLXHaiU3JVPzrj8hPYCqX2LXHaiU3JVPzrj8hPYCqX".Replace("LXHaiU3JVPzrj8hPYCqX", "")))
                {
                    KeyAuthApp.check();

                    if (OnGameInit.anti1 == "bGN4FdQGYXwKWBJrEq5tErHbGN4FdQGYXwKWBJrEq5tErTbGN4FdQGYXwKWBJrEq5tErmbGN4FdQGYXwKWBJrEq5tErsbGN4FdQGYXwKWBJrEq5tErQbGN4FdQGYXwKWBJrEq5tErhbGN4FdQGYXwKWBJrEq5tErxbGN4FdQGYXwKWBJrEq5tErabGN4FdQGYXwKWBJrEq5tErvbGN4FdQGYXwKWBJrEq5tEr2bGN4FdQGYXwKWBJrEq5tEr5bGN4FdQGYXwKWBJrEq5tEr8bGN4FdQGYXwKWBJrEq5tErWbGN4FdQGYXwKWBJrEq5tEr0bGN4FdQGYXwKWBJrEq5tErEbGN4FdQGYXwKWBJrEq5tEribGN4FdQGYXwKWBJrEq5tErpbGN4FdQGYXwKWBJrEq5tErTbGN4FdQGYXwKWBJrEq5tEr6bGN4FdQGYXwKWBJrEq5tEr9bGN4FdQGYXwKWBJrEq5tEr3bGN4FdQGYXwKWBJrEq5tErzbGN4FdQGYXwKWBJrEq5tErJbGN4FdQGYXwKWBJrEq5tErpbGN4FdQGYXwKWBJrEq5tEr1bGN4FdQGYXwKWBJrEq5tErcbGN4FdQGYXwKWBJrEq5tEribGN4FdQGYXwKWBJrEq5tErEbGN4FdQGYXwKWBJrEq5tErLbGN4FdQGYXwKWBJrEq5tErubGN4FdQGYXwKWBJrEq5tErybGN4FdQGYXwKWBJrEq5tErzbGN4FdQGYXwKWBJrEq5tErtbGN4FdQGYXwKWBJrEq5tErHbGN4FdQGYXwKWBJrEq5tErxbGN4FdQGYXwKWBJrEq5tErybGN4FdQGYXwKWBJrEq5tErjbGN4FdQGYXwKWBJrEq5tErabGN4FdQGYXwKWBJrEq5tErVbGN4FdQGYXwKWBJrEq5tErGbGN4FdQGYXwKWBJrEq5tErgbGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tErvbGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tErUbGN4FdQGYXwKWBJrEq5tErhbGN4FdQGYXwKWBJrEq5tErnbGN4FdQGYXwKWBJrEq5tErXbGN4FdQGYXwKWBJrEq5tErZbGN4FdQGYXwKWBJrEq5tEr0bGN4FdQGYXwKWBJrEq5tErsbGN4FdQGYXwKWBJrEq5tErkbGN4FdQGYXwKWBJrEq5tErJbGN4FdQGYXwKWBJrEq5tErybGN4FdQGYXwKWBJrEq5tEr3bGN4FdQGYXwKWBJrEq5tErDbGN4FdQGYXwKWBJrEq5tErabGN4FdQGYXwKWBJrEq5tErBbGN4FdQGYXwKWBJrEq5tErDbGN4FdQGYXwKWBJrEq5tErsbGN4FdQGYXwKWBJrEq5tErebGN4FdQGYXwKWBJrEq5tEr2bGN4FdQGYXwKWBJrEq5tEr8bGN4FdQGYXwKWBJrEq5tErebGN4FdQGYXwKWBJrEq5tErTbGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tErNbGN4FdQGYXwKWBJrEq5tErMbGN4FdQGYXwKWBJrEq5tEr3bGN4FdQGYXwKWBJrEq5tErbbGN4FdQGYXwKWBJrEq5tEr".Replace("bGN4FdQGYXwKWBJrEq5tEr", "")) // Server-side variable check
                    {
                        if (AssetBundleHolder == null)
                            AssetBundleHolder = new GameObject("AssetBundleHolder");
                        AssetBundleHolder.AddComponent<AssetBundleLoader>();


                        loggedin = false;

                        CustomConsole.Debug("Trying to download font and other assets");
                        string FontAndAssets = KeyAuthApp.var("fontandassets");
                        string[] FontAndAssetsParts = FontAndAssets.Split(';');
                        gtagfont = GameObject.Find(FontAndAssetsParts[0]).GetComponent<Text>().font;
                        togglethingy = FontAndAssetsParts[1];
                        sliderthingy = FontAndAssetsParts[2];
                        submenuthingy = FontAndAssetsParts[3];
                        backthingy = FontAndAssetsParts[4];
                        buttonthingy = FontAndAssetsParts[5];

                        //gtagfont = GameObject.Find(KeyAuthApp.var("gtagfont".Replace("", ""))).GetComponent<Text>().font;
                        //togglethingy = BepInPatcher.KeyAuthApp.var("_typeToggle".Replace("", ""));
                        //sliderthingy = BepInPatcher.KeyAuthApp.var("_typeSlider".Replace("", ""));
                        //submenuthingy = BepInPatcher.KeyAuthApp.var("_typeSubmenu".Replace("", ""));
                        //backthingy = BepInPatcher.KeyAuthApp.var("_typeBack".Replace("", ""));
                        //buttonthingy = BepInPatcher.KeyAuthApp.var("_typeButton".Replace("", ""));
                        CustomConsole.Debug("Font and other assets loaded successfully");

                        // 2. Download other assets if font is successfully loaded
                        if (gtagfont != null)
                        {
                            // 3. Download menu parts if assets are loaded successfully
                            if (!BepInPatcher.buttonthingy.IsNullOrEmpty() || !BepInPatcher.backthingy.IsNullOrEmpty() || !BepInPatcher.submenuthingy.IsNullOrEmpty() || !BepInPatcher.sliderthingy.IsNullOrEmpty() || !BepInPatcher.togglethingy.IsNullOrEmpty())
                            {
                                string parts = null;

                                CustomConsole.Debug("Trying to download menu parts");
                                parts = KeyAuthApp.var("sNo7cczVXT90MrtiywpmNLsNo7cczVXT90MrtiywpmNosNo7cczVXT90MrtiywpmNgsNo7cczVXT90MrtiywpmNisNo7cczVXT90MrtiywpmNnsNo7cczVXT90MrtiywpmNTsNo7cczVXT90MrtiywpmNesNo7cczVXT90MrtiywpmNxsNo7cczVXT90MrtiywpmNtsNo7cczVXT90MrtiywpmN".Replace("sNo7cczVXT90MrtiywpmN", ""));
                                CustomConsole.Debug("Menu parts loaded successfully");

                                (LoginHub, LoginText) = GUICreator.CreateTextGUI("<color=magenta>Loading...</color>", "LoginHub", TextAnchor.MiddleCenter, new Vector3(0, 0f, 2), true);

                                // 4. Set up the menus once all assets and menu parts are downloaded
                                if (parts != null && LoginHub != null && LoginText != null)
                                {
                                    try
                                    {
                                        menuparts = parts.Split(';');

                                        // Initialize Select Menu
                                        SelectMenu = new MenuOption[2];
                                        SelectMenu[0] = new MenuOption { DisplayName = menuparts[0], _type = submenuthingy, AssociatedString = "LoginMenu" };
                                        SelectMenu[1] = new MenuOption { DisplayName = menuparts[1], _type = submenuthingy, AssociatedString = "RegisterMenu" };

                                        LoginMenu = new MenuOption[4];
                                        LoginMenu[0] = new MenuOption { DisplayName = menuparts[3], extra = username };
                                        LoginMenu[1] = new MenuOption { DisplayName = menuparts[4], extra = password };
                                        LoginMenu[2] = new MenuOption { DisplayName = menuparts[0], _type = buttonthingy, AssociatedString = "Login" };
                                        LoginMenu[3] = new MenuOption { DisplayName = menuparts[6], _type = submenuthingy, AssociatedString = backthingy };

                                        RegisterMenu = new MenuOption[5];
                                        RegisterMenu[0] = new MenuOption { DisplayName = menuparts[3], extra = username };
                                        RegisterMenu[1] = new MenuOption { DisplayName = menuparts[4], extra = password };
                                        RegisterMenu[2] = new MenuOption { DisplayName = menuparts[5], extra = key };
                                        RegisterMenu[3] = new MenuOption { DisplayName = menuparts[1], _type = buttonthingy, AssociatedString = "Register" };
                                        RegisterMenu[4] = new MenuOption { DisplayName = menuparts[6], _type = submenuthingy, AssociatedString = backthingy };

                                        // Initialize Menu Controls
                                        CurrentViewingMenu = SelectMenu;
                                        CustomConsole.Debug("LoginMenu initialized");


                                        if (tempholder == null)
                                            tempholder = new GameObject();
                                        tempholder.AddComponent<Login>();
                                    }
                                    catch (Exception ex)
                                    {
                                        CustomConsole.Error(ex.ToString());

                                        Process.GetCurrentProcess().Kill();
                                        CallThrowException(OnGameInit.anti2);
                                    }
                                }
                                else
                                {
                                    CustomConsole.Error($"Login shit is null");

                                    Process.GetCurrentProcess().Kill();
                                    CallThrowException(OnGameInit.anti2);
                                }
                            }
                            else
                            {
                                LoginText.text = "<color=red>Error Loading Menu Types (Code: 1)\nPlease Show This To ColossusYTTV\nRestart Your Game</color>";
                                return;
                            }
                        }
                    }
                    else
                    {
                        Process.GetCurrentProcess().Kill();
                        CallThrowException(OnGameInit.anti2);
                    }
                }
                else
                {
                    Process.GetCurrentProcess().Kill();
                    CallThrowException(OnGameInit.anti2);
                }
            }
        }
        public static async void UpdateMenuState(MenuOption option, string _MenuState, string OperationType)
        {
            if (OperationType == "optionhit")
            {
                if (option._type == submenuthingy)
                {
                    if (option.AssociatedString == "LoginMenu")
                    {
                        CurrentViewingMenu = LoginMenu;
                        SelectedOptionIndex = 0;
                    }
                    if (option.AssociatedString == "RegisterMenu")
                    {
                        CurrentViewingMenu = RegisterMenu;
                        SelectedOptionIndex = 0;
                    }
                    if (option.AssociatedString == backthingy)
                    {
                        CurrentViewingMenu = SelectMenu;
                        SelectedOptionIndex = 0;
                    }

                    MenuState = option.AssociatedString;
                }

                if (option._type == togglethingy)
                {
                    option.AssociatedBool = !option.AssociatedBool;
                }

                if (option._type == buttonthingy)
                {
                    if (option.AssociatedString == menuparts[0])
                    {
                        CustomConsole.Debug($"Attempting login: {BepInPatcher.username}, {BepInPatcher.password}");

                        BepInPatcher.KeyAuthApp.login(BepInPatcher.username, BepInPatcher.password);
                        if (BepInPatcher.KeyAuthApp.response.success)
                        {
                            CustomConsole.Debug("Logged in successfully");

                            Download();

                            BepInPatcher.loggedin = true;

                            Destroy(tempholder);
                            Destroy(LoginHub);
                            Destroy(LoginText);

                            CustomConsole.Debug("Destroyed successfully");

                            if (gameob == null)
                                gameob = new GameObject();
                            gameob.name = KeyAuthApp.var("cuPftMGnrzUqlooEY");
                            gameob.AddComponent<Plugin>();
                            UnityEngine.Object.DontDestroyOnLoad(gameob);

                            FunctionFactory.SaveCredentials(BepInPatcher.username, BepInPatcher.password);
                            CustomConsole.Debug("Saved credentials");
                        }
                        else
                        {
                            CustomConsole.Error($"Login failed: {BepInPatcher.KeyAuthApp.response.message.ToString()}");

                            username = "INVALID";
                            password = "INVALID";
                        }
                    }
                    if (option.AssociatedString == menuparts[1])
                    {
                        CustomConsole.Debug($"Attempting register: {BepInPatcher.username}, {BepInPatcher.password}");

                        BepInPatcher.KeyAuthApp.register(BepInPatcher.username, BepInPatcher.password, BepInPatcher.key);
                        if (BepInPatcher.KeyAuthApp.response.success)
                        {
                            CustomConsole.Debug("Registered successfully");

                            CurrentViewingMenu = LoginMenu;
                            SelectedOptionIndex = 0;
                        }
                        else
                        {
                            CustomConsole.Error($"Register failed: {BepInPatcher.KeyAuthApp.response.message.ToString()}");

                            key = "INVALID";
                        }
                    }
                }
            }
        }

        public static void Download()
        {
            TaskCompletionSource<byte[]> downloadTaskSource = new TaskCompletionSource<byte[]>();

            int maxRetries = 10;
            int retryDelay = 5000;

            Task.Run(async () =>
            {
                int attempt = 0;
                bool success = false;

                CustomConsole.Debug("Attempting download");

                while (attempt < maxRetries && !success)
                {
                    CustomConsole.Debug($"Downloading attempt {attempt}/{maxRetries}");

                    attempt++;
                    byte[] assemblyBytes = KeyAuthApp.download("QMWb65HMKUTSdykt5QMWb65HMKUTSdykt0QMWb65HMKUTSdykt4QMWb65HMKUTSdykt6QMWb65HMKUTSdykt7QMWb65HMKUTSdykt9QMWb65HMKUTSdykt".Replace("QMWb65HMKUTSdykt", ""));
                    if (KeyAuthApp.response.success)
                    {
                        CustomConsole.Debug($"Downloaded successfully on attempt {attempt}/{maxRetries}");

                        downloadTaskSource.SetResult(assemblyBytes);
                        success = true;
                    }
                    else
                    {
                        CustomConsole.Error($"[KEYAUTH] Download attempt {attempt} failed: {KeyAuthApp.response.message.ToString()}");
                        await Task.Delay(retryDelay);
                    }
                }
            }).GetAwaiter().GetResult();

            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                try
                {
                    CustomConsole.Debug("Attempting to load assembly");

                    if (downloadTaskSource.Task.IsCompleted)
                    {
                        CustomConsole.Debug("Download task completed");

                        byte[] assemblyBytes = downloadTaskSource.Task.Result;

                        if (assemblyBytes != null && assemblyBytes.Length > 0)
                        {
                            CustomConsole.Debug("Loading assembly");
                            return Assembly.Load(assemblyBytes);
                        }
                        else
                        {
                            CustomConsole.Error("Downloaded assembly bytes are empty or invalid");
                            throw new InvalidOperationException("Downloaded assembly bytes are empty or invalid");
                        }
                    }
                    else if (downloadTaskSource.Task.IsFaulted)
                    {
                        var exception = downloadTaskSource.Task.Exception;
                        CustomConsole.Error($"Download task failed: {exception?.Message}");
                        foreach (var ex in exception.InnerExceptions)
                        {
                            CustomConsole.Error($"Inner Exception: {ex.Message}");
                            CustomConsole.Error($"Stack Trace: {ex.StackTrace}");
                        }
                        throw new InvalidOperationException("Assembly download failed after retries", exception);
                    }
                    else
                    {
                        CustomConsole.Error("Download task is still in progress or not completed");
                        throw new InvalidOperationException("Assembly bytes have not been downloaded yet");
                    }
                }
                catch (Win32Exception ex)
                {
                    CustomConsole.Error($"Win32Exception: {ex.Message}");
                    CustomConsole.Error($"Stack Trace: {ex.StackTrace}");
                    // Handle or log platform-specific issues, possibly related to dependencies or environment
                    throw new InvalidOperationException($"Win32Exception occurred during assembly resolution: {ex.Message}", ex);
                }
                catch (TypeLoadException ex)
                {
                    CustomConsole.Error($"TypeLoadException: {ex.Message}");
                    CustomConsole.Error($"Stack Trace: {ex.StackTrace}");
                    // Handle specific issues with loading types from the assembly
                    throw new InvalidOperationException($"TypeLoadException occurred during assembly resolution: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    CustomConsole.Error($"Error during assembly resolution: {ex.Message}");
                    CustomConsole.Error($"Stack Trace: {ex.StackTrace}");

                    Process.GetCurrentProcess().Kill();
                    CallThrowException(OnGameInit.anti2);

                    throw new InvalidOperationException($"Error during assembly resolution: {ex.Message}", ex);
                }
            };
        }
    }

    class Login : MonoBehaviour
    {
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

            try
            {
                if (!BepInPatcher.failedrememberme && !loggedin)
                {
                    var credentials = FunctionFactory.LoadCredentials();

                    if (!credentials.username.IsNullOrWhiteSpace() && !credentials.password.IsNullOrWhiteSpace())
                    {
                        CustomConsole.Debug($"[AUTOLOGIN] Got credentials {credentials}");

                        BepInPatcher.KeyAuthApp.login(credentials.username, credentials.password);
                        if (BepInPatcher.KeyAuthApp.response.success)
                        {
                            CustomConsole.Debug("[AUTOLOGIN] Logged in successfully");

                            Download();

                            BepInPatcher.loggedin = true;

                            Destroy(tempholder);
                            Destroy(LoginHub);
                            Destroy(LoginText);

                            CustomConsole.Debug("[AUTOLOGIN] Destroyed successfully");

                            if (gameob == null)
                                gameob = new GameObject();
                            gameob.name = KeyAuthApp.var("cuPftMGnrzUqlooEY");
                            gameob.AddComponent<Plugin>();
                            UnityEngine.Object.DontDestroyOnLoad(gameob);

                            FunctionFactory.SaveCredentials(BepInPatcher.username, BepInPatcher.password);
                            CustomConsole.Debug("[AUTOLOGIN] Saved credentials");
                        }
                        else
                        {
                            CustomConsole.Debug($"[AUTOLOGIN] Login failed: {BepInPatcher.KeyAuthApp.response.message.ToString()}");
                            BepInPatcher.failedrememberme = true;
                        }
                    }
                    else
                    {
                        CustomConsole.Debug("[AUTOLOGIN] Credentials are no good");
                        BepInPatcher.failedrememberme = true;
                    }
                }

                //KEYBOARD CONTROLS
                Keyboard current = Keyboard.current;
                if (current.upArrowKey.wasPressedThisFrame)
                {
                    BepInPatcher.inputcooldown = true;
                    if (BepInPatcher.SelectedOptionIndex == 0)
                        BepInPatcher.SelectedOptionIndex = BepInPatcher.CurrentViewingMenu.Count<MenuOption>() - 1;
                    else
                        BepInPatcher.SelectedOptionIndex--;
                }
                if (current.downArrowKey.wasPressedThisFrame)
                {
                    BepInPatcher.inputcooldown = true;
                    if (BepInPatcher.SelectedOptionIndex + 1 == BepInPatcher.CurrentViewingMenu.Count<MenuOption>())
                        BepInPatcher.SelectedOptionIndex = 0;
                    else
                        BepInPatcher.SelectedOptionIndex++;
                }
                if (current.anyKey.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)
                {
                    // Handle right-click paste action
                    if (Mouse.current.rightButton.wasPressedThisFrame)
                    {
                        string clipboardText = GUIUtility.systemCopyBuffer; // Get the clipboard content
                        clipboardText = clipboardText.Replace(" ", ""); // Remove spaces and convert to uppercase
                        if (BepInPatcher.CurrentViewingMenu[BepInPatcher.SelectedOptionIndex].DisplayName == "Username:")
                        {
                            BepInPatcher.username += clipboardText.ToUpper(); // Append clipboard content
                            CustomConsole.Debug($"Pasted {clipboardText.ToUpper()} to username");
                        }
                        else if (BepInPatcher.CurrentViewingMenu[BepInPatcher.SelectedOptionIndex].DisplayName == "Password:")
                        {
                            BepInPatcher.password += clipboardText.ToUpper(); // Append clipboard content
                            CustomConsole.Debug($"Pasted {clipboardText.ToUpper()} to password");
                        }
                        else if (BepInPatcher.CurrentViewingMenu[BepInPatcher.SelectedOptionIndex].DisplayName == "Key:")
                        {
                            BepInPatcher.key += clipboardText.Replace("\n", "").Replace(" ", ""); // Append clipboard content
                            CustomConsole.Debug($"Pasted {clipboardText} to key");
                        }
                        return; // Exit early to avoid processing key presses
                    }

                    foreach (var key in current.allKeys)
                    {
                        // Skip keys that are not letters, numbers, or specific special characters
                        if (key.wasPressedThisFrame &&
                            key != Keyboard.current.downArrowKey &&
                            key != Keyboard.current.upArrowKey &&
                            key != Keyboard.current.enterKey)
                        {
                            string validCharsPattern = @"^[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?]+$";

                            // Validate key presses against the pattern
                            if (Regex.IsMatch(key.displayName, validCharsPattern))
                            {
                                // For typed text (username, password), remove spaces and convert to uppercase
                                if (BepInPatcher.CurrentViewingMenu[BepInPatcher.SelectedOptionIndex].DisplayName == "Username:")
                                {
                                    string keyText = key.displayName.Replace(" ", "").ToUpper(); // Remove spaces and convert to uppercase
                                    if (key == Keyboard.current.backspaceKey)
                                    {
                                        if (BepInPatcher.username.Length > 0)
                                        {
                                            BepInPatcher.username = BepInPatcher.username.Substring(0, BepInPatcher.username.Length - 1); // Remove last character
                                        }
                                    }
                                    else
                                    {
                                        BepInPatcher.username += keyText; // Append to username
                                    }
                                }
                                else if (BepInPatcher.CurrentViewingMenu[BepInPatcher.SelectedOptionIndex].DisplayName == "Password:")
                                {
                                    string keyText = key.displayName.Replace(" ", "").ToUpper(); // Remove spaces and convert to uppercase
                                    if (key == Keyboard.current.backspaceKey)
                                    {
                                        if (BepInPatcher.password.Length > 0)
                                        {
                                            BepInPatcher.password = BepInPatcher.password.Substring(0, BepInPatcher.password.Length - 1); // Remove last character
                                        }
                                    }
                                    else
                                    {
                                        BepInPatcher.password += keyText; // Append to password
                                    }
                                }
                                else if (BepInPatcher.CurrentViewingMenu[BepInPatcher.SelectedOptionIndex].DisplayName == "Key:")
                                {
                                    string keyText = key.displayName.Replace(" ", ""); // Just remove spaces (no uppercase conversion)
                                    if (key == Keyboard.current.backspaceKey)
                                    {
                                        if (BepInPatcher.key.Length > 0)
                                        {
                                            BepInPatcher.key = BepInPatcher.key.Substring(0, BepInPatcher.key.Length - 1); // Remove last character
                                        }
                                    }
                                    else
                                    {
                                        BepInPatcher.key += keyText; // Append to key
                                    }
                                }
                            }
                        }
                    }
                }
                if (current.enterKey.wasPressedThisFrame)
                {
                    BepInPatcher.UpdateMenuState(BepInPatcher.CurrentViewingMenu[BepInPatcher.SelectedOptionIndex], null, "optionhit");
                    BepInPatcher.inputcooldown = true;
                }
            }
            catch (Exception ex)
            {
                CustomConsole.Error(ex.ToString());
            }


            // Updating the stuff on the UI
            BepInPatcher.LoginMenu[0].extra = BepInPatcher.username;
            BepInPatcher.LoginMenu[1].extra = BepInPatcher.password;

            BepInPatcher.RegisterMenu[0].extra = BepInPatcher.username;
            BepInPatcher.RegisterMenu[1].extra = BepInPatcher.password;
            BepInPatcher.RegisterMenu[2].extra = BepInPatcher.key;


            // Drawing the menu
            string ToDraw = $"<color=magenta>COLOSSAL</color>\n";
            int i = 0;
            if (BepInPatcher.CurrentViewingMenu != null)
            {
                foreach (MenuOption opt in BepInPatcher.CurrentViewingMenu)
                {
                    if (BepInPatcher.SelectedOptionIndex == i)
                        ToDraw = ToDraw + "> ";
                    ToDraw = ToDraw + opt.DisplayName + " " + opt.extra;

                    if (opt._type == togglethingy)
                    {
                        if (opt.AssociatedBool == true)
                        {
                            ToDraw = ToDraw + $" <color=magenta>[ON]</color>";
                        }
                        else
                            ToDraw = ToDraw + " <color=red>[OFF]</color>";
                    }
                    ToDraw = ToDraw + "\n";
                    i++;
                }
                BepInPatcher.LoginText.text = ToDraw;
            }
            else
                CustomConsole.Error("Menu null for some reason");
        }
    }
}
