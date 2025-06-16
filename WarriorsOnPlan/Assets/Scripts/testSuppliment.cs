using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSuppliment : MonoBehaviour {
    public void Start() {
        Vector2 tempVector;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponent<RectTransform>(),
            test.publitizedWorldPos,
            null,
            out tempVector
            );
        GetComponent<RectTransform>().localPosition = tempVector;
    }
}
