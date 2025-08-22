using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Processes {
    public class processByproductHpDecrease : processByproductAbst {
        // tupCounter helps several damage-number-texts to popup in order, not at once
        private static (int distinguisher, float delay) tupCounter = (-1, 0f);

        private Thing source;
        private Thing attackerThing;
        // value & valueFinal are positive
        private int value;
        public int valueFinal { get; private set; }
        private bool isShowInstant;

        public processByproductHpDecrease(Thing parSource, Thing parAttacker, int parValue, bool parIsShow = true, bool parIsShowInstant = false) : base(parIsShow) {
            source = parSource;
            attackerThing = parAttacker;
            value = Math.Max(0, parValue);
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
            valueFinal = Math.Abs(source.setCurHp(-value, true));

            if (source.stateCur > enumStateWarrior.dead && source.curHp <= 0) {
                combatManager.CM.executeProcess(new processByproductDie(source, attackerThing));
            }
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            if (isInterfered) {
                gameManager.GM.PC.popupBasicAlert(source.gameObject.getCanvasMainLocalPosition() + new Vector2(0f, gameManager.GM.option.stickDegreed), gameManager.GM.DHouC.bookWords.strHpDecrease + " " + gameManager.GM.DHouC.bookWords.strInterfere);
            }

            void showHpDecrease() {
                source.updatePanelHp();
                gameManager.GM.PC.popupDamage(source.gameObject.getCanvasMainLocalPosition() + new Vector2(gameManager.GM.option.stick * UnityEngine.Random.Range(-0.5f, 0.5f), gameManager.GM.option.stickDegreed), value.ToString());
            }

            // update counter
            if (tupCounter.distinguisher == thisCountDistinguisher) {
                tupCounter.delay += 0.1f;
            } else {
                tupCounter.distinguisher = thisCountDistinguisher;
                tupCounter.delay = 0f;
            }

            if (isShowInstant) {
                showHpDecrease();
            } else {
                gameManager.GM.TC.addDelegate(
                    () => {
                        showHpDecrease();
                    },
                    combatManager.CM.getBodyAnimationDuration() + tupCounter.delay
                );
            }
        }
    }
}