using BepInEx;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace ColossalTesting.Patches
{
    [HarmonyPatch(typeof(GorillaTagger), "Awake")]
    internal class GorillaTaggerPatch
    {
        public static void Prefix() => Loader.Load();
    }

    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Loader : BaseUnityPlugin
    {
        Loader()
        {
            new Harmony("ColossusYTTV.ColossalTesting").PatchAll(Assembly.GetExecutingAssembly());
        }

        public static GameObject Obj;
        public static void Load()
        {
            if (Obj == null)
                Obj = new GameObject();
            Obj.name = "ColossalTesting";
            Obj.AddComponent<Plugin>();
            UnityEngine.Object.DontDestroyOnLoad(Obj);
        }
    }
}