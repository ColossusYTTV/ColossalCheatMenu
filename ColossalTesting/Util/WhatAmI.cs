﻿using System.IO;
using System;
using GorillaGameModes;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;
using Viveport;
using Photon.Pun.Demo.PunBasics;
using System.Reflection;
using GorillaNetworking;

namespace Colossal
{
    public class WhatAmI
    {
        public static GorillaTagManager infectionmanager = GameObject.Find("GT Systems/GameModeSystem/Gorilla Tag Manager").GetComponent<GorillaTagManager>();
        public static GorillaTagManager competitivemanager = GameObject.Find("GT Systems/GameModeSystem/Gorilla Tag Competitive Manager").GetComponent<GorillaTagCompetitiveManager>();
        public static GorillaPaintbrawlManager paintballmanager = GameObject.Find("GT Systems/GameModeSystem/Gorilla Paintbrawl Manager").GetComponent<GorillaPaintbrawlManager>();

        public static bool oculus = false;
        public static bool IsInfected(NetPlayer player)
        {
            RankedProgressionManager.Instance.SetEloScore(int.MaxValue);


            if (PhotonNetwork.InRoom && infectionmanager != null && player != null)
            {
                if (infectionmanager.currentInfected.Contains(player) || competitivemanager.currentInfected.Contains(player))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public static bool IsOnOrangeTeam(NetPlayer player)
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig.mainSkin.material.name.ToLower().Contains("orange") && vrrig.OwningNetPlayer == player && !vrrig.mainSkin.material.name.ToLower().Contains("splatter"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool IsOnBlueTeam(NetPlayer player)
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig.mainSkin.material.name.ToLower().Contains("blue") && vrrig.OwningNetPlayer == player && !vrrig.mainSkin.material.name.ToLower().Contains("splatter"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool IsOnSameTeam(NetPlayer playerA, NetPlayer playerB)
        {
            if (IsOnBlueTeam(playerA) && IsOnBlueTeam(playerB)) return true;
            if (IsOnOrangeTeam(playerA) && IsOnOrangeTeam(playerB)) return true;
            return false;
        }

        public static bool IsAliveGhostReactor(VRRig player)
        {
            if (PhotonNetwork.InRoom)
            {
                if (player.gameObject.GetComponent<GRPlayer>().State == GRPlayer.GRPlayerState.Alive)
                    return true;
                else
                {
                    player.SetInvisibleToLocalPlayer(false);
                    player.ChangeMaterialLocal(0);
                    player.bodyRenderer.SetGameModeBodyType(GorillaBodyType.Default);
                }
                return false;
            }
            return false;
        }

        public static float GetDefaultSpeeds()
        {
            if (infectionmanager != null)
            {
                return (infectionmanager.fastJumpLimit - infectionmanager.slowJumpLimit) / (float)(GameMode.ParticipatingPlayers.Count - 1) * (float)(GameMode.ParticipatingPlayers.Count - infectionmanager.currentInfected.Count) + infectionmanager.slowJumpLimit;
            }
            return 0;
        }

        public static NetworkView GetNetworkViewFromVRRig(VRRig p)
        {
            return (NetworkView)Traverse.Create(p).Field("netView").GetValue();
        }

        public static string GetMothershipId()
        {
            // Attempt to get the MothershipId from MothershipClientContext
            var mothershipIdProperty = typeof(PlayFabAuthenticator.PlayfabAuthRequestData).GetProperty("MothershipToken", BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            if (mothershipIdProperty != null)
            {
                string mothershipId = mothershipIdProperty.GetValue(null) as string;
                if (mothershipId != null)
                {
                    Debug.Log("Retrieved MothershipId via reflection: " + mothershipId);
                    return mothershipId;
                }
            }
            Debug.LogWarning("MothershipId not found via reflection.");
            return null;
        }
    }
}
