using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CivilFX
{
    public class AnimController : MonoBehaviour
    {

        private static AnimController _instance;
        public static AnimController Instance
        {
            get
            {
                return _instance;
            }
        }

        [SerializeField]
        private Button _resetButton;

        [SerializeField]
        private Button _rewindButton;

        [SerializeField]
        private Button _playButton;

        [SerializeField]
        private Button _pauseButton;

        [SerializeField]
        private Button _forwardButton;

        [SerializeField]
        private GameObject _animControlsPanel;

        [SerializeField]
        private string[] _videoNodes;


        public string[] ViewNodes
        {
            get
            {
                return _videoNodes;
            }
        }

        [HideInInspector]
        public Animator _currentAnimationController;
        public Animator CurrentAnimationController
        {
            get { return _currentAnimationController; }
            set
            {
                _currentAnimationController = value;
                isRewinding = false;
                _currentAnimationController.speed = 1f;
            }
        }

        private bool isRewinding;

        private bool scheduledStop;

        private void Start()
        {
            _resetButton.onClick.AddListener(ResetButtonClicked);
            _rewindButton.onClick.AddListener(RewindButtonClicked);
            _playButton.onClick.AddListener(PlayButtonClicked);
            _pauseButton.onClick.AddListener(PauseButtonClicked);
            _forwardButton.onClick.AddListener(ForwardButtonClicked);
        }

        private void Awake()
        {
            _instance = this;
        }

        private void OnDestroy()
        {
            _resetButton.onClick.RemoveListener(ResetButtonClicked);
            _rewindButton.onClick.RemoveListener(RewindButtonClicked);
            _playButton.onClick.RemoveListener(PlayButtonClicked);
            _pauseButton.onClick.RemoveListener(PauseButtonClicked);
            _forwardButton.onClick.RemoveListener(ForwardButtonClicked);
        }

        public void PresentPanel()
        {
            isRewinding = false;
            _animControlsPanel.SetActive(true);
        }

        public void HidePanel()
        {
            isRewinding = false;
            _animControlsPanel.SetActive(false);
        }

        private void ResetButtonClicked()
        {
            if (CurrentAnimationController != null)
            {
                isRewinding = false;
                CurrentAnimationController.Play(CurrentAnimationController.GetCurrentAnimatorStateInfo(0).fullPathHash, -1, 0f);
            }
        }

        private void RewindButtonClicked()
        {
            if (CurrentAnimationController != null)
            {
                if (isRewinding)
                {
                    isRewinding = false;
                }
                else
                {
                    CurrentAnimationController.speed = 1f;
                    AnimatorStateInfo _stateInfo = CurrentAnimationController.GetCurrentAnimatorStateInfo(0);
                    CurrentAnimationController.Play(_stateInfo.fullPathHash, -1, _stateInfo.normalizedTime - 0.1f);
                    StartCoroutine(RewindLoop());
                }
            }
        }

        private void PlayButtonClicked()
        {
            if (CurrentAnimationController != null)
            {
                isRewinding = false;
                CurrentAnimationController.speed = 1f;
            }
        }

        private void PauseButtonClicked()
        {
            if (CurrentAnimationController != null)
            {
                isRewinding = false;
                CurrentAnimationController.speed = 0f;
            }
        }

        private void ForwardButtonClicked()
        {
            if (CurrentAnimationController != null)
            {
                isRewinding = false;
                CurrentAnimationController.speed = CurrentAnimationController.speed < 5f ? 5f : 1f;
            }
        }

        private IEnumerator RewindLoop()
        {
            isRewinding = true;

            while (isRewinding)
            {
                AnimatorStateInfo _stateInfo = CurrentAnimationController.GetCurrentAnimatorStateInfo(0);
                CurrentAnimationController.Play(_stateInfo.fullPathHash, -1, _stateInfo.normalizedTime - (Time.deltaTime / 10f));
                yield return null;
            }
        }


        #region Automated Animation Helpers
        public float CurrentPathLength()
        {
            if (CurrentAnimationController)
            {
                AnimatorStateInfo _stateInfo = CurrentAnimationController.GetCurrentAnimatorStateInfo(0);
                return _stateInfo.length;
            }
            else
                return 0f;
        }

        public void ResetCurrentPath()
        {
            ResetButtonClicked();
            PauseButtonClicked();
        }

        public void PlayCurrentPath()
        {
            PlayButtonClicked();
        }

        public static IEnumerator RecordVRClip()
        {
            Debug.Log("Start Recording VR Clip");

            AnimController.Instance.ResetCurrentPath();

            float r = AnimController.Instance.CurrentPathLength();

            //Let it Settle
            float t = 0;
            float w = 20f;
            while (t < w)
            {
                yield return null;
                t += Time.deltaTime;
            }

            RenderController.Instance.Capture8KPanoramaVideo();


            //Wait 5 seconds
            t = 0;
            w = 5f;
            while (t < w)
            {
                yield return null;
                t += Time.deltaTime;
            }

            AnimController.Instance.PlayCurrentPath();


            //Wait the record Time
            t = 0;
            w = r;
            while (t < w)
            {
                yield return null;
                t += Time.deltaTime;
            }

            RenderController.Instance.StopRecrodingVideo();
        }


        public static IEnumerator RecordVideoClip()
        {
            Debug.Log("Start Recording Video Clip");

            AnimController.Instance.ResetCurrentPath();

            float r = AnimController.Instance.CurrentPathLength();

            // Let it Settle
            float t = 0;
            float w = 20f;
            while (t < w)
            {
                yield return null;
                t += Time.deltaTime;
            }

            RenderController.Instance.Capture4kUltraVideo();
            AnimController.Instance.PlayCurrentPath();

            //Wait For the Record Time
            t = 0;
            w = r;

            while (t < w)
            {
                yield return null;
                t += Time.deltaTime;
            }

            RenderController.Instance.StopRecrodingVideo();
        }

        public static IEnumerator ScheduleStop()
        {
            AnimController.Instance.scheduledStop = true;

            //Wait For the Record Time
            float t = 0;
            float w = AnimController.Instance.CurrentPathLength();

            if (w > 0.0f)
            {
                Debug.Log("Stop Recording Video Clip After Delay: " + w);
                while (AnimController.Instance.scheduledStop && t < w)
                {
                    yield return null;
                    t += Time.deltaTime;
                }

                if (AnimController.Instance.scheduledStop)
                {
                    RenderController.Instance.StopRecrodingVideo();
                }
            }

            AnimController.Instance.scheduledStop = false;
        }

        public static void CancelAllScheuldedStops()
        {
            AnimController.Instance.scheduledStop = false;
        }

        public static IEnumerator RecordAllVideoClips()
        {
            foreach (string nodeName in Instance.ViewNodes)
            {
                Debug.Log("Start Recording " + nodeName + " Clip");
                GameObject nodeObject = GameObject.Find("nodeName");
                CameraNode_Old node = nodeObject.GetComponent<CameraNode_Old>();
                node.GainedFocus();

                yield return AnimController.Instance.StartCoroutine(RecordVideoClip());
            }
        }
        #endregion
    }
}