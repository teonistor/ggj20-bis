using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyExtensions {

    public static T RandomElement<T> (this IList<T> list) {
        return list[Random.Range(0, list.Count)];
    }

    public static V RandomValue<K, V> (this IDictionary<K, V> dict) {
        IEnumerator<V> iter = dict.Values.GetEnumerator();
        for (int n = Random.Range(0, dict.Count); n >= 0; n--) {
            iter.MoveNext();
        }
        return iter.Current;
    }

    public static K RandomKey<K,V> (this IDictionary<K,V> dict) {
        IEnumerator<K> iter = dict.Keys.GetEnumerator();
        for (int n = Random.Range(0, dict.Count); n >= 0; n--) {
            iter.MoveNext();
        }
        return iter.Current;
    }
}
