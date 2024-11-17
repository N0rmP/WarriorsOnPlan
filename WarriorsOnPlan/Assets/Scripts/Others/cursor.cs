using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor : MonoBehaviour
{
    private static Color distinct = new Color(0f, 1f, 0f, 1f);
    private static Color half = new Color(1f, 1f, 0f, 0.6f);
    private static Color transparent = new Color(0f, 0f, 0f, 0f);

    private bool isChosen;
    private bool isHovered;

    private SpriteRenderer SR;

    void Awake() {
        isChosen = false;
        isHovered = false;

        SR = GetComponent<SpriteRenderer>();
        SR.color = transparent;
    }

    public void setIsChosen(bool par) {
        isChosen = par;
        updateSR();
    }

    public void setIsHovered(bool par) {
        isHovered = par;
        updateSR();
    }

    private void updateSR() {
        SR.color = isChosen ? distinct :
            isHovered ? half :
            transparent;
    }
}
