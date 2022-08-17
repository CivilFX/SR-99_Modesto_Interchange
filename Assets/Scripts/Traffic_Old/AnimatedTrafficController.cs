using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace CivilFX
{
    public class AnimatedTrafficController : MonoBehaviour
    {
        #region Constants
        private const float MPH_TO_METERS_PER_SECOND = 0.44704f;    //1609.3440f / 60f / 60f;  
        #endregion

        #region Inspector Fields
        [SerializeField]
        private List<GameObject> _carPrefabs;

        [SerializeField]
        private List<TrafficPath_Old> _lanePaths;

        [SerializeField]
        private int[] _vehicleTypePercentage = new int[(int)(VehicleType.NUM_TYPES)];

        [SerializeField]
        private int _carCount;
        #endregion

        #region Fields
        private List<GameObject> _carPool;          //see what I did there?   HOV?   ok - moving on.
        #endregion

        #region Properties
        #endregion

        #region Behaviour Overrides
        private void Awake()
        {
            System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            int nowSeconds = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;

            int total_percent = 100;
            int[] vehicleTypeSpawnCount = new int[(int)(VehicleType.NUM_TYPES)];
            for (int i = 0; i < (int)(VehicleType.NUM_TYPES); i++)
            {
                // Determine how many cars of each vehicle type to create.
                vehicleTypeSpawnCount[i] = Mathf.CeilToInt((float)_vehicleTypePercentage[i] * .01f * (float)_carCount);
                total_percent -= _vehicleTypePercentage[i];
            }

            if (total_percent != 0)
            {
                // The user entered incorrect percentages that did not add up to 100.  Don't spawn any vehicles until fixed.
                return;
            }

            Random.seed = nowSeconds;
            if (_carPrefabs != null && _carPrefabs.Count > 0 && _lanePaths != null && _lanePaths.Count > 0)
            {
                int numCarsPerLane = Mathf.CeilToInt((float)_carCount / (float)_lanePaths.Count);
                float totalLength = 0f;
                int l = 0;
                for (l = 0; l < _lanePaths.Count; ++l)
                {
                    totalLength += _lanePaths[l].Spline().Length;
                }

                float averageLength = totalLength / l;

                int[] prefabVehiclePerTypeCount = new int[(int)(VehicleType.NUM_TYPES)];

                for (int i = 0; i < _carPrefabs.Count; i++)
                {
                    AnimatedTrafficVehicle comp = _carPrefabs[i].GetComponent<AnimatedTrafficVehicle>();

                    if (comp != null)
                    {
                        // Count how many of each vehicle type there are.
                        prefabVehiclePerTypeCount[(uint)(comp.GetVehicleType())]++;
                    }
                    else
                    {
                        Debug.LogWarning("Missing AnimatedTrafficVehicle Script: " + _carPrefabs[i].name);
                    }
                }

                List<int> spawnCountPerPrefabVehicle = new List<int>();
                for (int i = 0; i < _carPrefabs.Count; i++)
                {
                    AnimatedTrafficVehicle comp = _carPrefabs[i].GetComponent<AnimatedTrafficVehicle>();
                    uint vehicleType = (uint)(comp.GetVehicleType());
                    if (vehicleTypeSpawnCount[vehicleType] == 0)
                    {
                        // No vehicles of this type (like heavy) should be spawned.
                        spawnCountPerPrefabVehicle.Add(0);
                        continue;
                    }
                    // Calculate how many of each specific vehcile to spawn.
                    int count = Mathf.CeilToInt((float)vehicleTypeSpawnCount[vehicleType] / (float)prefabVehiclePerTypeCount[vehicleType]);
                    spawnCountPerPrefabVehicle.Add(count);
                }

                // For better visuals, spawn the same number of each type of vehicle in each lane.  When they are spawned is random so you can's see the pattern.
                List<int> vehiclePerLaneMaster = new List<int>();
                for (int i = 0; i < spawnCountPerPrefabVehicle.Count; i++)
                {
                    if (spawnCountPerPrefabVehicle[i] == 0)
                    {
                        // Special case where the vehicle of a certain type (like heavy) isn't spawned.
                        continue;
                    }
                    int count = Mathf.CeilToInt((float)spawnCountPerPrefabVehicle[i] / (float)_lanePaths.Count);
                    // Determine how many of each type of vehicle to put in each lane.
                    for (int j = 0; j < count; j++)
                    {
                        vehiclePerLaneMaster.Add(i);
                    }
                }

                for (int i = 0; i < _lanePaths.Count; ++i)
                {
                    TrafficPath_Old lanePath = _lanePaths[i];
                    GoSpline laneSpline = _lanePaths[i].Spline();

                    float weight = 1f;
                    float lengthMod = laneSpline.Length / averageLength;
                    int numCars = (int)(numCarsPerLane * lengthMod);

                    if (lanePath.MergingPaths != null)
                        weight = 1f + (lanePath.MergingPaths.Count * 0.25f) * lengthMod;

                    // Copy the list into the temporary list so that we can remove from it.
                    List<int> vehiclePerLane = new List<int>(vehiclePerLaneMaster);

                    for (int x = 0; x < numCars; ++x)
                    {
                        if (vehiclePerLane.Count == 0)
                        {
                            break;
                        }
                        int carIndex = Random.Range(0, vehiclePerLane.Count);

                        // Index into the vehiclePerLane which has the index into _carPrefabs.
                        GameObject carInstance = GameObject.Instantiate(_carPrefabs[vehiclePerLane[carIndex]]) as GameObject;
                        AnimatedTrafficVehicle vehicle = carInstance.GetComponent<AnimatedTrafficVehicle>();

                        float speed = lanePath.pathSpeedMPH * MPH_TO_METERS_PER_SECOND; //Meters Per Second
                        float duration = laneSpline.Length / speed;
                        float offset = ((x + Random.Range(-0.25f, 0.25f)) / (float)numCars);

                        if (vehicle != null)
                        {
                            vehicle.SetPath(speed, duration, offset, laneSpline, lanePath);
                            lanePath.VehiclesOnPath.Add(vehicle);
                        }

                        // Remove an element since we spawned a vehicle with it.
                        vehiclePerLane.RemoveAt(carIndex);

                        if (carInstance != null)
                        {
                            Transform carNode = lanePath.transform;
                            if (carNode != null)
                            {
                                carInstance.transform.parent = carNode;
                                carInstance.transform.localPosition = Vector3.zero;
                                carInstance.transform.localRotation = Quaternion.identity;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        public bool HasParameter(string paramName, Animator animator)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == paramName) return true;
            }
            return false;
        }
    }
}