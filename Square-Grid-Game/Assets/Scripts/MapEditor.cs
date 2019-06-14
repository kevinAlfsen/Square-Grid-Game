﻿using UnityEngine;
using UnityEngine.EventSystems;

public class MapEditor : MonoBehaviour {
    public Color[] colors;
    public Grid grid;

    Color activeColor;
    Color neighborColor;
    int activeElevation;

    bool showNeighbors = false;

    void Awake () {
        SelectColor(0);
        neighborColor = Color.red;
    }

    void Update () {
        if (Input.GetMouseButton (0) && !EventSystem.current.IsPointerOverGameObject()) {
            HandleInput ();
        }
    }

    void HandleInput () {
        Ray inputRay = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast (inputRay, out hit)) {
            if (showNeighbors) {
                grid.ColorCellNeighbors (hit.point, neighborColor);
            } else {
                EditCell(grid.GetCell(hit.point));
            }
            
        }
    }

    void EditCell (Cell cell) {
        cell.color = activeColor;
        cell.Elevation = activeElevation;
        grid.Refresh ();
    }

    public void SelectColor (int index) {
        activeColor = colors[index];
        //neighborColor = index < 3 ? colors[index + 1] : colors[0];
    }

    public void ToggleShowNeighbors () {
        showNeighbors = !showNeighbors;
    }

    public void SetElevation (float elevation) {
        activeElevation = (int) elevation;
    }
}