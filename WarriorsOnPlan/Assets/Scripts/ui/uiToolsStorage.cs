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
            //�� tool �̹��� �����ͼ� ��ġ��

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
        ����
        // parRidIndex�� �ش��ϴ� bubble�� tool�� �ο��Ǵ� ���� ������ ���ŵǸ� ȣ���, �ش� �ε��� ���� bubble���� �� �ܰ辿 �մ�� �����ϱ�
    }
}
