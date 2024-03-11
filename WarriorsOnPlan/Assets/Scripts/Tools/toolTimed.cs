using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class toolTimed : toolAbst, ICaseTimed
{
    protected float timerMax;
    protected float timerCur;

    public void timerUpdate(float parDeltaTime) {
        timerCur -= parDeltaTime;
        if (timerCur <= 0) {
            onAlarmed();
            timerCur = timerMax;
        }
    }

    //return true when process is finished without problem, return false when process is delayed due to stun / other skill use and etc.
    public void onAlarmed() { return; }
}
