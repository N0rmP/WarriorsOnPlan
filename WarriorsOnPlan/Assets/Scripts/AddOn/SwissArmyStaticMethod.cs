using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class SwissArmyStaticMethod {
    public static Color getSideColor(enumSide parSide) {
        return parSide switch {
            enumSide.player => new Color(0f, 0f, 1f , 1f),
            enumSide.enemy => new Color(1f, 0f, 0f, 1f),
            enumSide.neutral => new Color(1f, 1f, 0f, 1f),
            _ => Color.white
        };
    }

    public static T selectRandom<T>(IEnumerable<T> parIEnumerable) {
        int tempCount;
        switch (parIEnumerable) {
            case T[] tempArr:
                tempCount = tempArr.Length;
                break;
            case List<T> tempList:
                tempCount = tempList.Count;
                break;
            default:
                tempCount = 0;
                break;
        }

        int tempRandom = UnityEngine.Random.Range(0, tempCount - 1);
        IEnumerator<T> tempIEnumerator = parIEnumerable.GetEnumerator();
        tempIEnumerator.MoveNext();
        while (tempRandom > 0) {
            tempIEnumerator.MoveNext();
            tempRandom--;
        }
        return tempIEnumerator.Current;
    }
}
