using UnityEngine;
using System.Collections;

public class FriendlyFishMovement : MonoBehaviour {

	public TestModel path;
	public float dolphinSpeed;
	private Vector3 dolphinPosition;
	private float t = 0.0f;

	// Use this for initialization
	void Start () {
		path = GameObject.Find("LeftWall").GetComponent<TestModel>();
	}
	
	// Update is called once per frame
	void Update () {
	
		t += -0.004f * Time.deltaTime;
		if (t >= 1) t = 0;
		OrientedPoint p = new OrientedPoint();
		p = path.bezier.GetOrientedPoint(t);
		transform.rotation = p.rotation;
		transform.position = p.position + transform.position;
	}
}
