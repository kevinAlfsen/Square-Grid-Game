using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridMesh : MonoBehaviour {

    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;

    void Awake () {
        GetComponent<MeshFilter> ().mesh = mesh = new Mesh();
        mesh.name = "Grid Mesh";

        vertices = new List<Vector3> ();
        triangles = new List<int> ();
    }

    public void Triangulate (Cell[] cells) {
        mesh.Clear ();
        triangles.Clear ();
        vertices.Clear ();

        for (int i = 0; i < cells.Length; i++) {
            Triangulate (cells[i]);
        }

        mesh.vertices = vertices.ToArray ();
        mesh.triangles = triangles.ToArray ();
        mesh.RecalculateNormals ();
    }

    void Triangulate (Cell cell) {
        Vector3 center = cell.transform.position;

        Debug.Log ("Triangulating....");

        Vector3 v1 = center + CellMetrics.corners[0];
        Vector3 v2 = center + CellMetrics.corners[1];
        Vector3 v3 = center + CellMetrics.corners[2];
        Vector3 v4 = center + CellMetrics.corners[3];

        AddQuad (v1, v2, v3, v4);
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

    void AddQuad (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
        AddTriangle (v1, v2, v3);
        AddTriangle (v3, v4, v1);
    }
}