using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CivilFX
{
    public class CamWireframeMode : MonoBehaviour
    {
        public bool wireFrameOn;

        void OnPreRender()
        {
            if(wireFrameOn)
                GL.wireframe = true;
        }

        void OnPostRender()
        {
            if (wireFrameOn)
                GL.wireframe = false;
        }
    }
}