using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CivilFX.TrafficV5
{
    public class PedestrianController : MonoBehaviour
    {
        Animator animator;
        VehicleController vehicleController;


        void Awake()
        {
            animator = gameObject.GetComponent<Animator>();

            vehicleController = gameObject.GetComponent<VehicleController>();
        }


        void Update()
        {
            if (vehicleController.speed < 0.05f)
            {
                animator.speed = 0f;
                animator.Play("Walking", 0, 0);
            }

            else
            {
                animator.speed = 1f;
            }
        }
    }
}
