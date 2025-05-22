using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Processes {
    public class processByproductHpSet : processByproductAbst {
        private Thing source;
        private Thing attacker;
        private int value;

        public processByproductHpSet(Thing parSource, Thing parAttacker, int parValue, bool parIsShow = true) : base(parIsShow) {
            source = parSource;
            attacker = parAttacker;
            value = parValue;
        }

        protected override void actualDO() {
            source.setCurHp(value, false);

            if (source.curHp <= 0) {
                combatManager.CM.executeProcess(new processByproductDie(source, attacker));
            }
        }
    }
}