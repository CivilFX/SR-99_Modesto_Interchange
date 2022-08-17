using CivilFX.UI2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CivilFX.TrafficV5
{
    public class SpawnPointGroupController
    {
        private List<SpawnPointController> controllers = new List<SpawnPointController>();

        private Color selectedColor;
        private Color deselectedColor;

        private int lastSelectedIndex;

        private const int INVALID_INDEX = -1;
        private TrafficInflowCountPanelController panelController;
        public SpawnPointGroupController(Color _selectedColor, Color _deselectedColor)
        {
            selectedColor = _selectedColor;
            deselectedColor = _deselectedColor;
            lastSelectedIndex = INVALID_INDEX;
        }

        public void Add(SpawnPointController cont)
        {
            cont.id = controllers.Count;
            cont.selectedColor = selectedColor;
            cont.deselectedColor = deselectedColor;
            cont.groupController = this;
            controllers.Add(cont);
            
        }

        public void NotifyChange(SpawnPointController cont, bool changedStatus)
        {
            if (panelController == null) {
                panelController = Resources.FindObjectsOfTypeAll<TrafficInflowCountPanelController>()[0];
            }

            panelController.OnHidden();
            panelController.groupController = this;

            if (lastSelectedIndex != INVALID_INDEX) {
                controllers[lastSelectedIndex].OnDeselect();
            }
            if (changedStatus) {
                lastSelectedIndex = cont.id;
                controllers[cont.id].OnSelect();
                panelController.OnVisible(controllers[cont.id].pathController);
            } else {
                lastSelectedIndex = INVALID_INDEX;
            }        
        }

        public void ShowAllSpawnPoints()
        {
            foreach (var spawnPoint in controllers) {
                spawnPoint.gameObject.SetActive(true);
            }
        }
        public void HideAllSpawnPoints()
        {
            foreach (var spawnPoint in controllers) {
                spawnPoint.gameObject.SetActive(false);
            }
        }

        public void OnDone()
        {
            if (lastSelectedIndex != INVALID_INDEX) {
                controllers[lastSelectedIndex].OnDeselect();
                lastSelectedIndex = INVALID_INDEX;
            }
        }
    }
}