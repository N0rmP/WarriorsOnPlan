using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Processes {
    public class processSystemCombatEnd : processAbst {
        private combatResult curCombatResult;

        public processSystemCombatEnd(bool parIsSHOW = true) : base(parIsSHOW) { }

        protected override void actualDO() {
            // getCombatResult not only returns current combatResult, it creates new combatResult of current combat if it doesn't exist now
            // actualDo works twice in combat & reenact so ensuring it to work only once needed
            if (combatManager.CM.combatState == enumCombatState.combatDone) {
                curCombatResult = combatManager.CM.getCombatResult();
                // mark this level as cleared if player won and the cleared level isn't already added
                if (curCombatResult.isPlayerWin){
                    mapManager.MM.doWhenCombatVictory();
                }
            }
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            gameManager.GM.TC.addDelegate(
                () => {
                    combatManager.CM.CUM.BCR.activate(combatManager.CM.getCombatResult());
                    gameManager.GM.IC.inaugurateTemporayInputContinaer(null);
                },
                structInterValsAndDurations.fltBodyAnimationDuration
            );
        }
    }
}