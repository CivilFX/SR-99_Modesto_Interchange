using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proposedCarSpawner : MonoBehaviour {

    public GameObject[] cars;

    public float minTime;
    public float maxTime;
    public float timer;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = Random.Range(minTime, maxTime);
            Spawn();
        }
    }

    public void Spawn()
    {
        Instantiate(cars[Random.Range(0, cars.Length)]);
    }

}
