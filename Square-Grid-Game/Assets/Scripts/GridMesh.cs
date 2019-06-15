using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridMesh : MonoBehaviour {

    Mesh mesh;
    MeshCollider meshCollider;
    [NonSerialized] List<Vector3> vertices;
    [NonSerialized] List<int> triangles;
    [NonSerialized] List<Color> colors;
    [NonSerialized] List<Vector2> uvs;

    public bool useCollider, useColors, useUVCoordinates;

    void Awake () {
        GetComponent<MeshFilter> ().mesh = mesh = new Mesh();
        if (useCollider) {
            meshCollider = gameObject.AddComponent<MeshCollider> ();
        }
        mesh.name = "Grid Mesh";
    }

    public void Clear () {
        mesh.Clear ();

        vertices = ListPool<Vector3>.Get ();
        if (useColors) {
            colors = ListPool<Color>.Get ();
        }
        if (useUVCoordinates) {
            uvs = ListPool<Vector2>.Get ();
        }
        triangles = ListPool<int>.Get ();
    }

    public void Apply () {
        mesh.SetVertices (vertices);
        ListPool<Vector3>.Add (vertices);
        if (useColors) {
            mesh.SetColors (colors);
            ListPool<Color>.Add (colors);
        }
        if (useUVCoordinates) {
            mesh.SetUVs (0, uvs);
            ListPool<Vector2>.Add (uvs);
        }
        mesh.SetTriangles (triangles, 0);
        ListPool<int>.Add (triangles);
        mesh.RecalculateNormals ();
        if (useCollider) {
            meshCollider.sharedMesh = mesh;
        }
    }

    public void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3) {
        int vertexIndex = vertices.Count;
        vertices.Add (v1);
        vertices.Add (v2);
        vertices.Add (v3);
        triangles.Add (vertexIndex);
        triangles.Add (vertexIndex + 1);
        triangles.Add (vertexIndex + 2);
    }

    public void AddTriangleColor (Color color) {
        colors.Add (color);
        colors.Add (color);
        colors.Add (color);
    }

    public void AddTriangleColor (Color c1, Color c2, Color c3) {
        colors.Add (c1);
        colors.Add (c2);
        colors.Add (c3);
    }

    public void AddQuad (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
        int vertexIndex = vertices.Count;
        vertices.Add (v1);
        vertices.Add (v2);
        vertices.Add (v3);
        vertices.Add (v4);
        triangles.Add (vertexIndex);
        triangles.Add (vertexIndex + 1);
        triangles.Add (vertexIndex + 2);
        triangles.Add (vertexIndex + 2);
        triangles.Add (vertexIndex + 3);
        triangles.Add (vertexIndex);
    }

    public void AddFourTriQuad (Vector3 center, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
        int vertexIndex = vertices.Count;

        vertices.Add (center);
        vertices.Add (v1);
        vertices.Add (v2);
        vertices.Add (v3);
        vertices.Add (v4);
        triangles.Add (vertexIndex);
        triangles.Add (vertexIndex + 1);
        triangles.Add (vertexIndex + 2);
        triangles.Add (vertexIndex);
        triangles.Add (vertexIndex + 2);
        triangles.Add (vertexIndex + 3);
        triangles.Add (vertexIndex);
        triangles.Add (vertexIndex + 3);
        triangles.Add (vertexIndex + 4);
        triangles.Add (vertexIndex);
        triangles.Add (vertexIndex + 4);
        triangles.Add (vertexIndex + 1);
    }

    public void AddFourTriQuadColor (Color c1, Color c2, Color c3, Color c4, Color c5) {
        colors.Add (c1);
        colors.Add (c2);
        colors.Add (c3);
        colors.Add (c4);
        colors.Add (c5);
    }

    public void AddQuadColor (Color c1, Color c2, Color c3, Color c4) {
        colors.Add (c1);
        colors.Add (c2);
        colors.Add (c3);
        colors.Add (c4);
    }

    public void AddQuadColor (Color color) {
        colors.Add (color);
        colors.Add (color);
        colors.Add (color);
        colors.Add (color);
    }

    public void AddTriangleUV (Vector2 uv1, Vector2 uv2, Vector2 uv3) {
        uvs.Add (uv1);
        uvs.Add (uv2);
        uvs.Add (uv3);
    }

    public void AddQuadUV (Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4) {
        uvs.Add (uv1);
        uvs.Add (uv2);
        uvs.Add (uv3);
        uvs.Add (uv4);
    }

    public void AddQuadUV (float uMin, float uMax, float vMin, float vMax) {
        uvs.Add (new Vector2 (uMin, vMin));
        uvs.Add (new Vector2 (uMax, vMin));
        uvs.Add (new Vector2 (uMin, vMax));
        uvs.Add (new Vector2 (uMax, vMax));
    }
}