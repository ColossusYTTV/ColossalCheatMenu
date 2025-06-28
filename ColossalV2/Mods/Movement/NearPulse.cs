﻿using Colossal.Menu;
using Colossal.Mods;
using Colossal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Photon.Pun;

namespace Colossal.Mods
{
    public class NearPulse : MonoBehaviour
    {
        public void FixedUpdate()
        {
            if (PluginConfig.NearPulse != 0)
            {
                bool infected = WhatAmI.IsInfected(GorillaTagger.Instance.myVRRig.Owner);

                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (!vrrig.isOfflineVRRig)
                    {
                        float distance = Vector3.Distance(GorillaTagger.Instance.transform.position, vrrig.transform.position);

                        if (!infected)
                        {
                            if (WhatAmI.IsInfected(vrrig.Creator) && distance <= PluginConfig.NearPulseDistance)
                            {
                                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddExplosionForce(PluginConfig.NearPulse * 20, vrrig.transform.position, PluginConfig.NearPulseDistance);
                            }
                        }
                        else
                        {
                            if (!WhatAmI.IsInfected(vrrig.Creator) && distance <= PluginConfig.NearPulseDistance)
                            {
                                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddExplosionForce(-PluginConfig.NearPulse * 20, vrrig.transform.position, PluginConfig.NearPulseDistance);
                            }
                        }
                    }
                }
                //can someone fix this -Starry
                // Fixed it pookie <333 -Colossus
            }
            else
            {
                Destroy(this.GetComponent<NearPulse>());
            }
        }
    }
}