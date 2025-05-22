using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor : movableObject, IMovableSupplement {
    private readonly static Color distinct = new Color(0f, 1f, 0f, 1f);
    private readonly static Color half = new Color(1f, 1f, 0f, 0.6f);
    private readonly static Color transparent = new Color(0f, 0f, 0f, 0f);

    private bool isChosen;
    private bool isHovered;

    private SpriteRenderer SR;

    private Action delEndRun = null;

    public void Awake() {
        isChosen = false;
        isHovered = false;

        SR = GetComponent<SpriteRenderer>();
        SR.color = transparent;
    }

    public void setDelEndRun(Action parDelEndRun) {
        if (delEndRun != null) {
            return;
        }

        delEndRun = parDelEndRun;
    }

    public void setIsChosen(bool par) {
        isChosen = par;
        updateSR();
    }

    public void setIsHovered(bool par) {
        isHovered = par;
        updateSR();
    }

    public void whenEndMove() {
        delEndRun();
    }

    public void whenStartMove() { }

    private void updateSR() {
        SR.color = isChosen ? distinct :
            isHovered ? half :
            transparent;
    }
}
