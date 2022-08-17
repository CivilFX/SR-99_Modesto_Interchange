using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CivilFX
{
    public class TurnAroundWarningZone : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                DrivingStartup drivingStartup = GameObject.FindObjectOfType<DrivingStartup>();

                if (drivingStartup != null)
                    drivingStartup.WarnDriver(true);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                DrivingStartup drivingStartup = GameObject.FindObjectOfType<DrivingStartup>();

                if (drivingStartup != null)
                    drivingStartup.WarnDriver(false);
            }
        }
    }
}
