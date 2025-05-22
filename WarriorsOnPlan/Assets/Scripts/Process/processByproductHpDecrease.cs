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

            //�� �����ϸ� �� �ڵ带 combatManager���� ��� process ���� ���Ŀ� ��� warrior�� ���Ͽ� status ���Ű� �Բ� �����ϵ��� �����ϱ�
            if (source.curHp <= 0) {
                combatManager.CM.executeProcess(new processByproductDie(source, attackerThing));
            }
        }

        protected override void actualSHOW() {
            gameManager.GM.TC.addDelegate(
                () => {
                    source.updatePanelHp();
                    // �� ���� ���ذ� ���� ������� �� Ƣ����� �����
                },
                combatManager.CM.getBodyAnimationDuration()
                );
        }
    }
}