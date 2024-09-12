using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class uiBasic : MonoBehaviour
{
    public static Stack<uiBasic> stackUI = new Stack<uiBasic>();

    Coroutine coroutineDeactivate = null;

    private bool isMove = false;
    private Vector3 moveDestination_;
    public Vector3 moveDestination {
        get {
            return moveDestination_;
        }
        set {
            moveDestination_ = value;
            isMove = true;
        }
    }

    private RectTransform thisRectTransform;
    private Vector3 originalLocalPosition;

    public void Awake() {
        thisRectTransform = gameObject.GetComponent<RectTransform>();
        originalLocalPosition = thisRectTransform.localPosition;
        gameObject.SetActive(false);
    }

    public void Update() {
        // mouse left click input
        if (stackUI.Count > 0 && stackUI.Peek() == this && Input.GetMouseButtonDown(0) && !checkMouseOnThis()) {
            deactivatePanel();
        }

        // move
        if (isMove) {
            move(Time.deltaTime);
        }
    }

    private bool checkMouseOnThis() {
        Vector3 tempMousePosition = Input.mousePosition - new Vector3(optionAIO.screenWidth / 2, optionAIO.screenHeight / 2, 0);
        Vector2 tempRectMin = thisRectTransform.rect.min;
        Vector2 tempRectMax = thisRectTransform.rect.max;

        /*
        Debug.Log("obj : " + gameObject + 
            "\nmouse pos : " + tempMousePosition + 
            "\nthis rect : " + tempRectMin + " , " + tempRectMax
            );
        */
        

        return (
            (tempMousePosition.x >= tempRectMin.x) &&
            (tempMousePosition.x <= tempRectMax.x) &&
            (tempMousePosition.y >= tempRectMin.y) &&
            (tempMousePosition.y <= tempRectMax.y)
            );
    }

    public void activatePanel(Vector3 parDestination) {
        // if gameObject was being deactivated, cancel it
        if (coroutineDeactivate != null) {
            StopCoroutine(coroutineDeactivate);
            coroutineDeactivate = null;
        }

        gameObject.SetActive(true);
        stackUI.Push(this);        
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

        stackUI.Pop();
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

    private void move(float parDeltaTime) {
        Vector3 tempStick = moveDestination - thisRectTransform.localPosition;

        if (tempStick.magnitude < 5f) {
            thisRectTransform.localPosition = moveDestination;
            isMove = false;
        } else {
            thisRectTransform.localPosition += tempStick.normalized * parDeltaTime * tempStick.magnitude * 5f;
        }
    }
}
