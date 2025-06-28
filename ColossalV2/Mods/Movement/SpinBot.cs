using Colossal.Menu;
using Colossal.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

namespace Colossal.Mods
{
    public class SpinBot : MonoBehaviour
    {
        private GameObject ghost;
        public void Update()
        {
            if (PluginConfig.SpinBot)
            {
                if (ghost == null)
                    ghost = GhostManager.SpawnGhost();

                if (ghost != null)
                {
                    //if (DisableRig.disablerig)
                    //    DisableRig.disablerig = false;

                    VRRig vrrig = ghost.GetComponent<VRRig>();
                    vrrig.mainSkin.material.color = GhostManager.ghostColor;
                    vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");

                    if (VRRig.LocalRig.enabled)
                        VRRig.LocalRig.enabled = false;


                    VRRig.LocalRig.transform.Rotate(Vector3.up * 250 * Time.deltaTime);


                    VRRig.LocalRig.transform.position = vrrig.transform.position;

                    VRRig.LocalRig.rightHandPlayer.transform.position = vrrig.rightHandPlayer.transform.position;
                    VRRig.LocalRig.leftHandPlayer.transform.position = vrrig.leftHandPlayer.transform.position;

                    //VRRig.LocalRig.headConstraint.transform.rotation = vrrig.headConstraint.transform.rotation;
                }
            }
            else
            {
                if (!VRRig.LocalRig.enabled)
                    VRRig.LocalRig.enabled = true;

                if (ghost != null)
                    GhostManager.DestroyGhost(ghost);

                Destroy(this.GetComponent<SpinBot>());
            }
        }
    }
}
