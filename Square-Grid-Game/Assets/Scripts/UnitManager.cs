using UnityEngine;
using System.Collections.Generic;

public class UnityManager : MonoBehaviour {
    public List<Unit> units;

    void Awake () {
        units = new List<Unit> ();
    }


}