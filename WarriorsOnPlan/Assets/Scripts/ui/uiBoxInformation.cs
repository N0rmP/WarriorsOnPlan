using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class uiBoxInformation : MonoBehaviour
{
    private GameObject canvasStatus { get { return transform.GetChild(0).gameObject; } }
    private GameObject canvasStatistics { get { return transform.GetChild(1).gameObject; } }
    private GameObject canvasCircuitSetter { get { return transform.GetChild(2).gameObject; } }

    private Thing curThingShown = null;

    public void showStatus(Thing parThingShown) {
        if (parThingShown == null) {
            Debug.Log("par thing null in uiBoxInformation.cs");
            return;
        }

        curThingShown = parThingShown;
        updateStatus();

        // change the panel, show the status panel
    }

    public void updateStatus() {
        // update parThingShown's status

        canvasStatus.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = curThingShown.ToString();
    }

    public void clickSetCircuit() {
        canvasCircuitSetter.GetComponent<uiCanvasCircuitSetter>().activateSetter(curThingShown);
    }
}
