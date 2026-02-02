using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public static class SwissArmyStaticMethod {
    public static Color getSideColor(enumSide parSide) {
        return parSide switch {
            enumSide.player => new Color(0f, 0f, 1f , 1f),
            enumSide.enemy => new Color(1f, 0f, 0f, 1f),
            enumSide.neutral => new Color(1f, 1f, 0f, 1f),
            _ => Color.white
        };
    }
}
