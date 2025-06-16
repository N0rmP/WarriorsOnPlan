using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumHowToMove {
    soft,
    steady
}

interface IHowToMove {
    public void move(RectTransform parRect, Vector3 parDestination, float parSpeed, float parDeltaTime);
}

public class uiMovable : MonoBehaviour {
    private static HTMSoft instHTMSoft;
    private static HTMSteady instHTMSteady;

    public enumHowToMove thisEnumHowToMove;
    private IHowToMove thisHowToMove {
        get {
            return thisEnumHowToMove switch {
                enumHowToMove.soft => instHTMSoft,
                enumHowToMove.steady => instHTMSteady,
                _ => instHTMSoft
            };
        }
    }

    protected RectTransform thisRectTransform;

    public float speed = 1f;
    protected bool isMove { get; private set; }

    private Vector3 moveDestination;

    public void Awake() {
        instHTMSoft = instHTMSoft ?? new HTMSoft();
        instHTMSteady = instHTMSteady ?? new HTMSteady();

        thisRectTransform = gameObject.GetComponent<RectTransform>();
        isMove = false;
    }

    public void Update() {
        // move
        if (isMove) {
            if ((moveDestination - thisRectTransform.localPosition).magnitude < 3f) {
                thisRectTransform.localPosition = moveDestination;
                if (this is IMovableSupplement tempIMS) {
                    tempIMS.whenEndMove();
                }
                isMove = false;
            } else {
                thisHowToMove.move(thisRectTransform, moveDestination, speed, Time.deltaTime);
            }
        }
    }

    public void setMove(Vector3 parDestination, float parSpeed = 1f) {
        moveDestination = parDestination;
        speed = parSpeed;
        isMove = true;

        if (this is IMovableSupplement tempIMS) {
            tempIMS.whenStartMove();
        }
    }
}
