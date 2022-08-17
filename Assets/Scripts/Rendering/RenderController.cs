using UnityEngine;
using UnityEngine.UI;
using UnityEngine.FrameRecorder;
using UTJ.FrameCapturer.Recorders;
using System.Collections;

namespace CivilFX
{
    public class RenderController : MonoBehaviour
    {
        private static RenderController _instance;
        public static RenderController Instance
        {
            get
            {
                return _instance;
            }
        }

        #region Inspector Properties
        [SerializeField]
        private Button _screenGrabQuickButton;

        [SerializeField]
        private Button _screenGrabGoodButton;

        [SerializeField]
        private Button _screenGrabGreatButton;

        [SerializeField]
        private Button _screenGrab360Button;

        [SerializeField]
        private RenderManager _movieCapture;
        
        [SerializeField]
        private Button _video720pButton;

        [SerializeField]
        private Button _video1080pButton;

        [SerializeField]
        private Button _video4kButton;

        [SerializeField]
        private Button _video8kButton;

        [SerializeField]
        private Button _video4kPanoButton;

        [SerializeField]
        private Button _video8kPanoButton;

        [SerializeField]
        private Button _stopRecordingButton;

        [SerializeField]
        private MP4RecorderSettings _recording720Settings;

        [SerializeField]
        private MP4RecorderSettings _recording1080Settings;

        [SerializeField]
        private MP4RecorderSettings _recording4KSettings;

        [SerializeField]
        private MP4RecorderSettings _recording8KSettings;

        [SerializeField]
        private Camera _cam;

        [SerializeField]
        private RenderTexture _leftEyeCubemap;

        [SerializeField]
        private RenderTexture _rightEyeCubemap;

        [SerializeField]
        private RenderTexture _equirect;

        [SerializeField]
        private bool _stereoscopic = true;
        #endregion

        #region Fields
        private int _originalHeight;
        private int _originalWidth;
        private RecordingSession _session;
        #endregion

        #region Behaviour Overrides
        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            if (_screenGrabQuickButton != null)
                _screenGrabQuickButton.onClick.AddListener(CaptureQuickScreenShot);
            if (_screenGrabGoodButton != null)
                _screenGrabGoodButton.onClick.AddListener(CaptureGoodScreenShot);
            if (_screenGrabGreatButton != null)
                _screenGrabGreatButton.onClick.AddListener(CaptureGreatScreenShot);
            if (_screenGrab360Button != null)
                _screenGrab360Button.onClick.AddListener(Capture360ScreenShot);
            
            if (_video720pButton != null)
                _video720pButton.onClick.AddListener(Capture720pVideo);
            if (_video1080pButton != null)
                _video1080pButton.onClick.AddListener(Capture1080pVideo);
            if (_video4kButton != null)
                _video4kButton.onClick.AddListener(Capture4kUltraVideo);
            if (_video8kButton != null)
                _video8kButton.onClick.AddListener(Capture8kUltraVideo);
            if (_video4kPanoButton != null)
                _video4kPanoButton.onClick.AddListener(CapturePanoramaVideo);
            if (_video8kPanoButton != null)
                _video8kPanoButton.onClick.AddListener(Capture8KPanoramaVideo);

            if (_stopRecordingButton != null)
            {
                _stopRecordingButton.onClick.AddListener(StopRecrodingVideo);
                _stopRecordingButton.gameObject.SetActive(false);
            }
        }

        private void LateUpdate()
        {
            if (_session != null && _session.recording && false)
            {
                CaptureSingleFrame();
            }
        }

