using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class uiBubbleTool : MonoBehaviour
{
    public static int totalBubbles = 0;

    private int indexThis;

    public caseBase toolStored;

    public void Awake() {
        indexThis = totalBubbles++;
    }

    public void setImage(string parToolName) {
        transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Image_weaponTester.png");
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
