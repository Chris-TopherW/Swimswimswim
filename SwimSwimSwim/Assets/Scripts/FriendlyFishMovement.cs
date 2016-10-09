using UnityEngine;
using System.Collections;

public class FriendlyFishMovement : MonoBehaviour {

    public CurveImplementation path;
    public Vector3 fishOffset;
	private float t = 0.0f;
    private int seg = 0;

	// Use this for initialization
	void Start () {
        path = GameObject.FindGameObjectWithTag("Spline").GetComponent<CurveImplementation>();
        t = GameManager.splinePos + 0.2f;
        if (t > 1) t = 1;
        seg = GameManager.segmentPos + 1;
    }
	
	// Update is called once per frame
	void Update () {
	
		t += -0.02f * Time.deltaTime;
        if (t < 0)
        {
            t = 1;
            seg--;
        }
        if (seg < 0) seg = 0;
        int segPos = seg;
        float tPos = t;
        OrientedPoint p = path.GetPos(segPos, tPos);
        transform.position = p.position + (p.rotation * fishOffset);
        transform.rotation = p.rotation;
    }
}
