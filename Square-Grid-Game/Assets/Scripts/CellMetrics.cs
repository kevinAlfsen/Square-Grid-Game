using UnityEngine;

public class CellMetrics {
    public const float height = 10f;
    public const float width = 10f;

    public const int chunkSizeX = 6;
    public const int chunkSizeZ = 6;

    public const float elevationStep = 2f;

    public const float waterElevationOffset = -0.5f;

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

    public static Vector3 GetBridge (Direction direction) {
        return (corners[(int) direction / 2] + corners[((int) direction / 2) + 1]) * 0.1f;
    }

    public const int hashGridSize = 256;

    static float[] hashGrid;

    public static void InitializeHashGrid (int seed) {
        hashGrid = new float[hashGridSize * hashGridSize];
        Random.State currentState = Random.state;
        Random.InitState (seed);
        for (int i = 0; i < hashGrid.Length; i++) {
            hashGrid[i] = Random.value;
        }
        Random.state = currentState;
    }

    public static float SampleHasGrid (Vector3 position) {
        int x = (int) position.x % hashGridSize;
        if (x < 0) {
            x += hashGridSize;
        }
        int z = (int) position.z % hashGridSize;
        if (z < 0) {
            z += hashGridSize;
        }
        return hashGrid[x + z * hashGridSize];
    }
}