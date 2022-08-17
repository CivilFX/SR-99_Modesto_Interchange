using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace CivilFX
{
    public class CameraNodeButton : MonoBehaviour
    {
        public delegate void CameraNodeEvent(CameraNode_Old cameraNode);
        public event CameraNodeEvent OnCameraNodeSelected;

        private CameraNode_Old _cameraNode;
        private Button _button;

        [SerializeField]
        private Image _buttonBackground;

        [SerializeField]
        private TextMeshProUGUI _title;

        public CameraNode_Old CameraNode_Old
        {
            get
            {
                return _cameraNode;
            }

            set
            {
                _cameraNode = value;
                SetStyle(value);
            }
        }

        private void Awake()
        {
            _button = gameObject.GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(RaiseCameraNodeSelectedEvent);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(RaiseCameraNodeSelectedEvent);
        }

        public void RaiseCameraNodeSelectedEvent()
        {
            if (OnCameraNodeSelected != null)
                OnCameraNodeSelected(CameraNode_Old);

            CameraNode_Old.GainedFocus();
        }

        public void SetStyle(CameraNode_Old cameraNode)
        {
            if (_buttonBackground != null)
                _buttonBackground.color = _cameraNode.IsAnimated ? new Color(176f / 255f, 206f / 255f, 236f / 255f) : Color.white;
            SetTitle(cameraNode.Title);
        }

        public void SetTitle(string title)
        {
            _title.text = title;
        }
    }
}