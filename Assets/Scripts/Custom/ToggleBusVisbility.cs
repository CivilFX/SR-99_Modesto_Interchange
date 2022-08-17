using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CivilFX
{
    public class ToggleBusVisbility : MonoBehaviour
    {
        public GameObject[] busChevrons;


        void Update()
        {
            int chevronCounter = 0;

            if (busChevrons != null)
            {
                foreach(var busChevron in busChevrons)
                {
                    if(busChevron.gameObject.GetComponent<MaterialsSwapper>().isShowingTemp)
                    {
                        chevronCounter++;
                    }
                }
            }


            if (chevronCounter > 0)
                gameObject.GetComponent<Renderer>().enabled = true;

            else
                gameObject.GetComponent<Renderer>().enabled = false;

        }
    }
}