using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour {
    public int width = 6;
    public int height = 6;

    public Node nodePrefab;
    public Text labelPrefab;

    BoxCollider collider;
    Canvas gridCanvas;

    Node[] nodes;
    Text[] labels;

    void Awake () {
        nodes = new Node[width * height];
        labels = new Text[width * height];
        collider = GetComponent<BoxCollider> ();
        gridCanvas = GetComponentInChildren<Canvas> ();

        CreateNodes ();

        PlaceCollider ();
    }

    void CreateNodes () {
        for (int z = 0, i = 0; z < height; z++) {
            for (int x = 0; x < width; x++) {
                CreateNode (x, z, i++);
            }
        }
    }

    void CreateNode (int x, int z, int index) {
        Vector3 position;

        position.x = x * 10f;
        position.y = 0;
        position.z = z * 10f;

        Node node = nodes[index] = Instantiate<Node> (nodePrefab);
        node.transform.SetParent (transform, false);
        node.transform.localPosition = position;

        Text label = labels[index] = Instantiate<Text> (labelPrefab);
        label.rectTransform.localPosition = new Vector2 (position.x, position.z);
        label.rectTransform.SetParent (gridCanvas.transform, false);
        label.text = x.ToString () + ", " + z.ToString ();
    }

    void PlaceCollider () {
        Vector3 center;
        center.y = -0.5f;
        center.x = (width * 10f / 2f) - (10f / 2f);
        center.z = (height * 10f / 2f) - (10f / 2f);

        collider.center = center;

        Vector3 size = new Vector3 (width * 10f, 1f, height * 10f);

        collider.size = size;
    }

    public Text GetTextFromPosition (Vector3 position) {
        int posX = Mathf.RoundToInt (position.x / 10f);
        int posZ = Mathf.RoundToInt (position.z / 10f);

        int index = posX + posZ * width;

        return labels[index];
    }
}