using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CivilFX.TrafficV5;

namespace CivilFX.UI2
{
    public class TrafficPanelController : MainPanelController
    {
        public Slider simSpeed;
        public Toggle simPauseToggle;

        public Toggle trafficToggle;
        public CustomButton resetTraffic;
        public Toggle modifyInflow;
        public Toggle modifyDiverges;
        bool inflowPointsOn = false;
        bool divergePointsOn = false;


        void Start()
        {
            simSpeed.onValueChanged.AddListener((v) => {
                Time.timeScale = v;
            });
        }


        private void Awake()
        {
            trafficToggle.onValueChanged.AddListener((v) => {

                foreach (var trafficController in Resources.FindObjectsOfTypeAll<TrafficController>())
                {
                    trafficController.ToggleTraffic(trafficToggle.isOn);
                }
               
            });


            resetTraffic.RegisterMainButtonCallback(() => {
                foreach (var trafficController in Resources.FindObjectsOfTypeAll<TrafficController>())
                {
                    trafficController.ResetSimulation();
                    resetTraffic.RestoreInternalState();
                }
            });

            modifyInflow.onValueChanged.AddListener((v) => {
                inflowPointsOn = !inflowPointsOn;
                ToggleInflowPoints();         
            });

            modifyDiverges.onValueChanged.AddListener((v) => {
                divergePointsOn = !divergePointsOn;
                ToggleDivergePoints();
            });
        }


        void Update()
        {
            if (simPauseToggle.isOn)
                Time.timeScale = 0;

            else
                Time.timeScale = simSpeed.value;
        }


        public void ToggleInflowPoints()
        {
            foreach (var trafficController in Resources.FindObjectsOfTypeAll<TrafficController>())
            {
                trafficController.SetRuntimeVisualInflowCount(inflowPointsOn);
            }
        }


        public void ToggleDivergePoints()
        {
            foreach (var trafficController in Resources.FindObjectsOfTypeAll<TrafficController>())
            {
                trafficController.SetRuntimeVisualDivergeCount(divergePointsOn);
            }
        }
    }
}