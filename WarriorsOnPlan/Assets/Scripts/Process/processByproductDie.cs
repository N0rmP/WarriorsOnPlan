using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

namespace Processes {
    public class processByproductDie : processByproductAbst {
        private Thing dead;
        private Thing destroyer;

        public processByproductDie(Thing parDead, Thing parDestroyer, bool parIsSHOW = true) : base(parIsSHOW) {
            dead = parDead;
            destroyer = parDestroyer;
        }

        protected override void doBeforeActualDo() {
            base.doBeforeActualDo();

            // onInterferableDestroied, onInterferable
            isInterfered = 
                dead.observeInterferable<ICaseInterferableDestroied>(new object[2] { dead, destroyer }) ||
                destroyer.observeInterferable<ICaseInterferableDestroy>(new object[2] { destroyer, dead });
            if (isInterfered) {
                return;
            }

            // onBeforeDestroied
            dead.observeVoid<ICaseBeforeDestroied>(new object[2] { dead, destroyer });

            // onBeforeDestroy
            destroyer.observeVoid<ICaseBeforeDestroy>(new object[2] { destroyer, dead });
        }

        protected override void doAfterActualDo() {
            base.doBeforeActualDo();

            // onAfterDestroied
            dead.observeVoid<ICaseAfterDestroied>(new object[2] { dead, destroyer });

            // onDestroy
            destroyer.observeVoid<ICaseAfterDestroy>(new object[2] { destroyer, dead });
        }

        protected override void actualDO() {
            dead.destroied();
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            if (isInterfered) {
                gameManager.GM.PC.popupBasicAlert(dead.transform.position, "Dead, But It Refused", false);
            }

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