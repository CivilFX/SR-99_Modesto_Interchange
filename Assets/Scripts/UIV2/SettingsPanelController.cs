﻿using CivilFX.Generic2;
using CivilFX.TrafficV5;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CivilFX.UI2
{
    public class SettingsPanelController : MainPanelController
    {
        [Header("Camera Panel:")]
        public Slider rotation;
        public Slider flythrough;
        public Slider height;
        public Slider fov;
        public CustomButton resetFOV;
        public CameraDirector cameraDirector;
        CameraController camController;


        [Space()]
        [Header("Traffic Panel:")]
        public Toggle modifyInflow;
        public Slider simSpeed;
        public CustomButton resetTraffic;

        [Space()]
        [Header("Others Panel:")]
        public Toggle treesToggle;
        public Toggle compassToggle;
        public Toggle wireframeToggle;

        private GameObject[] trees;
        private bool treesOff = false;

        GameObject[] compass;
        private bool compassOff = true;



        void Start()
        {
            trees = GameObject.FindGameObjectsWithTag("Trees");
            compass = GameObject.FindGameObjectsWithTag("Compass");
        }


        private void Awake()
        {
            camController = GameManager.Instance.cameraController;

            /*
            * Camera
            */
            rotation.onValueChanged.AddListener((v) => {
                camController.SetRotationSpeed(v);
            });

            flythrough.onValueChanged.AddListener((v) => {
                camController.SetFlythroughSpeed(v);
            });

            height.onValueChanged.AddListener((v) => {
                cameraDirector.SetHeightOffset(v - 60);
            });

            fov.value = camController.ResetFOV();
            fov.onValueChanged.AddListener((v) => {
                camController.SetFOV(v);
            });
            resetFOV.RegisterMainButtonCallback(() => {
                fov.value = camController.ResetFOV();
                resetFOV.RestoreInternalState();
            });

            /*
             * Traffic
             */
            modifyInflow.onValueChanged.AddListener((v) => {
                foreach (var trafficController in Resources.FindObjectsOfTypeAll<TrafficController>()) {
                    //trafficController.SetRuntimeVisualInflowCount(v);
                }
            });
            simSpeed.onValueChanged.AddListener((v) => {
                Time.timeScale = v;
            });
            resetTraffic.RegisterMainButtonCallback(() => {
                foreach (var trafficController in Resources.FindObjectsOfTypeAll<TrafficController>()) {
                    //trafficController.ResetSimulation();
                    resetTraffic.RestoreInternalState();
                }              
            });


            /*
             * Others
             */

            treesToggle.onValueChanged.AddListener(delegate {
                ToggleTrees();
            });

            compassToggle.onValueChanged.AddListener(delegate {
                ToggleCompass();
            });

            wireframeToggle.onValueChanged.AddListener(delegate {
                ToggleWireFrameMode();
            });

            Camera.main.GetComponent<CamWireframeMode>().wireFrameOn = false;


        }


        void Update()
        {
            if (fov.value != camController.cam.fieldOfView)
                fov.value = camController.cam.fieldOfView;
        }


        // Toggle Buttons

        void ToggleTrees()
        {
            foreach(var tree in trees)
                tree.SetActive(treesOff);

            treesOff = !treesOff;
        }


        void ToggleCompass()
        {
            foreach(var compassPart in compass)
                compassPart.gameObject.GetComponent<Image>().enabled = compassOff;

            compassOff = !compassOff;
        }


        void ToggleWireFrameMode()
        {
            Camera.main.GetComponent<CamWireframeMode>().wireFrameOn = !Camera.main.GetComponent<CamWireframeMode>().wireFrameOn;
        }

}
}