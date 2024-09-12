using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circuitAbst<T> : ISingleInfo where T : circuitAbst<T> {
    #region SingleInfoImplementation
    // i really want to make singleInfo static and each derived classes have their static-singleInfo, but its implementation isnt efficient
    // anyway singleInfo of circuits is only called from uiCircuitPanel and uiCircuitPanel has its own circuit list, so it's fine with just non-static string
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

    // some circuit can be deactivated while combat, in that case circuit give its place to the circuitSpare
    public T circuitSpare { get; protected set; }
    public bool isSpareNeccesary { get; protected set; } = false;

    // code is used by circuitHub, it prevents creating same circuit twice
    public int code { get; protected set; }

    public virtual T getValidCircuit(Thing source) {
        return (T)this;
    }
}
