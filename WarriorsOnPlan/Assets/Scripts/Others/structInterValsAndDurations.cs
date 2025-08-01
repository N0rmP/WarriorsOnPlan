using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct structInterValsAndDurations{
    public const float fltInterval = 1.5f;
    public const float fltBodyAnimationDuration = fltInterval / 1.5f;
    
    

    public static float getFltProjectileDuration(Vector3 parDeparture, Vector3 parDestination) {
        // I want projectile fly across three nodes in 0.5s, 12f = three nodes' width 6 * velocity 2
        return (parDestination - parDeparture).magnitude / 12f;
    }
}
