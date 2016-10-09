using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode()]
public class CatmullRom
{
    public GameObject[] Points = new GameObject[4];
    public int CurveResolution = 20;
    public Vector3[] CurveCoordinates;
    public Vector3[] Tangents;
    public bool ClosedLoop = false;

    public enum Uniformity
    {
        Uniform,
        Centripetal,
        Chordal
    }

    public static Vector3 CalculateNormal(Vector3 tangent, Vector3 up)
    {
        Vector3 binormal = Vector3.Cross(up, tangent);
        return Vector3.Cross(tangent, binormal);
    }

    public static Vector3 Interpolate(Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t)
    {
        // Catmull-Rom splines are Hermite curves with special tangent values.
        // Hermite curve formula:
        // (2t^3 - 3t^2 + 1) * p0 + (t^3 - 2t^2 + t) * m0 + (-2t^3 + 3t^2) * p1 + (t^3 - t^2) * m1
        // For points p0 and p1 passing through points m0 and m1 interpolated over t = [0, 1]
        // Tangent M[k] = (P[k+1] - P[k-1]) / 2
        // With [] indicating subscript
        Vector3 position = (2.0f * t * t * t - 3.0f * t * t + 1.0f) * start
            + (t * t * t - 2.0f * t * t + t) * tanPoint1
            + (-2.0f * t * t * t + 3.0f * t * t) * end
            + (t * t * t - t * t) * tanPoint2;

        return position;
    }

    public static Vector3 Interpolate(Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t, out Vector3 tangent, out Vector3 normal, out Quaternion orientation)
    {
        // Calculate tangents
        // p'(t) = (6t² - 6t)p0 + (3t² - 4t + 1)m0 + (-6t² + 6t)p1 + (3t² - 2t)m1
        tangent = (6 * t * t - 6 * t) * start
            + (3 * t * t - 4 * t + 1) * tanPoint1
            + (-6 * t * t + 6 * t) * end
            + (3 * t * t - 2 * t) * tanPoint2;
        normal = CalculateNormal(tangent, Vector3.up);
        orientation = Quaternion.LookRotation(tangent, normal);
        return Interpolate(start, end, tanPoint1, tanPoint2, t);
    }
   

    public static OrientedPoint GetOrientedPoint(Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t)
    {
        Vector3 tangent, normal;
        Quaternion orientation;

        Vector3 point = Interpolate(start, end, tanPoint1, tanPoint2, t, out tangent, out normal, out orientation);
        return new OrientedPoint(point, orientation, 0);
    }

    public static IEnumerable<OrientedPoint> GeneratePath(Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float subDivisions)
    {
        float step = 1.0f / subDivisions;

        for (float f = 0; f < 1; f += step)
        {
            OrientedPoint p = GetOrientedPoint(start, end, tanPoint1, tanPoint2, f);
            yield return p;
        }

        //yield return GetOrientedPoint(start, end, tanPoint1, tanPoint2, 1);
    }


    public static void Extrude(Mesh mesh, ExtrudeShape shape, OrientedPoint[] path)
    {
        int vertsInShape = shape.verts.Length;
        int segments = path.Length - 1;
        int edgeLoops = path.Length;
        int vertCount = vertsInShape * edgeLoops;
        int triCount = shape.Lines.Length * segments * 2;
        int triIndexCount = triCount * 3;

        int[] triangleIndices = new int[triIndexCount];
        Vector3[] vertices = new Vector3[vertCount];
        Vector3[] normals = new Vector3[vertCount];
        Vector2[] uvs = new Vector2[vertCount];

        // Generate all of the vertices and normals
        for (int i = 0; i < path.Length; i++)
        {
            int offset = i * vertsInShape;
            for (int j = 0; j < vertsInShape; j++)
            {
                int id = offset + j;
                Vector3 vert = path[i].LocalToWorld(shape.verts[j]);
                if (shape.verts[j].y == -10)
                {
                   vert.y = -2000;
                }
                vertices[id] = vert;
                normals[id] = path[i].LocalToWorldDirection(shape.normals[j]);
                uvs[id] = new Vector2(shape.uCoords[j], path[i].vCoordinate);
            }
        }

        // Generate all of the triangles
        int ti = 0;
        for (int i = 0; i < segments; i++)
        {
            int offset = i * vertsInShape;
            for (int l = 0; l < shape.Lines.Length; l += 2)
            {
                int a = offset + shape.Lines[l];
                int b = offset + shape.Lines[l] + vertsInShape;
                int c = offset + shape.Lines[l + 1] + vertsInShape;
                int d = offset + shape.Lines[l + 1];
                triangleIndices[ti++] = a;
                triangleIndices[ti++] = b;
                triangleIndices[ti++] = c;
                triangleIndices[ti++] = c;
                triangleIndices[ti++] = d;
                triangleIndices[ti++] = a;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangleIndices;
    }
}

public class ExtrudeShape
{
    public Vector2[] verts;
    public Vector2[] normals;
    public float[] uCoords;

    IEnumerable<int> LineSegment(int i)
    {
        yield return i;
        yield return i + 1;
    }

    int[] lines;
    public int[] Lines
    {
        get
        {
            if (lines == null)
            {
                lines = Enumerable.Range(0, verts.Length - 1)
                    .SelectMany(i => LineSegment(i))
                    .ToArray();
            }

            return lines;
        }
    }
};

public struct OrientedPoint
{
    public Vector3 position;
    public Quaternion rotation;
    public float vCoordinate;

    public OrientedPoint(Vector3 position, Quaternion rotation, float vCoordinate = 0)
    {
        this.position = position;
        this.rotation = rotation;
        this.vCoordinate = vCoordinate;
    }

    public Vector3 LocalToWorld(Vector3 point)
    {
        return position + rotation * point;
    }

    public Vector3 WorldToLocal(Vector3 point)
    {
        return Quaternion.Inverse(rotation) * (point - position);
    }

    public Vector3 LocalToWorldDirection(Vector3 dir)
    {
        return rotation * dir;
    }
}