using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AIBehaviour : MonoBehaviour
{
    public bool drawMesh = true;
    public int vertexCount = 6;
    public float distance = 10;
    public float radius = 3;
    public Color meshColor;

    private Mesh _mesh;
    private List<Vector3> points = new List<Vector3>();

    private void OnValidate()
    {
        points.Clear();
        _mesh = CreateConeMesh();
    }

    Mesh CreateConeMesh()
    {
        Mesh mesh = new Mesh();

        int numTriangles = vertexCount * 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        int requiredAngle = 360 / vertexCount;
        int angle = 0;
        
        // Base Point
        Vector3 origin = Vector3.zero;
        
        // Circle Center Point
        Vector3 center = Vector3.forward * distance;
        
        // Circle Points
        for (int i = 0; i < vertexCount; i++)
        {
            var point = center + Quaternion.Euler(0, 0, angle - requiredAngle * i) * Vector3.up * radius;
            
            points.Add(point);
        }

        int vert = 0;

        // Create Circle Shape
        for (int i = 0; i < points.Count; i++)
        {
            if (i == points.Count - 1)
            {
                vertices[vert++] = points[i];
                vertices[vert++] = center;
                vertices[vert++] = points[Index.Start];
                break;
            }
            
            vertices[vert++] = points[i];
            vertices[vert++] = center;
            vertices[vert++] = points[i + 1];
        }

        // Create Cone Shape
        for (int i = points.Count - 1; i >= 0; i--)
        {
            if (i == 0)
            {
                vertices[vert++] = points[i];
                vertices[vert++] = origin;
                vertices[vert++] = points[points.Count - 1];
                break;
            }
            
            vertices[vert++] = points[i];
            vertices[vert++] = origin;
            vertices[vert++] = points[i - 1];
        }

        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnDrawGizmos()
    {
        if (_mesh && drawMesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(_mesh, transform.position, transform.rotation);
        }
    }
}
