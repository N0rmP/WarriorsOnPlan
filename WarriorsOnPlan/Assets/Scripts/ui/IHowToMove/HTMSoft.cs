using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTMSoft : IHowToMove {
    public void move(RectTransform parRect, Vector3 parDestination, float parSpeed, float parDeltaTime) {
        parRect.localPosition += /*tempStick.normalized **/ (parDestination - parRect.localPosition) * parDeltaTime * parSpeed * 5f;
    }
}
