using UnityEditor;
using UnityEngine;

namespace CivilFX.Generic2
{
    public class ToolBarCivilFXMenuEditor : Editor
    {
        [MenuItem("CivilFX/Phase/Pedestrian Underpass", false, 100)]
        static void ToggleProposedRSB()
        {
            PhasedManager.Invoke(PhaseType.Pedestrian_Underpass);
        }


        [MenuItem("CivilFX/Phase/Pedestrian Overpass", false, 200)]
        static void ToggleExisting()
        {
            PhasedManager.Invoke(PhaseType.Pedestrian_Overpass);
        }

    }
}
