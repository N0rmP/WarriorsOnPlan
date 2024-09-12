using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class sensorAbst : circuitAbst<sensorAbst> {
    protected int timerCur;
    protected int timerMax;

    public sensorAbst(int parTimerMax) {
        timerMax = parTimerMax;
    } 

    public void updateTimer() {
        if (timerCur > 0) {
            timerCur--;
        }
    }

    public abstract bool checkWigwagging(Thing source);

    public virtual bool checkKeepBasic(Thing source) { 
        return true;
    }
}
