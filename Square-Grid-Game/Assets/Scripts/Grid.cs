using UnityEngine;

public class Grid : MonoBehaviour {
    public int width = 6;
    public int height = 6;

    Cell[] cells;
    public Cell cellPrefab;

    GridMesh mesh;

    void Awake () {
        mesh = GetComponentInChildren<GridMesh> ();
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

    void CreateCell (int x, int z, int i) {
        Vector3 position;

        position.x = x * 10f;
        position.y = 0f;
        position.z = z * 10f;

        Cell cell = cells[i] = Instantiate<Cell> (cellPrefab, transform, false);
        cell.transform.SetParent (transform);
        cell.transform.localPosition = position;



    }
}