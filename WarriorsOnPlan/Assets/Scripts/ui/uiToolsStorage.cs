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
        Debug.Log("prefab test : " + prefabBubble);
        widthBubble = prefabBubble.GetComponent<RectTransform>().sizeDelta.x;
    }

    public void prepareBubbles(List<caseBase> parToolArray) {
        int tempIndex = 0;
        GameObject tempBubble;

        foreach (caseBase CB in parToolArray) {
            //★ tool 이미지 가져와서 겹치기

            // instantiate
            tempBubble = Instantiate(prefabBubble);
            tempBubble.transform.SetParent(contentviewToolStorage.transform);

            // prepare image
            tempBubble.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Image_" + CB.GetType().Name);

            // set position
            tempBubble.GetComponent<RectTransform>().anchoredPosition = new Vector2(widthBubble * (0.5f + tempIndex), 0f);

            tempIndex++;
        }
    }

    public void walkOneStepForward(int parRidIndex) {
        여기
        // parRidIndex에 해당하는 bubble의 tool이 부여되는 등의 이유로 제거되면 호출됨, 해당 인덱스 뒤의 bubble들을 한 단계씩 앞당겨 갱신하기
    }
}
