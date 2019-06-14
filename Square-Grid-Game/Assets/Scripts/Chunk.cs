using UnityEngine;
using UnityEngine.UI;

public class Chunk : MonoBehaviour {
    Cell[] cells;

    GridMesh gridMesh;
    Canvas gridCanvas;

    void Awake () {
        gridCanvas = GetComponentInChildren<Canvas> ();
        gridMesh = GetComponentInChildren<GridMesh> ();

        cells = new Cell[CellMetrics.chunkSizeX * CellMetrics.chunkSizeZ];
    }

    void Start () {
        Refresh ();
    }

    void LateUpdate () {
        /*gridMesh.Triangulate (cells);
        enabled = false;*/
    }

    public void AddCell (int index, Cell cell) {
        cells[index] = cell;
        cell.chunk = this;
        cell.transform.SetParent (transform, false);
        cell.uiRect.SetParent (gridCanvas.transform, false);
    }

    public void Refresh () {
        /*if (!enabled) {
            enabled = true;
        }*/

        gridMesh.Triangulate (cells);

    }

    public void ToggleCanvas() {
        gridCanvas.enabled = !gridCanvas.enabled;
    }
}