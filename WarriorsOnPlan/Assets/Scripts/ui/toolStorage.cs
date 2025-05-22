using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.UI;

using Cases;

public class toolStorage : MonoBehaviour
{
    private Transform transformContent;

    private carrierGeneric<GameObject> carrierBubble;

    //private List<uiBubbleTool> listBubble;

    public void Awake() {
        carrierBubble = new carrierGeneric<GameObject>(
            () => {
                GameObject tempResult = Instantiate(combatUIManager.CUM.prefabBubble);
                tempResult.AddComponent<dragableBubbleStorage>();
                return tempResult;
                },
            (x) => {
                x.transform.SetParent(null);
            }
            );

        transformContent = transform.GetChild(0).GetChild(0);
    }

    public void prepareBubbles(caseBase[] parToolArray) {
        foreach (caseBase CB in parToolArray) {
            addBubble(CB);
        }
    }

    public void addBubble(caseBase parTool) {
        // get bubble through carrierBubble
        GameObject tempBubble = carrierBubble.getInterceptor();
        tempBubble.transform.SetParent(transformContent.transform);

        // set Tool
        tempBubble.GetComponent<dragableBubbleStorage>().thisTool = parTool;
    }

    public void retrieveBubble(GameObject parBubble) {
        parBubble.transform.SetParent(transformContent.transform);
    }

    public void removeBubble(GameObject parBubble) {
        // return bubble to carrierBubble
        carrierBubble.returnSingle(parBubble);
    }
}