        private void OnDestroy()
        {
            if (_screenGrabQuickButton != null)
                _screenGrabQuickButton.onClick.RemoveListener(CaptureQuickScreenShot);
            if (_screenGrabGoodButton != null)
                _screenGrabGoodButton.onClick.RemoveListener(CaptureGoodScreenShot);
            if (_screenGrabGreatButton != null)
                _screenGrabGreatButton.onClick.RemoveListener(CaptureGreatScreenShot);
            if (_screenGrab360Button != null)
                _screenGrab360Button.onClick.RemoveListener(Capture360ScreenShot);
            
            if (_video720pButton != null)
                _video720pButton.onClick.RemoveListener(Capture720pVideo);
            if (_video1080pButton != null)
                _video1080pButton.onClick.RemoveListener(Capture1080pVideo);
            if (_video4kButton != null)
                _video4kButton.onClick.RemoveListener(Capture4kUltraVideo);
            if (_video8kButton != null)
                _video8kButton.onClick.RemoveListener(Capture8kUltraVideo);
            if (_video4kPanoButton != null)
                _video4kPanoButton.onClick.RemoveListener(CapturePanoramaVideo);
            if (_video8kPanoButton != null)
                _video8kPanoButton.onClick.RemoveListener(Capture8KPanoramaVideo);

            if (_stopRecordingButton != null)
                _stopRecordingButton.onClick.RemoveListener(StopRecrodingVideo);
        }
        #endregion

        #region Capture Functions
        public void CaptureQuickScreenShot()
        {
#if OLD_RENDER_TECH
            _movieCapture.Resolution = 1920;
            _movieCapture.ResolutionH = 1080;
            StartCoroutine(SingleFrameVideo());
#else
            _movieCapture.Resolution = 1920;
            _movieCapture.ResolutionH = 1080;
            StartCoroutine(SingleFrameVideo());
#endif
        }

        public void CaptureGoodScreenShot()
        {
#if OLD_RENDER_TECH
            _movieCapture.Resolution = 3840;
            _movieCapture.ResolutionH = 2160;
            StartCoroutine(SingleFrameVideo());
#else
            _movieCapture.Resolution = 3840;
            _movieCapture.ResolutionH = 2160;
            StartCoroutine(SingleFrameVideo());
#endif
        }

        public void CaptureGreatScreenShot()
        {
#if OLD_RENDER_TECH
            _movieCapture.Resolution = 3840 * 2;
            _movieCapture.ResolutionH = 2160 * 2;
            StartCoroutine(SingleFrameVideo());
#else
            _movieCapture.Resolution = 3840 * 2;
            _movieCapture.ResolutionH = 2160 * 2;
            StartCoroutine(SingleFrameVideo());
#endif
        }

        public void Capture360ScreenShot()
        {
#if OLD_RENDER_TECH
            _movieCapture.Resolution = 4096;
            _movieCapture.ResolutionH = 2048;
            //_movieCapture.panoramaType = VRPanorama.VRCaptureRuntime.VRModeList.EquidistantMono;
            StartCoroutine(SingleFrameVideo());
#else
            CaptureSingleFrame();
            _movieCapture.SaveRenderTexture(_equirect, "360-Stereo-Render");
#endif
        }

        public IEnumerator SingleFrameVideo()
        {
            int cachedQuality = _movieCapture.RenderQuality;
            _movieCapture.RenderQuality = 16;
            _stopRecordingButton.gameObject.SetActive(false);
            _movieCapture.TakeScreenShot("ScreenShot");
            yield return null;
            _movieCapture.RenderQuality = cachedQuality;
        }
        
        public void Capture720pVideo()
        {
#if OLD_RENDER_TECH
            _movieCapture.Resolution = 1280;
            _movieCapture.ResolutionH = 720;
            _movieCapture.Mp4Bitrate = 6000;
            _movieCapture.NumberOfFramesToRender = 36000;  //20 Minutes Max.
            _movieCapture.FPS = 60;

            StartRecordingVideo();
#else
            float currentLength = AnimController.Instance.CurrentPathLength() + 5000f;

            MP4Recorder recorder = new MP4Recorder();
            recorder.settings = _recording720Settings;
            if (currentLength > 0.1f)
            {
                recorder.settings.m_DurationMode = DurationMode.TimeInterval;
                recorder.settings.m_EndTime = currentLength;
            }

            _session = new RecordingSession()
            {
                m_Recorder = RecordersInventory.GenerateNewRecorder(typeof(MP4Recorder), recorder.settings),
            };

            RecorderComponent m_recordComponent = GetComponent<RecorderComponent>();
            m_recordComponent.session = _session;

            StartFrameCapture();
#endif
        }

