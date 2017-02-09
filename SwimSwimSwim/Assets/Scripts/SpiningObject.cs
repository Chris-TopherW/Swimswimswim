using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiningObject : MonoBehaviour {

	public int speed = 50;
	public bool vertical = false;
	public bool horizontal = true;

	void Update () {
		if(horizontal) {
		transform.Rotate (Vector3.down * Time.deltaTime * speed);
		}
		if(vertical) {
			transform.Rotate (Vector3.left * Time.deltaTime * speed);
		}
	}
}
