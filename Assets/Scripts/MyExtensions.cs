using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyExtensions {

    public static T RandomElement<T> (this IList<T> list) {
        return list[Random.Range(0, list.Count)];
    }

    public static void UpToTwoRandomElements<T>(this IList<T> list, out T a, out T b) {
        switch(list.Count) {
            case 0:
                a = default;
                b = default;
                return;
            case 1:
                a = list[0];
                b = default;
                return;
            case 2:
                bool flip = Random.Range(0, 2) == 0;
                a = flip ? list[1] : list[0];
                b = flip ? list[0] : list[1];
                return;
            default:
                int i = Random.Range(0, list.Count);
                int j = Random.Range(1, list.Count);
                if (j <= i) j--;
                a = list[i];
                b = list[j];
                return;
        }
    }

    public static void UpToTwoRandomElements<T> (this ISet<T> set, out T a, out T b) {
        switch (set.Count) {
            case 0:
                a = default;
                b = default;
                return;
            case 1:
                IEnumerator<T> next = set.GetEnumerator();
                next.MoveNext();
                a = next.Current;
                b = default;
                return;
            default:
                new List<T>(set).UpToTwoRandomElements(out a, out b);
                return;
        }
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
