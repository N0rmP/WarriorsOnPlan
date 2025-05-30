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
            if (isShowInstant) {
                source.updatePanelHp();
                // ★ 증가한 체력이 숫자 모양으로 뿅 튀어나오게 만들기
            } else {
                gameManager.GM.TC.addDelegate(
                    () => {
                        source.updatePanelHp();
                        // ★ 증가한 체력이 숫자 모양으로 뿅 튀어나오게 만들기
                    },
                    combatManager.CM.getBodyAnimationDuration()
                );
            }
        }
    }
}