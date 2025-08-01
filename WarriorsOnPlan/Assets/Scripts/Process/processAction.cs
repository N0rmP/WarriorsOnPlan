using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cases;

namespace Processes {
    // processAction doesn't have the concrete Action itself
    // concrete Action is implemented in each processByproductActionAbst
    // processAction implements only the most general code of Actions and outsources the detailed work to thisPAction
    public class processAction : processAbst {
        private Thing source;
        private processByproductActionAbst thisPAction;

        public processAction(Thing parSource, bool parIsSHOW = true) : base(parIsSHOW) {
            source = parSource;
        }

        protected override void doBeforeActualDo() {
            base.doBeforeActualDo();

            // countAction++, it should precede all other scripts because it's expected
            combatManager.CM.executeProcess(
                new processByproductDelecate(
                    () => combatManager.CM.incrementCountAction()
                )
            );

            // timer update
            foreach (caseTimerSelfishTurn ct in source.getCaseList<caseTimerSelfishTurn>()) {
                ct.updateOnActionStart(source);
            }

            // sorting houseComponent will be done in each selecterAbst or class which requires sorting
            // update targets, this precedes state decision because targets can affect it
            source.updateTargets();
            // state decision
            source.updateState();
            // decide action
            thisPAction = source.makeAction();

            // onInterferableAction
            isInterfered = source.observeInterferable<ICaseInterferableAction>(new object[1] { source });
            if (isInterfered) {
                return;
            }

            // onBeforeAction
            source.observeVoid<ICaseBeforeAction>(new object[1] { source });
        }

        protected override void doAfterActualDo() {
            base.doAfterActualDo();

            // onAfterAction
            source.observeVoid<ICaseAfterAction>(new object[1] { source });

            // timer update
            foreach (caseTimerSelfishTurn ct in source.getCaseList<caseTimerSelfishTurn>()) {
                ct.updateOnActionEnd(source);
            }
        }

        protected override void actualDO() {
            combatManager.CM.executeProcess(thisPAction);
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            if (isInterfered) {
                gameManager.GM.PC.popupBasicAlert(source.transform.position, gameManager.GM.DHouC.bookWords.strAction + " " + gameManager.GM.DHouC.bookWords.strInterfere, false);
            }

            combatManager.CM.CUM.setActionCounter(thisCountAction);
        }
    }
}