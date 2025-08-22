using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Processes {
    public class processByproductHpSetMax : processByproductAbst {
        private Thing source;
        private int value;

        public processByproductHpSetMax(Thing parSource, int parValue, bool parIsShow = true) : base(parIsShow) {
            source = parSource;
            value = parValue;
        }

        protected override void actualDO() {
            source.setMaxHp(value);

            if (source.curHp <= 0) {
                combatManager.CM.executeProcess(new processByproductDie(source, null));
            }
        }
    }
}