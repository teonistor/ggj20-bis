using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antibody : MonoBehaviour {

    [SerializeField] private AnimationCurve heightAnimation;
    [SerializeField] private AnimationCurve positionAnimation;

    private float spin;
    private Vector3 source, target;
    private float t;
    private System.Action callback;

    internal void Init (float spin, Vector3 target, System.Action callback) {
        this.spin = spin;
        this.source = transform.position;
        this.target = target;
        this.t = 0;
        this.callback = callback;
    }

    void Update () {
        transform.eulerAngles = new Vector3(90f, t * spin * 360f, 0f);
        Vector3 pos = Vector3.Lerp(source, target, positionAnimation.Evaluate(t));
        pos.y = 2* heightAnimation.Evaluate(t);
        transform.position = pos;

        // Assume the action takes a second
        t += Time.deltaTime;
        if (t > 1f) {
            callback();
            Destroy(gameObject);
        }
    }
}
