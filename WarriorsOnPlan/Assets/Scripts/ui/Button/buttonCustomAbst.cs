using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// ★ 여유있으면 holding은 따로 빼낼 것, 얘 생각보다 기능 많이 잡아먹는다 분리해야 됨
public enum enumButtonTiming { 
    pointerUp = 0,
    pointerDown = 1,
    holding = 2
}

public abstract class buttonCustomAbst : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
    private Color colorOriginal;
    [SerializeField]
    private Color colorWhenHovered = new Color(0.8f, 0.8f, 0.8f, 1f);
    [SerializeField]
    private Color colorWhenDisabled = new Color(1f, 1f, 1f, 0.7f);
    [SerializeField]
    private Color colorWhenHolding = new Color(0.5f, 0.5f, 0.5f, 1f);

    private Image thisImage;
    public enumButtonTiming thisEnumButtonTiming = enumButtonTiming.pointerUp;

    [SerializeField]
    private float timerMax_ = 0f;
    public float timerMax {
        get {
            return timerMax_;
        }
        private set {
            timerMax_ = value;
        }
    }
    private float timerCur;

    private bool isTriggerWithHolding;
    [SerializeField]
    private bool isInteractable_;
    public bool isInteractable{
        get {
            return isInteractable_;
        }
        set {
            isInteractable_ = value;
            if (thisImage is not null) {
                if (isInteractable_) {
                    thisImage.color = colorOriginal;
                } else {
                    colorOriginal = thisImage.color;
                    thisImage.color *= colorWhenDisabled;
                }
            }
        }
    }

    public void Awake() {
        if (!TryGetComponent<Image>(out thisImage)) {
            Debug.Log(this + " has no Image component");
        }

        timerCur = -1f;
        isTriggerWithHolding = false;
        isInteractable = isInteractable_;
    }

    public void Update() {
        if (timerCur > 0f) {
            timerCur -= Time.deltaTime;
        } else if (isTriggerWithHolding) {
            isTriggerWithHolding = false;
            doWhenTriggered();
        }

        if (thisEnumButtonTiming == enumButtonTiming.holding) {
            if (timerCur > 0f) {
                thisImage.color = colorWhenHolding;
            } else {
                thisImage.color = colorOriginal;
            }
        }
    }

    public abstract void actualDoWhenTriggered();

    public void doWhenTriggered() {
        if (isInteractable) {
            actualDoWhenTriggered();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!isInteractable) {
            return;
        }

        colorOriginal = thisImage.color;
        thisImage.color *= colorWhenHovered;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!isInteractable) {
            return;
        }

        thisImage.color = colorOriginal;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!isInteractable) {
            return;
        }

        switch(thisEnumButtonTiming){
            case enumButtonTiming.pointerDown:
                doWhenTriggered();
                break;
            case enumButtonTiming.holding:
                timerCur = timerMax;
                isTriggerWithHolding = true;
                break;
        }
    }    

    public void OnPointerUp(PointerEventData eventData) {
        if (!isInteractable) {
            return;
        }

        switch (thisEnumButtonTiming) {
            case enumButtonTiming.pointerUp:
                doWhenTriggered();
                break;
            case enumButtonTiming.holding:
                timerCur = -1f;
                isTriggerWithHolding = false;
                break;
        }
    }
}
