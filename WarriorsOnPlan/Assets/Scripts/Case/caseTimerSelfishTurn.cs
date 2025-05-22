using Processes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cases {
    // caseTimer which affects only when case-owner is acting
    // caseTimerSelfishTurn updates its timer when the owner's action ends after the action started once
    public abstract class caseTimerSelfishTurn : caseTimer {
        private bool isActionStartPassed;

        public caseTimerSelfishTurn(int[] parArrParameter, enumCaseType parEnumCaseType = enumCaseType.effect, bool parIsVisible = false) : base(parArrParameter, parEnumCaseType, parIsVisible) {
            isActionStartPassed = false;
        }

        public void updateOnActionStart(Thing source) {
            combatManager.CM.executeProcess(new processByproductDelecate(() => isActionStartPassed = true));
        }

        public void updateOnActionEnd(Thing source) {
            combatManager.CM.executeProcess(new processByproductDelecate(
                () => {
                    if (isActionStartPassed) {
                        updateTimer(source);
                    }
                    isActionStartPassed = false;
                }));
        }
    }
}