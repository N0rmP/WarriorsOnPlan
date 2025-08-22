using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor : MonoBehaviour, IMovableSupplement {
    /*
    private readonly static Color colorSelected = new Color(0f, 1f, 0f, 1f);
    private readonly static Color colorHovered = new Color(0.25f, 0.25f, 0.25f, 1f);
    private readonly static Color colorTransparent = new Color(0f, 0f, 0f, 0f);

    private bool isChosen;
    private bool isHovered;
    
    private SpriteRenderer SR;
    */
    private Color colorOriginal = Color.white;

    private Action delEndRun = null;

    /*
    public void Awake() {
        isChosen = false;
        isHovered = false;

        SR = GetComponent<SpriteRenderer>();
    }
    */

    public void setColorOriginal(enumSide parSide) {
        if (colorOriginal != Color.white) {
            return;
        }

        colorOriginal = SwissArmyStaticMethod.getSideColor(parSide);

        GetComponent<SpriteRenderer>().color = colorOriginal;
    }

    public void setDelEndRun(Action parDelEndRun) {
        if (delEndRun != null) {
            return;
        }

        delEndRun = parDelEndRun;
    }

    public void whenEndMove() {
        delEndRun();
    }

    public void whenStartMove() { }

    /*
    public void setIsChosen(bool par) {
        isChosen = par;
        updateSprite();
    }

    public void setIsHovered(bool par) {
        isHovered = par;
        updateSprite();
    }

    private void updateSprite() {
        // Debug.Log(this + " of " + gameObject + " : " + (SR == null) + " / " + System.Object.ReferenceEquals(SR, null));
        SR.color = isChosen ? colorSelected :
            isHovered ? colorHovered :
            colorOriginal;
    }
    */
}