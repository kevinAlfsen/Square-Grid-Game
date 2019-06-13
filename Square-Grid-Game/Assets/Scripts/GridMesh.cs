using System.Collections.Generic;
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
<<<<<<< Updated upstream
        for (Direction d = Direction.N; d < Direction.NW; d+=2) {
            Triangulate (d, cell);
=======
        Vector3 center = cell.transform.localPosition;

        Vector3 v1 = center + CellMetrics.GetFirstSolidCorner (Direction.N);
        Vector3 v2 = center + CellMetrics.GetSecondSolidCorner (Direction.N);
        Vector3 v3 = center + CellMetrics.GetFirstSolidCorner (Direction.S);
        Vector3 v4 = center + CellMetrics.GetSecondSolidCorner (Direction.S);

        AddQuad (v1, v2, v3, v4);
        AddQuadColor (cell.color);

        for (Direction d = Direction.N; d <= Direction.E; d++) {
            TriangulateConnection (d, cell);
>>>>>>> Stashed changes
        }
    }

    void TriangulateConnection (Direction direction, Cell cell) {
        Vector3 center = cell.transform.localPosition;
        Vector3 v1 = center + CellMetrics.GetFirstSolidCorner (direction);
        Vector3 v4 = center + CellMetrics.GetSecondSolidCorner (direction);
        Vector3 bridge = CellMetrics.GetBridge (direction);

        if ((int) direction % 2 == 0 && cell.GetNeighbor (direction) != null) {
            Vector3 v2 = v1 + bridge;
            Vector3 v3 = v4 + bridge;

            Cell neighbor = cell.GetNeighbor (direction);

<<<<<<< Updated upstream
        AddTriangleColor (cell.color, edgeColor, edgeColor);

        /*Vector3 v1 = center + CellMetrics.corners[0];
        Vector3 v2 = center + CellMetrics.corners[1];
        Vector3 v3 = center + CellMetrics.corners[2];
        Vector3 v4 = center + CellMetrics.corners[3];

        AddQuad (v1, v2, v3, v4);
        AddQuadColor (cell.color);*/
=======
            AddQuad (v1, v2, v3, v4);
            AddQuadColor (cell.color, neighbor.color, neighbor.color, cell.color);
        } else if (cell.GetNeighbor (direction) != null) {
            Vector3 nextBridge = CellMetrics.GetBridge (direction.Next ());
            center += CellMetrics.GetSecondBlendedCorner (direction);
            v1 = v4;
            Vector3 v2 = v1 + bridge;
            Vector3 v3 = v2 + nextBridge;
            v4 = v3 - bridge;

            Cell prevNeighbor = cell.GetNeighbor (direction.Previous ());
            Cell neighbor = cell.GetNeighbor (direction);
            Cell nextNeighbor = cell.GetNeighbor (direction.Next ());

            AddFourTriQuad (center, v1, v2, v3, v4);
            AddFourTriQuadColor (
                (cell.color + prevNeighbor.color + neighbor.color + nextNeighbor.color) / 4f,
                cell.color, 
                prevNeighbor.color, 
                neighbor.color,
                nextNeighbor.color
                );
        }

        /*Vector3 center = cell.transform.localPosition;

        if ((int) direction % 2 == 0 && cell.GetNeighbor (direction) != null) {
            Vector3 bridge = CellMetrics.GetBridge (direction);
            Vector3 v1 = center + CellMetrics.GetFirstSolidCorner (direction);
            Vector3 v2 = v1 + bridge;

            Vector3 v4 = center + CellMetrics.GetSecondSolidCorner (direction);
            Vector3 v3 = v4 + bridge;

            Cell neighbor = cell.GetNeighbor (direction) ?? cell;
            Color edgeColor = (neighbor.color + cell.color) / 2f;

            AddQuad (v1, v2, v3, v4);
            AddQuadColor (cell.color, edgeColor, edgeColor, cell.color);
        } else if (cell.GetNeighbor (direction) != null){
            Vector3 bridge = CellMetrics.GetBridge (direction);

            Vector3 v1 = center + CellMetrics.GetSecondSolidCorner(direction);
            Vector3 v2 = v1 + bridge;
            Vector3 v3 = center + CellMetrics.GetSecondBlendedCorner(direction);
            Vector3 v4 = v3 - bridge;

            Debug.Log (((int) direction).ToString ());

            Cell neighbor = cell.GetNeighbor (direction);
            Cell prevNeightbor = cell.GetNeighbor (direction.Previous ());
            Cell nextNeighbor = cell.GetNeighbor (direction.Next ());

            AddQuad (v1, v2, v3, v4);
            AddQuadColor (cell.color, (cell.color + prevNeightbor.color) / 2f, (cell.color + neighbor.color + prevNeightbor.color + nextNeighbor.color) / 4f, (cell.color + nextNeighbor.color) / 2f);
        }*/
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======

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

    void AddFourTriQuad (Vector3 center, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
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

    void AddFourTriQuadColor (Color c1, Color c2, Color c3, Color c4, Color c5) {
        colors.Add (c1);
        colors.Add (c2);
        colors.Add (c3);
        colors.Add (c4);
        colors.Add (c5);
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
>>>>>>> Stashed changes
}