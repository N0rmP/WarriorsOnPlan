using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class buttonCircuitType : buttonCustomAbst {
    [SerializeField]
    private int orderCircuitType_ = -1;
    public int orderCircuitType {
        get {
            return orderCircuitType_;
        }
        set {
            if (orderCircuitType_ == -1) {
                orderCircuitType_ = value;
            }
        }
    }

    public void activateBoxCircuitConcrete(int parCircuitTypeBeingChosen) {
        combatUIManager.CUM.CCS.activateBoxCircuitConcrete(parCircuitTypeBeingChosen);
    }

    public override void actualDoWhenTriggered() {
        activateBoxCircuitConcrete(orderCircuitType);
    }
}
