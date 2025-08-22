using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;
using System.Xml.Xsl;

// all popup methods work with LocalPosition on full-screen-canvas
public class popupComponent {
    private const int defaultFontSize = 24;

    private carrierGeneric<GameObject> carrierPopupText;
    private carrierGeneric<GameObject> carrierPopupConfirm;
    private carrierGeneric<GameObject> carrierPopupFloating;
    private carrierGeneric<GameObject> carrierPopupCaseBase;

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
                    x.GetComponent<RectTransform>().position = new Vector2(3000f, 3000f);
                }
            );
        }
        carrierPopupText = makeCarrier("boxPopupText");
        carrierPopupConfirm = makeCarrier("boxPopupConfirm");
        carrierPopupFloating = makeCarrier("boxPopupFloating");

        // carrierPopupCaseBase doesn't use unique prefab, it creates its intercepter with imgRoundRectangle
        GameObject tempIRR = Resources.Load<GameObject>("Prefab/UI/imgRoundRectangle");
        carrierPopupCaseBase = new carrierGeneric<GameObject>(
            () => {
                GameObject tempResult = GameObject.Instantiate(tempIRR);
                gameManager.GM.UC.setParentPopup(tempResult.transform);
                (tempResult.transform as RectTransform).anchorMin = new Vector2(0f, 0f);
                (tempResult.transform as RectTransform).anchorMax = new Vector2(0f, 0f);
                (tempResult.transform as RectTransform).sizeDelta = new Vector2(50f, 50f);
                tempResult.AddComponent<popupCaseBase>();
                tempResult.AddComponent<uiMovable>().thisEnumHowToMove = enumHowToMove.steady;
                return tempResult;
            },
            (x) => {
                (x.transform as RectTransform).position = new Vector2(3000f, 3000f);
                x.GetComponent<popupCaseBase>().appear();
            }
        );
    }    

    #region show
    public void showPopupText(Vector3 parPosition, Color parTextColor, Color parBackgroundColor, string parString, int parFontSize = defaultFontSize, float parDuration = -1f) {
        GameObject tempPopupText = carrierPopupText.getInterceptor();
        tempPopupText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = parFontSize;
        tempPopupText.GetComponent<popupText>().setPopupText(parTextColor, parBackgroundColor, parString, parDuration);
        tempPopupText.GetComponent<RectTransform>().localPosition = parPosition;
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


    public void showPopupFloating(Vector3 parPosition, Color parTextColor, Color parBackgroundColor, string parString, float parDuration = -1f) {
        GameObject tempPopupText = carrierPopupFloating.getInterceptor();
        tempPopupText.GetComponent<popupOutline>().setPopupOutline(parTextColor, parBackgroundColor, parString, parDuration);
        tempPopupText.GetComponent<RectTransform>().localPosition = parPosition;
    }

    public void showPopupCaseBase(Vector3 parPosition, Vector3 parDestination, Sprite parSpriteCaseBase, bool parIsDestinationScreenVector = true, float parWatingBeforeMove = 0f, float parDuration = -1f) {
        GameObject tempPopupCaseBase = carrierPopupCaseBase.getInterceptor();
        tempPopupCaseBase.GetComponent<imgRoundRectangle>().setImg(parSpriteCaseBase);
        tempPopupCaseBase.GetComponent<popupCaseBase>().setPopup(parDuration);
        tempPopupCaseBase.GetComponent<RectTransform>().localPosition = parPosition;

        // set move
            // convert parDestination's anchoredPosition to localPosition (uiMovable works with localPosition)
        parDestination -= new Vector3(
            Screen.width * 0.5f,
            Screen.height * 0.5f,
            0f
        );
        if (parWatingBeforeMove <= 0f) {
            tempPopupCaseBase.GetComponent<uiMovable>().setMove(parDestination, 4f);
        } else {
            gameManager.GM.TC.addDelegate(
                () => {
                    tempPopupCaseBase.GetComponent<uiMovable>().setMove(parDestination, 4f);
                },
                parWatingBeforeMove
            );
        }
    }
    #endregion show

    #region Ready_to_Popup
    public void popupBasicAlert(Vector3 parPosition, string parString, int parFontSize = defaultFontSize, float parDuration = -1f) {
        showPopupText(parPosition, Color.white, new Color(0f, 0f, 0f, 0.7f), parString, parFontSize, parDuration);
    }

    public void popupDamage(Vector3 parPosition, string parString) {
        showPopupFloating(parPosition, Color.red, new Color(0f, 0f, 0f, 0.1f), parString, structInterValsAndDurations.fltInterval / combatManager.CM.combatSpeed);
    }

    public void popupDamageMagic(Vector3 parPosition, string parString) {
        showPopupFloating(parPosition, Color.white, new Color(0f, 0f, 0f, 0.1f), parString, structInterValsAndDurations.fltInterval / combatManager.CM.combatSpeed);
    }

    public void popupHeal(Vector3 parPosition, string parString) {
        showPopupFloating(parPosition, Color.green, new Color(0f, 0f, 0f, 0.1f), parString, structInterValsAndDurations.fltInterval / combatManager.CM.combatSpeed);
    }

    public void popupAddCaseBase(Vector3 parPosition, Sprite parSpriteCaseBase) {
        float tempRandomRadian = UnityEngine.Random.Range(0f, 6.28f);
        Vector3 tempRandomRadius = gameManager.GM.option.stick * 1.5f * new Vector2(Mathf.Cos(tempRandomRadian), Mathf.Sin(tempRandomRadian));
        
        showPopupCaseBase(
            parPosition + tempRandomRadius,
            parPosition, 
            parSpriteCaseBase,
            parWatingBeforeMove: 0.5f, 
            parDuration: 2f
        );
    }

    public void popupRemoveCaseBase(Vector3 parPosition, Sprite parSpriteCaseBase) {
        float tempRandomRadian = UnityEngine.Random.Range(0f, 6.28f);
        Vector3 tempRandomRadius = gameManager.GM.option.stick * 1.5f * new Vector2(Mathf.Cos(tempRandomRadian), Mathf.Sin(tempRandomRadian));

        showPopupCaseBase(
            parPosition, 
            parPosition + tempRandomRadius, 
            parSpriteCaseBase,
            parWatingBeforeMove: 0.5f, 
            parDuration: 2f
        );
    }
    #endregion Ready_to_Popup

    #region carrier_control
    public void returnTextSingle(popupText parPopupText) {
        carrierPopupText.returnSingle(parPopupText.gameObject);
    }

    public void returnConfirmSingle(popupConfirm parPopupConfirm) {
        carrierPopupConfirm.returnSingle(parPopupConfirm.gameObject);
    }

    public void returnFloatingSingle(popupOutline parPopupFloating) {
        carrierPopupFloating.returnSingle(parPopupFloating.gameObject);
    }

    public void returnCaseBaseSingle(popupCaseBase parPopupCaseBase) {
        carrierPopupCaseBase.returnSingle(parPopupCaseBase.gameObject);
    }

    public void returnTotal() {
        carrierPopupText.returnTotal();
        carrierPopupConfirm.returnTotal();
        carrierPopupFloating.returnTotal();
        carrierPopupCaseBase.returnTotal();
    }
    #endregion carrier_control
}
