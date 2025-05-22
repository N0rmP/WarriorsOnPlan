using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Processes;

namespace Cases {
    // caseTimer which affects only when case-owner's hostile warrior is acting
    // caseTimerHostileTurn update its timer when the warrior's side's turn starts
    public abstract class caseTimerHostileTurn : caseTimer {

        public caseTimerHostileTurn(int[] parArrParameter, enumCaseType parEnumCaseType = enumCaseType.effect, bool parIsVisible = false) : base(parArrParameter, parEnumCaseType, parIsVisible) { }

        public void updateOnTurnStart(Thing source) {
            combatManager.CM.executeProcess(new processByproductDelecate(() => updateTimer(source)));
        }
    }
}
