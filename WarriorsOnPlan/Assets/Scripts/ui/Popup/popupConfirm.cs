using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class popupConfirm : MonoBehaviour {
    private static inputContainer temporayInputContainer;

    private Action delWhenYes = null;
    private Action delWhenNo = null;

    public void Awake() {
        temporayInputContainer = new inputContainer();
        temporayInputContainer.addKeyActionPair(KeyCode.Z, executeDelWhenYes);
        temporayInputContainer.addKeyActionPair(KeyCode.X, executeDelWhenNo);
    }

    public void init(string parQuestion) {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = parQuestion;

        transform.GetChild(0).GetComponent<ContentSizeFitter>().SetLayoutVertical();
        transform.GetChild(0).GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
        float tempQuestionHeight = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
        GetComponent<RectTransform>().sizeDelta = new Vector2(480f, 240f + Mathf.Max(0f, transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.y - 100f));

        gameManager.GM.UC.setCurtainPopup(true);
        gameManager.GM.IC.inaugurateTemporayInputContinaer(temporayInputContainer);
    }

    private void doWhenConfirmComplete() {
        delWhenYes = null;
        delWhenNo = null;

        gameManager.GM.PC.returnConfirmSingle(this);
        gameManager.GM.UC.setCurtainPopup(false);
        gameManager.GM.IC.dismissTemporayInputContinaer();
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
