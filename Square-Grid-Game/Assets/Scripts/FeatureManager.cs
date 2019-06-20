using UnityEngine;

public class FeatureManager : MonoBehaviour {

    public Transform featurePrefab;
    public Transform[] buildings;

    Transform container;
    Transform activeBuilding;

    public void Clear () {
        if (container) {
            Destroy (container.gameObject);
        }
        container = new GameObject ("Features Container").transform;
        container.SetParent (transform, false);
    }

    public void Apply () {

    }

    public void AddFeature (Cell cell, Vector3 position) {
        HashGrid hash = CellMetrics.SampleHashGrid (position);
        if (hash.a >= cell.ForestLevel * 0.25f) {
            return;
        }
        Transform instance = Instantiate<Transform> (featurePrefab);
        position.y += instance.localScale.y * 0.5f;
        instance.localPosition = position;
        instance.localRotation = Quaternion.Euler (0f, 360f * hash.b, 0f);
        instance.SetParent (container, false);
    }

    public void AddBuilding (Cell cell, Vector3 position) {
        Transform instance = Instantiate (buildings[cell.SpecialIndex - 1]);
        instance.localPosition = position;
        HashGrid hash = CellMetrics.SampleHashGrid (position);
        instance.localRotation = Quaternion.Euler (0f, 360f * hash.b, 0f);
        instance.SetParent (container, false);

    }
}