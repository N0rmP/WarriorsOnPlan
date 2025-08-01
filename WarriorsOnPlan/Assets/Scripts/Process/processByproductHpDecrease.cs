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
        private bool isShowInstant;

        public processByproductHpDecrease(Thing parSource, Thing parAttacker, int parValue, bool parIsShow = true, bool parIsShowInstant = false) : base(parIsShow) {
            source = parSource;
            attackerThing = parAttacker;
            value = parValue;
            isShowInstant = parIsShowInstant;
        }

        protected override void doBeforeActualDo() {
            base.doBeforeActualDo();

            // onInterferableAttack
            object[] tempParameters = new object[2] { source, value };
            isInterfered = source.observeInterferable<ICaseInterferableHpDecrease>(tempParameters);
            value = (int)tempParameters[1];
            if (isInterfered) {
                return;
            }

            // onBeforeHpDecrease            
            source.observeVoid<ICaseBeforeHpDecrease>(tempParameters);
            value = (int)tempParameters[1];
        }

        protected override void doAfterActualDo() {
            base.doAfterActualDo();

            // onAfterHpDecrease
            source.observeVoid<ICaseAfterHpDecrease>(new object[2] { source, value });
        }

        protected override void actualDO() {
            valueFinal = source.setCurHp(-value, true);

            //★ 가능하면 이 코드를 combatManager에서 모든 process 실행 직후에 모든 warrior에 대하여 status 갱신과 함께 시행하도록 변경하기
            if (source.curHp <= 0) {
                combatManager.CM.executeProcess(new processByproductDie(source, attackerThing));
            }
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            if (isInterfered) {
                gameManager.GM.PC.popupBasicAlert(source.transform.position, gameManager.GM.DHouC.bookWords.strHpDecrease + " " + gameManager.GM.DHouC.bookWords.strInterfere, false);
            }

            void showHpDecrease() {
                source.updatePanelHp();
                gameManager.GM.PC.popupDamage(source.transform.position + new Vector3(Random.Range(-0.25f, 0.25f), 0f, 1f), value.ToString(), false);
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