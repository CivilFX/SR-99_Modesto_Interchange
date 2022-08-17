using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class videoPlaybackSpeed : MonoBehaviour {


    private VideoPlayer vid;

	// Use this for initialization
	void Start () {
        vid = gameObject.GetComponent<VideoPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (vid.isActiveAndEnabled)
        {
            vid.Pause();
            vid.StepForward();
        }
    }
}
