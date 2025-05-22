using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Cases;

namespace Processes {
    public class processActionAttack : processActionAbst {
        private Thing target;
        private List<toolWeapon> listWeapon;
        private List<damageInfo> listDInfo;

        public processActionAttack(Thing parSource, bool parIsSHOW = true) : base(parIsSHOW) {
            source = parSource;
            target = source.whatToAttack;
            listWeapon = new List<toolWeapon>();
            listDInfo = new List<damageInfo>();

        }

        protected override void doBeforeActualDo() {
            base.doBeforeActualDo();
            foreach (ICaseBeforeAttack cb in source.getCaseList<ICaseBeforeAttack>()) {
                cb.onBeforeAttack(source, target);
            }

            foreach (toolWeapon tw in source.getListAvailableWeapon(target)) {
                listWeapon.Add(tw);
            }
        }

        protected override void doAfterActualDo() {
            base.doAfterActualDo();
            foreach (ICaseAfterAttack cb in source.getCaseList<ICaseAfterAttack>()) {
                cb.onAfterAttack(source, target, listDInfo.ToArray());
            }
        }

        protected override void actualDO() {
            base.actualDO();

            List<damageInfo> tempListDInfo = new List<damageInfo>();                        
            
            foreach (toolWeapon tw in listWeapon){
                foreach (damageInfo di in tw.attack(source)) {
                    tempListDInfo.Add(di);
                }
            }

            combatManager.CM.executeProcess(
                    new processByproductDealDamage(tempListDInfo.ToArray(), target)
                );
        }

        protected override void actualSHOW() {
            base.actualSHOW();

            // source body animation
            source.Look(target.transform.position);
            source.clearAttackAnimation();
            foreach (toolWeapon tw in listWeapon) {
                source.addAttackAnimation(tw.attackAnimation);
            }
            source.animateAttack();

            // each weapon vfx animation
            int tempI = 0;
            //for (int i = 0; i < listWeapon.Count; i++) {
            foreach(toolWeapon tw in listWeapon){
                gameManager.GM.TC.addDelegate(
                    () => tw.showEffect(source, target), 
                    combatManager.CM.getBodyAnimationDuration() * (tempI + 1) / (float)(listWeapon.Count + 1)
                );
                tempI++;
            }
        }
    }
}