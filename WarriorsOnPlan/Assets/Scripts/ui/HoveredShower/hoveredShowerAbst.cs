using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class hoveredShowerAbst : MonoBehaviour {
    private const float fltTimerMax = 0.5f;

    // to make static GameObject-canvasShower in each derived classes and set it to objGut is recommended
    public GameObject objGut { get; protected set; }

    private RectTransform thisRectTransform;

    private bool isEntered;
    private float fltTimerCur;

    #region callbacks
    public void Awake() {
        thisRectTransform = GetComponent<RectTransform>();
        isEntered = false;
        fltTimerCur = fltTimerMax;
        init();
    }

    public void Start(){        
        objGut = makeGut();
    }

    public void Update(){
        // that fltTimer is above 0 means hovering is just started if , that fltTimer is below -1 means window is shown and nothing to do now
        checkEntered();
        if (isEntered) {
            if (fltTimerCur > 0.0f) {
                fltTimerCur -= Time.deltaTime;
            } else if (fltTimerCur > -1.0f) {
                show();
                fltTimerCur -= 2.0f;
            }
        }
    }
    #endregion callbacks

    protected void checkEntered() {
        if (!isEntered) {
            if (thisRectTransform.checkHovered()) {
                isEntered = true;
                fltTimerCur = fltTimerMax;
            }
        } else if (!thisRectTransform.checkHovered()) {
            isEntered = false;
            deshow();
        }
    }

    protected void gutMoveToMouse() {
        RectTransform tempRect = objGut.GetComponent<RectTransform>();
        tempRect.anchorMin = new Vector2(0f, 0f);
        tempRect.anchorMax = new Vector2(0f, 0f);
        tempRect.anchoredPosition = Input.mousePosition;
    }

    // change pivot of parObj when mouse position is near the edges of screen, it assures that popped GUI to be fully inside the screen
    // if space is enough, this method will change pivot to place parObj on right / bottom side of mouse
    protected void gutInterpolatePivot() {
        RectTransform tempRect = objGut.GetComponent<RectTransform>();
        tempRect.pivot = new Vector2(
            (Input.mousePosition.x + tempRect.rect.width > gameManager.GM.option.screenWidth) ? 1f : 0f,
            (Input.mousePosition.y - tempRect.rect.height > 0f) ? 1f : 0f
            );
    }

    protected void show() {
        return; //★ 이거부터 고치고 circuitSetter 좀 어떻게 해봐
        if (!doBeforeShow()) {
            return;
        }        

        objGut.GetComponent<RectTransform>().localPosition = new Vector3(9999f, 9999f);
        objGut.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(objGut.GetComponent<RectTransform>());

        gutInterpolatePivot();
        gutMoveToMouse();
    }

    protected void deshow() {
        objGut.SetActive(false);

        doAfterDeshow();
    }

    protected virtual void init() { }
    protected abstract GameObject makeGut();
    protected virtual bool doBeforeShow() { return true; }
    protected virtual void doAfterDeshow() { }
}
