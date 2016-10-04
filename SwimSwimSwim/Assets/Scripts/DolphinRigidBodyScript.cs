using UnityEngine;
using System.Collections;

public class DolphinRigidBodyScript : MonoBehaviour {

    private Rigidbody body;
	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 destination = Vector3.zero - transform.localPosition;
        body.velocity = (destination.normalized * 2);
	
	}
}
