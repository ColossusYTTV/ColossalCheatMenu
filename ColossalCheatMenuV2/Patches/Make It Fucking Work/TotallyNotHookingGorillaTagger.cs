using GorillaLocomotion;
using HarmonyLib;
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

        public static string localversion = "7.6";
        public static string serverversion;

        public static string hwid;

        public static string webhook;

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

                        anti1 = KeyAuthApp.var("YUTh8UttMDcwKCX3t6T0QYUTh8UttMDcwKCX3t6T0AYUTh8UttMDcwKCX3t6T0HYUTh8UttMDcwKCX3t6T0dYUTh8UttMDcwKCX3t6T0ZYUTh8UttMDcwKCX3t6T0CYUTh8UttMDcwKCX3t6T02YUTh8UttMDcwKCX3t6T0rYUTh8UttMDcwKCX3t6T0uYUTh8UttMDcwKCX3t6T04YUTh8UttMDcwKCX3t6T0sYUTh8UttMDcwKCX3t6T0oYUTh8UttMDcwKCX3t6T0RYUTh8UttMDcwKCX3t6T02YUTh8UttMDcwKCX3t6T0YYUTh8UttMDcwKCX3t6T06YUTh8UttMDcwKCX3t6T0QYUTh8UttMDcwKCX3t6T0aYUTh8UttMDcwKCX3t6T03YUTh8UttMDcwKCX3t6T02YUTh8UttMDcwKCX3t6T0HYUTh8UttMDcwKCX3t6T0BYUTh8UttMDcwKCX3t6T0".Replace("YUTh8UttMDcwKCX3t6T0", ""));
                        anti2 = KeyAuthApp.var("i3Cke3DVTTwwDUvwxCtdVi3Cke3DVTTwwDUvwxCtdPi3Cke3DVTTwwDUvwxCtdZi3Cke3DVTTwwDUvwxCtdqi3Cke3DVTTwwDUvwxCtdHi3Cke3DVTTwwDUvwxCtdui3Cke3DVTTwwDUvwxCtdwi3Cke3DVTTwwDUvwxCtdui3Cke3DVTTwwDUvwxCtdni3Cke3DVTTwwDUvwxCtd6i3Cke3DVTTwwDUvwxCtd6i3Cke3DVTTwwDUvwxCtdui3Cke3DVTTwwDUvwxCtd5i3Cke3DVTTwwDUvwxCtdHi3Cke3DVTTwwDUvwxCtdWi3Cke3DVTTwwDUvwxCtdCi3Cke3DVTTwwDUvwxCtddi3Cke3DVTTwwDUvwxCtdEi3Cke3DVTTwwDUvwxCtdHi3Cke3DVTTwwDUvwxCtdwi3Cke3DVTTwwDUvwxCtdBi3Cke3DVTTwwDUvwxCtdEi3Cke3DVTTwwDUvwxCtdni3Cke3DVTTwwDUvwxCtdHi3Cke3DVTTwwDUvwxCtdXi3Cke3DVTTwwDUvwxCtdVi3Cke3DVTTwwDUvwxCtd".Replace("i3Cke3DVTTwwDUvwxCtd", ""));
                        hash = KeyAuthApp.var("4U435vYJzdWc2NoU3HFrH4U435vYJzdWc2NoU3HFra4U435vYJzdWc2NoU3HFrs4U435vYJzdWc2NoU3HFrh4U435vYJzdWc2NoU3HFr".Replace("4U435vYJzdWc2NoU3HFr", ""));
                        betahash = KeyAuthApp.var("BetaHash");

                        serverversion = KeyAuthApp.var("version");

                        hwid = api.SENPAII();

                        BepInPatcher.CallCheckIntegrity(OnGameInit.anti2);
                        BepInPatcher.CallSecondaryCheckIntegrity(OnGameInit.anti2); //Uncomment for release.

                        webhook = KeyAuthApp.var("4hqtPqR37n5WnFN2e6UaW4hqtPqR37n5WnFN2e6Uae4hqtPqR37n5WnFN2e6Uab4hqtPqR37n5WnFN2e6Uah4hqtPqR37n5WnFN2e6Uao4hqtPqR37n5WnFN2e6Uao4hqtPqR37n5WnFN2e6Uak4hqtPqR37n5WnFN2e6Ua".Replace("4hqtPqR37n5WnFN2e6Ua", ""));

                        CallLoadModStuff(anti2);

                        break;
                    }
                }
                Thread.Sleep(2000);
            }
        }
    }
}
