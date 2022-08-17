using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace CivilFX
{
    public class VideoPlayerRenderController : MonoBehaviour
    {
        [SerializeField]
        private VideoPlayer _video;

        private int lastVideoFrame = 0;

        private void Update()
        {
            if (Time.captureFramerate > 0)  //Recording
            {
                if (_video.isPlaying)
                {
                    lastVideoFrame = Time.frameCount;
                    _video.Pause();
                }
            }
            else if (!_video.isPlaying)
            {
                _video.Play();
            }
        }

        private void LateUpdate()
        {
            if (Time.captureFramerate > 0)  //Recording
            {
                float videoFramesPerCapture = Time.captureFramerate / _video.frameRate;
                if (Time.frameCount > lastVideoFrame + videoFramesPerCapture)
                {
                    _video.StepForward();
                    lastVideoFrame = Time.frameCount;
                }
            }
        }
    }
}