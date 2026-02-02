using System.Collections;
using System.Collections.Generic;
using System.Text;
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

        #region test
        protected override void testAnythingSay(StringBuilder parSB) {
            parSB.Append(source?.ToString());
            parSB.Append("\'s max hp is set to ");
            parSB.Append(value.ToString());
        }
        #endregion test
    }
}