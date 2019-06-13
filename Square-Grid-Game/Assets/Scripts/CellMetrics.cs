using UnityEngine;

public class CellMetrics {
    public const float height = 10f;
    public const float width = 10f;

    public const float solidFactor = 0.90f;
    public const float blendFactor = 1f - solidFactor;

    public static Vector3[] corners = {
        new Vector3(-width * 0.5f, 0f, height * 0.5f),
        new Vector3(width * 0.5f, 0f, height * 0.5f),
        new Vector3(width * 0.5f, 0f, -height * 0.5f),
        new Vector3(-width * 0.5f, 0f, -height * 0.5f),
        new Vector3(-width * 0.5f, 0f, height * 0.5f)
    };

    public static Vector3 GetFirstSolidCorner (Direction direction) {
        return corners[(int) direction / 2];
    }

    public static Vector3 GetSecondSolidCorner (Direction direction) {
        return corners[((int) direction / 2) + 1];
    }

    public static Vector3 GetFirsBlendedCorner (Direction direction) {
        return corners[(int) direction / 2] * solidFactor;
    }

    public static Vector3 GetSecondBlendedCorner (Direction direction) {
        return corners[((int) direction / 2) + 1] * solidFactor;
    }

    public static Vector3 GetBridge (Direction direction) {
        return (corners[(int) direction / 2] + corners[((int) direction / 2) + 1]) * blendFactor;
    }

}