using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CivilFX.TrafficV5;

namespace CivilFX.UI2
{
    public class DivergePercentPanelController : UIDraggable
    {

        public TextMeshProUGUI pathNameTMP;
        public Slider divergePercent;
        public TextMeshProUGUI divergePercentTMP;
        public CustomButton done;

        public TrafficPathController pathDiverging;
        public TrafficPathController pathEntering;
        public DivergePointGroupController groupController;


        // Start is called before the first frame update
        void Awake()
        {
            divergePercent.onValueChanged.AddListener((v) => {
                divergePercentTMP.text = v.ToString();

                pathDiverging.UpdateVehicleDivergePercentage((int) v);
            });

            done.RegisterMainButtonCallback(() => {
                done.RestoreInternalState();
                groupController.OnDone();
                OnHidden();
            });
        }


        public void OnVisible(TrafficPathController _pathDiverging, TrafficPathController _pathEntering)
        {
            gameObject.SetActive(true);
            pathDiverging = _pathDiverging;
            pathEntering = _pathEntering;
            pathNameTMP.text = pathEntering.name;
            divergePercent.value = pathDiverging.vehicleDivergePercentage;
            divergePercentTMP.text = pathDiverging.vehicleDivergePercentage.ToString();
        }


        public void OnHidden()
        {
            gameObject.SetActive(false);
            pathEntering = null;
            pathDiverging = null;
        }


    }
}