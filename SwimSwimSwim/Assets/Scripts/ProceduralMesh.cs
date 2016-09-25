using UnityEngine;
using System.Collections;

public class ProceduralMesh : MonoBehaviour {

    // Use this for initialization
    void OnDrawGizmos() {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter.sharedMesh == null)
            meshFilter.sharedMesh = new Mesh();
        Mesh mesh = meshFilter.sharedMesh;

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(1,0,1),
            new Vector3(0,0,1),
            new Vector3(0,0,0),
            new Vector3(1,0,0)
        };
        Vector3[] normals = new Vector3[]
        {
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up
        };
        Vector2[] uvs = new Vector2[]
        {
            new Vector2(1, 1),
            new Vector2(0, 1),
            new Vector2(0, 0),
            new Vector2(1, 0)
        };
        int[] triangles = new int[]
        {
            2,1,0,
            0,3,2
        };

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;
            
            
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
