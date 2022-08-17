using UnityEngine;
using System.Collections.Generic;

namespace CivilFX
{
    [RequireComponent(typeof(MeshFilter))]
    public class PhaseMeshSwap : MonoBehaviour
    {
        #region Properties
        public List<PhaseMesh> _meshes;
        #endregion

        #region Fields
        private MeshFilter _meshFilter;
        #endregion

        #region Inspector Overrides
        void Start()
        {
            _meshFilter = gameObject.GetComponent<MeshFilter>();
            PhaseManager.PhaseShifted += PhaseShifted;
        }

        void OnDestroy()
        {
            PhaseManager.PhaseShifted -= PhaseShifted;
        }
        #endregion

        #region Callbacks
        public void PhaseShifted(Phase newPhase)
        {
            if (_meshes != null && _meshes.Count > 0)
            {
                bool meshForPhase = false;

                if (_meshFilter == null)
                    _meshFilter = gameObject.GetComponent<MeshFilter>();

                for (int i = 0; i < _meshes.Count; ++i)
                {
                    PhaseMesh phaseMesh = _meshes[i];
                    if (((int)phaseMesh.Phase & (int)newPhase) == (int)newPhase)
                    {
                        _meshFilter.mesh = phaseMesh.Mesh;
                        meshForPhase = true;
                        break;
                    }
                }

                if (!meshForPhase)
                {
                    _meshFilter.mesh = null;
                }
            }
        }
        #endregion
    }
}