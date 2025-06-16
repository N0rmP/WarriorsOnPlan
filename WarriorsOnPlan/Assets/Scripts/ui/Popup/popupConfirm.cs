using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class popupConfirm : MonoBehaviour {
    private Action delWhenYes = null;
    private Action delWhenNo = null;

    public void init(string parQuestion) {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = parQuestion;

        transform.GetChild(0).GetComponent<ContentSizeFitter>().SetLayoutVertical();
        transform.GetChild(0).GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
        float tempQuestionHeight = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
        GetComponent<RectTransform>().sizeDelta = new Vector2(480f, 240f + Mathf.Max(0f, transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.y - 100f));

        gameManager.GM.PC.setCurtainBeneathCanvasPopup(true);

        gameManager.GM.BIC.addKeyActionPair(KeyCode.Space, executeDelWhenYes);
        gameManager.GM.BIC.addKeyActionPair(KeyCode.Return, executeDelWhenYes);
    }

    private void doWhenConfirmComplete() {
        delWhenYes = null;
        delWhenNo = null;

        gameManager.GM.BIC.removeKeyAction(KeyCode.Space, executeDelWhenYes);
        gameManager.GM.BIC.removeKeyAction(KeyCode.Return, executeDelWhenYes);

        gameManager.GM.PC.returnConfirmSingle(this);
        gameManager.GM.PC.setCurtainBeneathCanvasPopup(false);
    }

    #region setDel
    public void setDelWhenYes(Action parAction) {
        delWhenYes = parAction;
    }

    public void setDelWhenNo(Action parAction) {
        delWhenNo = parAction;
    }
    #endregion setDel

    #region execute
    public void executeDelWhenYes() {
        if (delWhenYes != null) {
            delWhenYes();
        }

        doWhenConfirmComplete();
    }

    public void executeDelWhenNo() {
        if (delWhenNo != null) {
            delWhenNo();
        }

        doWhenConfirmComplete();
    }
    #endregion execute
}
