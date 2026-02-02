using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public enum enumVerticalDirection { 
    up= 1,
    neutral = 0,
    down = -1,
    random = 99
}

public enum enumHorizontalDirection { 
    right = 1,
    neutral = 0,
    left = -1,
    random = 99
}

public enum enumUiSimpleMoveType { 
    constant = 0,
    duration = 1,
    destination = 2
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

    public float durationMax = 1f;
    private float durationCur;
    public enumUiSimpleMoveType thisEnumUiSimpleMoveType;

    private RectTransform thisRectTransform;
    private Vector3 movePerFrame;

    public void Awake() {
        if (!TryGetComponent<RectTransform>(out thisRectTransform)) {
            Destroy(this);
        }

        if (thisEnumUiSimpleMoveType == enumUiSimpleMoveType.duration) {
            durationCur = durationMax;
        }
        prepareMovePerFrame();
    }

    public void Update() {
        switch (thisEnumUiSimpleMoveType) {
            case (enumUiSimpleMoveType.constant):
                thisRectTransform.localPosition += movePerFrame * Time.deltaTime;
                break;
            case (enumUiSimpleMoveType.duration):
                if (durationCur > 0f) {
                    thisRectTransform.localPosition += movePerFrame * Time.deltaTime;
                    durationCur -= Time.deltaTime;
                }
                break;
            case (enumUiSimpleMoveType.destination):
                여기 구현
                break;
        }
        이후 커스텀 에디터 작성해서 thisEnumUiSimpleType에 따라 인스펙터 에디터 변경
        이후 popupFloating 재설정하고 데미지 폰트 조금만 움직인 뒤, 좀 더 오래 보여주고 사라지도록 변경
    }

    public void OnEnable() {
        durationCur = durationMax;
    }

    private void prepareMovePerFrame() {
        destination 방식인 경우 speed 자체 계산


        movePerFrame = new Vector2(
            thisHorizontalDirection == enumHorizontalDirection.random ? Random.Range(-1, 1) : (float)thisHorizontalDirection,
            thisVerticalDirection == enumVerticalDirection.random ? Random.Range(-1, 1) : (float)thisVerticalDirection
        ) * speed;
    }
}
*/