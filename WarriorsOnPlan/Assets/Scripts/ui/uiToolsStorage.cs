using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiToolsStorage : MonoBehaviour
{
    private float widthBubble;

    private GameObject prefabBubble;

    public GameObject contentviewToolStorage;

    private List<uiBubbleTool> listBubble;

    public void Awake() {
        prefabBubble = Resources.Load<GameObject>("Prefabs/UI/bubbleTool");
        widthBubble = prefabBubble.GetComponent<RectTransform>().sizeDelta.x;
    }

    public void prepareBubbles(List<caseBase> parToolArray) {
        int tempIndex = 0;
        GameObject tempBubble;

        uiBubbleTool.resetCount();

        foreach (caseBase CB in parToolArray) {
            // instantiate
            tempBubble = Instantiate(prefabBubble);
            tempBubble.transform.SetParent(contentviewToolStorage.transform);

            // prepare image
            tempBubble.GetComponent<uiBubbleTool>().setImage(CB.GetType().Name);

            // set position
            tempBubble.GetComponent<RectTransform>().anchoredPosition = new Vector2(widthBubble * (0.5f + tempIndex), 0f);

            tempIndex++;
        }
    }

    public void walkOneStepForward(int parRidIndex) {
        int tempBubbleCount = uiBubbleTool.countTotalBubbles;
        while (parRidIndex < tempBubbleCount) {
            listBubble[parRidIndex].setImage(
                listBubble[parRidIndex + 1].transform.GetChild(0).GetComponent<Image>().sprite
                );
        }

        listBubble[parRidIndex].setImage(parSprite: null);
    }
}
