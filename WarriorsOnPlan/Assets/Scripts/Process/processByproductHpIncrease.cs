using System.Collections;
using System.Collections.Generic;
using System.Text;
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

            // onInterferableAttack
            object[] tempParameters = new object[2] { source, value };
            isInterfered = source.observeInterferable<ICaseInterferableHpIncrease>(tempParameters);
            value = (int)tempParameters[1];
            if (isInterfered) {
                return;
            }

            // onBeforeHpIncrease
            source.observeVoid<ICaseBeforeHpIncrease>(tempParameters);
            value = (int)tempParameters[1];
        }

        protected override void doAfterActualDo() {
            base.doAfterActualDo();

            // onAfterHpIncrease
            source.observeVoid<ICaseAfterHpIncrease>(new object[2] { source, value });
        }

        protected override void actualDO() {
            value = source.setCurHp(value, true);
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            if (isInterfered) {
                gameManager.GM.PC.popupBasicAlert(source.gameObject.getCanvasMainLocalPosition(), gameManager.GM.DHouC.bookWords.strHpIncrease + " " + gameManager.GM.DHouC.bookWords.strInterfere);
            }

            void showHpIncrease() {
                source.updatePanelHp();
                gameManager.GM.PC.popupHeal(source.gameObject.getCanvasMainLocalPosition() + new Vector2(0, gameManager.GM.option.stick), value.ToString());
            }

            if (isShowInstant) {
                showHpIncrease();
            } else {
                gameManager.GM.TC.addDelegate(
                    () => {
                        showHpIncrease();
                    },
                    combatManager.CM.getBodyAnimationDuration()
                );
            }
        }

        #region test
        protected override void testAnythingSay(StringBuilder parSB) {
            parSB.Append(source?.ToString());
            parSB.Append("\'s hp increases by ");
            parSB.Append(value.ToString());
        }
        #endregion test
    }
}