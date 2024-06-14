using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caseTimer : caseBase
{
    public int timerMax { get; private set; }
    public int timerCur { get; private set; }

    public caseTimer(int parTimerMax, enumCaseType parEnumCaseType, bool parIsTimerMax = false) : base(parEnumCaseType) {
        timerMax = parTimerMax;
        timerCur = parIsTimerMax ? timerMax : 0;
    }

    public void updateTimer() {
        if (timerCur > 0) {
            timerCur--;
        }
    }

    public void resetTimer() {
        timerCur = timerMax;
    }
}
