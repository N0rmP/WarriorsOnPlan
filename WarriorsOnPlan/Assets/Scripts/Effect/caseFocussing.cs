using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Processes;

namespace Cases {
    public class caseFocussing : caseTimerSelfishTurn, ICaseUpdateState {
        public Action delActivate { get; private set; }
        public Action delShow { get; private set; }

        public caseFocussing() : base("Image/Case/Effect/image_caseFocussing", enumCaseType.effect, true) {
            code = 4100;
            isRemovedOnAlarmed = true;
        }

        #region ICase
        (ICaseUpdateState, enumStateWarrior) ICaseUpdateState.caseFunc(Thing source) {
            return (this, timerCur > 1 ? enumStateWarrior.focussing : enumStateWarrior.focussingEnd);
        }

        // interfered focussing is canceled without activating
        public void onInterfered(Thing source) {
            if (source.stateCur == enumStateWarrior.dead) {
                return;
            }

            combatManager.CM.executeProcess(
                new processByproductRemoveCase(source, this)
            );
        }
        #endregion ICase

        public override List<object> getReferences() {
            return new List<object>() { delActivate, delShow };
        }

        public override void restoreReferences(List<object> parListReference) {
            base.restoreReferences(parListReference);

            delActivate = parListReference[0] as Action;
            delShow = parListReference[1] as Action;
        }
    }
}
