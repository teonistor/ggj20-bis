using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateOnce : MonoBehaviour {

    private const int edge = 10;

    void Start () {
        GameObject go = transform.GetChild(0).gameObject;
        for (int z = 0; z < edge; z++) {
            for (int x = 0; x < edge; x++) {
                Instantiate(go, new Vector3(x, 0, z), Quaternion.identity, transform);
            }
        }
    }

    void Update () {

    }
}
