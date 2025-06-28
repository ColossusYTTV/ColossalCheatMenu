using Colossal.Menu;
using Colossal.Patches;
using GorillaGameModes;
using Photon.Pun;
using PlayFab.GroupsModels;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Colossal.Mods
{
    public class TagAura : MonoBehaviour
    {
        private LineRenderer radiusLine;
        private Material lineMaterial;
        private float ammount;

        private static readonly Color[] BeamColors = {
            new Color(0.6f, 0f, 0.8f, 0.5f), // Purple
            new Color(1f, 0f, 0f, 0.5f), // Red
            new Color(1f, 1f, 0f, 0.5f), // Yellow
            new Color(0f, 1f, 0f, 0.5f), // Green
            new Color(0f, 0f, 1f, 0.5f)  // Blue
        };

        private void Awake()
        {
            // Initialize the material only once to avoid unnecessary recreations.
            lineMaterial = new Material(Shader.Find("GUI/Text Shader"));
        }

        public void Update()
        {
            if (PluginConfig.tagaura == 0) Destroy(this.GetComponent<TagAura>()); // I js dont wanna figure out the error I get putting it into the switch case

            // Set ammount based on the config.
            ammount = GetAmmountFromConfig();

            // Set Line material color based on the config.
            lineMaterial.color = BeamColors[Mathf.Min(PluginConfig.BeamColour, BeamColors.Length - 1)];

            // Proceed only if there's a tag aura to process.
            if (PhotonNetwork.InRoom && WhatAmI.IsInfected(PhotonNetwork.LocalPlayer))
            {
                HandleTagAura();
            }
            else
            {
                Cleanup();
            }
        }

        private float GetAmmountFromConfig()
        {
            switch (PluginConfig.tagaura)
            {
                case 1: return 4.5f;
                case 2: return 4f;
                case 3: return 3.5f;
                case 4: return 3f;
                case 5: return 2.5f;
                case 6: return 2f;
                case 7: return 1f;
                default: return 0f;
            }
        }

        private void HandleTagAura()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                // Skip if the VRRig creator is infected.
                if (WhatAmI.IsInfected(vrrig.Creator)) continue;

                if (Vector3.Distance(GorillaTagger.Instance.offlineVRRig.transform.position, vrrig.transform.position) <= GorillaGameManager.instance.tagDistanceThreshold / ammount && !vrrig.isMyPlayer)
                {
                    CreateOrUpdateLine(vrrig);
                    SetAntiScreenShareLayer();

                    //GorillaLocomotion.GTPlayer.Instance.leftControllerTransform.position = vrrig.transform.position;
                    GameMode.ReportTag(vrrig.Creator);
                }
            }
        }

        private void CreateOrUpdateLine(VRRig vrrig)
        {
            if (radiusLine == null)
            {
                GameObject lineObject = new GameObject("RadiusLine");
                lineObject.transform.parent = vrrig.transform;
                radiusLine = lineObject.AddComponent<LineRenderer>();
                radiusLine.positionCount = 2;
                radiusLine.startWidth = 0.05f;
                radiusLine.endWidth = 0.05f;
                radiusLine.material = lineMaterial;
                radiusLine.startColor = lineMaterial.color;
                radiusLine.endColor = lineMaterial.color;
            }

            radiusLine.SetPosition(0, vrrig.transform.position);
            radiusLine.SetPosition(1, GorillaTagger.Instance.transform.position);
        }

        private void SetAntiScreenShareLayer()
        {
            // Set appropriate layer based on AntiScreenShare setting.
            if (PluginConfig.AntiScreenShare == 1)
            {
                radiusLine.gameObject.layer = 25;
            }
            else if (PluginConfig.AntiScreenShare == 2)
            {
                radiusLine.gameObject.layer = 16;
            }
        }

        private void Cleanup()
        {
            if (radiusLine != null)
            {
                Destroy(radiusLine.gameObject);
                radiusLine = null;
            }
        }
    }
}
