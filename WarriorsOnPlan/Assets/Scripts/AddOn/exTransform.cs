using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class exTransform {
    // FindThoroughly find child by name, it searches all children in hierachy
    public static Transform FindThoroughly(this Transform parThis, string parName) {
        Transform tempResult = null;
        foreach (Transform tr in parThis) {
            if (tr.name == parName) {
                tempResult = tr;
            } else {
                tempResult = tr.FindThoroughly(parName);
            }

            if (tempResult != null) {
                break;
            }
        }
        return tempResult;
    }
}
