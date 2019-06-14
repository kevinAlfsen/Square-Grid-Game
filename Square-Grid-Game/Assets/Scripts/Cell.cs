﻿using UnityEngine;

public class Cell : MonoBehaviour {
    public GridCoordinates coordinates;
    public RectTransform uiRect;
    public Chunk chunk;

    public int Elevation {
        get {
            return elevation;
        }
        set {
            if (elevation == value) {
                return;
            }
            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * CellMetrics.elevationStep;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = elevation * -CellMetrics.elevationStep;
            uiRect.localPosition = uiPosition;

            Refresh ();
        }
    }

    public Color Color {
        get {
            return color;
        }
        set {
            if (color == value) {
                return;
            }

            color = value;
            Refresh ();
        }
    }

    public bool IsEdge {
        get {
            return isEdge;
        }
        set {
            isEdge = value;
        }
    }

    Color color;
    bool isEdge = false;
    int elevation = int.MinValue;

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

    public bool hasEdgeInDirection (Direction direction) {
        if (elevation != GetNeighbor (direction).elevation) {
            return true;
        } else {
            return false;
        }
    }

    void Refresh () {
        if (chunk) {
            chunk.Refresh ();
            for (int i = 0; i < neighbors.Length; i++) {
                Cell neighbor = neighbors[i];
                if (neighbor != null && neighbor.chunk != this.chunk) {
                    neighbor.Refresh ();
                }
            }
        }
    }
}