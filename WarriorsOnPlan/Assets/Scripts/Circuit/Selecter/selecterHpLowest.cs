using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Circuits {
    public class selecterHpLowest : selecterAbst {
        public selecterHpLowest(int[] parParameter) : base(parParameter) {
            code = 1302;
        }

        public override Thing select(Thing source) {
            combatManager.CM.HouC.sortByHp();

            if (combatManager.CM.HouC.arrEnemyHpSorted.Length == 0) {
                Debug.Log("selecterHpLowest tried to access combatManager.CM.HouC.arrEnemyHpSorted, but its length was zero");
                return null;
            }

            return combatManager.CM.HouC.arrPlayerHpSorted[0];
        }
    }
}