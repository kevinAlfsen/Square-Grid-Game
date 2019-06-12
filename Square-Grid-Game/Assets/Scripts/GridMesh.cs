﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridMesh : MonoBehaviour {

    Mesh mesh;
    MeshCollider meshCollider;
    List<Vector3> vertices;
    List<int> triangles;
    List<Color> colors;

    void Awake () {
        GetComponent<MeshFilter> ().mesh = mesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider> ();
        mesh.name = "Grid Mesh";

        vertices = new List<Vector3> ();
        triangles = new List<int> ();
        colors = new List<Color> ();
    }

    public void Triangulate (Cell[] cells) {
        mesh.Clear ();
        triangles.Clear ();
        vertices.Clear ();
        colors.Clear ();

        for (int i = 0; i < cells.Length; i++) {
            Triangulate (cells[i]);
        }

        mesh.vertices = vertices.ToArray ();
        mesh.triangles = triangles.ToArray ();
        mesh.colors = colors.ToArray ();
        mesh.RecalculateNormals ();
        meshCollider.sharedMesh = mesh;
    }

    void Triangulate (Cell cell) {
        Vector3 center = cell.transform.localPosition;

        Vector3 v1 = center + CellMetrics.GetFirstSolidCorner (Direction.N);
        Vector3 v2 = center + CellMetrics.GetSecondSolidCorner (Direction.N);
        Vector3 v3 = center + CellMetrics.GetFirstSolidCorner (Direction.S);
        Vector3 v4 = center + CellMetrics.GetSecondSolidCorner (Direction.S);

        AddQuad (v1, v2, v3, v4);
        AddQuadColor (cell.color);

        for (Direction d = Direction.N; d < Direction.NW; d+=2) {
            TriangulateConnection (d, cell);
        }

        //TriangulateConnection (Direction.N, cell);
    }

    void Triangulate (Direction direction, Cell cell) {
        Vector3 center = cell.transform.position;

        Vector3 v1 = center + CellMetrics.GetFirstSolidCorner (direction);
        Vector3 v2 = center + CellMetrics.GetSecondSolidCorner (direction);

        AddTriangle (center, v1, v2);

        Cell neighbor = cell.GetNeighbor (direction) ?? cell;
        Color edgeColor = (cell.color + neighbor.color) * 0.5f;

        AddTriangleColor (cell.color, edgeColor, edgeColor);
    }

    void TriangulateConnection (Direction direction, Cell cell) {
        if ((int)direction <= 6) {
            Vector3 center = cell.transform.localPosition;

            Vector3 v1 = center + CellMetrics.GetFirstSolidCorner (direction);
            Vector3 v2 = center + CellMetrics.GetFirstBlendedCorner (direction);
            Vector3 v3 = center + CellMetrics.GetSecondBlendedCorner (direction);
            Vector3 v4 = center + CellMetrics.GetSecondSolidCorner (direction);

            Cell neighbor = cell.GetNeighbor (direction) ?? cell;
            Color edgeColor = (cell.color + neighbor.color) * 0.5f;

            AddQuad (v1, v2, v3, v4);
            AddQuadColor (cell.color, edgeColor, edgeColor, cell.color);
        }
    }

    void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3) {
        int vertexIndex = vertices.Count;
        vertices.Add (v1);
        vertices.Add (v2);
        vertices.Add (v3);
        triangles.Add (vertexIndex);
        triangles.Add (vertexIndex + 1);
        triangles.Add (vertexIndex + 2);
    }

    void AddTriangleColor (Color color) {
        colors.Add (color);
        colors.Add (color);
        colors.Add (color);
    }

    void AddTriangleColor (Color c1, Color c2, Color c3) {
        colors.Add (c1);
        colors.Add (c2);
        colors.Add (c3);
    }

    void AddQuad (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
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

    void AddQuadColor (Color c1, Color c2, Color c3, Color c4) {
        colors.Add (c1);
        colors.Add (c2);
        colors.Add (c3);
        colors.Add (c4);
    }

    void AddQuadColor (Color color) {
        colors.Add (color);
        colors.Add (color);
        colors.Add (color);
        colors.Add (color);
    }
}