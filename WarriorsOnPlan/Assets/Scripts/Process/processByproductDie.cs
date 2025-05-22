using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Processes {
    public class processByproductDie : processByproductAbst {
        private Thing dead;
        private Thing destoryer;

        public processByproductDie(Thing parDead, Thing parDestroyer, bool parIsSHOW = true) : base(parIsSHOW) {
            dead = parDead;
            destoryer = parDestroyer;
        }

        protected override void doAfterActualDo() {
            base.doBeforeActualDo();

            // onDestroied
            foreach (ICaseDestroied cb in dead.getCaseList<ICaseDestroied>()) {
                cb.onDestroied(dead, destoryer);
            }

            // onDestroy
            if (destoryer != null) {
                foreach (ICaseDestroy cb in destoryer.getCaseList<ICaseDestroy>()) {
                    cb.onDestroy(destoryer, dead);
                }
            }
        }

        protected override void actualDO() {
            dead.destroied();
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            gameManager.GM.TC.addDelegate(
                () => dead.animateDead(),
                combatManager.CM.getBodyAnimationDuration()
            );
            gameManager.GM.TC.addDelegate(
                () => dead.fadeOut(),
                combatManager.CM.getBodyAnimationDuration() + 1f
            );
            gameManager.GM.TC.addDelegate(
                () => dead.setPosition(new Vector3(50f, 0f, 50f)),
                combatManager.CM.getBodyAnimationDuration() + 2f
            );
        }
    }
}