using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sensorHp : sensorAbst
{
    private int threshold;
    private bool isMoreThan;

    public sensorHp(int parTimerMax, int parThreshold, int parSelecter) : base(parTimerMax) {
        threshold = parThreshold;
        isMoreThan = (parSelecter == 0);
    }

    public override bool checkWigwagging(Thing source) {
        return (
            isMoreThan ? 
                source.curHp >= threshold :
                source.curHp <= threshold
            );
    }
}
