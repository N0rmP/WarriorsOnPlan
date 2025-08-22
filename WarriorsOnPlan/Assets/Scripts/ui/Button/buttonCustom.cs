using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
// using UnityEngine.UIElements;

// ★ 필요해지면 holding은 따로 빼낼 것, 얘 생각보다 기능 많이 잡아먹는다 분리해야 됨
public enum enumButtonTiming { 
    pointerUp       = 0,
    pointerDown     = 1,
    pointerClick    = 2,
    // holding = 2
}

public class buttonCustom : Button, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {
    [SerializeField]
    private Image[] thisImage;
    public enumButtonTiming thisEnumButtonTiming = enumButtonTiming.pointerUp;

    [SerializeField]
    private ButtonClickedEvent eventRightClick = new ButtonClickedEvent();

    protected override void DoStateTransition(SelectionState state, bool instant) {
        Color tempColor;
        switch (state){
            case SelectionState.Disabled:
                tempColor = colors.disabledColor;
                break;
            case SelectionState.Highlighted:
                tempColor = colors.highlightedColor;
                break;
            case SelectionState.Normal:
                tempColor = colors.normalColor;
                break;            
            case SelectionState.Pressed:
                tempColor = colors.pressedColor;
                break;
            case SelectionState.Selected:
                tempColor = colors.selectedColor;
                break;            
            default:
                tempColor = Color.white;
                break;
        }

        foreach (Image img in thisImage) {
            img.CrossFadeColor(tempColor, instant ? 0f : colors.fadeDuration, true, true);
        }
    }

    #region pointer
    public new void OnPointerDown(PointerEventData eventData) {
        base.OnPointerDown(eventData);

        if (IsInteractable() && thisEnumButtonTiming == enumButtonTiming.pointerDown) {
            switch (eventData.button){
                case PointerEventData.InputButton.Left:
                    onClick.Invoke();
                    break;
                case PointerEventData.InputButton.Right:
                    eventRightClick.Invoke();
                    break;
            }
        }
    }    

    public new void OnPointerUp(PointerEventData eventData) {
        base.OnPointerUp(eventData);

        if (IsInteractable() && (thisEnumButtonTiming == enumButtonTiming.pointerUp)) {
            switch (eventData.button) {
                case PointerEventData.InputButton.Left:
                    onClick.Invoke();
                    break;
                case PointerEventData.InputButton.Right:
                    eventRightClick.Invoke();
                    break;
            }
        }
    }

    // this OnPointerClick blocks parent work ignoring PointerUp or Pointer Down
    public new void OnPointerClick(PointerEventData eventData) {
        if (IsInteractable() && thisEnumButtonTiming == enumButtonTiming.pointerClick) {
            // checking left mouse button is skipped to be done in parent
            base.OnPointerClick(eventData);
            if(eventData.button == PointerEventData.InputButton.Right)
                eventRightClick.Invoke();
            }
    }
    #endregion pointer
}
