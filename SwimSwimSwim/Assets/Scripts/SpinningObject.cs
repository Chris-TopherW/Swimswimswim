using UnityEngine;
using System.Collections;

public class SpinningObject : MonoBehaviour {

	public int speed = 50;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.Rotate (Vector3.down * Time.deltaTime * speed);
	}
}
