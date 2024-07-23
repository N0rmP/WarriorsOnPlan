using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class uiBubbleTool : MonoBehaviour {
    public static int countTotalBubbles { get; private set; }

    private int indexThis;

    public void Awake() {
        indexThis = countTotalBubbles++;
    }

    public static void resetCount() {
        countTotalBubbles = 0;
    }

    public void setImage(string parToolName) {
        transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Image_" + parToolName);

        if (parToolName == null) {
            gameObject.SetActive(false);
        }
    }

    public void setImage(Sprite parSprite) {
        transform.GetChild(0).GetComponent<Image>().sprite = parSprite;

        if (parSprite == null) {
            gameObject.SetActive(false);
        }
    }

    public void OnMouseDown() {
        //������Ʈ�� ToolStorage���� �и��ϰ�, ���콺 Ŀ���� ����ٴϵ��� ����
    }

    public void OnMouseUp() {
        //������Ʈ�� ����ũ Ŀ������ �и��ϰ�, ���� ���콺 Ŀ�� ��ġ�� ���� Thing���� tool�� �ο��ϰų� ToolStorage�� ���ͽ�Ŵ
        //Thing���� tool�� �ο��� ���, ToolStorage�� ����� �־� ���Ž�Ű��
        //ToolStorage�� ������ ������ indexThis Ȱ���ϱ�
    }
}
