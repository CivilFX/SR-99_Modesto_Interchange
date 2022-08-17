using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CivilFX.TrafficV5
{
    public class DivergePointController : MonoBehaviour
    {
        public ParticleSystem particle;

        public DivergePointGroupController groupController;
        public TrafficPathController pathDiverging;
        public TrafficPathController pathEntering;


        public Color selectedColor {
            private get; set;
        }
        public Color deselectedColor {
            private get; set;
        }

        public int id {
            get;
            set;
        }

        private bool isSelected {
            get; set;
        }

        
        public void SetColor(Color color)
        {
            var main = particle.main;
            main.startColor = color;          
        }

        public void OnVisible()
        {
            gameObject.SetActive(true);
        }

        public void OnHidden()
        {
            gameObject.SetActive(false);
        }

        public void OnSelect()
        {
            isSelected = true;
            SetColor(selectedColor);
        }

        public void OnDeselect()
        {
            isSelected = false;
            SetColor(deselectedColor);
        }

        private void OnMouseUp()
        {
            Debug.Log("OnMouseDown");
            isSelected = !isSelected;
            groupController.NotifyChange(this, isSelected);
        }

        private void OnMouseEnter()
        {
            Debug.Log("OnMouseEnter");
            SetColor(selectedColor);
        }

        private void OnMouseExit()
        {
            Debug.Log("OnMouseExit");
            if (!isSelected) {
                SetColor(deselectedColor);
            }
        }
    }
}