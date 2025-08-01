using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Circuits {
    public class selecterHpLowest : selecterAbst {

        public selecterHpLowest() {
            code = 1302;
        }

        protected override Thing actualSelect(Thing source, List<Thing> parTargetList) {
            parTargetList.Sort(houseComponent.instComparerHp);
            return parTargetList[0];
        }
    }
}