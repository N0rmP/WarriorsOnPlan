using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Cases;

public class toolStorage : MonoBehaviour {
    private Transform transformContent;

    private carrierGeneric<dragableBubbleStorage> carrierBubble;

    public void Awake() {
        transformContent = transform.GetChild(0).GetChild(0);
        carrierBubble = new carrierGeneric<dragableBubbleStorage>(
            () => {
                return Instantiate(combatManager.CM.CUM.prefabBubble).AddComponent<dragableBubbleStorage>();
            },
            (x) => {
                x.gameObject.SetActive(false);
            }
            );
    }

    #region bubble
    public void updateBubbles(caseBase[] parToolArray) {
        clearBubble();
        foreach (caseBase CB in parToolArray) {
            addBubble(CB);
        }
    }

    public void addBubble(caseBase parTool) {
        // get bubble through carrierBubble
        dragableBubbleStorage tempBubble = carrierBubble.getInterceptor();
        tempBubble.transform.SetParent(transformContent);
        tempBubble.transform.localScale = Vector3.one;
        tempBubble.transform.SetAsLastSibling();
        tempBubble.gameObject.SetActive(true);

        // set Tool
        tempBubble.GetComponent<dragableBubbleStorage>().thisTool = parTool;
    }

    public void removeBubble(dragableBubbleStorage parBubble) {
        carrierBubble.returnSingle(parBubble);
    }

    public void clearBubble() {
        dragableBubbleStorage tempDBS;
        // remove all bubbles from content-transform
        foreach (Transform tr in transform.GetChild(0).GetChild(0)) {
            if (tr.TryGetComponent<dragableBubbleStorage>(out tempDBS)) {
                removeBubble(tempDBS);
            }            
        }
    }
    #endregion bubble
}
