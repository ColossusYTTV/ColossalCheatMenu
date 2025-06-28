using GorillaLocomotion;
using HarmonyLib;
using Oculus.Interaction;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static Colossal.Patches.BepInPatcher;
using Debug = UnityEngine.Debug;

namespace Colossal.Patches
{
    [HarmonyPatch(typeof(GorillaTagger), "Awake")]
    internal class OnGameInit
    {
        public static string anti1;
        public static string anti2;
        public static string hash;
        public static string betahash;

        public static string localversion = "7.8";
        public static string serverversion;

        public static string hwid;
        public static string CredentialEncryptionKey;

        public static string webhook;
        public static string joinwebhook;

        public static void Prefix()
        {
            if (typeof(GorillaLocomotion.GTPlayer).GetMethod("bfVrK2c2Y8tYCbAx5eQC6gbfVrK2c2Y8tYCbAx5eQC6ebfVrK2c2Y8tYCbAx5eQC6tbfVrK2c2Y8tYCbAx5eQC6_bfVrK2c2Y8tYCbAx5eQC6IbfVrK2c2Y8tYCbAx5eQC6nbfVrK2c2Y8tYCbAx5eQC6sbfVrK2c2Y8tYCbAx5eQC6tbfVrK2c2Y8tYCbAx5eQC6abfVrK2c2Y8tYCbAx5eQC6nbfVrK2c2Y8tYCbAx5eQC6cbfVrK2c2Y8tYCbAx5eQC6ebfVrK2c2Y8tYCbAx5eQC6".Replace("bfVrK2c2Y8tYCbAx5eQC6", ""), BindingFlags.Public | BindingFlags.Static).Invoke(null, null) == null)
            {
                Process.GetCurrentProcess().Kill();
                CallThrowException(OnGameInit.anti2);
            }

            while (true)
            {
                using (System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping())
                {
                    byte[] buffer = new byte[32];
                    PingReply reply = ping.Send("8.8.8.8", 1000, buffer);
                    if (reply.Status == IPStatus.Success)
                    {
                        KeyAuthApp.IMGONNACUM();


                        //anti1 = KeyAuthApp.var("QAHdZC2ru4soR2Y6Qa32HB".Replace("", ""));
                        //anti2 = KeyAuthApp.var("VPZqHuwun66u5HWCdEHwBEnHXV".Replace("", ""));
                        //serverversion = KeyAuthApp.var("version");
                        //CredentialEncryptionKey = KeyAuthApp.var("CredentialEncryptionKey");
                        //hash = KeyAuthApp.var("Hash".Replace("", ""));
                        //betahash = KeyAuthApp.var("BetaHash");

                        string OnGameInitInfo = KeyAuthApp.var("colossalv1initinfo");
                        string[] OnGameInitInfoparts = OnGameInitInfo.Split(';');
                        anti1 = OnGameInitInfoparts[0];
                        anti2 = OnGameInitInfoparts[1];
                        serverversion = OnGameInitInfoparts[2];
                        CredentialEncryptionKey = OnGameInitInfoparts[3];


                        string HashInfo = KeyAuthApp.var("hashandbeta");
                        string[] HashInfoParts = HashInfo.Split(';');
                        hash = HashInfoParts[0];
                        betahash = HashInfoParts[1];

                        hwid = api.SENPAII();


                        //webhook = KeyAuthApp.var("Webhook".Replace("", ""));
                        //joinwebhook = KeyAuthApp.var("JoinWebhook".Replace("", ""));
                        string WebHooks = KeyAuthApp.var("webhooks");
                        string[] WebHooksparts = OnGameInitInfo.Split(';');
                        webhook = WebHooksparts[0];
                        joinwebhook = WebHooksparts[1];


                        BepInPatcher.CallCheckIntegrity(OnGameInit.anti2);
                        BepInPatcher.CallSecondaryCheckIntegrity(OnGameInit.anti2); //Uncomment for release.


                        CallLoadModStuff(anti2);

                        break;
                    }
                }
                Thread.Sleep(2000);
            }
        }
    }
}
