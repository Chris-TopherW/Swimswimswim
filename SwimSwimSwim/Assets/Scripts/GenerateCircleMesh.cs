using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GenerateCircleMesh : MonoBehaviour {

    private MeshFilter filter;
    public int points;
    private int lastPoints;
	// Use this for initialization
	void Awake () {
        lastPoints = points;
        filter = GetComponent<MeshFilter>();
        if (filter != null)
        {
            Mesh circleMesh = MakeCircle(points);
            filter.mesh = circleMesh;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (points != lastPoints)
        {
            lastPoints = points;
            Mesh circleMesh = MakeCircle(points);
            filter.mesh = circleMesh;
        }
	}
    //http://answers.unity3d.com/questions/944228/creating-a-smooth-round-flat-circle.html

    public Mesh MakeCircle(int numOfPoints)
    {
        float angleStep = 360.0f / (float)numOfPoints;
        List<Vector3> vertexList = new List<Vector3>();
        List<int> triangleList = new List<int>();
        Quaternion quaternion = Quaternion.Euler(0.0f, angleStep, 0f);
        // Make first triangle.
        vertexList.Add(new Vector3(0.0f, 0f, 0.0f));  // 1. Circle center.
        vertexList.Add(new Vector3(0.0f, 0f, 0.5f));  // 2. First vertex on circle outline (radius = 0.5f)
        vertexList.Add(quaternion * vertexList[1]);     // 3. First vertex on circle outline rotated by angle)
                                                        // Add triangle indices.
        triangleList.Add(0);
        triangleList.Add(1);
        triangleList.Add(2);
        for (int i = 0; i < numOfPoints - 1; i++)
        {
            triangleList.Add(0);                      // Index of circle center.
            triangleList.Add(vertexList.Count - 1);
            triangleList.Add(vertexList.Count);
            vertexList.Add(quaternion * vertexList[vertexList.Count - 1]);
        }
        Mesh mesh = new Mesh();
        mesh.vertices = vertexList.ToArray();
        mesh.triangles = triangleList.ToArray();
        return mesh;
    }

    public Mesh MakeCircleOutline(int numOfPoints)
    {
        float angleStep = 360.0f / (float)numOfPoints;
        List<Vector3> vertexList = new List<Vector3>();
        List<int> triangleList = new List<int>();
        Quaternion quaternion = Quaternion.Euler(0, 0, angleStep);
        // Make first triangle.
        vertexList.Add(new Vector3(0.0f, 0f, 0.0f));  // 1. Circle center.
        vertexList.Add(new Vector3(0.5f, 0f, 0.0f));  // 2. First vertex on circle outline (radius = 0.5f)
        vertexList.Add(quaternion * vertexList[1]);     // 3. First vertex on circle outline rotated by angle)
                                                        // Add triangle indices.
        triangleList.Add(0);
        triangleList.Add(1);
        triangleList.Add(2);
        for (int i = 0; i < numOfPoints - 1; i++)
        {
            triangleList.Add(0);                      // Index of circle center.
            triangleList.Add(vertexList.Count - 1);
            triangleList.Add(vertexList.Count);
            vertexList.Add(quaternion * vertexList[vertexList.Count - 1]);
        }
        Mesh mesh = new Mesh();
        mesh.vertices = vertexList.ToArray();
        mesh.triangles = triangleList.ToArray();
        return mesh;
    }

}
