
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using TMPro;

namespace CivilFX
{
    public class CameraNodeList : MonoBehaviour
    {
        #region Inspector Fields
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private Transform _cameraTarget;

        [SerializeField]
        private GameObject _cameraPanel;

        [SerializeField]
        private GridLayoutGroup _grid;

        [SerializeField]
        private GameObject _cameraNodeCell;

        [SerializeField]
        private GameObject _cameraCategoryCell;
        #endregion

        #region Fields
        private List<CameraNode_Old> _cameraNodes;
        private CameraNode_Old _currentNode;
        private List<CameraNodeButton> _cameraNodeButtons;
        private bool isVisible;
        #endregion

        #region Properties
        public CameraNode_Old CurrentCameraNode { get { return _currentNode; } private set { _currentNode = value; } }
        #endregion


        private void Start()
        {
            if (_cameraNodeCell != null && _cameraCategoryCell != null)
            {
                //get all Camera Nodes in the scene.
                _cameraNodes = GameObject.FindObjectsOfType<CameraNode_Old>().ToList();
                _cameraNodeButtons = new List<CameraNodeButton>();

                if (_cameraNodes != null)
                    _cameraNodes.Sort((x, y) => x.CompareNodes(y));

                CameraNode_Old.CameraCategory currentCategory = CameraNode_Old.CameraCategory.None;
                foreach (CameraNode_Old cameraNode in _cameraNodes)
                {
                    GameObject newCameraNodeCell = GameObject.Instantiate(_cameraNodeCell) as GameObject;
                    CameraNodeButton cameraNodeButton = newCameraNodeCell.GetComponent<CameraNodeButton>();

                    if (cameraNodeButton != null)
                    {
                        _cameraNodeButtons.Add(cameraNodeButton);

                        if (currentCategory != cameraNode.ViewCategory)
                        {
                            currentCategory = cameraNode.ViewCategory;
                            GameObject newCameraCategoryCell = GameObject.Instantiate(_cameraCategoryCell) as GameObject;

                            if (cameraNodeButton != null)
                            {
                                TextMeshProUGUI textComponent = newCameraCategoryCell.GetComponentInChildren<TextMeshProUGUI>();
                                textComponent.text = currentCategory.ToString();
                                newCameraCategoryCell.transform.SetParent(_grid.transform);
                                newCameraCategoryCell.transform.localScale = Vector3.one;
                                newCameraCategoryCell.transform.localPosition = Vector3.zero;
                            }
                        }

                        cameraNodeButton.CameraNode_Old = cameraNode;
                        cameraNodeButton.OnCameraNodeSelected += MoveCameraToNode;
                        cameraNodeButton.transform.SetParent(_grid.transform);
                        cameraNodeButton.transform.localScale = Vector3.one;
                        cameraNodeButton.transform.localPosition = Vector3.zero;
                    }
                }
            }

            AnimController.Instance.HidePanel();
        }

        private void OnDestroy()
        {

        }
        
        public void MoveCameraToNode(CameraNode_Old cameraNode)
        {
            DisableSelectedNodes();

            //_cameraTarget.SetParent(cameraNode.gameObject.transform);
            //_cameraTarget.transform.localPosition = Vector3.zero;
            //_cameraTarget.transform.localRotation = Quaternion.identity;
            //cameraNode.GainedFocus();
            _currentNode = cameraNode;
        }

        public void DisableSelectedNodes()
        {
            foreach (CameraNodeButton cameraNodeButton in _cameraNodeButtons)
            {
                //cameraNodeButton.Unselect();
                //cameraNodeButton.CameraNode.LostFocus();
            }
        }
        
        private void ShowCameraNodePanel()
        {
            isVisible = !isVisible;
            _cameraPanel.SetActive(isVisible);
        }
    }

}
