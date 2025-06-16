using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

namespace Processes {
    public class processByproductHpIncrease : processByproductAbst {
        private Thing source;
        private int value;
        private bool isShowInstant;

        public processByproductHpIncrease(Thing parSource, int parValue, bool parIsShow = true , bool parIsShowInstant = false) : base(parIsShow) {
            source = parSource;
            value = parValue;
            isShowInstant = parIsShowInstant;
        }

        protected override void doBeforeActualDo() {
            base.doBeforeActualDo();

            // onBeforeHpIncrease
            foreach (ICaseBeforeHpIncrease cb in source.getCaseList<ICaseBeforeHpIncrease>()) {
                cb.onBeforeHpIncrease(source, ref value);
            }
        }

        protected override void doAfterActualDo() {
            base.doAfterActualDo();

            // onAfterHpIncrease
            foreach (ICaseAfterHpIncrease cb in source.getCaseList<ICaseAfterHpIncrease>()) {
                cb.onAfterHpIncrease(source, value);
            }
        }

        protected override void actualDO() {
            value = source.setCurHp(value, true);
        }

        protected override void actualSHOW() {
            void showHpDecrease() {
                source.updatePanelHp();
                gameManager.GM.PC.popupHeal(source.transform.position + new Vector3(0f, 0f, 1f), value.ToString(), false);
            }

            if (isShowInstant) {
                showHpDecrease();
            } else {
                gameManager.GM.TC.addDelegate(
                    () => {
                        showHpDecrease();
                    },
                    combatManager.CM.getBodyAnimationDuration()
                );
            }
        }
    }
}