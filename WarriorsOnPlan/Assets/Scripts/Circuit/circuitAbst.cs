using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuits {
    public abstract class circuitAbst<T> : codableObject, ISingleInfo where T : circuitAbst<T> {
        #region SingleInfoImplementation
        protected string singleInfo_ = "E";

        public string singleInfo {
            get {
                if (singleInfo_ == "E") {
                    singleInfo_ = gameManager.GM.JC.getJson<dataArbitraryString>("Circuit/" + this.GetType().Name).SwissArmyString;
                }
                return singleInfo_;
            }
        }
        #endregion SingleInfoImplementation

        public circuitAbst(IEnumerable<int> parParameters) : base(parParameters) { }

        // some circuit can be deactivated while combat, in that case circuit give its place to the circuitSpare
        public T circuitSpare { get; protected set; }
        public bool isSpareNeccesary { get; protected set; } = false;        

        public virtual T getValidCircuit(Thing source) {
            return (T)this;
        }
    }
}