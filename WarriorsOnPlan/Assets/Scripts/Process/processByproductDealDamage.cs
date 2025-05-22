using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Processes {
    public class processByproductDealDamage : processByproductAbst {
        // catution : sourceAttacker of damageInfo can be null if the damage ain't done directly but by effect (posion or burnt etc.)
        private damageInfo[] arrDInfo;
        private Thing target;

        public int damageTotal { get; private set; }

        public processByproductDealDamage(damageInfo[] parArrDInfo, Thing parTarget, bool parIsSHOW = true) : base(parIsSHOW) {
            arrDInfo = parArrDInfo;
            target = parTarget;
            damageTotal = 0;
        }

        protected override void doBeforeActualDo() {
            base.doBeforeActualDo();

            // total onBefore~ obeserving
            Thing tempSource;
            foreach (damageInfo di in arrDInfo) {
                tempSource = di.sourceAttacker;
                // onBeforeDealDamage (source's case)
                if (tempSource != null) {
                    foreach (ICaseBeforeDealDamage cb in tempSource.getCaseList<ICaseBeforeDealDamage>()) {
                        cb.onBeforeDealDamage(tempSource, target, di);
                    }
                }
                // onBeforeDamaged (targte's case)
                foreach (ICaseBeforeDamaged cb in target.getCaseList<ICaseBeforeDamaged>()) {
                    cb.onBeforeDamaged(tempSource, target, di);
                }
            }
        }

        protected override void doAfterActualDo() {
            base.doAfterActualDo();

            //total onAfter~ observing
            Thing tempSource;
            foreach (damageInfo di in arrDInfo) {
                tempSource = di.sourceAttacker;
                // onAfterDealDamage (source's case)
                if (tempSource != null) {
                    foreach (ICaseAfterDealDamage cb in tempSource.getCaseList<ICaseAfterDealDamage>()) {
                        cb.onAfterDealDamage(tempSource, target, di);
                    }
                }
                // onAfterDamaged (target's case)
                foreach (ICaseAfterDamaged cb in target.getCaseList<ICaseAfterDamaged>()) {
                    cb.onAfterDamaged(tempSource, target, di);
                }
            }
        }

        protected override void actualDO() {
            processByproductHpDecrease tempPBHD;
            damageInfo tempDI;
            for (int i = 0; i < arrDInfo.Length; i++) {
                tempDI = arrDInfo[i];
                tempPBHD = new processByproductHpDecrease(target, tempDI.sourceAttacker, tempDI.damage);
                combatManager.CM.executeProcess(tempPBHD);
                tempDI.damageDealt = tempPBHD.valueFinal;
                damageTotal += tempPBHD.valueFinal;
            }
        }

        protected override void actualSHOW() {
            foreach (damageInfo di in arrDInfo) {
                gameManager.GM.TC.addDelegate(
                    () => {
                        di.SHOW(target.transform.position);
                        // target.updatePanelHp();
                    },
                    combatManager.CM.getBodyAnimationDuration()
                    );
            }
        }
    }
}