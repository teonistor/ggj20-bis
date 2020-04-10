using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour {
    [SerializeField] private Vector3 puzzlePosition;
    [SerializeField] private AnimationCurve curve;

    internal void MoveToPuzzle(System.Action callback) {
        StartCoroutine(MoveToPuzzle(1.5f, callback));
    }

    IEnumerator MoveToPuzzle (float duration, System.Action callback) {
        Vector3 startPosition = transform.position;
        for (float t = 0f; t < duration; t += Time.deltaTime) {
            transform.position = Vector3.Lerp(startPosition, puzzlePosition, curve.Evaluate(t / duration));
            yield return new WaitForEndOfFrame();
        }
        callback();
        Destroy(this);
    }
}
