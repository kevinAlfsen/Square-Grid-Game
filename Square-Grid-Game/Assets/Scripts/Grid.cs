using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour {
    public int chunkCountX = 4;
    public int chunkCountZ = 3;

    public int cellCountX, cellCountZ;

    public Chunk chunkPrefab;

    Chunk[] chunks;

    Cell[] cells;
    public Cell cellPrefab;

    public Text cellLabelPrefab;

    public Color defaultColor;

    void Awake () {
        cellCountX = chunkCountX * CellMetrics.chunkSizeX;
        cellCountZ = chunkCountZ * CellMetrics.chunkSizeZ;

        CreateChunks ();
        CreateCells ();
    }

    void CreateChunks () {
        chunks = new Chunk[chunkCountX * chunkCountZ];

        for (int z = 0, i = 0; z < chunkCountZ; z++) {
            for (int x = 0; x < chunkCountX; x++) {
                Chunk chunk = chunks[i++] = Instantiate<Chunk> (chunkPrefab);
                chunk.transform.SetParent (transform);
            }
        }
    }

    void CreateCells () {
        cells = new Cell[cellCountZ * cellCountX];

        for (int z = 0, i = 0; z < cellCountZ; z++) {
            for (int x = 0; x < cellCountX; x++) {
                CreateCell (x, z, i++);
            }
        }
    }

    public Cell GetCell (Vector3 position) {
        position = transform.InverseTransformPoint (position);
        GridCoordinates coordinates = GridCoordinates.FromPosition (position);
        int index = coordinates.X + coordinates.Z * cellCountX;
        return cells[index];
    }

    public Cell GetCell (GridCoordinates coordinates) {
        int index = coordinates.X + coordinates.Z * cellCountX;
        return cells[index];
    }

    void CreateCell (int x, int z, int i) {
        Vector3 position;

        position.x = x * 10f;
        position.y = 0f;
        position.z = z * 10f;

        Cell cell = cells[i] = Instantiate<Cell> (cellPrefab);
        cell.transform.localPosition = position;
        cell.coordinates = new GridCoordinates (x, z);
        cell.Color = defaultColor;

        if (x > 0) {
            cell.SetNeighbor (Direction.W, cells[i - 1]);
        }
        if (z > 0) {
            cell.SetNeighbor (Direction.S, cells[i - cellCountX]);
            if (x < cellCountX - 1) {
                cell.SetNeighbor (Direction.SE, cells[i - cellCountX + 1]);
            }
            if (x > 0) {
                cell.SetNeighbor (Direction.SW, cells[i - cellCountX - 1]);
            }
        }

        Text label = Instantiate<Text> (cellLabelPrefab);
        label.transform.localPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToString ();

        cell.uiRect = label.rectTransform;
        cell.Elevation = 0;

        if (z == 0 || x == cellCountX - 1) {
            cell.IsEdge = true;
        } 

        AddCellToChunk (x, z, cell);
    }

    void AddCellToChunk (int x, int z, Cell cell) {
        int chunkX = x / CellMetrics.chunkSizeX;
        int chunkZ = z / CellMetrics.chunkSizeZ;

        Chunk chunk = chunks[chunkX + chunkZ * chunkCountX];

        int localX = x - chunkX * CellMetrics.chunkSizeX;
        int localZ = z - chunkZ * CellMetrics.chunkSizeZ;

        chunk.AddCell (localX + localZ * CellMetrics.chunkSizeX, cell);
    }

    public void ToggleLabels () {
        for (int i = 0; i < chunks.Length; i++) {
            chunks[i].ToggleCanvas ();
        }
    }
}