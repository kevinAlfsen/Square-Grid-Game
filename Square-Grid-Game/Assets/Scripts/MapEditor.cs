using UnityEngine;
using UnityEngine.EventSystems;

public class MapEditor : MonoBehaviour {
    public Color[] colors;
    public Grid grid;

    public Unit unitPrefab;

    Color activeColor;
    int activeElevation;
    int activeWaterLevel;
    int activeForestLevel;
    int activeSpecialIndex;

    bool applyForestLevel = false;
    bool applyElevation = false;
    bool applyWaterLevel = false;
    bool applyColor = true;
    bool applySpecialIndex = true;

    int brushSize = 1;

    void Awake () {
        SelectColor(0);
    }

    void Update () {
        if (!EventSystem.current.IsPointerOverGameObject ()) {
            if (Input.GetMouseButton (0)) {
                HandleInput ();
                return;
            }
            if (Input.GetKeyDown (KeyCode.U)) {
                if (Input.GetKey (KeyCode.LeftShift)) {
                    DestroyUnit ();
                } else {
                    CreateUnit ();
                    return;
                }             
            }
        }
    }

    void HandleInput () {
        Cell cell = GetCellUnderCursor ();
        if (cell) {
            EditCells (cell);
        }
    }

    Cell GetCellUnderCursor () {
        Ray inputRay = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast (inputRay, out hit)) {
            return grid.GetCell (hit.point);
        } else {
            return null;
        }
    }

    void CreateUnit () {
        Cell cell = GetCellUnderCursor ();
        if (cell && !cell.Unit) {
            Unit unit = Instantiate (unitPrefab);
            unit.transform.SetParent (grid.transform, false);
            unit.Location = cell;
            unit.Oreintation = Random.Range (0f, 360f);
        }
    }

    void DestroyUnit () {
        Cell cell = GetCellUnderCursor ();
        if (cell && cell.Unit) {
            Destroy (cell.Unit.gameObject);
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
            if (applyForestLevel) {
                cell.ForestLevel = activeForestLevel;
            }
            if (applySpecialIndex) {
                cell.SpecialIndex = activeSpecialIndex;
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


    public void setApplyForestLevel (bool toggle) {
        applyForestLevel = toggle;
    }

    public void setForestLevel (float value) {
        activeForestLevel = (int) value;
    }

    public void SetApplySpecialIndex (bool toggle) {
        applySpecialIndex = toggle;
    }

    public void SetSpecialIndex (float index) {
        activeSpecialIndex = (int) index;
    }
}