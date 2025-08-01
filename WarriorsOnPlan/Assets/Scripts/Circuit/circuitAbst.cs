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
                    infoDescription_ = gameManager.GM.FC.importResourcesJson<dataArbitraryString>("Circuit/" + this.GetType().Name).SwissArmyString;
                }
                return string.Format(infoDescription_, getDescriptionArgument());
            }
        }

        public virtual object[] getDescriptionArgument() { 
            return new object[0]; 
        }
        #endregion InfoImplementation

        public circuitAbst() { }
    }
}