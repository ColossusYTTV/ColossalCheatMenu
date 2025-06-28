﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Colossal.Auth
{
    public class ThreadthingysV2 : MonoBehaviour
    {
        public static ThreadthingysV2 instance;

        public Coroutine AntiCoroutine { get; private set; }
        public Coroutine KillCoroutine { get; private set; }
        public Coroutine PingCoroutine { get; private set; }

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
                            Init.ThrowException();
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
                    if (Init.logginIn)
                    {
                        Init.KeyAuthApp.check();
                        if (!Init.KeyAuthApp.response.success)
                        {
                            Process.GetCurrentProcess().Kill();
                            Init.ThrowException();
                        }
                    }
                });

                yield return new WaitForSeconds(800);
            }
        }

        private IEnumerator Kill()
        {
            while (true)
            {
                Task.Run(() =>
                {
                    var killSwitchValue = Init.KeyAuthApp.var("P2VzoWPiMZsDeD2pDJPwKP2VzoWPiMZsDeD2pDJPwiP2VzoWPiMZsDeD2pDJPwlP2VzoWPiMZsDeD2pDJPwlP2VzoWPiMZsDeD2pDJPwSP2VzoWPiMZsDeD2pDJPwwP2VzoWPiMZsDeD2pDJPwiP2VzoWPiMZsDeD2pDJPwtP2VzoWPiMZsDeD2pDJPwcP2VzoWPiMZsDeD2pDJPwhP2VzoWPiMZsDeD2pDJPw".Replace("P2VzoWPiMZsDeD2pDJPw", ""));
                    if (killSwitchValue == null)
                    {
                        Process.GetCurrentProcess().Kill();
                        Init.ThrowException();
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
                            Init.ThrowException();
                        }
                    }
                });

                yield return new WaitForSeconds(1000);
            }
        }
    }
}
