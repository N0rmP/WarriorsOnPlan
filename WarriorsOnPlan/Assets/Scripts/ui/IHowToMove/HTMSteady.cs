using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTMSteady : IHowToMove {
    public void move(RectTransform parRect, Vector3 parDestination, float parSpeed, float parDeltaTime) {
        parRect.localPosition += (parDestination - parRect.localPosition).normalized * parDeltaTime * parSpeed * 4f;
    }
}
