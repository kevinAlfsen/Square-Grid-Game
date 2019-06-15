using UnityEngine;
using UnityEngine.UI;

public class Chunk : MonoBehaviour {
    Cell[] cells;

    public GridMesh terrain, water;
    Canvas gridCanvas;

    void Awake () {
        gridCanvas = GetComponentInChildren<Canvas> ();

        cells = new Cell[CellMetrics.chunkSizeX * CellMetrics.chunkSizeZ];
    }

    void LateUpdate () {
        Triangulate (cells);
        enabled = false;
    }

    void Triangulate (Cell[] cells) {
        terrain.Clear ();
        water.Clear ();

        for (int i = 0; i < cells.Length; i++) {
            Triangulate (cells[i]);
        }

        terrain.Apply ();
        water.Apply ();
    }

    void Triangulate (Cell cell) {
        Vector3 center = cell.transform.localPosition;

        Vector3 v1 = center + CellMetrics.corners[0];
        Vector3 v2 = center + CellMetrics.corners[1];
        Vector3 v3 = center + CellMetrics.corners[2];
        Vector3 v4 = center + CellMetrics.corners[3];

        terrain.AddQuad (v1, v2, v3, v4);
        terrain.AddQuadColor (cell.Color);
        if (cell.isUnderWater) {

            Vector3 waterCenter = center;
            waterCenter.y = cell.WaterSurfaceY;

            v1 = waterCenter + CellMetrics.corners[0];
            v2 = waterCenter + CellMetrics.corners[1];
            v3 = waterCenter + CellMetrics.corners[2];
            v4 = waterCenter + CellMetrics.corners[3];
        
            water.AddQuad (v1, v2, v3, v4);
        }
        


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

                terrain.AddQuad (v1, v2, v3, v4);
                if (cell.isUnderWater) {
                    water.AddQuad (v1, v2, v3, v4);
                }

                if (cell.Elevation > neighbor.Elevation) {
                    terrain.AddQuadColor (cell.Color);
                } else {
                    terrain.AddQuadColor (neighbor.Color);
                }
            }
        } else if (cell.IsEdge) {
            Vector3 center = cell.transform.localPosition;

            Vector3 v1 = center + CellMetrics.GetFirstSolidCorner (direction);
            Vector3 v4 = center + CellMetrics.GetSecondSolidCorner (direction);
            Vector3 v2 = v1;
            Vector3 v3 = v4;

            v2.y = v3.y = -1 * CellMetrics.elevationStep;

            terrain.AddQuad (v1, v2, v3, v4);
            if (cell.isUnderWater) {
                water.AddQuad (v1, v2, v3, v4);
            }
            terrain.AddQuadColor (cell.Color);
        }
    }

    public void AddCell (int index, Cell cell) {
        cells[index] = cell;
        cell.chunk = this;
        cell.transform.SetParent (transform, false);
        cell.uiRect.SetParent (gridCanvas.transform, false);
    }

    public void Refresh () {
        enabled = true;
    }

    public void ToggleCanvas() {
        gridCanvas.enabled = !gridCanvas.enabled;
    }
}