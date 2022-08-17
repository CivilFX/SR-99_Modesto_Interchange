using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CivilFX
{

    public class DisableTrafficButton : MonoBehaviour
    {
        [SerializeField]
        private Button _disableButton;
        [SerializeField]
        private Button _enableButton;

        void OnEnable()
        {
            if (_disableButton != null)
                _disableButton.onClick.AddListener(DisableAllVehicles);

            if (_enableButton != null)
                _enableButton.onClick.AddListener(EnableAllVehicles);
        }

        void OnDisable()
        {
            if (_disableButton != null)
                _disableButton.onClick.RemoveListener(DisableAllVehicles);

            if (_enableButton != null)
                _enableButton.onClick.RemoveListener(EnableAllVehicles);
        }

        private void DisableAllVehicles()
        {
            AnimatedTrafficVehicle[] allVehicles = GameObject.FindObjectsOfType<AnimatedTrafficVehicle>();

            foreach (AnimatedTrafficVehicle vehicle in allVehicles)
            {
                vehicle.VISIBLE_RANGE = 0f;
            }
        }

        private void EnableAllVehicles()
        {
            AnimatedTrafficVehicle[] allVehicles = GameObject.FindObjectsOfType<AnimatedTrafficVehicle>();

            foreach (AnimatedTrafficVehicle vehicle in allVehicles)
            {
#if UNITY_EDITOR
                vehicle.VISIBLE_RANGE = 32000000;
#elif UNITY_ANDROID || UNITY_IOS
                vehicle.VISIBLE_RANGE = 160000f;
#else
                vehicle.VISIBLE_RANGE = 3200000f;
#endif
            }
        }
    }
}
