using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Cases;

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

            // source body animation
            source.Look(target.transform.position);
            source.clearAttackAnimation();
            foreach (toolWeapon tw in listWeapon) {
                source.addAttackAnimation(tw.attackAnimation);
            }
            source.animateAttack();

            // each weapon vfx animation
            int tempI = 0;
            foreach (toolWeapon tw in listWeapon) {
                gameManager.GM.TC.addDelegate(
                    () => tw.showEffect(source, target),
                    combatManager.CM.getBodyAnimationDuration() * (tempI + 1) / (float)(listWeapon.Count + 1)
                );
                tempI++;
            }
        }
    }
}