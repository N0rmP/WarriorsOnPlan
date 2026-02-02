using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Processes {
    public class processByproductDelegate : processByproductAbst {
        private Action del;

        public processByproductDelegate(Action parDel) {
            del = parDel;
        }

        public void addDel(Action parDel) {
            // addDel works only when combat is in looping because it doesn't only add delegate in this but also execute the added delegate once
            if (combatManager.CM.combatState != enumCombatState.combat) {
                return;
            }

            parDel();
            del += parDel;
        }

        public void addDel(processByproductDelegate parPBD) {
            parPBD.del();
            del += parPBD.del;
        }

        protected override void actualDO() {
            del();
        }

        #region test
        protected override void testAnythingSay(StringBuilder parSB) {
            parSB.Append("delegate count ");
            parSB.Append(del.GetInvocationList().Length);
        }
        #endregion test
    }
}