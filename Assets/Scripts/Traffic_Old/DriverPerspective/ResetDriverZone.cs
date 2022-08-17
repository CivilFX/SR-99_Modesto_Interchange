using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CivilFX
{
    public class ResetDriverZone : MonoBehaviour
    {

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                DrivingStartup drivingStartup = GameObject.FindObjectOfType<DrivingStartup>();

                if (drivingStartup != null)
                    drivingStartup.ResetCar();
            }
        }

        //void OnTriggerExit(Collider other)
        //{    
        //}
    }
}