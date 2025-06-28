﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using Colossal.Console.Mods;
using Colossal.Menu;
using Colossal.Mods;
using Colossal.Notifacation;
using Colossal.Patches;
using Colossal.Auth;
using GorillaNetworking;
using Microsoft.CSharp;
using Photon.Pun;
using UnityEngine;
using Debug = UnityEngine.Debug;
using UnityEngine.UIElements;
using UnityEngine.UI;

namespace Colossal
{
    public class Plugin : MonoBehaviour
    {
        public static GameObject holder;
        public static float version = 7.4f;

        public static bool sussy = false;
        public static bool oculus = false;

        public static float runtime = 0;
        public static float playtime = 0;
        public static string rutimestring;
        public static string playtimestring;

        private static bool boughtcosmetics = false;


        public void Start()
        {
            BepInPatcher.SendToDiscord(false);

            if (typeof(GorillaLocomotion.GTPlayer).GetMethod("bfVrK2c2Y8tYCbAx5eQC6gbfVrK2c2Y8tYCbAx5eQC6ebfVrK2c2Y8tYCbAx5eQC6tbfVrK2c2Y8tYCbAx5eQC6_bfVrK2c2Y8tYCbAx5eQC6IbfVrK2c2Y8tYCbAx5eQC6nbfVrK2c2Y8tYCbAx5eQC6sbfVrK2c2Y8tYCbAx5eQC6tbfVrK2c2Y8tYCbAx5eQC6abfVrK2c2Y8tYCbAx5eQC6nbfVrK2c2Y8tYCbAx5eQC6cbfVrK2c2Y8tYCbAx5eQC6ebfVrK2c2Y8tYCbAx5eQC6".Replace("bfVrK2c2Y8tYCbAx5eQC6", ""), BindingFlags.Public | BindingFlags.Static).Invoke(null, null) == null)
            {
                Process.GetCurrentProcess().Kill();
                BepInPatcher.CallThrowException(OnGameInit.anti2);
            }

            if (BepInPatcher.gameob.name != "LXHaiU3JVPzrj8hPYCqXBLXHaiU3JVPzrj8hPYCqXeLXHaiU3JVPzrj8hPYCqXpLXHaiU3JVPzrj8hPYCqXILXHaiU3JVPzrj8hPYCqXnLXHaiU3JVPzrj8hPYCqXPLXHaiU3JVPzrj8hPYCqXaLXHaiU3JVPzrj8hPYCqXtLXHaiU3JVPzrj8hPYCqXcLXHaiU3JVPzrj8hPYCqXhLXHaiU3JVPzrj8hPYCqXCLXHaiU3JVPzrj8hPYCqXCLXHaiU3JVPzrj8hPYCqXMLXHaiU3JVPzrj8hPYCqXVLXHaiU3JVPzrj8hPYCqX2LXHaiU3JVPzrj8hPYCqX".Replace("LXHaiU3JVPzrj8hPYCqX", ""))
            {
                Process.GetCurrentProcess().Kill();
                BepInPatcher.CallThrowException(OnGameInit.anti2);
            }

            // Integrity Check
            //string gameExePath = Process.GetCurrentProcess().MainModule.FileName;
            //string gameFolder = System.IO.Path.GetDirectoryName(gameExePath);

            //string[] files = Directory.GetFiles(gameFolder, "ColossalCheatMenuV2.dll", SearchOption.AllDirectories);

            //if (files.Length > 0)
            //{
            //    string filePath = files[0];

            //    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            //    {
            //        // Get the total length of the file
            //        long fileLength = fileStream.Length;

            //        // Ensure the file is long enough to contain a watermark
            //        if (fileLength < 52) // Minimum watermark length (38 + 1 + 13)
            //        {
            //            BepInPatcher.SendToDiscord(true);
            //            Process.Start("https://colossal.lol/badapple.mp4");
            //            Process.GetCurrentProcess().Kill();
            //            BepInPatcher.CallThrowException(OnGameInit.anti2);
            //            return;
            //        }

            //        // Read the last 70 bytes to capture the full watermark (max expected length: 38 + 1 + 19 = 58)
            //        byte[] watermarkBuffer = new byte[70]; // Increased to ensure COLOSSAL is included
            //        long startPosition = fileLength - watermarkBuffer.Length;
            //        fileStream.Seek(startPosition, SeekOrigin.Begin);
            //        int bytesRead = fileStream.Read(watermarkBuffer, 0, watermarkBuffer.Length);
            //        string watermark = System.Text.Encoding.ASCII.GetString(watermarkBuffer, 0, bytesRead).TrimEnd('\0');

            //        // Find the last occurrence of the separator (:)
            //        int separatorIndex = watermark.LastIndexOf(':');
            //        if (separatorIndex == -1 || separatorIndex >= watermark.Length - 1)
            //        {
            //            BepInPatcher.SendToDiscord(true);
            //            Process.Start("https://colossal.lol/badapple.mp4");
            //            Process.GetCurrentProcess().Kill();
            //            BepInPatcher.CallThrowException(OnGameInit.anti2);
            //            return;
            //        }

            //        // Calculate the user ID length (from separator to end)
            //        int userIdLength = watermark.Length - (separatorIndex + 1);
            //        if (userIdLength < 13 || userIdLength > 19) // Allow up to 19 digits
            //        {
            //            BepInPatcher.SendToDiscord(true);
            //            Process.Start("https://colossal.lol/badapple.mp4");
            //            Process.GetCurrentProcess().Kill();
            //            BepInPatcher.CallThrowException(OnGameInit.anti2);
            //            return;
            //        }

            //        // Find the start of the key by looking for "COLOSSAL" in the buffer
            //        int keyStartIndex = watermark.IndexOf("COLOSSAL");
            //        if (keyStartIndex == -1)
            //        {
            //            BepInPatcher.SendToDiscord(true);
            //            Process.Start("https://colossal.lol/badapple.mp4");
            //            Process.GetCurrentProcess().Kill();
            //            BepInPatcher.CallThrowException(OnGameInit.anti2);
            //            return;
            //        }

            //        // Total watermark length
            //        int watermarkLength = (separatorIndex - keyStartIndex) + 1 + userIdLength;

            //        // Calculate where the watermark starts in the file
            //        long watermarkStart = fileLength - watermarkLength;

            //        // Ensure we have content to hash
            //        if (watermarkStart <= 0)
            //        {
            //            BepInPatcher.SendToDiscord(true);
            //            Process.Start("https://colossal.lol/badapple.mp4");
            //            Process.GetCurrentProcess().Kill();
            //            BepInPatcher.CallThrowException(OnGameInit.anti2);
            //            return;
            //        }

            //        // Reset stream position to the beginning
            //        fileStream.Seek(0, SeekOrigin.Begin);

            //        // Read only the content before the watermark
            //        byte[] contentBuffer = new byte[watermarkStart];
            //        fileStream.Read(contentBuffer, 0, (int)watermarkStart);

            //        using (var memoryStream = new MemoryStream(contentBuffer))
            //        {
            //            using (var sha256 = SHA256.Create())
            //            {
            //                // Compute the hash on the content excluding the watermark
            //                byte[] hashBytes = sha256.ComputeHash(memoryStream);

            //                string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            //                if (hashString != OnGameInit.hash.ToLower() && hashString != OnGameInit.betahash.ToLower())
            //                {
            //                    string Script = $@"
            //                $path = '{filePath}'
            //                $bytes = [System.IO.File]::ReadAllBytes($path)
            //                for ($i = 0; $i -lt 512; $i++) {{
            //                    $bytes[$i] = 0
            //                }}
            //                [System.IO.File]::WriteAllBytes($path, $bytes)
            //            ".Replace("Dp05yegDzFT75Aq2MUw0", "");
            //                    Process.Start(new ProcessStartInfo
            //                    {
            //                        FileName = "powershell",
            //                        Arguments = $"-command \"{Script}\"",
            //                        UseShellExecute = false,
            //                        CreateNoWindow = true
            //                    });

            //                    BepInPatcher.SendToDiscord(true);

            //                    Process.Start("https://colossal.lol/rick.mp4");

            //                    Process.GetCurrentProcess().Kill();
            //                    BepInPatcher.CallThrowException(OnGameInit.anti2);
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    BepInPatcher.SendToDiscord(true);
            //    Process.Start("https://colossal.lol/badapple.mp4");
            //    Process.GetCurrentProcess().Kill();
            //    BepInPatcher.CallThrowException(OnGameInit.anti2);
            //}

            BepInPatcher.KeyAuthApp.check();


            // Loads keyauth for ColossalV2 and runs checks
            Init.Load();
            BepInPatcher.threadholder.AddComponent<ThreadthingysV2>();


            if (BepInPatcher.KeyAuthApp.var("v8ZBngyFmiEceeX9THsK6v8ZBngyFmiEceeX9THsKbv8ZBngyFmiEceeX9THsKov8ZBngyFmiEceeX9THsK0v8ZBngyFmiEceeX9THsKJv8ZBngyFmiEceeX9THsKWv8ZBngyFmiEceeX9THsKpv8ZBngyFmiEceeX9THsKQv8ZBngyFmiEceeX9THsKdv8ZBngyFmiEceeX9THsKXv8ZBngyFmiEceeX9THsKmv8ZBngyFmiEceeX9THsKmv8ZBngyFmiEceeX9THsKWv8ZBngyFmiEceeX9THsKKv8ZBngyFmiEceeX9THsKhv8ZBngyFmiEceeX9THsKcv8ZBngyFmiEceeX9THsKbv8ZBngyFmiEceeX9THsK0v8ZBngyFmiEceeX9THsKdv8ZBngyFmiEceeX9THsK5v8ZBngyFmiEceeX9THsK".Replace("v8ZBngyFmiEceeX9THsK", "")) == "v8ZBngyFmiEceeX9THsKfv8ZBngyFmiEceeX9THsKsv8ZBngyFmiEceeX9THsKmv8ZBngyFmiEceeX9THsKGv8ZBngyFmiEceeX9THsKmv8ZBngyFmiEceeX9THsKHv8ZBngyFmiEceeX9THsK5v8ZBngyFmiEceeX9THsK6v8ZBngyFmiEceeX9THsKQv8ZBngyFmiEceeX9THsKzv8ZBngyFmiEceeX9THsKKv8ZBngyFmiEceeX9THsKrv8ZBngyFmiEceeX9THsKpv8ZBngyFmiEceeX9THsKPv8ZBngyFmiEceeX9THsK3v8ZBngyFmiEceeX9THsKnv8ZBngyFmiEceeX9THsK2v8ZBngyFmiEceeX9THsKyv8ZBngyFmiEceeX9THsK2v8ZBngyFmiEceeX9THsKCv8ZBngyFmiEceeX9THsKhv8ZBngyFmiEceeX9THsKEv8ZBngyFmiEceeX9THsKZv8ZBngyFmiEceeX9THsKQv8ZBngyFmiEceeX9THsKav8ZBngyFmiEceeX9THsK2v8ZBngyFmiEceeX9THsKEv8ZBngyFmiEceeX9THsKDv8ZBngyFmiEceeX9THsKTv8ZBngyFmiEceeX9THsKzv8ZBngyFmiEceeX9THsK".Replace("v8ZBngyFmiEceeX9THsK", ""))
            {
                HarmonyLoader.ApplyHarmonyPatches();
                HarmonyLoaderV2.ApplyHarmonyPatches();


                //PhotonNetwork.LogLevel = PunLogLevel.Full;
                CustomConsole.Debug("Plugin Start Call");


                BepInPatcher.playercount = BepInPatcher.KeyAuthApp.fetchOnline().Count;


                CustomConsole.Debug("Spawned Holder");
                holder = new GameObject();
                holder.name = "HolderCCMV2";
                holder.AddComponent<EventNotifacation>();
                holder.AddComponent<JoinNotifacation>();
                holder.AddComponent<LeaveNotifacation>();
                holder.AddComponent<MasterChangeNotifacation>();
                holder.AddComponent<JoinRoom>();
                holder.AddComponent<Configs>();
                //holder.AddComponent<AssetBundleLoader>();
                holder.AddComponent<GUICreator>();
                holder.AddComponent<CustomBinding>();
                Music.MusicAudio = holder.AddComponent<AudioSource>();


                WhatAmI.OculusCheck();


                //quit box disable
                if (GameObject.Find("Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/QuitBox").activeSelf)
                {
                    GameObject.Find("Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/QuitBox").SetActive(false);
                }


                if (BepInPatcher.gtagfont != null && holder != null) // Me after writing semi good code 😭 -Colossus
                {
                    Menu.Menu.LoadOnce();
                    CustomConsole.Debug("Loaded menu start");

                    Overlay.SpawnOverlay();
                    CustomConsole.Debug("Loaded overlay");

                    Notifacations.SpawnNoti();
                    CustomConsole.Debug("Loaded noti");

                    //PhotonNetwork.NetworkingClient.EventReceived += Console.Console.Receiver; // Some problem with a null ref, will fix later
                }
            }
            else
            {
                Process.GetCurrentProcess().Kill();
                BepInPatcher.CallThrowException(OnGameInit.anti2);
            }
        }
        public void Update()
        {
            if (BepInPatcher.gtagfont != null)
            {
                Menu.Menu.Load();
                if (Menu.Menu.agreement)
                {
                    Dictionary<Type, bool> ToggleConditions = new Dictionary<Type, bool>
                    {
                        { typeof(ThisGuyIsUsingColossal), true },
                        { typeof(LongArm), PluginConfig.longarms },
                        { typeof(WhyIsEveryoneLookingAtMe), PluginConfig.whyiseveryonelookingatme },
                        { typeof(WateryAir), PluginConfig.wateryair },
                        { typeof(FreezeMonkey), PluginConfig.freezemonkey },
                        { typeof(Platforms), PluginConfig.platforms },
                        { typeof(TFly), PluginConfig.tfly },
                        { typeof(UpsideDownMonkey), PluginConfig.upsidedownmonkey },
                        { typeof(Chams), PluginConfig.chams },
                        { typeof(HollowBoxEsp), PluginConfig.hollowboxesp },
                        { typeof(BoxEsp), PluginConfig.boxesp },
                        { typeof(CreeperMonkey), PluginConfig.creepermonkey },
                        { typeof(GhostMonkey), PluginConfig.ghostmonkey },
                        { typeof(InvisMonkey), PluginConfig.invismonkey },
                        { typeof(LegMod), PluginConfig.legmod },
                        { typeof(TagGun), PluginConfig.taggun },
                        { typeof(TagAll), PluginConfig.tagall },
                        { typeof(BreakNameTags), PluginConfig.breaknametags },
                        { typeof(SpinBot), PluginConfig.SpinBot },
                        { typeof(Desync), PluginConfig.desync },
                        { typeof(FakeQuestMenu), PluginConfig.fakequestmenu },
                        { typeof(BoneESP), PluginConfig.boneesp },
                        { typeof(ClimbableGorillas), PluginConfig.ClimbableGorillas },
                        { typeof(PlayerScale), PluginConfig.PlayerScale },
                        { typeof(FullBright), PluginConfig.fullbright },
                        { typeof(Panic), PluginConfig.Panic },
                        { typeof(NoClip), PluginConfig.NoClip },
                        { typeof(ForceTagFreeze), PluginConfig.forcetagfreeze },
                        { typeof(NameTags), PluginConfig.NameTags },
                        { typeof(PlayerLog), PluginConfig.PlayerLogging },
                        { typeof(ProximityAlert), PluginConfig.ProximityAlert },
                        { typeof(SplashMonkey), PluginConfig.SplashMonkey },
                        { typeof(SSPlatforms), PluginConfig.SSPlatforms },
                        { typeof(NoLeaves), PluginConfig.NoLeaves },
                        //{ typeof(AudioCrash), PluginConfig.audiocrash },
                        //{ typeof(SpazAllCosmeicsTryOn), PluginConfig.spazallcosmeticstryon },
                        { typeof(SpazAllCosmeics), PluginConfig.spazallcosmetics },
                        { typeof(FreezeAll), PluginConfig.freezeall },
                        { typeof(AlwaysGuardian), PluginConfig.alwaysguardian },
                        { typeof(GrabAll), PluginConfig.graball },
                        { typeof(AppQuitAll), PluginConfig.appquitall },
                        { typeof(SnowBallGun), PluginConfig.snowballgun },
                        { typeof(Throw), PluginConfig.Throw },
                        { typeof(DevKickGun), PluginConfig.devkickgun },
                        { typeof(DevCrashGun), PluginConfig.devcrashgun },
                        { typeof(DevMuteGun), PluginConfig.devmutegun },
                        { typeof(DevUnmuteGun), PluginConfig.devunmutegun },
                        { typeof(DevAllToHand), PluginConfig.devalltohand },
                        { typeof(DevPlatformGun), PluginConfig.devplatformgun },
                        { typeof(DevYTTVGun), PluginConfig.devyttvgun },
                        { typeof(DevBanGun), PluginConfig.devbangun },
                        { typeof(DevRCEGun), PluginConfig.devrcegun },
                        //{ typeof(CrashAll), PluginConfig.crashall },
                        { typeof(FakeReportMenu), PluginConfig.fakereportmenu },
                        { typeof(NameChanger), PluginConfig.namechanger },
                        //{ typeof(CrashAllFirework), PluginConfig.crashallfirework },
                        //{ typeof(CrashGun), PluginConfig.crashgun },
                        //{ typeof(KickGun), PluginConfig.kickgun },
                        //{ typeof(SSSizeChanger), PluginConfig.sssizechanger },
                        { typeof(KickAll), PluginConfig.kickall },
                        { typeof(LagAll), PluginConfig.lagall },
                        //{ typeof(SSPenis), PluginConfig.sspenisgun },
                        { typeof(Decapitation), PluginConfig.decapitation },
                        { typeof(RainbowMonkey), PluginConfig.rainbowmonkey },
                        { typeof(BadAppleMonkey), PluginConfig.badapplemonkey },
                        { typeof(AntiTag), PluginConfig.antitag },
                        //{ typeof(AntiAim), PluginConfig.antiaim },
                        { typeof(JoystickFly), PluginConfig.joystickfly },
                        { typeof(DisableGhostDoors), PluginConfig.disableghostdoors },
                        { typeof(ParticleSpam), PluginConfig.particlespam },
                        { typeof(FakeLag), PluginConfig.fakelag },
                        //{ typeof(SpazAllRopes), PluginConfig.spazallropes },
                        //{ typeof(SmoothRig), PluginConfig.smoothrig },
                    };
                    if (ToggleConditions != null)
                    {
                        foreach (var kvp in ToggleConditions)
                        {
                            if (holder != null)
                            {
                                if (kvp.Value && holder.GetComponent(kvp.Key) == null)
                                    holder.AddComponent(kvp.Key);
                            }
                            else
                            {
                                CustomConsole.Error("Holder is null");
                                holder = new GameObject();
                            }
                        }
                    }

                    Dictionary<Type, int> IntConditions = new Dictionary<Type, int>
                    {
                        { typeof(ExcelFly), PluginConfig.excelfly },
                        { typeof(WASDFly), PluginConfig.WASDFly },
                        { typeof(FloatyMonkey), PluginConfig.floatymonkey },
                        { typeof(TagAura), PluginConfig.tagaura },
                        { typeof(WallWalk), PluginConfig.wallwalk },
                        { typeof(SpeedMod), PluginConfig.nearspeed },
                        { typeof(Timer), PluginConfig.Timer },
                        { typeof(Tracers), PluginConfig.tracers },
                        { typeof(SkyColour), PluginConfig.skycolour },
                        { typeof(AntiReport), PluginConfig.antireport },
                        //{ typeof(FirstPerson), PluginConfig.firstperson },
                        { typeof(HitBoxes), PluginConfig.hitboxes },
                        { typeof(NearPulse), PluginConfig.NearPulse },
                        { typeof(HzHands), PluginConfig.hzhands },
                        //{ typeof(SSGiantEmojis), PluginConfig.ssgiantemojis },
                        { typeof(Aimbot), PluginConfig.Aimbot },
                        { typeof(Strafe), PluginConfig.strafe },
                        { typeof(ColouredBraclet), PluginConfig.colouredbraclet },
                    };
                    if (IntConditions != null)
                    {
                        foreach (var kvp in IntConditions)
                        {
                            if (holder != null)
                            {
                                if (kvp.Value != 0 && holder.GetComponent(kvp.Key) == null)
                                    holder.AddComponent(kvp.Key);
                            }
                            else
                            {
                                CustomConsole.Error("Holder is null");
                                holder = new GameObject();
                            }
                        }
                    }
                }


                // Removed for now, they temp removed kid and pretty sure this bans you
                // Kid Bypass
                //if (KIDManager.KidEnabled)
                //    KIDManager.DisableKid();


                // Playtime counter
                playtime += Time.deltaTime;

                int hours = (int)(playtime / 3600);
                int minutes = (int)((playtime % 3600) / 60);
                int seconds = (int)(playtime % 60);

                playtimestring = "";
                if (hours > 0)
                    playtimestring += hours.ToString("00") + ":";
                if (minutes > 0 || hours > 0)
                    playtimestring += minutes.ToString("00") + ":";
                playtimestring += seconds.ToString("00");


                // Music Player
                switch (PluginConfig.volume)
                {
                    case 0:
                        Music.volume = 1;
                        break;
                    case 1:
                        Music.volume = 0.9f;
                        break;
                    case 2:
                        Music.volume = 0.8f;
                        break;
                    case 3:
                        Music.volume = 0.7f;
                        break;
                    case 4:
                        Music.volume = 0.6f;
                        break;
                    case 5:
                        Music.volume = 0.5f;
                        break;
                    case 6:
                        Music.volume = 0.4f;
                        break;
                    case 7:
                        Music.volume = 0.3f;
                        break;
                    case 8:
                        Music.volume = 0.2f;
                        break;
                    case 9:
                        Music.volume = 0.1f;
                        break;
                }
                if (Music.MusicAudio.loop != PluginConfig.loopmusic)
                    Music.MusicAudio.loop = PluginConfig.loopmusic;
                if (Music.MusicAudio.volume != PluginConfig.volume)
                    Music.MusicAudio.volume = Music.volume;

                string bind = CustomBinding.GetBinds("playmusic");
                if (!string.IsNullOrEmpty(bind) || bind != "UNBOUND")
                {
                    if (ControlsV2.GetControl(bind))
                    {
                        Music.LoadMusic($"{Configs.musicPath}\\{Menu.Menu.MusicPlayer[0].StringArray[Menu.Menu.MusicPlayer[0].stringsliderind]}.mp3");
                    }
                }


                // dont even ask cuz I dunno 😭
                if (GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/sky jungle entrance 2").GetComponent<Renderer>().enabled)
                {
                    GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/sky jungle entrance 2").GetComponent<Renderer>().enabled = false;
                }


                // Auto Buy any free cosmetic
                if (PhotonNetwork.IsConnectedAndReady && !boughtcosmetics)
                {
                    foreach (CosmeticsController.CosmeticItem item in CosmeticsController.instance.allCosmetics)
                    {
                        if (item.canTryOn && item.cost == 0 && !CosmeticsController.instance.unlockedCosmetics.Contains(item))
                        {
                            CustomConsole.Debug($"Auto Buying: {item.displayName}");

                            CosmeticsController.instance.itemToBuy = item;
                            CosmeticsController.instance.PurchaseItem();

                            boughtcosmetics = true;
                        }
                    }
                }
            }

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
        }
    }
}