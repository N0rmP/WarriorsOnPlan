using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;

public class popupComponent {
    private GameObject canvasPopup;
    private Image curtainBeneathCanvasPopup;

    private carrierGeneric<GameObject> carrierPopupText;
    private carrierGeneric<GameObject> carrierPopupConfirm;
    private carrierGeneric<GameObject> carrierPopupFloating;

    private GameObject popupConfirm;

    public popupComponent() {
        canvasPopup = new GameObject("canvasPopup");
        GameObject.DontDestroyOnLoad(canvasPopup);
        canvasPopup.AddComponent<Canvas>();
        canvasPopup.AddComponent<CanvasScaler>();  // CanvasScaler is for the case when canvasPopup becomes CanvasMain
        canvasPopup.AddComponent<GraphicRaycaster>();
        curtainBeneathCanvasPopup = canvasPopup.AddComponent<Image>();
        curtainBeneathCanvasPopup.sprite = Resources.Load<Sprite>("Image/tempFrame");
        curtainBeneathCanvasPopup.color -= new Color(0f, 0f, 0f, 1f);
        curtainBeneathCanvasPopup.enabled = false;

        // carrier creation
        carrierGeneric<GameObject> makeCarrier(string parPrefabName) {
            return new carrierGeneric<GameObject>(
                () => {
                    GameObject tempObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/UI/Popup/" + parPrefabName));
                    tempObject.transform.SetParent(canvasPopup.transform);
                    return tempObject;
                },
                (x) => {
                    x.GetComponent<RectTransform>().position = new Vector2(9999f, 9999f);
                }
            );
        }
        carrierPopupText = makeCarrier("boxPopupText");
        carrierPopupConfirm = makeCarrier("boxPopupConfirm");
        carrierPopupFloating = makeCarrier("boxPopupFloating");

        // gameManager.GM.doWhenSceneLoaded += prepareCanvasPopup;
        SceneManager.sceneLoaded += prepareCanvasPopup;
    }

    public void prepareCanvasPopup(Scene parScene, LoadSceneMode parLoadSceneMode) {
        if (canvasPopup.GetComponent<Canvas>() == gameManager.GM.canvasMain) {
            canvasPopup.GetComponent<RectTransform>().sizeDelta = new Vector2(gameManager.GM.option.screenWidth, gameManager.GM.option.screenHeight);
            return;
        }

        carrierPopupText.returnTotal();
        canvasPopup.transform.SetParent(gameManager.GM.canvasMain.transform);
        RectTransform tempRecttransform = canvasPopup.GetComponent<RectTransform>();
        tempRecttransform.anchorMin = new Vector2(0f, 0f);
        tempRecttransform.anchorMax = new Vector2(1f, 1f);
        tempRecttransform.offsetMin = new Vector2(0f, 0f);
        tempRecttransform.offsetMax = new Vector2(1f, 1f);
    }

    public void setCurtainBeneathCanvasPopup(bool parIsEnabled) {
        curtainBeneathCanvasPopup.enabled = parIsEnabled;
    }

    #region show
    public void showPopupText(Vector3 parPosition, Color parTextColor, Color parBackgroundColor, string parString, bool parIsScreenVector = true, float parDuration = -1f) {
        // convert world-position to screen-position
        if (!parIsScreenVector) {
            parPosition = Camera.main.WorldToScreenPoint(parPosition);
        }

        GameObject tempTextPopup = carrierPopupText.getInterceptor();
        tempTextPopup.GetComponent<popupText>().setPopupText(parTextColor, parBackgroundColor, parString, parDuration);
        tempTextPopup.GetComponent<RectTransform>().anchoredPosition = parPosition;
    }

    public void showPopupConfirm(Vector3 parScreenPosition, string parQuestion = "you forgot to set question, dumbass", Action parDelWhenYes = null, Action parDelWhenNo = null) {
        setCurtainBeneathCanvasPopup(true);

        popupConfirm tempPopupConfirm = carrierPopupConfirm.getInterceptor().GetComponent<popupConfirm>();
        tempPopupConfirm.setDelWhenYes(parDelWhenYes);
        tempPopupConfirm.setDelWhenNo(parDelWhenNo);
        tempPopupConfirm.init(parQuestion);
        tempPopupConfirm.GetComponent<RectTransform>().localPosition = parScreenPosition;
    }

    
    public void showPopupFloating(Vector3 parPosition, Color parTextColor, Color parBackgroundColor, string parString, bool parIsScreenVector = true, float parDuration = -1f) {
        // convert world-position to screen-position
        if (!parIsScreenVector) {
            parPosition = Camera.main.WorldToScreenPoint(parPosition);
        }

        GameObject tempTextPopup = carrierPopupFloating.getInterceptor();
        tempTextPopup.GetComponent<popupOutline>().setPopupOutline(parTextColor, parBackgroundColor, parString, parDuration);
        tempTextPopup.GetComponent<RectTransform>().anchoredPosition = parPosition;
    }

    public void showPopupConfirm(string parQuestion = "you forgot to set question, dumbass", Action parDelWhenYes = null, Action parDelWhenNo = null) {
        showPopupConfirm(new Vector3(0f, 0f, 0f), parQuestion, parDelWhenYes, parDelWhenNo);
    }
    #endregion show

    #region Ready_to_PopupText
    public void popupBasicAlert(Vector3 parScreenPosition, string parString, bool parIsScreenVector = true, float parDuration = -1f) {
        showPopupText(parScreenPosition, Color.white, new Color(0f, 0f, 0f, 0.7f), parString, parIsScreenVector, parDuration);
    }

    public void popupDamage(Vector3 parScreenPosition, string parString, bool parIsScreenVector = true) {
        showPopupFloating(parScreenPosition, Color.red, new Color(0f, 0f, 0f, 0.1f), parString, parIsScreenVector, combatManager.fltInterval / combatManager.CM.combatSpeed);
    }

    public void popupDamageMagic(Vector3 parScreenPosition, string parString, bool parIsScreenVector = true) {
        showPopupFloating(parScreenPosition, Color.white, new Color(0f, 0f, 0f, 0.1f), parString, parIsScreenVector, combatManager.fltInterval / combatManager.CM.combatSpeed);
    }

    public void popupHeal(Vector3 parScreenPosition, string parString, bool parIsScreenVector = true) {
        showPopupFloating(parScreenPosition, Color.green, new Color(0f, 0f, 0f, 0.1f), parString, parIsScreenVector, combatManager.fltInterval / combatManager.CM.combatSpeed);
    }
    #endregion Ready_to_Popup

    #region carrier_control
    public void returnTextSingle(popupText parPopupText) {
        carrierPopupText.returnSingle(parPopupText.gameObject);
    }

    public void returnConfirmSingle(popupConfirm parPopupConfirm) {
        carrierPopupConfirm.returnSingle(parPopupConfirm.gameObject);
    }

    public void returnTotal() {
        carrierPopupText.returnTotal();
        carrierPopupConfirm.returnTotal();
    }
    #endregion carrier_control
}
