using UnityEngine;

public class Cell : MonoBehaviour {
    public GridCoordinates coordinates;
    public Color color;
    public RectTransform uiRect;

    public int Elevation {
        get {
            return elevation;
        }
        set {
            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * CellMetrics.elevationStep;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = elevation * -CellMetrics.elevationStep;
            uiRect.localPosition = uiPosition;
        }
    }

    int elevation;

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

    public EdgeType GetEdgeType (Direction direction) {
        return CellMetrics.GetEdgeType (elevation, GetNeighbor (direction).elevation);
    }
}