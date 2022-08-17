using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveHighlightScript : MonoBehaviour {


    public Transform[] target;
    
    public float speed;

    public int curInt;

    //public GameObject button;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float step = speed * 100 * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, target[curInt].position, step);
	}


    //public void Click() {

    //    curInt = button.GetComponent<buttonIntScript>().value;
    //}

}
