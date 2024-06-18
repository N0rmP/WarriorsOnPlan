using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class caseTimerFriendlyTurn : caseTimer {

    public caseTimerFriendlyTurn(int parTimerMax, enumCaseType parEnumCaseType, bool parIsTimerMax = true) : base(parTimerMax, parEnumCaseType, parIsTimerMax) { }

    public void updateOnTurnEnd(Thing source) {
        updateTimer();
    }
}
