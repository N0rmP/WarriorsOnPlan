using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumVerticalDirection { 
    up= 1,
    neutral = 0,
    down = -1,
}

public enum enumHorizontalDirection { 
    right = 1,
    neutral = 0,
    left = -1
}

public class moverUiSimple : MonoBehaviour {
    [SerializeField]
    private enumVerticalDirection thisVerticalDirection_;
    public enumVerticalDirection thisVerticalDirection {
        get {
            return thisVerticalDirection_;
        }
        set {
            thisVerticalDirection_ = value;
            prepareMovePerFrame();
        }
    }

    [SerializeField]
    private enumHorizontalDirection thisHorizontalDirection_;
    public enumHorizontalDirection thisHorizontalDirection {
        get {
            return thisHorizontalDirection_;
        }
        set {
            thisHorizontalDirection_ = value;
            prepareMovePerFrame();
        }
    }

    [SerializeField]
    private float speed_;
    public float speed {
        get {
            return speed_;
        }
        set {
            speed_ = value;
            prepareMovePerFrame();
        }
    }

    private RectTransform thisRectTransform;
    private Vector3 movePerFrame;

    public void Awake() {
        if (!TryGetComponent<RectTransform>(out thisRectTransform)) {
            Destroy(this);
        }
        
        prepareMovePerFrame();
    }

    public void Update() {
        thisRectTransform.localPosition += movePerFrame * Time.deltaTime;
    }

    private void prepareMovePerFrame() {
        movePerFrame = new Vector2((float)thisHorizontalDirection, (float)thisVerticalDirection) * speed;
    }
}
