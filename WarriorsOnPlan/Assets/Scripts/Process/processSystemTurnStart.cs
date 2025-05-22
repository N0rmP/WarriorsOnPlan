using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cases;

namespace Processes {
    public class processSystemTurnStart : processAbst {
        private Thing[] arrActors;

        public processSystemTurnStart(Thing[] parArrActors, bool parIsSHOW = true) : base(parIsSHOW) {
            arrActors = parArrActors;
        }

        protected override void doAfterActualDo() {
            base.doBeforeActualDo();            

            // onTurnStart
            foreach (Thing th in arrActors) {
                foreach (ICaseTurnStart cb in th.getCaseList<ICaseTurnStart>()) {
                    cb.onTurnStart(th);
                }
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
    }
}