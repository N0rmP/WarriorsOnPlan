using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cases;

namespace Processes {
    public class processSystemTurnEnd : processAbst {
        private Thing[] arrActors;
        private Action delTurnChange;

        // turn doesn't change if isExtraTurn is true
        // unlike HearthStone ExtraTurn can't be stored many during one turn, player should achieve ExtraTurn every turn for eternal ExtraTurn
        private bool isExtraTurn;

        public processSystemTurnEnd(Thing[] parArrActors, Action parDelTurnChange, bool parIsSHOW = true) : base(parIsSHOW) {
            arrActors = parArrActors;
            delTurnChange = parDelTurnChange;
            isExtraTurn = false;
        }

        protected override void doBeforeActualDo() {
            base.doBeforeActualDo();

            // onTurnEnd
            foreach (Thing th in arrActors) {
                foreach(bool tempIsExtraTurn in th.observeReturnEnumerate<ICaseTurnEnd, bool>(new object[1] { th }))
                isExtraTurn = isExtraTurn || tempIsExtraTurn;
            }            
        }

        protected override void actualDO() {
            // update timer
            foreach (Thing th in arrActors) {
                foreach (caseTimerFriendlyTurn ct in th.getCaseList<caseTimerFriendlyTurn>()) {
                    ct.updateOnTurnEnd(th);
                }
            }

            if (isExtraTurn) {
                // ★ 추가 턴 그래릭 효과, 대충 끼얏호우한 효과 넣으셈
            } else {
                delTurnChange();
            }
        }
    }
}