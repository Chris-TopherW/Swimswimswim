using UnityEngine;
using System.Collections;

public class FriendlyFishMovement : MonoBehaviour {

    private BezierCurve path;
	public Vector3 fishOffset;
	private float t = 0.0f;

	// Use this for initialization
	void Start () {
        path = GameObject.FindGameObjectWithTag("Spline").GetComponent<TestModel>().bezier;
        t = GameManager.splinePos + 0.02f;
	}
	
	// Update is called once per frame
	void Update () {
	
		t += -0.02f * Time.deltaTime;
		if (t >= 1) t = 0;
		OrientedPoint p = new OrientedPoint();
		p = path.GetOrientedPoint(t);
		transform.rotation = p.rotation;
		transform.position = p.position + (p.rotation * fishOffset);
	}
}
