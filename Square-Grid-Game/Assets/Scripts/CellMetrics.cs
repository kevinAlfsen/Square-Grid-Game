using UnityEngine;

public class CellMetrics {
    public const float height = 10f;
    public const float width = 10f;
    public const float elevationStep = 5f;
    public const int terracesPerSlope = 2;
    public const int terraceSteps = terracesPerSlope * 2 + 1;
    public const float horizontalTerraceStepSize = 1f / terraceSteps;
    public const float verticalTerraceStepSize = 1f / (terracesPerSlope + 1);

    public const float solidFactor = 0.75f;
    public const float blendFactor = 1f - solidFactor;

    public static Vector3[] corners = {
        new Vector3(-width * 0.5f, 0f, height * 0.5f),
        new Vector3(width * 0.5f, 0f, height * 0.5f),
        new Vector3(width * 0.5f, 0f, -height * 0.5f),
        new Vector3(-width * 0.5f, 0f, -height * 0.5f),
        new Vector3(-width * 0.5f, 0f, height * 0.5f)
    };

    public static Vector3 GetFirstSolidCorner (Direction direction) {
        return corners[(int) direction / 2] * solidFactor;
    }

    public static Vector3 GetSecondSolidCorner (Direction direction) {
        return corners[((int) direction / 2) + 1] * solidFactor;
    }

    public static Vector3 GetFirstBlendedCorner (Direction direction) {
        return corners[(int) direction / 2];
    }

    public static Vector3 GetSecondBlendedCorner (Direction direction) {
        return corners[((int) direction / 2) + 1];
    }

    public static Vector3 GetBridge (Direction direction) {
        return (corners[(int) direction / 2] + corners[((int) direction / 2) + 1]) * blendFactor;
    }

    public static Vector3 TerraceLerp (Vector3 a, Vector3 b, int step) {
        float h = step * CellMetrics.horizontalTerraceStepSize;
        a.x += (b.x - a.x) * h;
        a.z += (b.z - a.z) * h;

        float v = ((step + 1) / 2) * CellMetrics.verticalTerraceStepSize;
        a.y += (b.y - a.y) * v; 
        return a;
    }

    public static Color TerraceLerp (Color a, Color b, int step) {
        float h = step * CellMetrics.horizontalTerraceStepSize;
        return Color.Lerp (a, b, h);
    }

    public static EdgeType GetEdgeType (int elevation1, int elevation2) {
        if (elevation1 == elevation2) {
            return EdgeType.Flat;
        }

        int delta = elevation2 - elevation1;
        if (delta == 1 || delta == -1) {
            return EdgeType.Slope;
        }

        return EdgeType.Cliff;
    }
}