using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caseTimerHostilTurn : caseTimer
{
    private bool isTurnStartPassed;

    public caseTimerHostilTurn(int parTimerMax, enumCaseType parEnumCaseType, bool parIsTimerMax = true) : base(parTimerMax, parEnumCaseType, parIsTimerMax) {
        isTurnStartPassed = false;
    }

    public void updateOnTurnStart(Thing source) {
        isTurnStartPassed = true;
    }

    public void updateOnTurnEnd(Thing source) {
        if (isTurnStartPassed) {
            updateTimer();
            isTurnStartPassed = false;
        }
    }
}
