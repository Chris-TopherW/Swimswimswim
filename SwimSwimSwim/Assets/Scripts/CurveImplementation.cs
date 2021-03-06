﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(MeshFilter))]
public class CurveImplementation : MonoBehaviour
{

    //public GameObject[] Points = new GameObject[4];
    public List<GameObject> Points;
    public int CurveResolution = 20;
    private ExtrudeShape shape;
    public GameObject[] walls;

    private List<Vector3> lWallPoints = new List<Vector3>();
    private List<Vector3> rWallPoints = new List<Vector3>();

    void Awake()
    {
    }


    void Start()
    {
        Points = GetComponent<SplineMaker>().points;

        MakeShape();
        Generate();
    }

    void MakeShape()
    {
        shape = new ExtrudeShape();
        shape.verts = new Vector2[]
        {
            new Vector2(0,-10),
            new Vector2(0,10),
            new Vector2(0,10),
            new Vector2(0,10),
            new Vector2(1,10),
            new Vector2(1,-10),
            new Vector2(1,-10),
            new Vector2(0,-10)
        };
        shape.normals = new Vector2[]
        {
            new Vector2(1,0),
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(0,1),
            new Vector2(-1,0),
            new Vector2(-1,0),
            new Vector2(0,-1),
            new Vector2(0,-1)
        };
        shape.uCoords = new float[]
        {
            0,
            1,
            1,
            1,
            0,
            1,
            1,
            1
        };
    }

    internal float GetWidth(int segPos, float tPos)
    {
        OrientedPoint left = GetWallPos(segPos, tPos, lWallPoints);
        OrientedPoint right = GetWallPos(segPos, tPos, rWallPoints);
        return Vector3.Distance(left.position, right.position) - 1.0f - (0.68f * 2.0f);
    }


    /**
     * Method to get position based on spline segment (Points[segPos]-Points[segPos+1]) and 
     * t value.
     **/
    public OrientedPoint GetPos(int segPos, float t)
    {
        Vector3 p0;
        Vector3 p1;
        Vector3 m0;
        Vector3 m1;

        p0 = Points[segPos].transform.position;
        p1 = Points[segPos + 1].transform.position;

        if (segPos == 0)
        {
            m0 = p1 - p0;
        }
        else
        {
            m0 = 0.5f * (p1 - Points[segPos - 1].transform.position);
        }

        if (segPos < Points.Count - 2)
        {
            m1 = 0.5f * (Points[(segPos + 2) % Points.Count].transform.position - p0);
        }
        else
        {
            m1 = p1 - p0;
        }

        return CatmullRom.GetOrientedPoint(p0, p1, m0, m1, t);

    }

    public OrientedPoint GetWallPos(int segPos, float t, List<Vector3> wallPoints)
    {
        Vector3 p0;
        Vector3 p1;
        Vector3 m0;
        Vector3 m1;

        p0 = wallPoints[segPos];
        p1 = wallPoints[segPos + 1];

        if (segPos == 0)
        {
            m0 = p1 - p0;
        }
        else
        {
            m0 = 0.5f * (p1 - wallPoints[segPos - 1]);
        }

        if (segPos < Points.Count - 2)
        {
            m1 = 0.5f * (wallPoints[(segPos + 2) % wallPoints.Count] - p0);
        }
        else
        {
            m1 = p1 - p0;
        }

        return CatmullRom.GetOrientedPoint(p0, p1, m0, m1, t);

    }

