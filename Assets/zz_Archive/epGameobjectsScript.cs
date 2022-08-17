using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class epGameobjectsScript : MonoBehaviour {


    public GameObject existing;
    public GameObject proposed;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Existing() {
        existing.SetActive(true);
        proposed.SetActive(false);

    }

    public void Proposed()
    {
        proposed.SetActive(true);
        existing.SetActive(false);
    }

    public void BothOn()
    {
        proposed.SetActive(true);
        existing.SetActive(true);
    }
}
