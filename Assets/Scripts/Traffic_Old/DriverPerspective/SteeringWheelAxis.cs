using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CivilFX
{
    public class SteeringWheelAxis : MonoBehaviour
    {
        private Transform _transform;

        private void Awake()
        {
            _transform = gameObject.transform;
        }

        void Update()
        {

            Vector3 newRotation = _transform.localEulerAngles;

            newRotation.y = Input.GetAxis("Horizontal") * 60f;

            _transform.localEulerAngles = newRotation;

        }
    }
}
