using UnityEngine;

public struct HashGrid {

    public float a, b;

    public static HashGrid Create () {
        HashGrid hashGrid;
        hashGrid.a = Random.value;
        hashGrid.b = Random.value;
        return hashGrid;
    }

}