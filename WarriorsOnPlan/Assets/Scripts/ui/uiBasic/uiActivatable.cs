using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum enumUiActivatableState {
    activating = 0,
    active = 1,
    deactivating = 2,
    inactive = 3    
}

// uiActivatable should be with RectTransform & exRectTransform
public class uiActivatable : uiMovable {
    private Coroutine coroutineDeactivate = null;

    public enumUiActivatableState thisEnumUiActivatableState { get; protected set; }   
    public bool isOutClickDeactivate = true;
    private Vector3 originalLocalPosition;

    public void Start() {
        thisEnumUiActivatableState = enumUiActivatableState.inactive;
        originalLocalPosition = thisRectTransform.localPosition;
    }

    public new void Update() {
        base.Update();

        // if mouse left clicked out of this uiActivatable, deactivate this uiActivatable when needed
        if (isOutClickDeactivate &&
            Input.GetMouseButtonDown(0) &&         
            !GetComponent<RectTransform>().checkHovered() &&
            thisEnumUiActivatableState == enumUiActivatableState.active) {
            deactivatePanel();
        }

        // change state, update stackUI
        if (!isMove && thisEnumUiActivatableState is enumUiActivatableState.activating or enumUiActivatableState.deactivating) {
            thisEnumUiActivatableState++;
        }
    }

    #region activate
    public void activatePanel(Vector3 parDestination) {
        // if gameObject was being deactivated, cancel it
        if (coroutineDeactivate != null) {
            StopCoroutine(coroutineDeactivate);
            coroutineDeactivate = null;
        }

        gameObject.SetActive(true);
        if (isOutClickDeactivate) {
            gameManager.GM.UC.pushUiActivatable(this);
        }

        if (this is IUIActivate tempIUIActivate) {
            tempIUIActivate.doWhenUIActovate();
        }

        thisEnumUiActivatableState = enumUiActivatableState.activating;
        setMove(parDestination);
    }

    // recommend to call this method with inactive object
    // save gameObject's position temporarily, move it out of screen, and move it again to the saved position (make it active with dynamic emerging)
    public void activatePanel() {
        thisRectTransform.localPosition += new Vector3(3000f, 0f, 0f);
        activatePanel(originalLocalPosition);
    }
    #endregion activate

    #region deactivate
    public void deactivatePanel(Vector3 parDestination) {
        if (isOutClickDeactivate) {
            gameManager.GM.UC.popUiActivatable();
        }

        if (this is IUIDeactivate tempIUIDeactivate) {
            tempIUIDeactivate.doWhenUIDeactivate();
        }

        thisEnumUiActivatableState = enumUiActivatableState.deactivating;
        setMove(parDestination);
        // coroutineDeactivate = StartCoroutine(delayedInactive(thisRectTransform.localPosition));
    }

    // make gameObject fly away out of right side of screen
    public void deactivatePanel() {
        deactivatePanel(new Vector3(3000f, thisRectTransform.localPosition.y, thisRectTransform.localPosition.z));
    }

    private IEnumerator delayedInactive(Vector3 parDestination) {
        yield return new WaitForSeconds(3f);
        thisRectTransform.localPosition = parDestination;
        gameObject.SetActive(false);
    }
    #endregion deactivate
}
