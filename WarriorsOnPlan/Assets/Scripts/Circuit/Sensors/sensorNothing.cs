using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sensorNothing : sensorAbst {
    public sensorNothing(int parTimerMax = -1) : base(parTimerMax) { }

    public override bool checkWigwagging(Thing source) {
        return false;
    }
}
