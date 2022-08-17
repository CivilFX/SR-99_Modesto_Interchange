using UnityEngine;
using System.Collections;
using Cinemachine;

namespace CivilFX
{
    public class CameraNode_Old : MonoBehaviour
    {
        public enum CameraCategory
        {
            Hidden = -2,
            None = -1,
            Overview = 0,
            Roadway = 1,
            PointsOfInterest = 2,
            DrivePaths = 3,
            Cinematics = 3,
            Other = 4,
        }

        #region Inspector Fields
        //[Range(0f, 1f)]
        //[SerializeField]
        //private float _animationStartPercentage = 0f;

        [SerializeField]
        private string _title;

        [SerializeField]
        private CameraCategory _viewCategory;

        [SerializeField]
        private int _sortOrder;

        public string Title { get { return _title; } private set { } }

        public CameraCategory ViewCategory { get { return _viewCategory; } private set { } }

        public int SortOrder { get { return _sortOrder; } private set { } }
        #endregion

        #region Fields
        private Animator _animator;
        private CinemachineVirtualCamera _virtualCamera;
        //private Director _director;
        #endregion

        #region Properties
        public Animator CameraAnim { get { return _animator; } private set { _animator = value; } }
        public bool IsAnimated { get { return CameraAnim != null; } private set { } }
        public bool IsExisting { get { return (int)ViewCategory <= 3 && (int)ViewCategory >= 0; } private set { } }
        #endregion

        public void Awake()
        {
            _animator = gameObject.GetComponent<Animator>();
            _virtualCamera = gameObject.GetComponentInChildren<CinemachineVirtualCamera>(true);

            if (_virtualCamera)
            {
                _virtualCamera.enabled = false;
            }
        }

        public void GainedFocus()
        {
            UnityStandardAssets.Cameras.AutoCam autoCam = Camera.main.GetComponent<UnityStandardAssets.Cameras.AutoCam>();
            Cinemachine.CinemachineBrain brain = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();

            if (autoCam != null)
            {
                autoCam.enabled = true;
            }

            if (brain.ActiveVirtualCamera != null)
            {
                var activeCamera = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
                activeCamera.enabled = false;
            }
            
            if (_virtualCamera)
            {
                _virtualCamera.enabled = true;
            }

            if (_animator != null)
            {
                AnimController.Instance.CurrentAnimationController = _animator;
                AnimController.Instance.PresentPanel();
            }
            else
            {
                if (AnimController.Instance.CurrentAnimationController != null)
                {
                    AnimController.Instance.ResetCurrentPath();
                }

                AnimController.Instance.HidePanel();
            }
        }

        public int CompareNodes(CameraNode_Old second)
        {
            if (second != null)
            {
                int compareResult = this._viewCategory.CompareTo(second.ViewCategory);
                if (compareResult == 0)
                    compareResult = this._sortOrder.CompareTo(second.SortOrder);

                return compareResult;
            }
            else
            {
                return 1;
            }
        }
    }
}