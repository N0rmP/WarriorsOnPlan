using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Cases;
using System.Text;

namespace Processes {
    public class processByproductActionAttack : processByproductActionAbst {
        private Thing target;
        private List<toolWeapon> listWeapon;
        private List<damageInfo> listDInfo;

        public processByproductActionAttack(Thing parSource, bool parIsSHOW = true) : base(parSource, parIsSHOW) {
            target = source.whatToAttack;
            listWeapon = new List<toolWeapon>();
            listDInfo = new List<damageInfo>();

        }

        protected override void doBeforeActualDo() {
            base.doBeforeActualDo();

            // if target is null, invalidate all other execution as processActionAttack
            if (target is null) {
                isInterfered = true;
                return;
            }

            // onInterferableAttack
            isInterfered = source.observeInterferable<ICaseInterferableAttack>(new object[2] { source, target });
            if (isInterfered) {
                return;
            }

            // onBeforeAttack
            source.observeVoid<ICaseBeforeAttack>(new object[2] { source, target });

            // rake available weapons
            listWeapon = source.getListAvailableWeapon(target);
        }

        protected override void doAfterActualDo() {
            base.doAfterActualDo();

            // onAfterAttack
            source.observeVoid<ICaseAfterAttack>(new object[3] { source, target, listDInfo.ToArray() });
        }

        protected override void actualDO() {
            base.actualDO();
            
            // if target is null, invalidate all other execution as processActionAttack
            if (target is null) {
                return;
            }
            
            List<damageInfo> listDInfo = new List<damageInfo>();               
            foreach (toolWeapon tw in listWeapon){
                foreach (damageInfo di in tw.attack(source)) {
                    listDInfo.Add(di);
                }
            }
            
            if (target is not null && listDInfo.Count > 0) {
                combatManager.CM.executeProcess(
                        new processByproductDealDamage(listDInfo.ToArray(), target)
                    );
            }
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            if (isInterfered) {
                gameManager.GM.PC.popupBasicAlert(source.gameObject.getCanvasMainLocalPosition() + new Vector2(0f, gameManager.GM.option.stickDegreed), gameManager.GM.DHouC.bookWords.strAttack + " " + gameManager.GM.DHouC.bookWords.strInterfere);
            }

            if (target is null) {
                gameManager.GM.PC.popupBasicAlert(source.gameObject.getCanvasMainLocalPosition() + new Vector2(0f, gameManager.GM.option.stickDegreed), gameManager.GM.DHouC.bookPopupAlert.strAlertNoAttackTarget);
                return;
            }

            // attack animation, including source-body-animation & each weapon SHOW
            source.Look(target.transform.position);
            int[] tempArrCountAttackAnimation = new int[(int)enumAttackAnimation.max];
            foreach (toolWeapon tw in listWeapon) {
                source.thisOrganAnimation.addAttackAnimation(tw.attackAnimation);
                tw.showEffect(source, target, tempArrCountAttackAnimation[(int)tw.attackAnimation]);
            }
            source.thisOrganAnimation.animateAttack();
        }

        #region test
        protected override void testAnythingSay(StringBuilder parSB) {
            parSB.Append(source?.ToString());
            parSB.Append(" attacked ");
            parSB.Append(target?.ToString());
            parSB.Append(", used weapons list {");
            foreach (toolWeapon tw in listWeapon) {
                parSB.Append(tw?.ToString());
                parSB.Append(',');
            }
            parSB.Append('}');
        }
        #endregion test
    }
}