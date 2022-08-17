using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonIntScript : MonoBehaviour {

    public int value;

    public GameObject highlight;

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public void Click() {
        highlight.GetComponent<moveHighlightScript>().curInt = value;
    }
}
