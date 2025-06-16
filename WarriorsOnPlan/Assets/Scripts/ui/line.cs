using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class line : MonoBehaviour {
    public void arrange(Vector2 parVertex1, Vector2 parVertex2) {
        RectTransform tempRectTransform = GetComponent<RectTransform>();
        tempRectTransform.localPosition = parVertex1;
        tempRectTransform.rotation = Quaternion.Euler(0f, 0f, Vector2.Angle(parVertex1, parVertex2));
    }
}
