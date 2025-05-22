using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Processes {
    public class processByproductHpDecrease : processByproductAbst {
        private Thing source;
        private Thing attackerThing;
        private int value;
        public int valueFinal { get; private set; }

        public processByproductHpDecrease(Thing parSource, Thing parAttacker, int parValue, bool parIsShow = true) : base(parIsShow) {
            source = parSource;
            attackerThing = parAttacker;
            value = parValue;
        }

        protected override void doBeforeActualDo() {
            base.doBeforeActualDo();

            // onBeforeHpDecrease
            foreach (ICaseBeforeHpDecrease cb in source.getCaseList<ICaseBeforeHpDecrease>()) {
                cb.onBeforeHpDecrease(source, ref value);
            }
        }

        protected override void doAfterActualDo() {
            base.doAfterActualDo();

            // onAfterHpDecrease
            foreach (ICaseAfterHpDecrease cb in source.getCaseList<ICaseAfterHpDecrease>()) {
                cb.onAfterHpDecrease(source, value);
            }
        }

        protected override void actualDO() {
            valueFinal = source.setCurHp(-value, true);

            //★ 가능하면 이 코드를 combatManager에서 모든 process 실행 직후에 모든 warrior에 대하여 status 갱신과 함께 시행하도록 변경하기
            if (source.curHp <= 0) {
                combatManager.CM.executeProcess(new processByproductDie(source, attackerThing));
            }
        }

        protected override void actualSHOW() {
            gameManager.GM.TC.addDelegate(
                () => {
                    source.updatePanelHp();
                    // ★ 입은 피해가 숫자 모양으로 뿅 튀어나오게 만들기
                },
                combatManager.CM.getBodyAnimationDuration()
                );
        }
    }
}