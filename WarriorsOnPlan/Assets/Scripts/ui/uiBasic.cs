using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

interface IHowToMove {
    public void move(RectTransform parRect, Vector3 parDestination, float parSpeed, float parDeltaTime);
}

public enum enumHTM { 
    soft,
    steady
}

// uiBasic should be with RectTransform & exRectTransform
public class uiBasic : MonoBehaviour {
    public static Stack<uiBasic> stackUI = new Stack<uiBasic>();

    private static HTMSoft instHTMSoft;
    private static HTMSteady instHTMSteady;

    public bool isRightClickDeactivate = true;
    public enumHTM thisEnumHTM = enumHTM.soft;

    public float speed = 1f;

    private bool isMove = false;
    private Vector3 moveDestination_;
    public Vector3 moveDestination {
        get {
            return moveDestination_;
        }
        set {
            moveDestination_ = value;
            if (delDoWhenMoveStart != null) {
                delDoWhenMoveStart_();
            }
            isMove = true;
        }
    }

    private Coroutine coroutineDeactivate = null;

    private Action delDoWhenMoveStart_ = null;
    private Action delDoWhenMoveEnd_ = null;
    public Action delDoWhenMoveStart {
        private get {
            return delDoWhenMoveStart_;
        }
        set {
            if (delDoWhenMoveStart_ != null) {
                delDoWhenMoveStart_ = value;
            }
        }
    }
    public Action delDoWhenMoveEnd {
        private get {
            return delDoWhenMoveStart_;
        }
        set {
            if (delDoWhenMoveEnd_ != null) {
                delDoWhenMoveEnd_ = value;
            }
        }
    }

    private IHowToMove thisHTM;

    private RectTransform thisRectTransform;
    private Vector3 originalLocalPosition;

    public void Awake() {
        instHTMSoft = instHTMSoft ?? new HTMSoft();
        instHTMSteady = instHTMSteady ?? new HTMSteady();

        setHTM(thisEnumHTM);

        thisRectTransform = gameObject.GetComponent<RectTransform>();
        originalLocalPosition = thisRectTransform.localPosition;
    }

    public void Update() {
        // if mouse left click out of this uiBasic, deactivate this uiBasic when needed
        if (isRightClickDeactivate && stackUI.Count > 0 && stackUI.Peek() == this && Input.GetMouseButtonDown(0) && !GetComponent<RectTransform>().checkHovered()) {
            deactivatePanel();
        }

        // move
        if (isMove) {
            if ((moveDestination - thisRectTransform.localPosition).magnitude < 5f) {
                thisRectTransform.localPosition = moveDestination;
                if (delDoWhenMoveEnd != null) {
                    delDoWhenMoveEnd();
                }
                isMove = false;
            } else {
                thisHTM.move(thisRectTransform, moveDestination, speed, Time.deltaTime);
            }
        }
    }

    public void activatePanel(Vector3 parDestination) {
        // if gameObject was being deactivated, cancel it
        if (coroutineDeactivate != null) {
            StopCoroutine(coroutineDeactivate);
            coroutineDeactivate = null;
        }

        gameObject.SetActive(true);
        if (isRightClickDeactivate) {
            stackUI.Push(this);
        }
        moveDestination = parDestination;
    }

    // recommend to call this method with inactive object
    // save gameObject's position temporarily, move it out of screen, and move it again to the saved position (make it active with dynamic emerging)
    public void activatePanel() {
        thisRectTransform.localPosition += new Vector3(3000f, 0f, 0f);
        activatePanel(originalLocalPosition);
    }

    public void deactivatePanel(Vector3 parDestination) {
        // deactivatePanel doesn't work while another deactivatePanel was working
        if (coroutineDeactivate != null) {
            return;
        }

        if (isRightClickDeactivate) {
            stackUI.Pop();
        }
        moveDestination = parDestination;
        coroutineDeactivate = StartCoroutine(delayedInactive(thisRectTransform.localPosition));
    }

    // make gameObject fly away out of right side of screen
    public void deactivatePanel() {        
        deactivatePanel(thisRectTransform.localPosition + new Vector3(3000f, 0f, 0f));
    }

    private IEnumerator delayedInactive(Vector3 parDestination) {
        yield return new WaitForSeconds(3f);
        thisRectTransform.localPosition = parDestination;
        gameObject.SetActive(false);
    }

    public void setHTM(enumHTM parEnumHTM) {
        thisHTM = parEnumHTM switch {
            enumHTM.soft => instHTMSoft,
            enumHTM.steady => instHTMSteady,
            _ => instHTMSoft
        };
    }

    public void setMove(Vector3 parDestination, float parSpeed = 1f) {
        moveDestination = parDestination;
        speed = parSpeed;
    }
}
