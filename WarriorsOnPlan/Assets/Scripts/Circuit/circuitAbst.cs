using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuits {
    public abstract class circuitAbst/*<T>*/ : codableObject, IInfo /*where T : circuitAbst<T> */{
        #region InfoImplementation
        public string infoName {
            get {
                return this.GetType().Name;
            }
        }

        private string infoDescription_ = "E";
        public string infoDescription {
            get {
                if (infoDescription_ == "E") {
                    infoDescription_ = gameManager.GM.JC.getJson<dataArbitraryString>("Circuit/" + this.GetType().Name).SwissArmyString;
                }
                return string.Format(infoDescription_, getDescriptionArgument());
            }
        }

        public virtual object[] getDescriptionArgument() { 
            return new object[0]; 
        }
        #endregion InfoImplementation

        public circuitAbst(IEnumerable<int> parParameters) : base(parParameters) { }

        /* 서킷이 각자 스페어를 저장하던 것에서 circuitHub가 일괄 가지고 있는 것으로 변경됨
        // some circuit can be deactivated while combat, in that case circuit give its place to the circuitSpare
        public T circuitSpare { get; protected set; }
        public bool isSpareNeccesary { get; protected set; } = false;        

        public virtual T getValidCircuit(Thing source) {
            return (T)this;
        }
        */
    }
}