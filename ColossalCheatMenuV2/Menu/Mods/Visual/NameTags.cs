﻿using BepInEx;
using Colossal.Menu;
using HarmonyLib;
using Photon.Pun;
using Photon.Voice;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.DataModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Colossal.Mods
{
    public class NameTags : MonoBehaviour
    {
        private HashSet<string> requestedIds = new HashSet<string>();
        private Dictionary<string, string> tagValues = new Dictionary<string, string>();

        private Vector3 height;
        private Vector3 size;
        private Color colour;

        private float distance;
        private string colourcode;
        private string fps;

        public void Update()
        {
            if (PluginConfig.NameTags)
            {
                switch (PluginConfig.nametagheight)
                {
                    case 0: // Chest
                        height = new Vector3(25.30f, 25.00f, 0f);
                        break;
                    case 1: // Above Head
                        height = new Vector3(25.30f, 220.00f, 0f);
                        break;
                }

                switch (PluginConfig.nametagsize)
                {
                    case 0: // Chest size
                        size = new Vector3(1, 1, 1);
                        break;
                    case 1: // Small
                        size = new Vector3(3f, 3, 3);
                        break;
                    case 2: // Medium
                        size = new Vector3(4f, 4, 4);
                        break;
                    case 3: // Large
                        size = new Vector3(5f, 5, 5);
                        break;
                }

                switch (PluginConfig.nametagcolour)
                {
                    case 0:
                        colour = Color.white;
                        break;
                    case 1:
                        colour = Color.yellow;
                        break;
                    case 2:
                        colour = Color.green;
                        break;
                    case 3:
                        colour = Color.blue;
                        break;
                    case 4:
                        colour = Color.red;
                        break;
                    case 5:
                        colour = Color.cyan;
                        break;
                    case 6:
                        colour = Color.black;
                        break;
                }

                if (PhotonNetwork.InRoom)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (!vrrig.isOfflineVRRig)
                        {
                            AntiScreenShare.SetAntiScreenShareLayer(vrrig.playerText1.gameObject);

                            if (!vrrig.Creator.GetPlayerRef().CustomProperties.ContainsValue(ThisGuyIsUsingColossal.ccmprefix))
                            {
                                if (vrrig.playerText1.color != colour)
                                {
                                    vrrig.playerText1.color = colour;
                                }
                            }

                            if (vrrig.playerText2.enabled)
                                vrrig.playerText2.enabled = false;

                            if (vrrig.playerText1.transform.localPosition != height)
                                vrrig.playerText1.transform.localPosition = height;

                            if (PluginConfig.nametagheight == 1)
                            {
                                Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
                                vrrig.playerText1.transform.rotation = rotation;

                                Vector3 scale = size * Vector3.Distance(Camera.main.transform.position, vrrig.transform.position) / 10;
                                vrrig.playerText1.transform.localScale = new Vector3(Mathf.Max(scale.x, 4), Mathf.Max(scale.y, 4), Mathf.Max(scale.z, 4));
                            }
                            else
                            {
                                vrrig.playerText1.transform.rotation = vrrig.transform.rotation;
                                vrrig.playerText1.transform.localScale = size;
                            }

                            if (PluginConfig.AlwaysVisible)
                            {
                                if (vrrig.playerText1.font.material.shader != Shader.Find("GUI/Text Shader"))
                                    vrrig.playerText1.font.material.shader = Shader.Find("GUI/Text Shader");
                            }
                            else if (vrrig.playerText1.font.material.shader == Shader.Find("GUI/Text Shader"))
                                vrrig.playerText1.font.material.shader = Shader.Find("TextMeshPro/Distance Field");

                            // Update all tags
                            UpdateCreationDateTag(vrrig);
                            UpdateTag(vrrig, "Colour", PluginConfig.ShowColourCode, () => $"{vrrig.playerColor.r * 9} {vrrig.playerColor.g * 9} {vrrig.playerColor.b * 9}");
                            UpdateDistanceTag(vrrig); // Special handling for Distance
                            UpdateFPSTag(vrrig);
                        }
                    }
                }
            }
            else
            {
                if (PhotonNetwork.InRoom)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (!vrrig.isOfflineVRRig)
                        {
                            requestedIds.Remove(vrrig.Creator.UserId);

                            vrrig.playerText1.transform.localPosition = new Vector3(25.30f, 25.00f, 0f);
                            vrrig.playerText1.transform.localScale = new Vector3(1, 1, 1);
                            vrrig.playerText1.transform.rotation = vrrig.transform.rotation;

                            if (!vrrig.playerText2.enabled)
                                vrrig.playerText2.enabled = true;
                        }
                    }
                }

                Destroy(this.GetComponent<NameTags>());
            }
        }

        private void UpdateFPSTag(VRRig vrrig)
        {
            if (PluginConfig.ShowFPS)
            {
                string fps = Traverse.Create(vrrig).Field("fps").GetValue().ToString();
                AddOrUpdateLine(vrrig, "FPS", fps);
            }
            else
            {
                RemoveLine(vrrig, "FPS");
            }
        }
        private async void UpdateCreationDateTag(VRRig vrrig)
        {
            if (PluginConfig.ShowCreationDate)
            {
                if (!requestedIds.Contains(vrrig.Creator.UserId))
                {
                    requestedIds.Add(vrrig.Creator.UserId);
                    string creationDate = await CreationDate.GetCreationDateAsync(vrrig);
                    AddOrUpdateLine(vrrig, "Creation", creationDate);
                }
            }
            else
            {
                requestedIds.Remove(vrrig.Creator.UserId);
                RemoveLine(vrrig, "Creation");
            }
        }
        private void UpdateDistanceTag(VRRig vrrig)
        {
            if (PluginConfig.ShowDistance)
            {
                string distanceValue = $"[{(int)Vector3.Distance(vrrig.transform.position, Camera.main.transform.position)}M]";
                AddOrUpdateDistanceLine(vrrig, distanceValue);
            }
            else
            {
                RemoveDistanceLine(vrrig);
            }
        }
        private void AddOrUpdateDistanceLine(VRRig vrrig, string value)
        {
            string[] lines = vrrig.playerText1.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> newLines = new List<string>();
            bool distanceFound = false;

            foreach (string line in lines)
            {
                if (line.StartsWith("[") && line.EndsWith("M]"))
                {
                    newLines.Add(value);
                    distanceFound = true;
                }
                else
                {
                    newLines.Add(line);
                }
            }

            if (!distanceFound)
            {
                newLines.Add(value);
            }

            vrrig.playerText1.text = string.Join("\n", newLines);
        }
        private void RemoveDistanceLine(VRRig vrrig)
        {
            string[] lines = vrrig.playerText1.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> newLines = new List<string>();

            foreach (string line in lines)
            {
                if (!(line.StartsWith("[") && line.EndsWith("M]")))
                {
                    newLines.Add(line);
                }
            }

            vrrig.playerText1.text = string.Join("\n", newLines);
        }


        private void UpdateTag(VRRig vrrig, string tagName, bool shouldShow, Func<string> getValue)
        {
            if (shouldShow)
            {
                AddOrUpdateLine(vrrig, tagName, getValue());
            }
            else
            {
                RemoveLine(vrrig, tagName);
            }
        }
        private void AddOrUpdateLine(VRRig vrrig, string tagName, string value)
        {
            string[] lines = vrrig.playerText1.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> newLines = new List<string>();
            bool tagFound = false;

            foreach (string line in lines)
            {
                if (line.StartsWith(tagName + ":"))
                {
                    newLines.Add($"{tagName}: {value}");
                    tagFound = true;
                }
                else
                {
                    newLines.Add(line);
                }
            }

            if (!tagFound)
            {
                newLines.Add($"{tagName}: {value}");
            }

            vrrig.playerText1.text = string.Join("\n", newLines);
        }
        private void RemoveLine(VRRig vrrig, string tagName)
        {
            string[] lines = vrrig.playerText1.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> newLines = new List<string>();

            foreach (string line in lines)
            {
                if (!line.StartsWith(tagName + ":"))
                {
                    newLines.Add(line);
                }
            }

            vrrig.playerText1.text = string.Join("\n", newLines);
        }
    }
}