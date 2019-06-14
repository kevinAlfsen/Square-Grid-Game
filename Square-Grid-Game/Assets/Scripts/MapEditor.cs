using UnityEngine;
using UnityEngine.EventSystems;

public class MapEditor : MonoBehaviour {
    public Color[] colors;
    public Grid grid;

    Color activeColor;
    int activeElevation;

    void Awake () {
        SelectColor(0);
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
                EditCell(grid.GetCell(hit.point));  
        }
    }

    void EditCell (Cell cell) {
        cell.Color = activeColor;
        cell.Elevation = activeElevation;
    }

    public void SelectColor (int index) {
        activeColor = colors[index];
    }

    public void SetElevation (float elevation) {
        activeElevation = (int) elevation;
    }
}