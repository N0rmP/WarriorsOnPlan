using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class uiMoreBoxCircuitConcrete : uiActivatable, IUIDeactivate {
    private GameObject prefabButtonCircuitConcrete;

    private List<GameObject> listButtonCircuitConcrete;

    public new void Awake() {
        base.Awake();
        prefabButtonCircuitConcrete = Resources.Load<GameObject>("Prefab/UI/buttonCircuitConcrete");

        listButtonCircuitConcrete = new List<GameObject>();
    }

    public void activateBoxCircuitConcrete(int parCurCircuitTypeBeingChosen) {
        GameObject makeNewButtonCircuitConcrete(int parIndex) {
            buttonCircuitConcrete tempButton = Instantiate(prefabButtonCircuitConcrete).GetComponent<buttonCircuitConcrete>();
            tempButton.orderCircuitConcrete = parIndex;
            return tempButton.gameObject;
        }

        // prepare circuit concrete buttons
        GameObject tempObject;
        int tempButtonIndex = 0;
        // write descriptions of circuits
        foreach (codableObject co in gameManager.GM.MC.iterateAdequateSet(canvasCircuitSetter.convertNumToCode(parCurCircuitTypeBeingChosen))) {
            if (co is not IInfo) {
                continue;
            }

            // get valid button circuit concrete, instantiate new one if there is no enough button
            if (listButtonCircuitConcrete.Count <= tempButtonIndex) {
                tempObject = makeNewButtonCircuitConcrete(tempButtonIndex);
                listButtonCircuitConcrete.Add(tempObject);
                tempObject.transform.SetParent(transform);
            } else {
                tempObject = listButtonCircuitConcrete[tempButtonIndex];
            }

            tempObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (co as IInfo).infoDescription;
            tempObject.SetActive(true);

            if (++tempButtonIndex >= 8) {
                break;
            }

            if (tempButtonIndex > 100) {
                Debug.Log("activate BoxCircuitConcrete error : listButtonCircuitConcrete updating exceeds 100 now");
                break;
            }
        }

        // deactivate buttons out of range of circuits
        for (; tempButtonIndex < listButtonCircuitConcrete.Count; tempButtonIndex++) {
            listButtonCircuitConcrete[tempButtonIndex].SetActive(false);
        }

        activatePanel(new Vector3(0f, 360f, 0f));
    }

    public void doWhenUIDeactivate() {
        combatUIManager.CUM.CCS.reactivateButtonCircuitType();
    }
}
