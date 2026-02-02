using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cases;

namespace Processes {
    public class processSystemCombatStart : processAbst {
        public processSystemCombatStart(bool parIsSHOW = true) : base(parIsSHOW) { }

        protected override void doAfterActualDo() {
            // onEngage

            foreach (Thing th in combatManager.CM.HouC.arrTotalAlive) {
                th.observeVoid<ICaseEngage>(new object[1] { th });
            }
        }

        protected override void actualDO() {
            foreach (Thing th in combatManager.CM.HouC.arrTotalAlive) {
                // warrior without any weapon can get a weaponBareKnuckle
                // adding weaponBareKnuckle is treated as system procedure, it doesn't trigger processByproductAddCase or ICaseBeforeAddCase
                if (th.getCaseCount<toolWeapon>() == 0) {
                    th.addCase(th.weaponDefault);
                }
            }
        }

        protected override void actualSHOW() {
            // ★ 대충 전투 시작 간판 띄우기
        }
    }
}