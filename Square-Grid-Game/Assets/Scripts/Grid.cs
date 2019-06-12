using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour {
    public int width = 6;
    public int height = 6;

    Cell[] cells;
    public Cell cellPrefab;

    GridMesh mesh;

    Canvas cellLabelCanvas;
    public Text cellLabelPrefab;

    public Color defaultColor;

    void Awake () {
        mesh = GetComponentInChildren<GridMesh> ();
        cellLabelCanvas = GetComponentInChildren<Canvas> ();

        cells = new Cell[height * width];

        for (int z = 0, i = 0; z < height; z++) {
            for (int x = 0; x < width; x++) {
                CreateCell (x, z, i++);
            }
        }
    }

    void Start () {
        mesh.Triangulate (cells);
    }

    public void ColorCell (Vector3 position, Color color) {
        position = transform.InverseTransformPoint (position);
        GridCoordinates coordinates = GridCoordinates.FromPosition (position);
        int index = coordinates.X + coordinates.Z * width;
        Cell cell = cells[index];
        cell.color = color;
        mesh.Triangulate (cells);
    }

    public void ColorCellNeighbors (Vector3 position, Color color) {
        position = transform.InverseTransformPoint (position);
        GridCoordinates coordinates = GridCoordinates.FromPosition (position);
        int index = coordinates.X + coordinates.Z * width;
        Cell cell = cells[index];
        for (int i = 0; i < 8; i++) {
            Cell neighbor = cell.GetNeighbor ((Direction) i);
            if (neighbor != null) {
                neighbor.color = color;
            }
        }
        mesh.Triangulate (cells);
    }

    void CreateCell (int x, int z, int i) {
        Vector3 position;

        position.x = x * 10f;
        position.y = 0f;
        position.z = z * 10f;

        Cell cell = cells[i] = Instantiate<Cell> (cellPrefab, transform, false);
        cell.transform.SetParent (transform);
        cell.transform.localPosition = position;
        cell.coordinates = new GridCoordinates (x, z);
        cell.color = defaultColor;

        if (x > 0) {
            cell.SetNeighbor (Direction.W, cells[i - 1]);
        }
        if (z > 0) {
            cell.SetNeighbor (Direction.S, cells[i - width]);
            if (x < width - 1) {
                cell.SetNeighbor (Direction.SE, cells[i - width + 1]);
            }
            if (x > 0) {
                cell.SetNeighbor (Direction.SW, cells[i - width - 1]);
            }
        }

        Text label = Instantiate<Text> (cellLabelPrefab, cellLabelCanvas.transform, false);
        label.transform.localPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToString ();



    }
}