using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class buttonCircuitConcrete : MonoBehaviour {
    [SerializeField]
    private int orderCircuitConcrete_ = -1;
    public int orderCircuitConcrete {
        get {
            return orderCircuitConcrete_;
        }
        set {
            if (orderCircuitConcrete_ == -1) {
                orderCircuitConcrete_ = value;
            }
        }
    }

    public void chooseCircuitConcrete() {
        combatManager.CM.CUM.CCS.chooseCircuitConcrete(orderCircuitConcrete);
    }
}
