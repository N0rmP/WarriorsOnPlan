using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sensorNothing : sensorAbst {
    public sensorNothing(int parTimerMax = -1) : base(parTimerMax) {
        code = 0;
    }

    public override bool checkWigwagging(Thing source) {
        return false;
    }
}
