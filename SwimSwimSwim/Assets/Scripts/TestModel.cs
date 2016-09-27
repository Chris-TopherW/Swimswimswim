using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestModel : MonoBehaviour {

    public int xOffset;
    public BezierCurve bezier;
    public List<Transform> controlPointsList = new List<Transform>();

    void Awake()
    {
        Vector3[] positions = new Vector3[controlPointsList.Count];
        for (int i = 0; i < controlPointsList.Count; i++)
        {
            positions[i] = controlPointsList[i].position;
        }
        bezier = new BezierCurve(positions);
    }

    void OnDrawGizmos()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter.sharedMesh == null)
            meshFilter.sharedMesh = new Mesh();
        Mesh mesh = meshFilter.sharedMesh;

        ExtrudeShape shape = new ExtrudeShape();
        shape.verts = new Vector2[]
        {
            new Vector2(xOffset,-10),
            new Vector2(xOffset,10),
            new Vector2(xOffset,10),
            new Vector2(xOffset,-10)
        };
        shape.normals = new Vector2[]
        {
            new Vector2(1,0),
            new Vector2(1,0),
            new Vector2(-1,0),
            new Vector2(-1,0)
        };
        shape.uCoords = new float[]
        {
            0,
            1,
            0,
            1
        };
        for (int i = 0; i < controlPointsList.Count; i++)
        {
            Gizmos.DrawWireSphere(controlPointsList[i].position, 0.3f);
        }
        Vector3[] positions = new Vector3[controlPointsList.Count];
        for (int i = 0; i < controlPointsList.Count; i++)
        {
            positions[i] = controlPointsList[i].position;
        }
        bezier = new BezierCurve(positions);

        DisplayCatmullRomSpline();
        OrientedPoint[] path = bezier.GeneratePath(100).ToArray<OrientedPoint>();
        bezier.Extrude(mesh, shape, path);

    }

    void DisplayCatmullRomSpline()
    {

        //Just assign a tmp value to this
        Vector3 lastPos = Vector3.zero;

        //t is always between 0 and 1 and determines the resolution of the spline
        //0 is always at p1
        for (float t = 0; t < 1; t += 0.01f)
        {
            //Find the coordinates between the control points with a Catmull-Rom spline
            Vector3 newPos = bezier.GetPoint(t);

            //Cant display anything the first iteration
            if (t == 0)
            {
                lastPos = newPos;
                continue;
            }

            Gizmos.DrawLine(lastPos, newPos);
            lastPos = newPos;
        }

        //Also draw the last line since it is always less than 1, so we will always miss it
        Gizmos.DrawLine(lastPos, bezier.GetPoint(1));
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
