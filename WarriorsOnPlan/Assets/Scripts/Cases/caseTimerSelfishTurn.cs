using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caseTimerSelfishTurn : caseTimer {
    private bool isActionStartPassed;

    public caseTimerSelfishTurn(int parTimerMax, enumCaseType parEnumCaseType, bool parIsTimerMax = true, bool parIsAutoReset = true) : base(parTimerMax, parEnumCaseType, parIsTimerMax, parIsAutoReset) {
        isActionStartPassed = false;
    }

    public void updateOnActionStart(Thing source) {
        isActionStartPassed = true;
    }

    public void updateOnActionEnd(Thing source) {
        if (isActionStartPassed) {
            updateTimer(source);
            isActionStartPassed = false;
        }
    }
}