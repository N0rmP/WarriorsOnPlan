using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class uiBubbleTool : MonoBehaviour
{
    public caseBase toolStored;

    public void setImage(string parToolName) {
        transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Image_weaponTester.png");
    }

    public void OnMouseDown() {
        //오브젝트를 ToolStorage에서 분리하고, 마우스 커서를 따라다니도록 만듬
    }

    public void OnMouseUp() {
        //오브젝트를 마우크 커서에서 분리하고, 현재 마우스 커서 위치에 따라 Thing에게 tool을 부여하거나 ToolStorage로 복귀시킴
    }
}
