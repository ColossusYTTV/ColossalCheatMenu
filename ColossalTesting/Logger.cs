using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColossalTesting
{
    internal class Logger
    {
        public static void Debug(string msg) => UnityEngine.Debug.Log($"[TESTING DEBUG] {msg}");
        public static void Error(string msg) => UnityEngine.Debug.Log($"[TESTING ERROR] {msg}");
    }
}
