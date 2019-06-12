using UnityEngine;

[System.Serializable]
public struct GridCoordinates {
    public int X {
        get;
        private set;
    }

    public int Z {
        get;
        private set;
    }

    public GridCoordinates (int x, int z) {
        X = x;
        Z = z;
    }

    public static GridCoordinates FromPosition (Vector3 position) {
        float x = position.x / CellMetrics.width;
        float z = position.z / CellMetrics.height;

        int iX = Mathf.RoundToInt (x);
        int iZ = Mathf.RoundToInt (z);

        return new GridCoordinates (iX, iZ);
    }

    public override string ToString () {
        return "(" + X.ToString () + ", " + Z.ToString () + ")";
    }
}