using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode]
public class Cell : MonoBehaviour {

    [SerializeField] internal bool isNucleus;
    [SerializeField] internal Color cellColor;

    IEnumerator Start () {
        if (isNucleus) {
            GetComponent<Renderer>().material.color = cellColor;
        }
        else {
            yield return new WaitForEndOfFrame();
            float dist = float.PositiveInfinity;
            Material material = null;
            foreach (Cell cell in transform.parent.GetComponentsInChildren<Cell>()) {
                if (!cell.isNucleus) {
                    continue;
                }
                float thisDist = Vector3.SqrMagnitude(transform.position - cell.transform.position);
                if (thisDist < dist) {
                    dist = thisDist;
                    material = cell.GetComponent<Renderer>().sharedMaterial;
                }
            }
            GetComponent<Renderer>().sharedMaterial = material;
        }
    }

    void Update () {
        print("A");
    }
}
