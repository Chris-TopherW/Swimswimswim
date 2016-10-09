using UnityEngine;
using System.Collections;

public class FriendlyFishMovement : MonoBehaviour {

    public CurveImplementation path;
    public Vector3 fishOffset;
	private float t = 0.0f;

	// Use this for initialization
	void Start () {
        path = GameObject.FindGameObjectWithTag("Spline").GetComponent<CurveImplementation>();
        t = GameManager.splinePos + 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
	
		t += -0.02f * Time.deltaTime;
		if (t >= 1) t = 0;
        if (t < 0) t = 0;
        int segPos = (int)(t * (path.Points.Count - 1));
        float tPos = t * (path.Points.Count - 1) - segPos;
        OrientedPoint p = path.GetPos(segPos, tPos);
        transform.position = p.position;
        transform.rotation = p.rotation;
    }
}
