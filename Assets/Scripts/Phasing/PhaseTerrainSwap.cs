using UnityEngine;
using System.Collections.Generic;

namespace CivilFX
{
    [RequireComponent(typeof(Terrain))]
    [RequireComponent(typeof(TerrainCollider))]
    public class PhaseTerrainSwap : MonoBehaviour
    {
        #region Properties
        public List<PhaseTerrain> _terrains;
        #endregion

        #region Fields
        private Terrain _terrain;
        private TerrainCollider _terrainCollider;
        #endregion

        #region Inspector Overrides
        void Start()
        {
            _terrain = gameObject.GetComponent<Terrain>();
            _terrainCollider = gameObject.GetComponent<TerrainCollider>();
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
            if (_terrain == null)
                _terrain = gameObject.GetComponent<Terrain>();

            if (_terrainCollider == null)
                _terrainCollider = gameObject.GetComponent<TerrainCollider>();

            if (_terrain != null && _terrains.Count > 0)
            {
                bool terrainForPhase = false;

                for (int i = 0; i < _terrains.Count; ++i)
                {
                    PhaseTerrain phaseTerrain = _terrains[i];
                    if (((int)phaseTerrain.Phase & (int)newPhase) == (int)newPhase)
                    {
                        _terrain.terrainData = phaseTerrain.Data;
                        _terrainCollider.terrainData = phaseTerrain.Data;
                        terrainForPhase = true;
                        break;
                    }
                }

                if (!terrainForPhase)
                {
                    _terrain.terrainData = null;
                }
            }
        }
        #endregion
    }
}