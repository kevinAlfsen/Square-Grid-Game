using UnityEngine;

public class Cell : MonoBehaviour {
    public GridCoordinates coordinates;
    public Color color;

    [SerializeField]
    Cell[] neighbors;

    void Awake () {
        neighbors = new Cell[8];
    }

    public Cell GetNeighbor (Direction direction) {
        return neighbors[(int) direction];
    }

    public void SetNeighbor (Direction direction, Cell cell) {
        neighbors[(int) direction] = cell;
        cell.neighbors[(int) direction.Opposite ()] = this;
    }
}