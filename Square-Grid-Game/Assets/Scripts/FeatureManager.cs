using UnityEngine;

public class FeatureManager : MonoBehaviour {

    public Transform featurePrefab;

    Transform container;

    public void Clear () {
        if (container) {
            Destroy (container.gameObject);
        }
        container = new GameObject ("Features Container").transform;
        container.SetParent (transform, false);
    }

    public void Apply () {

    }

    public void AddFeature (Vector3 position) {
        Transform instance = Instantiate<Transform> (featurePrefab);
        position.y += instance.localScale.y * 0.5f;
        instance.localPosition = position;
        instance.localRotation = Quaternion.Euler (0f, 360f * Random.value, 0f);
        instance.SetParent (container, false);
    }
}