        public void Capture1080pVideo()
        {
#if OLD_RENDER_TECH
            _movieCapture.Resolution = 1920;
            _movieCapture.ResolutionH = 1080;
            _movieCapture.Mp4Bitrate = 12000;
            _movieCapture.NumberOfFramesToRender = 36000;  //20 Minutes Max.
            _movieCapture.FPS = 60;

            StartRecordingVideo();
#else
            float currentLength = AnimController.Instance.CurrentPathLength() + 5000f;

            MP4Recorder recorder = ScriptableObject.CreateInstance<MP4Recorder>();
            recorder.settings = _recording1080Settings;
            if (currentLength > 0.1f)
            {
                recorder.settings.m_DurationMode = DurationMode.TimeInterval;
                recorder.settings.m_EndTime = currentLength;
            }

            _session = new RecordingSession()
            {
                m_Recorder = RecordersInventory.GenerateNewRecorder(typeof(MP4Recorder), recorder.settings),
            };

            RecorderComponent m_recordComponent = GetComponent<RecorderComponent>();
            m_recordComponent.session = _session;

            StartFrameCapture();
#endif
        }

        public void Capture4kUltraVideo()
        {
#if OLD_RENDER_TECH
            _movieCapture.Resolution = 3840;
            _movieCapture.ResolutionH = 2160;
            _movieCapture.Mp4Bitrate = 28539; //0.14285711814566623 Bits Per Pixel
            _movieCapture.NumberOfFramesToRender = 36000;  //20 Minutes Max.
            _movieCapture.FPS = 60;

            StartRecordingVideo(true);
#else
            float currentLength = AnimController.Instance.CurrentPathLength() + 5000f;

            MP4Recorder recorder = new MP4Recorder();
            recorder.settings = _recording4KSettings;
            if (currentLength > 0.1f)
            {
                recorder.settings.m_DurationMode = DurationMode.TimeInterval;
                recorder.settings.m_EndTime = currentLength;
            }

            _session = new RecordingSession()
            {
                m_Recorder = RecordersInventory.GenerateNewRecorder(typeof(MP4Recorder), recorder.settings),
            };

            RecorderComponent m_recordComponent = GetComponent<RecorderComponent>();
            m_recordComponent.session = _session;

            StartFrameCapture();
#endif
        }

        public void Capture8kUltraVideo()
        {
#if OLD_RENDER_TECH
            _movieCapture.Resolution = 3840 * 2;
            _movieCapture.ResolutionH = 2160 * 2;
            _movieCapture.Mp4Bitrate = 28539 * 2; //0.14285711814566623 Bits Per Pixel
            _movieCapture.NumberOfFramesToRender = 36000;  //20 Minutes Max.
            _movieCapture.FPS = 60;

            StartRecordingVideo(true);
#else
            float currentLength = AnimController.Instance.CurrentPathLength() + 5000f;

            MP4Recorder recorder = new MP4Recorder();
            recorder.settings = _recording8KSettings;
            if (currentLength > 0.1f)
            {
                recorder.settings.m_DurationMode = DurationMode.TimeInterval;
                recorder.settings.m_EndTime = currentLength;
            }

            _session = new RecordingSession()
            {
                m_Recorder = RecordersInventory.GenerateNewRecorder(typeof(MP4Recorder), recorder.settings),
            };

            RecorderComponent m_recordComponent = GetComponent<RecorderComponent>();
            m_recordComponent.session = _session;

            StartFrameCapture();
#endif
        }

        public void CapturePanoramaVideo()
        {
            _movieCapture.Resolution = 4096;
            _movieCapture.ResolutionH = 2048;
            _movieCapture.Mp4Bitrate = 25270;
            _movieCapture.NumberOfFramesToRender = 36000;
            _movieCapture.FPS = 60;
            Time.captureFramerate = 60;
            //StartRecordingVideo(true);

            StartCoroutine(PanoCaptureRoutine());
        }

        private IEnumerator PanoCaptureRoutine()
        {
            _movieCapture.StartRecording("360Video", false, true);
            _stopRecordingButton.gameObject.SetActive(true);

            while (RenderManager.Recording)
            {
                CaptureSingleFrame();
                _movieCapture.SaveRenderTexture(_equirect, "360-Stereo-Render");
                yield return null;
            }

            //_movieCapture.StopRecording();
            //_stopRecordingButton.gameObject.SetActive(false);

            Time.captureFramerate = 0;  // Turn off Render Mode Framerate
        }

