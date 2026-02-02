using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class exArray {
    public static IEnumerator<T> GetEnumerator<T>(this T[] parArray){
        foreach (T t in parArray) {
            yield return t;
        }
    }
}
