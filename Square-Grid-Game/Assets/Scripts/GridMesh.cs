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
        Vector3 center = cell.transform.localPosition;

        Vector3 v1 = center + CellMetrics.corners[0];
        Vector3 v2 = center + CellMetrics.corners[1];
        Vector3 v3 = center + CellMetrics.corners[2];
        Vector3 v4 = center + CellMetrics.corners[3];

        AddQuad (v1, v2, v3, v4);
        AddQuadColor (cell.Color);


        TriangulateConnection (cell, Direction.S);
        TriangulateConnection (cell, Direction.E);
    }

    void TriangulateConnection (Cell cell, Direction direction) {
        Cell neighbor = cell.GetNeighbor (direction);

        if (neighbor != null) {
            if (cell.hasEdgeInDirection (direction)) {
                Vector3 center = cell.transform.localPosition;

                Vector3 v1 = center + CellMetrics.GetFirstSolidCorner (direction);
                Vector3 v4 = center + CellMetrics.GetSecondSolidCorner (direction);
                Vector3 v2 = v1;
                Vector3 v3 = v4;

                v2.y = v3.y = cell.GetNeighbor (direction).Elevation * CellMetrics.elevationStep;

                AddQuad (v1, v2, v3, v4);

                if (cell.Elevation > neighbor.Elevation) {
                    AddQuadColor (cell.Color);
                } else {
                    AddQuadColor (neighbor.Color);
                }
            }
        } else if (cell.IsEdge) {
            Vector3 center = cell.transform.localPosition;

            Vector3 v1 = center + CellMetrics.GetFirstSolidCorner (direction);
            Vector3 v4 = center + CellMetrics.GetSecondSolidCorner (direction);
            Vector3 v2 = v1;
            Vector3 v3 = v4;

            v2.y = v3.y = - 1 * CellMetrics.elevationStep;

            AddQuad (v1, v2, v3, v4);
            AddQuadColor (cell.Color);
        }




    }

    /*void Triangulate (Cell cell) {
        Vector3 center = cell.transform.localPosition;

        Vector3 v1 = center + CellMetrics.GetFirstSolidCorner (Direction.N);
        Vector3 v2 = center + CellMetrics.GetSecondSolidCorner (Direction.N);
        Vector3 v3 = center + CellMetrics.GetFirstSolidCorner (Direction.S);
        Vector3 v4 = center + CellMetrics.GetSecondSolidCorner (Direction.S);

        AddQuad (v1, v2, v3, v4);
        AddQuadColor (cell.color);

        for (Direction d = Direction.N; d <= Direction.E; d++) {
            TriangulateConnection (d, cell);
        }
    }

    void TriangulateConnection (Direction direction, Cell cell) {
        Vector3 center = cell.transform.localPosition;
        Vector3 v1 = center + CellMetrics.GetFirstSolidCorner (direction);
        Vector3 v4 = center + CellMetrics.GetSecondSolidCorner (direction);
        Vector3 bridge = CellMetrics.GetBridge (direction);
        Cell neighbor = cell.GetNeighbor (direction);

        if ((int) direction % 2 == 0 && cell.GetNeighbor (direction) != null) {
            Vector3 v2 = v1 + bridge;
            Vector3 v3 = v4 + bridge;
            v2.y = v3.y = neighbor.Elevation * CellMetrics.elevationStep;

            

            if (cell.GetEdgeType (direction) == EdgeType.Slope) {
                TriangulateEdgeTerraces (cell, neighbor, v1, v2, v3, v4);
            } else {
                AddQuad (v1, v2, v3, v4);
                AddQuadColor (cell.color, neighbor.color, neighbor.color, cell.color);
            }
        } else if (cell.GetNeighbor (direction) != null) {
            Cell prevNeighbor = cell.GetNeighbor (direction.Previous ());
            Cell nextNeighbor = cell.GetNeighbor (direction.Next ());

            Vector3 nextBridge = CellMetrics.GetBridge (direction.Next ());
            center += CellMetrics.GetSecondBlendedCorner (direction);
            v1 = v4;
            Vector3 v2 = v1 + bridge;
            Vector3 v3 = v2 + nextBridge;
            v4 = v3 - bridge;

            v2.y = prevNeighbor.Elevation * CellMetrics.elevationStep;
            v3.y = neighbor.Elevation * CellMetrics.elevationStep;
            v4.y = nextNeighbor.Elevation * CellMetrics.elevationStep;

            center.y = (cell.Elevation + neighbor.Elevation) * CellMetrics.elevationStep * 0.5f;

            AddFourTriQuad (center, v1, v2, v3, v4);
            AddFourTriQuadColor (
                (cell.color + prevNeighbor.color + neighbor.color + nextNeighbor.color) / 4f,
                cell.color, 
                prevNeighbor.color, 
                neighbor.color,
                nextNeighbor.color
                );
        }
    }

    void TriangulateEdgeTerraces (Cell beginCell, Cell endCell, Vector3 beginLeft, Vector3 endLeft, Vector3 endRight, Vector3 beginRight) {
        Vector3 v2 = CellMetrics.TerraceLerp (beginLeft, endLeft, 1);
        Vector3 v3 = CellMetrics.TerraceLerp (beginRight, endRight, 1);
        Color c2 = CellMetrics.TerraceLerp (beginCell.color, endCell.color, 1);

        AddQuad (beginLeft, v2, v3, beginRight);
        AddQuadColor (beginCell.color, c2, c2, beginCell.color);

        for (int i = 2; i < CellMetrics.terraceSteps; i++) {
            Vector3 v1 = v2;
            Vector3 v4 = v3;
            Color c1 = c2;
            v2 = CellMetrics.TerraceLerp (beginLeft, endLeft, i);
            v3 = CellMetrics.TerraceLerp (beginRight, endRight, i);
            c2 = CellMetrics.TerraceLerp (beginCell.color, endCell.color, i);
            AddQuad (v1, v2, v3, v4);
            AddQuadColor (c1, c2, c2, c1);
        }

        AddQuad (v2, endLeft, endRight, v3);
        AddQuadColor (c2, endCell.color, endCell.color, c2);
    }*/

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
}