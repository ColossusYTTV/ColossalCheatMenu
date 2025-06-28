using System.Diagnostics;
using System.Reflection;
using Colossal.Patches;
using HarmonyLib;
using UnityEngine;

namespace Colossal
{
    public class HarmonyLoaderV2
    {
        public static bool IsPatched { get; private set; }
        private static Harmony instance;
        public const string InstanceId = "org.Colossal";

        public static void ApplyHarmonyPatches()
        {
            if (!IsPatched)
            {
                if (instance == null)
                {
                    instance = new Harmony("org.Colossal");
                }
                instance.PatchAll(Assembly.GetExecutingAssembly());
                IsPatched = true;
            }
        }

        public static void RemoveHarmonyPatches()
        {
            if (instance != null && IsPatched)
            {
                instance.UnpatchSelf();
                IsPatched = false;
            }
        }
    }
}
