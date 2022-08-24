using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CivilFX.Generic2;

namespace CivilFX.UI2
{
    public class PhasingPanelController : MonoBehaviour
    {
        public CustomButton pedestrianUnderpass;
        public CustomButton pedestrianOverpass;
        private CustomButton lastSelected;

        public TrafficPanelController trafficPanel;


        private void Awake()
        {
            pedestrianUnderpass.RegisterMainButtonCallback(() => {
                if (pedestrianUnderpass == lastSelected) {
                    return;
                }
                if (lastSelected != null) {
                    lastSelected.RestoreInternalState();
                }

                //invoke phase
                PhasedManager.Invoke(PhaseType.Pedestrian_Underpass);
                lastSelected = pedestrianUnderpass;

                trafficPanel.ToggleInflowPoints();
                trafficPanel.ToggleDivergePoints();

                // Underpass camera 3
                Vector3 pos = new Vector3(-968.70717f, 3.566599f, -84.8459f);
                Vector3 rot = new Vector3(14.583f, 333.5f, 0f);
                float fov = 40;
                GameManager.Instance.cameraController.HookView(pos, rot, fov);
            });

            pedestrianOverpass.RegisterMainButtonCallback(() => {
                if (pedestrianOverpass == lastSelected) {
                    return;
                }
                if (lastSelected != null) {
                    lastSelected.RestoreInternalState();
                }

                //invoke phase
                PhasedManager.Invoke(PhaseType.Pedestrian_Overpass);
                lastSelected = pedestrianOverpass;

                trafficPanel.ToggleInflowPoints();
                trafficPanel.ToggleDivergePoints();
            });


            pedestrianOverpass.InvokeMainButton();
        }

    }
}