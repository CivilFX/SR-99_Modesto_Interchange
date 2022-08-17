using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace CivilFX
{
    public class RenderManager : MonoBehaviour
    {
        #region Constants
        private const int FRAME_DELAY = 3;
        #endregion

        #region Inspector Fields
        [Header("Inspector Assigned Objects")]
        [SerializeField]
        private RawImage _previewDisplay;

        [SerializeField]
        private RenderTexture _renderTexture;

        [Header("Render Settings")]
        [Tooltip("1 is for preview, 8 is standard, and 32 is rendered 4x and then downsampled")]
        [Range(1, 32)]
        public int _renderQuality = 8;

        public bool _downsample8K = true;
        public bool _deleteImages = true;

        public string _folder = "RenderCache";
        public string _absolutePath = String.Empty;
        #endregion

        #region private Inspector Fields
        private Camera _camera;
        #endregion

        #region Fields
        private string _extensionString = ".jpg";
        private string _renderName = "CivilFX";
        private string _folderWithDate = "CivilFX";
        private static bool _isRecording;
        private int _startFrame;

        private int _resolution;
        private int _resolutionH;
        private int _mp4Bitrate;
        private int _fps;
        private bool _manualRender;
        #endregion

        #region Properties
        public int Resolution { get { return _resolution; } set { _resolution = value; } }
        public int ResolutionH { get { return _resolutionH; } set { _resolutionH = value; } }
        public int Mp4Bitrate { get { return _mp4Bitrate; } set { _mp4Bitrate = value; } }
        public int FPS { get { return _fps; } set { _fps = value; } }
        public int RenderQuality { get { return _renderQuality; } set { _renderQuality = value; } }
        public static bool Recording {  get { return _isRecording; }  }
        #endregion

        #region Placeholder To Ween Code Off
        [HideInInspector]
        public int NumberOfFramesToRender;
        #endregion

        #region Behaviour Overrides
        void Awake()
        {
            _camera = GetComponent<Camera>();
            _camera.targetTexture = null;
            _previewDisplay.enabled = false;
        }

        void LateUpdate()
        {
            if (_isRecording && !_manualRender)
            {
                RenderVideo();
            }
        }
        #endregion

        #region Public Calls
        public void TakeScreenShot(String renderName = default(String), bool newTexture = false)
        {
            _renderName = renderName;
            _startFrame = Time.frameCount - FRAME_DELAY;
            _folderWithDate = _folder + DateTime.Now.ToString("yyyyMMdd_hh_mm_ss");
            Directory.CreateDirectory(Folder());
            _previewDisplay.enabled = true;

            if (newTexture)
            {
                _renderTexture = NewRenderTexture();
            }

            _camera.targetTexture = _renderTexture;

            _previewDisplay.texture = _renderTexture;

            StartCoroutine(TakeScreenShotAfterDelay());
        }

        private IEnumerator TakeScreenShotAfterDelay()
        {
            yield return null;
            yield return null;

            CaptureScreenShot(_renderTexture);

            StopRecording(false);

        }

        public void StartRecording(String renderName = default(String), bool newTexture = false, bool manualRender = false)
        {
            _renderName = renderName;
            _startFrame = Time.frameCount;
            _folderWithDate = _folder + DateTime.Now.ToString("yyyyMMdd_hh_mm_ss");
            _manualRender = manualRender;
            Directory.CreateDirectory(Folder());
            _previewDisplay.enabled = true;

            if (newTexture)
            {
                _renderTexture = NewRenderTexture();
            }

            _previewDisplay.texture = _renderTexture;
            _camera.targetTexture = _renderTexture;
            _isRecording = true;
        }

        public void StopRecording(bool renderVideo = true)
        {
            _isRecording = false;
            _previewDisplay.enabled = false;
            _camera.targetTexture = null;

#if UNITY_EDITOR
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = Folder(),
                UseShellExecute = true,
                Verb = "open"
            });
#endif

            if (renderVideo)
            {
                ExportVideo();
            }
        }
        #endregion

        private RenderTexture NewRenderTexture()
        {
            int width = Resolution * 4 / 32 * RenderQuality;
            int height = ResolutionH * 4 / 32 * RenderQuality;
            UnityEngine.Debug.LogWarning("Creating New Render Texture" + width + " x " + height + "  :: HDR sRGB");
            return new RenderTexture(width, height, 0, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.sRGB);
        }

        private String Folder()
        {
            return (string.IsNullOrEmpty(_absolutePath)) ? Path.GetFullPath(string.Format(@"{0}/", _folderWithDate)) : Path.Combine(_absolutePath, string.Format(@"{0}/", _folderWithDate));
        }

        #region Render Functions
        private void RenderVideo()
        {
            if ((Time.frameCount - _startFrame) > FRAME_DELAY)
            {
                CaptureScreenShot(_renderTexture);
            }
        }

        public void SaveRenderTexture(RenderTexture source, string renderName)
        {
            _renderName = renderName;
            
            if (!_manualRender)
            {
                _folderWithDate = _folder + DateTime.Now.ToString("yyyyMMdd_hh_mm_ss");
                Directory.CreateDirectory(Folder());
            }

            CaptureScreenShot(source);

            if (!_manualRender)
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = Folder(),
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
        }

        private void CaptureScreenShot(RenderTexture source)
        {
            Texture2D captureTex = GrabScreenShot(source);
            _previewDisplay.material.mainTexture = captureTex;
            byte[] bytes = captureTex.EncodeToJPG(100);
            string filePath = string.Format("{0}/{1}{2:D05}{3}", Folder(), _renderName, (Time.frameCount - _startFrame + (_manualRender ? 0 : - FRAME_DELAY)), _extensionString);
            File.WriteAllBytes(filePath, bytes);
        }

        private Texture2D GrabScreenShot(RenderTexture rt)
        {
            RenderTexture currentActiveRT = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D tex = new Texture2D(rt.width, rt.height);
            tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
            RenderTexture.active = currentActiveRT;
            return tex;
        }


        private void ExportVideo()
        {
            string fullPath = Folder();
#if UNITY_EDITOR
            string ffmpegPath = Path.GetFullPath(string.Format(@"{0}/", "Assets\\StreamingAssets\\"));
#else
                string ffmpegPath = Path.GetFullPath(Application.dataPath + "/StreamingAssets/"); //Path.DirectorySeparatorChar + "Plugins"
#endif

            if (Resolution > 4096)
            {
#if UNITY_EDITOR
                string strCmdText = String.Empty;
                if (!_downsample8K)
                {
                    strCmdText = "/C " + ffmpegPath + "ffmpeg -f image2" + " -framerate " + FPS + " -i \"" + fullPath + _renderName + "%05d" + _extensionString + "\" -vf scale=" + Resolution + ":" + ResolutionH + "  -r " + FPS + " -vcodec libx265 -y -pix_fmt yuv420p -b:v " + Mp4Bitrate + "k \"" + fullPath + _renderName + ".mp4\"";
                    strCmdText = strCmdText + "&" + ffmpegPath + "ffmpeg - f image2" + " - framerate " + FPS + " - i \"" + fullPath + _renderName + "%05d" + _extensionString + "\" -vf scale=" + (int)(Resolution * 0.5f) + ":" + (int)(ResolutionH * 0.5f) + "  -r " + FPS + " -vcodec libx264 -y -pix_fmt yuv420p -b:v " + (Mp4Bitrate * 0.5f) + "k \"" + fullPath + _renderName + "_AAx2.mp4\"";
                }
                else
                {
                    strCmdText = "/C " + ffmpegPath + "ffmpeg -f image2" + " -framerate " + FPS + " -i \"" + fullPath + _renderName + "%05d" + _extensionString + "\" -vf scale=" + (int)(Resolution * 0.5f) + ":" + (int)(ResolutionH * 0.5f) + "  -r " + FPS + " -vcodec libx264 -y -pix_fmt yuv420p -b:v " + (Mp4Bitrate * 0.5f) + "k \"" + fullPath + _renderName + "_AAx2.mp4\"";
                }

                if (_deleteImages)
                    strCmdText = strCmdText + "&del " + fullPath + "*" + _extensionString + " /a";

                Process.Start("CMD.exe", strCmdText);
                UnityEngine.Debug.LogWarning("Executing: " + strCmdText);
#else
                Process.Start(ffmpegPath + "ffmpeg", " -f image2" + " -framerate " + FPS + " -i \"" + fullPath + _renderName + "%05d" + _extensionString + "\" -vf scale=" + (int)(Resolution * 0.5f) + ":" + (int)(ResolutionH * 0.5f) + "  -r " + FPS + " -vcodec libx264 -y -pix_fmt yuv420p -b:v " + (Mp4Bitrate * 0.5f) + "k \"" + fullPath + _renderName + "_AAx2.mp4\"");
#endif
            }
            else {
#if UNITY_EDITOR
                string strCmdText = "/C " + ffmpegPath + "ffmpeg -f image2" + " -framerate " + FPS + " -i \"" + fullPath + _renderName + "%05d" + _extensionString + "\" -vf scale=" + Resolution + ":" + ResolutionH + "  -r " + FPS + " -vcodec libx264 -y -pix_fmt yuv420p -b:v " + Mp4Bitrate + "k \"" + fullPath + _renderName + ".mp4\"";

                if (_deleteImages)
                    strCmdText = strCmdText + "&del " + fullPath + "*" + _extensionString + " /a";

                Process.Start("CMD.exe", strCmdText);
                UnityEngine.Debug.LogWarning("Executing: " + strCmdText);
#else
                Process.Start(ffmpegPath + "ffmpeg", " -f image2" + " -framerate " + FPS + " -i \"" + fullPath + _renderName + "%05d" + _extensionString + "\" -vf scale=" + Resolution + ":" + ResolutionH + "  -r " + FPS + " -vcodec libx264 -y -pix_fmt yuv420p -b:v " + Mp4Bitrate + "k \"" + fullPath + _renderName + ".mp4\"");
#endif
            }

        }
        #endregion
    }
}