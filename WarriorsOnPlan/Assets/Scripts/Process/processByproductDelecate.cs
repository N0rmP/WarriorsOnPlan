using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Processes {
    public class processByproductDelecate : processByproductAbst {
        private Action del;

        public processByproductDelecate(Action parDel) {
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

        public void addDel(processByproductDelecate parPBD) {
            parPBD.del();
            del += parPBD.del;
        }

        protected override void actualDO() {
            del();
        }
    }
}