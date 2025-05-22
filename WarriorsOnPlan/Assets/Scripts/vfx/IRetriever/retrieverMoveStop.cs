using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class retrieverMoveStop : retrieverAbst {
    public override bool checkRetrieve(vfxMovable parVM) {
        return parVM.stateMove == enumMoveType.stationary;
    }
}
