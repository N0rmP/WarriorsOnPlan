using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Processes;

namespace Cases {
    public class effectFocussing : caseTimerSelfishTurn, ICaseUpdateState {
        public Action delActualDo { get; private set; }
        public Action delShow { get; private set; }

        public effectFocussing() : base("Image/Case/Effect/Image_effectFocussing", enumCaseType.effect, true) {
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
            return new List<object>() { delActualDo, delShow };
        }

        public override void restoreReferences(List<object> parListReference) {
            base.restoreReferences(parListReference);

            delActualDo = parListReference[0] as Action;
            delShow = parListReference[1] as Action;
        }
    }
}
