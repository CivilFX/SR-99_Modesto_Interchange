using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypointScript : MonoBehaviour {

    public Transform[] target;
    public float speed;

    public int current;

    public int rotateSpeed;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (transform.position != target[current].position)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, target[current].position, speed * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(pos);
        }
        else current = (current + 1) % target.Length;

        // DESTROY IF REACHED THE LAST TARGET - 1 
        if (current >= target.Length - 1) {
            Destroy(gameObject);
        }


        // ROTATE

        Vector3 targetDir = target[current].position - transform.position;

        // The step size is equal to speed times frame time.
        float step = rotateSpeed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        Debug.DrawRay(transform.position, newDir, Color.red);

        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);
    }


}
