using UnityEngine;
using UnityEngine.EventSystems;

public class MapEditor : MonoBehaviour {
    public Color[] colors;
    public Grid grid;

    Color activeColor;
    int activeElevation;
    int activeWaterLevel;

    bool applyElevation = false;
    bool applyWaterLevel = false;
    bool applyColor = true;

    int brushSize = 1;

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
                EditCells(grid.GetCell(hit.point));  
        }
    }

    void EditCell (Cell cell) {
        if (cell) {
            if (applyColor) {
                cell.Color = activeColor;
            }
            if (applyElevation) {
                cell.Elevation = activeElevation;
            }
            if (applyWaterLevel) {
                cell.WaterLevel = activeWaterLevel;
            }
        }
    }

    void EditCells (Cell center) {
        int minZ = (center.coordinates.Z - brushSize) > 0 ? center.coordinates.Z - brushSize : 0;
        int minX = (center.coordinates.X - brushSize) > 0 ? center.coordinates.X - brushSize : 0;
        int maxZ = (center.coordinates.Z + brushSize) < grid.cellCountZ ? center.coordinates.Z + brushSize : grid.cellCountZ - 1;
        int maxX = (center.coordinates.X + brushSize) < grid.cellCountX ? center.coordinates.X + brushSize : grid.cellCountX - 1;

        for (int z = minZ; z <= maxZ; z++) {
            for (int x = minX; x <= maxX; x++) {
                EditCell (grid.GetCell (new GridCoordinates (x, z)));
            }
        }
    }

    public void SelectColor (int index) {
        activeColor = colors[index];
    }

    public void SetElevation (float elevation) {
        activeElevation = (int) elevation;
    }

    public void SetApplyElevation (bool toggle) {
        applyElevation = toggle;
    }

    public void SetWaterLevel (float waterLevel) {
        activeWaterLevel = (int) waterLevel;
    }

    public void SetApplyWaterLevel (bool toggle) {
        applyWaterLevel = toggle;
    }

    public void setBrushSize (float size) {
        brushSize = (int) size;
    }

    public void setApplyColor (bool toggle) {
        applyColor = toggle;
    }

}