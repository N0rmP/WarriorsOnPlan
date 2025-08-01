using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;

public class popupComponent {
    private const int defaultFontSize = 20;

    private carrierGeneric<GameObject> carrierPopupText;
    private carrierGeneric<GameObject> carrierPopupConfirm;
    private carrierGeneric<GameObject> carrierPopupFloating;

    private GameObject popupConfirm;

    public popupComponent() {
        

        // carrier creation
        carrierGeneric<GameObject> makeCarrier(string parPrefabName) {
            return new carrierGeneric<GameObject>(
                () => {
                    GameObject tempObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/UI/Popup/" + parPrefabName));
                    gameManager.GM.UC.setParentPopup(tempObject.transform);
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
    }    

    #region show
    public void showPopupText(Vector3 parPosition, Color parTextColor, Color parBackgroundColor, string parString, bool parIsScreenVector = true, int parFonrSize = defaultFontSize, float parDuration = -1f) {
        // convert world-position to screen-position
        if (!parIsScreenVector) {
            parPosition = Camera.main.WorldToScreenPoint(parPosition);
        }

        GameObject tempTextPopup = carrierPopupText.getInterceptor();
        tempTextPopup.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = parFonrSize;
        tempTextPopup.GetComponent<popupText>().setPopupText(parTextColor, parBackgroundColor, parString, parDuration);
        tempTextPopup.GetComponent<RectTransform>().anchoredPosition = parPosition;
    }

    public void showPopupConfirm(Vector3 parScreenPosition, string parQuestion = "you forgot to set question, dumbass", Action parDelWhenYes = null, Action parDelWhenNo = null) {
        popupConfirm tempPopupConfirm = carrierPopupConfirm.getInterceptor().GetComponent<popupConfirm>();
        tempPopupConfirm.setDelWhenYes(parDelWhenYes);
        tempPopupConfirm.setDelWhenNo(parDelWhenNo);
        tempPopupConfirm.init(parQuestion);
        tempPopupConfirm.GetComponent<RectTransform>().localPosition = parScreenPosition;
    }

    public void showPopupConfirm(string parQuestion = "you forgot to set question, dumbass", Action parDelWhenYes = null, Action parDelWhenNo = null) {
        showPopupConfirm(new Vector3(0f, 0f, 0f), parQuestion, parDelWhenYes, parDelWhenNo);
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
    #endregion show

    #region Ready_to_PopupText
    public void popupBasicAlert(Vector3 parScreenPosition, string parString, bool parIsScreenVector = true, int parFonrSize = defaultFontSize, float parDuration = -1f) {
        showPopupText(parScreenPosition, Color.white, new Color(0f, 0f, 0f, 0.7f), parString, parIsScreenVector, parFonrSize, parDuration);
    }

    public void popupDamage(Vector3 parScreenPosition, string parString, bool parIsScreenVector = true) {
        showPopupFloating(parScreenPosition, Color.red, new Color(0f, 0f, 0f, 0.1f), parString, parIsScreenVector, structInterValsAndDurations.fltInterval / combatManager.CM.combatSpeed);
    }

    public void popupDamageMagic(Vector3 parScreenPosition, string parString, bool parIsScreenVector = true) {
        showPopupFloating(parScreenPosition, Color.white, new Color(0f, 0f, 0f, 0.1f), parString, parIsScreenVector, structInterValsAndDurations.fltInterval / combatManager.CM.combatSpeed);
    }

    public void popupHeal(Vector3 parScreenPosition, string parString, bool parIsScreenVector = true) {
        showPopupFloating(parScreenPosition, Color.green, new Color(0f, 0f, 0f, 0.1f), parString, parIsScreenVector, structInterValsAndDurations.fltInterval / combatManager.CM.combatSpeed);
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
