using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

개편, 가능하면 원하는 캔버스를 받아서 출력할 수 있도록
public abstract class uiHoveredShowerAbst : MonoBehaviour       //, IPointerEnterHandler, IPointerExitHandler
{
    private const float fltTimerMax = 0.5f;

    RectTransform thisRectTransform;

    private bool isHovered;
    private float fltTimerCur;

    #region callbacks
    public void Awake(){
        thisRectTransform = GetComponent<RectTransform>();
        isHovered = false;
        fltTimerCur = fltTimerMax;
        init();
    }

    public void Update(){
         if (thisRectTransform.checkHovered()) {
        //if (gameManager.GM.UC.checkMouseOnIt(gameObject)) {
            // if mouse enters on this GUI
            if (!isHovered) {
                isHovered = true;
                fltTimerCur = fltTimerMax;
            }
        } else {
            // if mouse exits from this GUI
            if (isHovered) {
                isHovered = false;
                fltTimerCur = fltTimerMax;
                deshow();
            }
        }

        if (isHovered) {
            if (fltTimerCur > 0.0f) {
                fltTimerCur -= Time.deltaTime;
            } else if (fltTimerCur > -1.0f) {
                show();
                fltTimerCur -= 2.0f;
            } else {
                methWhileHovered();
            }
        }
    }
    #endregion callbacks

    protected void methWhileHovered() {
        if (!thisRectTransform.checkHovered()) {
            fltTimerCur = fltTimerMax;
            deshow();
            isHovered = false;
        }
    }

    protected void moveToMouse(GameObject parObj) {
        parObj.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition - new Vector3(optionAIO.screenWidth, 0, 0);
    }

    // change pivot of parObj when mouse position is near the edges of screen, and assures that popped GUI to be fully inside the screen
    // interpolation is done independently on vertically / horizontally
    // if space is enough, this method will change pivot to place parObj on right / bottom side of mouse
    protected void interpolatePivot(GameObject parObj) {
        // this method only works with RectTransform, return immediately if parObj doesn't have it
        RectTransform tempRect;
        if (!parObj.TryGetComponent<RectTransform>(out tempRect)) {
            return;
        }

        tempRect.pivot = new Vector2(
            (Input.mousePosition.x + tempRect.rect.width > optionAIO.screenWidth) ? 1f : 0f,
            (Input.mousePosition.y - tempRect.rect.height > 0f) ? 1f : 0f
            );
    }

    protected abstract void init();
    // ★ secondary-implementation : each element (status, tool, skill etc.) has their own imformation panel prefab, and each uiHoveredShower.show just shows them
    protected abstract void show();
    protected abstract void deshow();

    
}
