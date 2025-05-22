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
            listDel.Add(parDel);
        }

        public void addDel(IEnumerable<Action> parEnumerable) {
            foreach (Action a in parEnumerable) {
                listDel.Add(a);
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