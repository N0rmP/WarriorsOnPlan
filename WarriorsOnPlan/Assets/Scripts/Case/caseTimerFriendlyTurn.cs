using Processes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cases {
    // caseTimer which affects only when case-owner is acting
    // caseTimerFriendlyTurn updates its timer when the owner's side's turn ends
    public abstract class caseTimerFriendlyTurn : caseTimer {

        public caseTimerFriendlyTurn(int[] parArrParameter, enumCaseType parEnumCaseType = enumCaseType.effect, bool parIsVisible = false) : base(parArrParameter, parEnumCaseType, parIsVisible) { }

        public void updateOnTurnEnd(Thing source) {
            combatManager.CM.executeProcess(new processByproductDelecate(() => updateTimer(source)));
        }
    }
}