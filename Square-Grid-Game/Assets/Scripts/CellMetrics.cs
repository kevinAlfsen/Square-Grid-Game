using UnityEngine;

public class CellMetrics {
    public const float height = 10f;
    public const float width = 10f;

    public static Vector3[] corners = {
        new Vector3(-width * 0.5f, 0f, height * 0.5f),
        new Vector3(width * 0.5f, 0f, height * 0.5f),
        new Vector3(width * 0.5f, 0f, -height * 0.5f),
        new Vector3(-width * 0.5f, 0f, -height * 0.5f),
        new Vector3(-width * 0.5f, 0f, height * 0.5f)
    };



}