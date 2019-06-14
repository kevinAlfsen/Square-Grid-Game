using UnityEngine;

public class CameraMovement : MonoBehaviour {

    Camera cam;

    public float speed = 10f;
    public float scrollSpeed = 10f;

    void Awake () {
        cam = GetComponentInChildren<Camera> ();
    }

    void Update () {
        if (Input.GetKey (KeyCode.W)) {
            transform.Translate (Vector3.forward * speed * Time.deltaTime);
        }
        if (Input.GetKey (KeyCode.S)) {
            transform.Translate (Vector3.forward * -speed * Time.deltaTime);
        }
        if (Input.GetKey (KeyCode.A)) {
            transform.Translate (Vector3.right * -speed * Time.deltaTime);
        }
        if (Input.GetKey (KeyCode.D)) {
            transform.Translate (Vector3.right * speed * Time.deltaTime);
        }

        cam.orthographicSize += Input.GetAxis ("Mouse ScrollWheel") * -scrollSpeed;
    }

    
}