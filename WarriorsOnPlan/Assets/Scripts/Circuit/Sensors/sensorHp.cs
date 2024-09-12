using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sensorHpBelow : sensorAbst
{
    private int threshold;

    public sensorHpBelow(int parTimerMax, int parThreshold) : base(parTimerMax) {
        code = 1;
        threshold = parThreshold;
    }

    public override bool checkWigwagging(Thing source) {
        return source.curHp <= threshold;
    }
}
