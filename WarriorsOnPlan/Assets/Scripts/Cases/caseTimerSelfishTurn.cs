using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caseTimerSelfishTurn : caseTimer {
    private bool isActionStartPassed;

    public caseTimerSelfishTurn(int parTimerMax, enumCaseType parEnumCaseType, bool parIsTimerMax = true) : base(parTimerMax, parEnumCaseType, parIsTimerMax) {
        isActionStartPassed = false;
    }

    public void updateOnActionStart(Thing source) {
        isActionStartPassed = true;
    }

    public void updateOnActionEnd(Thing source) {
        if (isActionStartPassed) {
            updateTimer();
            isActionStartPassed = false;
        }
    }
}