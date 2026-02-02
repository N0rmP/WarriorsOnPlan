using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static Unity.VisualScripting.Member;

namespace Processes {
    public class processByproductDie : processByproductAbst {
        private Thing dead;
        private Thing destroyer;

        public processByproductDie(Thing parDead, Thing parDestroyer, bool parIsSHOW = true) : base(parIsSHOW) {
            if (parDead == null) {
                Debug.Log("processByproductDie results in error because parDead is null - parDestroyer : " + parDestroyer);
            }

            dead = parDead;
            destroyer = parDestroyer;
        }

        protected override void doBeforeActualDo() {
            base.doBeforeActualDo();

            // onInterferableDestroied, onInterferable
            isInterfered = 
                dead.observeInterferable<ICaseInterferableDestroied>(new object[2] { dead, destroyer }) ||
                (destroyer != null && destroyer.observeInterferable<ICaseInterferableDestroy>(new object[2] { destroyer, dead }));
            if (isInterfered) {
                return;
            }

            // onBeforeDestroied
            dead.observeVoid<ICaseBeforeDestroied>(new object[2] { dead, destroyer });

            // onBeforeDestroy
            destroyer?.observeVoid<ICaseBeforeDestroy>(new object[2] { destroyer, dead });
        }

        protected override void doAfterActualDo() {
            base.doBeforeActualDo();

            // onAfterDestroied
            dead.observeVoid<ICaseAfterDestroied>(new object[2] { dead, destroyer });

            // onDestroy
            destroyer?.observeVoid<ICaseAfterDestroy>(new object[2] { destroyer, dead });
        }

        protected override void actualDO() {
            dead.destroied();
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            // ★ 문자열 대체
            if (isInterfered) {
                gameManager.GM.PC.popupBasicAlert(dead.gameObject.getCanvasMainLocalPosition() + new Vector2(0, gameManager.GM.option.stick), "Dead, But It Refused");
            }

            gameManager.GM.TC.addDelegate(
                () => dead.thisOrganAnimation.animateDead(),
                combatManager.CM.getBodyAnimationDuration()
            );
            gameManager.GM.TC.addDelegate(
                () => dead.fadeOut(),
                combatManager.CM.getBodyAnimationDuration() + 1f
            );
            gameManager.GM.TC.addDelegate(
                () => dead.setPosition(new Vector3(20f, 0f, 20f)),
                combatManager.CM.getBodyAnimationDuration() + 2f
            );
        }

        #region test
        protected override void testAnythingSay(StringBuilder parSB) {
            parSB.Append(destroyer?.ToString());
            parSB.Append(" killed ");
            parSB.Append(dead?.ToString());
        }
        #endregion test
    }
}