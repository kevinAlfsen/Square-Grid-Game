using UnityEngine;

public class Unit : MonoBehaviour {
    
    public Cell Location {
        get {
            return location;
        }
        set {
            location = value;
            value.Unit = this;
            transform.localPosition = value.transform.localPosition;
        }
    }

    public float Oreintation {
        get {
            return orientation;
        }
        set {
            orientation = value;
            transform.localRotation = Quaternion.Euler (0f, value, 0f);
        }
    }

    Cell location;
    float orientation;

    public void ValidateLocation () {
        transform.localPosition = location.transform.localPosition;
    }
}