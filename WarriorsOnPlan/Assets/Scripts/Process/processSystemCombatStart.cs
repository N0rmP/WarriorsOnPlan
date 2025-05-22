using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cases;

namespace Processes {
    public class processSystemCombatStart : processAbst {
        public processSystemCombatStart(bool parIsSHOW = true) : base(parIsSHOW) { }

        protected override void doAfterActualDo() {
            foreach (Thing th in combatManager.CM.HouC.arrTotalAlive) {
                // onEngage
                // 
                foreach (ICaseEngage cb in th.getCaseList<ICaseEngage>()) {
                    cb.onEngage(th);
                }
            }
        }

        protected override void actualDO() {
            foreach (Thing th in combatManager.CM.HouC.arrTotalAlive) {
                // warrior without any weapon can get a weaponBareKnuckle
                // adding weaponBareKnuckle is treated as system procedure, it doesn't trigger processByproductAddCase or ICaseBeforeAddCase
                if (th.getCaseList<toolWeapon>(false).Count == 0) {
                    th.addCase(gameManager.GM.MC.makeCodableObject<caseBase>(3001, new int[4] { 1, 1, 1, 1 }));
                }                
            }
        }

        protected override void actualSHOW() {
            // ★ 대충 전투 시작 간판 띄우기
        }
    }
}