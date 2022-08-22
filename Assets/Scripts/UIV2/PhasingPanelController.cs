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