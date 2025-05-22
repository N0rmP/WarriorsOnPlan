using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathPrivate {
    public static int Sign(float parValue) {
        return parValue switch {
            > 0 => 1,
            0 => 0,
            < 0 => -1,
            _ => 0
        };
    }
}
