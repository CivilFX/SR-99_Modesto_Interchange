using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CivilFX.Generic2;

namespace CivilFX.UI2
{
    public class PhasingPanelController : MonoBehaviour
    {
        public CustomButton proposed;
        public CustomButton existing;
        private CustomButton lastSelected;

        public TrafficPanelController trafficPanel;


        private void Awake()
        {
            proposed.RegisterMainButtonCallback(() => {
                if (proposed == lastSelected) {
                    return;
                }
                if (lastSelected != null) {
                    lastSelected.RestoreInternalState();
                }

                //invoke phase
                PhasedManager.Invoke(PhaseType.Proposed);
                lastSelected = proposed;

                trafficPanel.ToggleInflowPoints();
                trafficPanel.ToggleDivergePoints();
            });

            existing.RegisterMainButtonCallback(() => {
                if (existing == lastSelected) {
                    return;
                }
                if (lastSelected != null) {
                    lastSelected.RestoreInternalState();
                }

                //invoke phase
                PhasedManager.Invoke(PhaseType.Existing);
                lastSelected = existing;

                trafficPanel.ToggleInflowPoints();
                trafficPanel.ToggleDivergePoints();
            });


            proposed.InvokeMainButton();
        }

    }
}