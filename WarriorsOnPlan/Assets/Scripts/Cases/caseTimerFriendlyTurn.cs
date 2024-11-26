using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class caseTimerFriendlyTurn : caseTimer {

    public caseTimerFriendlyTurn(int parTimerMax, enumCaseType parEnumCaseType, bool parIsVisible, bool parIsTimerMax = true, bool parIsAutoReset = true) : base(parTimerMax, parEnumCaseType, parIsVisible, parIsTimerMax, parIsAutoReset) { }

    public void updateOnTurnEnd(Thing source) {
        updateTimer(source);
    }
}
