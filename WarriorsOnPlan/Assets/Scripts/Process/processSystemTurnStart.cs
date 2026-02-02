using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Cases;
using System.Text;

namespace Processes {
    public class processSystemTurnStart : processAbst {
        private Thing[] arrActors;

        // endedTurnSide only exists for debugging
        private enumSide startedTurnSide;

        public processSystemTurnStart(Thing[] parArrActors, bool parIsSHOW = true) : base(parIsSHOW) {
            arrActors = parArrActors;
        }

        protected override void doAfterActualDo() {
            base.doBeforeActualDo();
            startedTurnSide = combatManager.CM.sideTurn;

            // onTurnStart
            foreach (Thing th in arrActors) {
                th.observeVoid<ICaseTurnStart>(new object[1] { th });
            }
        }

        protected override void actualDO() {
            // update timers
            foreach (Thing th in arrActors) {
                foreach (caseTimerHostileTurn ct in th.getCaseList<caseTimerHostileTurn>()) {
                    ct.updateOnTurnStart(th);
                }
            }
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            combatManager.CM.CUM.testShowTurn();
        }

        #region test
        protected override void testAnythingSay(StringBuilder parSB) {
            parSB.Append(startedTurnSide);
            parSB.Append(" turn start");
        }
        #endregion test
    }
}