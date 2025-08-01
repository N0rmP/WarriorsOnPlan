using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class buttonCircuitType : MonoBehaviour {
    [SerializeField]
    private int orderCircuitType;

    public void activateBoxCircuitConcrete() {
        combatManager.CM.CUM.CCS.activateBoxCircuitConcrete(orderCircuitType);
    }
}