        public void Capture8KPanoramaVideo()
        {
#if OLD_RENDER_TECH
            _movieCapture.Resolution = 8192;
            _movieCapture.ResolutionH = 4096;
            _movieCapture.Mp4Bitrate = 100000;  //0.1249999966389106 Bits Per Pixel
            _movieCapture.NumberOfFramesToRender = 36000;  //20 Minutes Max.
            _movieCapture.FPS = 60;
            //_movieCapture.panoramaType = VRPanorama.VRCaptureRuntime.VRModeList.EquidistantMono;

            StartRecordingVideo(true);
#else
#endif
        }

        public void CaptureSingleFrame()
        {
            if (_cam == null)
            {
                Debug.Log("stereo 360 capture node has no camera or parent camera");
            }

            if (_stereoscopic)
            {
                _cam.stereoSeparation = 0.064f;
                _cam.RenderToCubemap(_leftEyeCubemap, 63, Camera.MonoOrStereoscopicEye.Left);
                _cam.RenderToCubemap(_rightEyeCubemap, 63, Camera.MonoOrStereoscopicEye.Right);
            }
            else
            {
                _cam.RenderToCubemap(_leftEyeCubemap, 63, Camera.MonoOrStereoscopicEye.Mono);
            }

            if (_stereoscopic)
            {
                _leftEyeCubemap.ConvertToEquirect(_equirect, Camera.MonoOrStereoscopicEye.Left);
                _rightEyeCubemap.ConvertToEquirect(_equirect, Camera.MonoOrStereoscopicEye.Right);
            }
            else
            {
                _leftEyeCubemap.ConvertToEquirect(_equirect, Camera.MonoOrStereoscopicEye.Mono);
            }
        }

        public void StopRecrodingVideo()
        {
            _stopRecordingButton.gameObject.SetActive(false);

#if OLD_RENDER_TECH
            _movieCapture.StopRecording();

            if (_originalHeight >= 0)
            {
                Screen.SetResolution(_originalWidth, _originalHeight, true);
                Debug.LogWarning("Set Resolution to " + _originalHeight + " for normal rendering");
                _originalHeight = 0;
                _originalWidth = 0;
            }

            AnimController.CancelAllScheuldedStops();
#else
            if (RenderManager.Recording)
            {
                _movieCapture.StopRecording();

                if (_originalHeight >= 0)
                {
                    Screen.SetResolution(_originalWidth, _originalHeight, true);
                    Debug.LogWarning("Set Resolution to " + _originalHeight + " for normal rendering");
                    _originalHeight = 0;
                    _originalWidth = 0;
                }

                AnimController.CancelAllScheuldedStops();
            }

            StopFrameCapture();
#endif
        }

        public void StartRecordingVideo(bool upscale = false)
        {
            if (upscale && _originalHeight <= 0)
            {
                _originalHeight = Screen.height;
                _originalWidth = Screen.width;
                Screen.SetResolution(4096, 8192, true);
                Debug.LogWarning("Set Resolution to 4096 for upscaled rendering");
            }

            _stopRecordingButton.gameObject.SetActive(true);
            _movieCapture.StartRecording("Test", true);

            AnimController.Instance.StartCoroutine(AnimController.ScheduleStop());
        }

        public void StartFrameCapture()
        {
            _session.SessionCreated();
            _session.BeginRecording();

            _stopRecordingButton.gameObject.SetActive(true);
        }

        public void StopFrameCapture()
        {
            _stopRecordingButton.gameObject.SetActive(false);

            if (_session != null)
                _session.EndRecording();
        }

        public void RenderCurrentClip()
        {
            AnimController.Instance.StartCoroutine(AnimController.RecordVideoClip());
        }

        public void RenderVR()
        {
            AnimController.Instance.StartCoroutine(AnimController.RecordVRClip());
        }

        public void RenderAllVideo()
        {
            AnimController.Instance.StartCoroutine(AnimController.RecordAllVideoClips());
        }
#endregion
    }
}