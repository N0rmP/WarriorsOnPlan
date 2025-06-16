using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class exList {
    public static List<T> getListSum<T>(this List<T> parList1, List<T> parList2) {
        if (parList1 is null || parList2 is null) {
            Debug.Log("getListSum results in error due to nullness");
            return null;
        }

        List<T> tempResult = new List<T>();
        tempResult.AddRange(parList1);
        tempResult.AddRange(parList2);
        return tempResult;
    }
}
