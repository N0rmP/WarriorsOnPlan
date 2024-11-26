using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caseTimerHostileTurn : caseTimer
{
    private bool isTurnStartPassed;

    public caseTimerHostileTurn(int parTimerMax, enumCaseType parEnumCaseType, bool parIsVisible, bool parIsTimerMax = true, bool parIsAutoReset = true) : base(parTimerMax, parEnumCaseType, parIsVisible, parIsTimerMax, parIsAutoReset) {
        isTurnStartPassed = false;
    }

    public void updateOnTurnStart(Thing source) {
        isTurnStartPassed = true;
    }

    public void updateOnTurnEnd(Thing source) {
        if (isTurnStartPassed) {
            updateTimer(source);
            isTurnStartPassed = false;
        }
    }
}
