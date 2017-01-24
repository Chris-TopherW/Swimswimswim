using UnityEngine;
using System.Collections;

public class Copycam : MonoBehaviour
{
    public Camera oldCam;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    gameObject.transform.position = new Vector3(oldCam.transform.position.x + 100, gameObject.transform.position.y, gameObject.transform.position.z);
	}
}