    public void GenerateWallPoints()
    {
        Vector3 p0, p1, m0, m1;
        float width1, width2;
        //Debug.Log("This is a test " + Points[0]);
        lWallPoints.Clear();
        rWallPoints.Clear();

        // First for loop goes through each individual control point and connects it to the next, so 0-1, 1-2, 2-3 and so on
        for (int i = 0; i < Points.Count-1; i++)
        {
            //if (Points[i] == null || Points[i + 1] == null || (i > 0 && Points[i - 1] == null) || (i < Points.Count - 2 && Points[i + 2] == null))
            //{
            //    return;
            //}
            //Debug.Log(GameManager.segmentPos);
            p0 = Points[i].transform.position;
            p1 = Points[i + 1].transform.position;
            width1 = Points[i].GetComponent<PointControl>().width;
            width2 = Points[i + 1].GetComponent<PointControl>().width;

            // Tangent calculation for each control point
            // Tangent M[k] = (P[k+1] - P[k-1]) / 2
            // With [] indicating subscript

            // m0
            if (i == 0)
            {
                m0 = p1 - p0;
            }
            else
            {
                m0 = 0.5f * (p1 - Points[i - 1].transform.position);
            }

            // m1
            if (i < Points.Count - 2)
            {
                m1 = 0.5f * (Points[(i + 2) % Points.Count].transform.position - p0);
            }
            else
            {
                m1 = p1 - p0;
            }

            Vector3 normal = CatmullRom.CalculateNormal(m0, Vector3.up);
            Quaternion orientation = Quaternion.LookRotation(m0, normal);
            Vector3 leftOffset = orientation * new Vector3(-width1 / 2.0f, 0, 0);
            Vector3 rightOffset = orientation * new Vector3(width1 / 2.0f, 0, 0);
            if (i == 0)
            {
                lWallPoints.Add(p0 + leftOffset);
                rWallPoints.Add(p0 + rightOffset);
            }

            normal = CatmullRom.CalculateNormal(m1, Vector3.up);
            orientation = Quaternion.LookRotation(m1, normal);
            leftOffset = orientation * new Vector3(-width2 / 2.0f, 0, 0);
            rightOffset = orientation * new Vector3(width2 / 2.0f, 0, 0);

            lWallPoints.Add(p1 + leftOffset);
            rWallPoints.Add(p1 + rightOffset);
        }
    }

    public void Generate()
    {
        if (lWallPoints.Count == 0 || rWallPoints.Count == 0)
        {
            GenerateWallPoints();
        }


        int startGen = GameManager.segmentPos - 2;
        int endGen = GameManager.segmentPos + 8;
        if (startGen < 0) startGen = 0;
        if (endGen > Points.Count) endGen = Points.Count;
        if (endGen < 0) endGen = 0;

        List<Vector3> pointsforLeftMesh = lWallPoints.GetRange(startGen, endGen - startGen);
        List<Vector3> pointsforRightMesh = rWallPoints.GetRange(startGen, endGen - startGen);

        MeshFilter meshFilter = walls[0].GetComponent<MeshFilter>();
        MeshCollider meshCollider = walls[0].GetComponent<MeshCollider>();
        if (meshFilter.sharedMesh == null)
            meshFilter.sharedMesh = new Mesh();
        Mesh mesh = new Mesh();
        CatmullRom.Extrude(mesh, shape, GetWallPath(pointsforLeftMesh));
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;

        meshFilter = walls[1].GetComponent<MeshFilter>();
        meshCollider = walls[1].GetComponent<MeshCollider>();
        if (meshFilter.sharedMesh == null)
            meshFilter.sharedMesh = new Mesh();
        mesh = new Mesh();

        CatmullRom.Extrude(mesh, shape, GetWallPath(pointsforRightMesh));
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    public OrientedPoint[] GetWallPath(List<Vector3> pts)
    {
        OrientedPoint[] path = new OrientedPoint[0];
        for (int i = 0; i < pts.Count - 1; i++)
        {
            Vector3 p0, p1, m0, m1;

            p0 = pts[i];
            p1 = pts[i + 1];

            // m0
            if (i == 0)
            {
                m0 = p1 - p0;
            }
            else
            {
                m0 = 0.5f * (p1 - pts[i - 1]);
            }

            // m1
            if (i < pts.Count - 2)
            {
                m1 = 0.5f * (pts[(i + 2) % Points.Count] - p0);
            }
            else
            {
                m1 = p1 - p0;
            }
            OrientedPoint[] pathSegment = CatmullRom.GeneratePath(p0, p1, m0, m1, CurveResolution).ToArray<OrientedPoint>();
            path = path.Concat(pathSegment).ToArray<OrientedPoint>();
        }
        return path;
    }
}
