using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CivilFX
{
    public class DrivingStartup : MonoBehaviour
    {

        [SerializeField]
        private Button _startButton;

        [SerializeField]
        private List<GameObject> _itemsToDisable;

        [SerializeField]
        private GameObject _cameraTarget;

        [SerializeField]
        private GameObject _car;

        [SerializeField]
        private GameObject _carFollowTarget;

        [SerializeField]
        private GameObject _warningDisplay;

        [SerializeField]
        private Transform _resetNode2;

        [SerializeField]
        private Transform _resetNode3;

        [SerializeField]
        private Transform _resetNode4;

        private Vector3 _cachedOrigin;


        void Awake()
        {
            _cachedOrigin = _car.transform.position;

        }

        void Start()
        {
            _car.gameObject.SetActive(false);
            WarnDriver(false);
        }

        void OnEnable()
        {
            _startButton.onClick.AddListener(StartDrivingExperience);
        }

        void OnDisable()
        {
            _startButton.onClick.RemoveListener(StartDrivingExperience);
            ResetCar();
            _car.gameObject.SetActive(false);
        }

        void Update()
        {
            if (_car.gameObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Cancel"))
                {
                    ResetCar();
                }

                if (Input.GetButtonDown("Reset1") || Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                {
                    Debug.LogError("Reset 1 Pressed");
                    ResetCar();
                }

                if (Input.GetButtonDown("Reset2") || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                {
                    Debug.LogError("Reset 2 Pressed");
                    ResetCar(_resetNode2.position, _resetNode2.localEulerAngles);
                }

                if (Input.GetButtonDown("Reset3") || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                {
                    Debug.LogError("Reset 3 Pressed");
                    ResetCar(_resetNode3.position, _resetNode3.localEulerAngles);
                }

                if (Input.GetButtonDown("Reset4") || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                {
                    Debug.LogError("Reset 4 Pressed");
                    ResetCar(_resetNode4.position, _resetNode4.localEulerAngles);
                }
            }
            else
            {
                if (Input.GetButtonDown("Reset1") || Input.GetButtonDown("Reset2"))
                {
                    StartDrivingExperience();
                }
            }
        }

        public void ResetCar(Vector3 location = default(Vector3), Vector3 orientation = default(Vector3))
        {
            Rigidbody rigidCar = _car.GetComponentInChildren<Rigidbody>();
            if (rigidCar != null)
            {
                rigidCar.velocity = Vector3.zero;
                rigidCar.angularVelocity = Vector3.zero;
                rigidCar.Sleep();
            }

            if (location == default(Vector3))
            {
                _car.transform.position = _cachedOrigin + (Vector3.up * 2f);
                _car.transform.rotation = Quaternion.identity;
            }
            else
            {
                _car.transform.position = location + (Vector3.up * 2f);
                _car.transform.localEulerAngles = orientation;
            }


            WarnDriver(false);
        }

        public void WarnDriver(bool display)
        {
            _warningDisplay.SetActive(display);
        }

        void StartDrivingExperience()
        {
            _car.gameObject.SetActive(true);

            //Disable UI
            Camera uiCamera = gameObject.GetComponentInChildren<Camera>();
            uiCamera.enabled = false;

            for (int i = 0; i < _itemsToDisable.Count; ++i)
            {
                _itemsToDisable[i].SetActive(false);
            }

            Camera mainCamera = Camera.main;
            UnityStandardAssets.Cameras.AutoCam autoCam = mainCamera.gameObject.GetComponent<UnityStandardAssets.Cameras.AutoCam>();

            //Without AutoCam
            autoCam.enabled = false;
            mainCamera.transform.parent = _carFollowTarget.transform;
            mainCamera.transform.localPosition = Vector3.zero;
            mainCamera.transform.localRotation = Quaternion.identity;
            mainCamera.nearClipPlane = 0.05f;
            mainCamera.fieldOfView = 45f;

            //With AutoCam
            //_cameraTarget.transform.parent = _carFollowTarget.transform;
            //_cameraTarget.transform.localPosition = Vector3.zero;
            //_cameraTarget.transform.localRotation = Quaternion.identity;

            QualitySettings.shadowDistance = 200f;

        }
    }
}