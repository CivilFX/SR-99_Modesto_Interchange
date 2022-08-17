using UnityEngine;
using System.Collections;

public class RenderElementManager : MonoBehaviour
{
    public delegate void RenderElementToggle(bool enabled);
    public static RenderElementToggle REToggle;

    private static RenderElementManager instance = null;

    public static RenderElementManager GetInstance()
    {
        if (instance == null) {
            GameObject renderElementManagerInstance = new GameObject("RenderElementManager");
            instance = renderElementManagerInstance.AddComponent<RenderElementManager>();
        }

        return instance;
    }

    public static void RaiseRenderElementToggled(bool enabled)
    {
        if (REToggle != null)
            REToggle(enabled);
    }


    private bool displayFeatures = false;
    private bool releasedKeys = true;

    //I've always enjoyed coding easter Eggs.
    private void Update()
    {
        if (releasedKeys) {
            if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && (Input.GetKey(KeyCode.R) || Input.GetKey("r"))) {
                                                                                                                         
                Debug.Log("Toggling Render Element Features");
                displayFeatures = !displayFeatures;
                releasedKeys = false;
                RaiseRenderElementToggled(displayFeatures);

                ForceLOD();
            }
        }
        else {
            if (!((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && (Input.GetKey(KeyCode.R) || Input.GetKey("r")))) {
                Debug.Log("Keys Released");
                releasedKeys = true;
            }
        }
    }

    private void ForceLOD()
    {
        ///   Don't Force LODs for now.

        /*
        LODGroup[] lodGroups = GameObject.FindObjectsOfType<LODGroup>() as LODGroup[];

        if(lodGroups != null)
        {
            foreach(LODGroup lodGroup in lodGroups)
            {
                lodGroup.ForceLOD(0);
            }
        }
        */
    }

}
