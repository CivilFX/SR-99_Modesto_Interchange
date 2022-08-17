using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CivilFX
{
    public enum VehicleType
    {
        LIGHT,
        MEDIUM,
        HEAVY,
        NUM_TYPES,
    }

    public class AnimatedTrafficVehicle : MonoBehaviour
    {
        #region Constants
        private const float MPH_TO_METERS_PER_SECOND = 0.44704f;    //1609.3440f / 60f / 60f;  
        private const float GAP_SECONDS_PER_MPH = 0.1f;
#if UNITY_EDITOR
        public float VISIBLE_RANGE = 3200000f;
#elif UNITY_ANDROID || UNITY_IOS
        public float VISIBLE_RANGE = 160000f;
#else
        public float VISIBLE_RANGE = 3200000f;
#endif
        #endregion

        #region Inspector Fields
        /// <summary>
        /// The wheel transform.  This is a cached reference of the transform.
        /// </summary>
        [SerializeField]
        private List<Transform> _wheelTransforms;

        [SerializeField]
        private VehicleType _vehicleType = VehicleType.LIGHT;
        #endregion

        #region Private Fields
        private float _rotationValue;
        private Renderer _renderer;
        private Transform _cameraTransform;
        private bool isVisible;
        private AudioSource _engineSound;
        private Transform _transform;
        private float _checkVisibility;

        private TrafficPath_Old _path;
        private GoSpline _laneSpline;
        private float _speed;
        private float _duration;
        private float _offset;
        private float _progress;
        private Vector3 _pos;

        private bool _merged;
        private bool _cached;
        private TrafficPath_Old _tempPath;
        private GoSpline _tempSpline;
        private float _tempSpeed;
        private float _tempDuration;
        private float _tempOffset;
        private float _tempProgress;
        #endregion

        #region Public Accessors
        public VehicleType GetVehicleType()
        {
            return _vehicleType;
        }

        public bool IsVisible
        {
            get { return isVisible; }
        }

        public TrafficPath_Old Path
        {
            get { return _path; }
            set { _path = value; }
        }
        #endregion

        #region behaviour Overrides
        void Start()
        {
            _transform = transform;
            _renderer = _transform.GetComponentInChildren<Renderer>();
            _cameraTransform = Camera.main.transform;
            _engineSound = _transform.GetComponentInChildren<AudioSource>();

            isVisible = true;
        }

        private void OnEnable()
        {
            StartCoroutine(ProcessVehicle());
        }
        #endregion

        #region OO
        public void SetPath(float speed, float duration, float offset, GoSpline laneSpline, TrafficPath_Old path, bool merged = false)
        {
            _merged = merged;

            if (_merged)
            {
                //Store Passed Variables
                _tempPath = path;
                _tempSpline = laneSpline;
                _tempSpeed = speed;
                _tempDuration = duration;
                _tempOffset = offset;

                //Setup Local Logic
                _tempProgress = offset;
            }
            else
            {
                //Store Passed Variables
                _path = path;
                _laneSpline = laneSpline;
                _speed = speed;
                _duration = duration;
                _offset = offset;

                //Setup Local Logic
                _progress = _offset;
            }
        }

        public bool IsNearStart(float timeComparison)
        {
            bool nearStart = false;

            float gap = _speed * GAP_SECONDS_PER_MPH;

            if (_progress * _duration <= gap)
            {
                nearStart = true;
            }

            if ((_progress + (timeComparison / _duration) % 1.0f) * _duration <= gap)
            {
                nearStart = true;
            }

            return nearStart;
        }

        private void SetVisiblity()
        {
            if (_renderer != null)
                _renderer.enabled = isVisible;

            if (_engineSound != null)
                _engineSound.enabled = isVisible;
        }
        #endregion

        #region Coroutines
        private IEnumerator ProcessVehicle()
        {
            while (true)
            {
                // UPDATE PROGRESS
                if (_laneSpline != null)
                {
                    _progress = (_progress + (Time.deltaTime / _duration));
                    if (_cached && _progress > 1f) { _cached = false; }
                    _progress = _progress % 1f;

                    if (isVisible)
                        _pos = _laneSpline.getPointOnPath(_progress);
                }

                // UPDAT MERGED PATH PROGRESS
                if (_merged)
                {
                    _tempProgress = (_tempProgress + (Time.deltaTime / _tempDuration));
                    if (_tempProgress > 1f)
                    {
                        _merged = false;
                        _cached = true;
                        isVisible = false;
                        SetVisiblity();
                    }

                    if (isVisible)
                        _pos = _tempSpline.getPointOnPath(_tempProgress);
                }


                // MANAGE VISIBILITY
                _checkVisibility -= Time.deltaTime;
                if (_checkVisibility <= 0.0f && !_cached)
                {
                    if (!_merged && !isVisible && _laneSpline != null)
                    {
                        _pos = _laneSpline.getPointOnPath(_progress);
                    }
                    else if (_merged && isVisible && _tempSpline != null)
                    {
                        _pos = _tempSpline.getPointOnPath(_tempProgress);
                    }

                    _checkVisibility = 1f + Random.Range(-0.5f, 0.5f);

                    if (_cameraTransform != null)
                    {
                        isVisible = (_cameraTransform.position - _pos).sqrMagnitude < VISIBLE_RANGE;
                        SetVisiblity();
                    }
                }

                // VISUALLY MODIFY PROGRESS
                if (isVisible)
                {
                    if (_merged && _tempSpline != null)
                    {
                        float forward = _tempProgress + 0.001f;
                        Vector3 look = _tempSpline.getPointOnPath(forward);
                        _transform.position = _pos;
                        _transform.LookAt(look);
                    }
                    else if (_laneSpline != null)
                    {
                        float forward = _progress + 0.001f;
                        Vector3 look = _laneSpline.getPointOnPath(forward);
                        _transform.position = _pos;
                        _transform.LookAt(look);
                    }
                }


                // CHECK MERGE GAP
                if (!_cached && !_merged && isVisible && _path != null && _path.MergingPaths != null && _path.MergingPaths.Count > 0)
                {
                    for (int i = 0; i < _path.MergingPaths.Count; i++)
                    {
                        TrafficPath_Old mergePath = _path.MergingPaths[i];

                        if (mergePath != null)
                        {
                            if (mergePath.gapAcceptance && mergePath.nodes.Count > 1 && (mergePath.nodes[1] - _pos).sqrMagnitude <= 1f)
                            {
                                float roll = Random.Range(0f, 1f);
                                if(roll <= mergePath.mergeChance)
                                {
                                    _path.VehiclesOnPath.Remove(this);
                                    float speed = mergePath.pathSpeedMPH * MPH_TO_METERS_PER_SECOND; // Meters Per Second
                                    float duration = mergePath.Spline().Length / speed;
                                    float offset = 0f;

                                    SetPath(mergePath.pathSpeedMPH, duration, offset, mergePath.Spline(), mergePath, true);
                                    mergePath.VehiclesOnPath.Add(this);
                                    break;
                                }
                            }
                        }
                    }
                }

                // ROTATE TIRES
                if (isVisible)
                {
                    // Rotate Tires
                    for (int i = 0; i < _wheelTransforms.Count; ++i)
                    {
                        Transform wheelAxle = _wheelTransforms[i];
                        if (wheelAxle != null)
                        {
                            //Set the rotation of the transform
                            wheelAxle.localEulerAngles = new Vector3(0, 0 /* + steer Angle */, _rotationValue);
                        }

                        //Increase the rotation value
                        _rotationValue -= 90f /*RPM*/ * (360f / 60f) * Time.deltaTime;
                    }
                }

                yield return null;
            }
        }
        #endregion
    }
}