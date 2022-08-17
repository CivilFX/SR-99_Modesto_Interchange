using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CivilFX
{
    public class TrafficPath_Old : MonoBehaviour
    {
        public string pathName = string.Empty;
        public Color pathColor = Color.magenta; // color of the path if visible in the editor
        public List<Vector3> nodes = new List<Vector3>() { Vector3.zero, Vector3.zero };
        public List<TrafficPath_Old> MergingPaths = new List<TrafficPath_Old>();
        public bool useStandardHandles = false;
        public bool forceStraightLinePath = false;
        public int pathResolution = 50;
        public int pathSpeedMPH = 65;
        public bool isMergingPath = false;
        public float mergeChance = 0.33f;
        public bool gapAcceptance = false;
        public bool displayInEditor = true;

        private GoSpline _spline;

        private float _checkDelay = 0f;
        

        protected List<AnimatedTrafficVehicle> _vehiclesOnPath;
        public List<AnimatedTrafficVehicle> VehiclesOnPath
        {
            get
            {
                if (_vehiclesOnPath == null)
                {
                    _vehiclesOnPath = new List<AnimatedTrafficVehicle>();
                }

                return _vehiclesOnPath;
            }
            set { _vehiclesOnPath = value; }
        }

        #region MonoBehaviours
        private void Awake()
        {
            StartCoroutine(ManageMerging());
        }

        private IEnumerator ManageMerging()
        {
            while (true)
            {
                _checkDelay -= Time.deltaTime;

                if (_checkDelay <= 0f){
                    _checkDelay = 0.55f + Random.Range(-0.5f, 0.5f);
                    bool acceptableGap = true;

                    if(_vehiclesOnPath != null) {
                        for (int i = 0; i < _vehiclesOnPath.Count; i++){
                            AnimatedTrafficVehicle vehicle = _vehiclesOnPath[i];

                            if (vehicle.IsNearStart(_checkDelay))
                            {
                                acceptableGap = false;
                                break;
                            }
                        }
                    }

                    gapAcceptance = acceptableGap;
                }
                
                yield return null;
            }
        }
        #endregion


        #region Editor Drawing)
        public void OnDrawGizmos()
        {
            // the editor will draw paths when force straight line is on
            if (!forceStraightLinePath && displayInEditor)
            {
                var spline = new GoSpline(nodes);
                Gizmos.color = pathColor;
                spline.drawGizmos(pathResolution);
            }
        }
        #endregion

        #region Helper Functions
        public GoSpline Spline()
        {
            if (_spline == null)
            {
                _spline = new GoSpline(nodes);
                _spline.buildPath();
            }

            return _spline;
        }
        #endregion

#if UNITY_EDITOR
        public void ProjectNodesOntoPhysics()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                Vector3 nodeLocation = nodes[i];

                RaycastHit upHit;
                RaycastHit downHit;
                float max = 10000f;
                float radius = 1f;
                float nearest = max;
                if (Physics.SphereCast(nodeLocation - Vector3.up * radius * 5f, radius, Vector3.up, out upHit, max))
                {
                    if (upHit.distance < nearest)
                    {
                        nearest = upHit.distance;
                        nodeLocation.y = upHit.point.y;
                    }
                }

                if (Physics.SphereCast(nodeLocation - Vector3.down * radius * 5f, radius, Vector3.down, out downHit, max))
                {
                    if (downHit.distance < nearest)
                    {
                        nearest = downHit.distance;
                        nodeLocation.y = downHit.point.y;
                    }
                }

                //if (nearest >= max && i > 0)
                //{
                //    nodeLocation.y = nodes[i - 1].y;
                //}

                nodes[i] = nodeLocation;
            }
        }

        public void Nudge(Vector3 mod)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                Vector3 nodeLocation = nodes[i];
                nodeLocation += mod;
                nodes[i] = nodeLocation;
            }
        }
#endif
    }
}