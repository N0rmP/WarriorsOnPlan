using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Processes {
    public class processByproductDelecate : processByproductAbst, IEnumerable {
        private List<Action> listDel;

        public processByproductDelecate(Action parDel) {
            listDel = new List<Action>() { parDel };            
        }

        public processByproductDelecate(IEnumerable<Action> parEDel) {
            listDel.AddRange(parEDel);
        }

        public void addDel(Action parDel) {
            // addDel works only when combat is in looping because it doesn't only add delegate in this but also execute the added delegate once
            if (combatManager.CM.combatState != enumCombatState.combat) {
                return;
            }

            parDel();
            listDel.Add(parDel);
        }

        public void addDel(IEnumerable<Action> parEnumerable) {
            foreach (Action a in parEnumerable) {
                addDel(a);
            }
        }

        public IEnumerator GetEnumerator() {
            return listDel.GetEnumerator();
        }

        protected override void actualDO() {
            foreach (Action a in listDel) {
                a();
            }
        }
    }
}