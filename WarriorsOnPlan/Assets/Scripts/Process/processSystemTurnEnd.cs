using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cases;

namespace Processes {
    public class processSystemTurnEnd : processAbst {
        private Thing[] arrActors;
        private Action delTurnChange;

        public processSystemTurnEnd(Thing[] parArrActors, Action parDelTurnChange, bool parIsSHOW = true) : base(parIsSHOW) {
            arrActors = parArrActors;
            delTurnChange = parDelTurnChange;
        }

        protected override void doBeforeActualDo() {
            base.doBeforeActualDo();

            // onTurnEnd
            foreach (Thing th in arrActors) {
                foreach (ICaseTurnEnd cb in th.getCaseList<ICaseTurnEnd>()) {
                    cb.onTurnEnd(th);
                }
            }            
        }

        protected override void actualDO() {
            // update timer
            foreach (Thing th in arrActors) {
                foreach (caseTimerFriendlyTurn ct in th.getCaseList<caseTimerFriendlyTurn>()) {
                    ct.updateOnTurnEnd(th);
                }
            }

            // ★ 턴 스킵 효과를 구현하고 싶다면 조건문을 추가해야 함
            delTurnChange();
        }
    }
}