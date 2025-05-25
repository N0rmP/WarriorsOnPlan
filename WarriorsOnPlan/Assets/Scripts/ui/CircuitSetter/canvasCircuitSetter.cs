using System;
using System.Collections;
using System.Collections.Generic;
//using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

using Circuits;

public class canvasCircuitSetter : MonoBehaviour {
    private GameObject panelChoice;
    private GameObject[] arrButtonCircuitType;

    #region CircuitLists
    /*
    // because all these circuits should be used only for getting information in this class, creator parameters don't matter
    private static List<ISingleInfo> listSelecters = new List<ISingleInfo> { 
            new selecterClosest(enumSide.none, 000)
        };
    private static List<ISingleInfo> listSensors = new List<ISingleInfo> {
            new sensorNothing(),
            new sensorHpBelow(0, 0)
        };
    private static List<ISingleInfo> listNavigators = new List<ISingleInfo> { 
            new navigatorStationary(),
            new navigatorAttackOneWeapon()
        };
    */
    #endregion CircuitLists

    private string wordNUMBER;

    private int[] arrCircuitCodes;
    private LinkedList<GameObject>[] arrParameters;

    private static Thing curThingBeingSet;
    private static int curCircuitTypeBeingChosen_ = -1;
    public static int curCircuitTypeBeingChosen {
        get{
            return curCircuitTypeBeingChosen_;
        }
        private set {
            curCircuitTypeBeingChosen_ = (value >= 0 && value <= 7) ? value : -1;
        }
    }

    private carrierGeneric<GameObject> carrierInputfield;

    private List<GameObject> listButtonChoice;

    //�� public TextMeshProUGUI tempTMProU;
    public void Start() {
        panelChoice = transform.GetChild(1).gameObject;
        panelChoice.SetActive(false);

        arrButtonCircuitType = new GameObject[6];
        int tempInd = 0;
        Button tempButton;
        foreach (Transform t in transform.GetChild(0)) {
            if (t.TryGetComponent<Button>(out tempButton)) {
                arrButtonCircuitType[tempInd++] = t.gameObject;
                if (tempInd == arrButtonCircuitType.Length) {
                    break;
                }
            }
        }

        listButtonChoice = new List<GameObject>();
        foreach (Transform trn in panelChoice.transform) {
            listButtonChoice.Add(trn.gameObject);
        }

        wordNUMBER = gameManager.GM.option.curTranslation switch {
            enumTranslation.english => "(Number)",
            enumTranslation.korean => "(숫자)",
            _ => ""
        };

        arrCircuitCodes = new int[6];
        arrParameters = new LinkedList<GameObject>[6];
        for (int i = 0; i < arrParameters.Length; i++) {
            arrParameters[i] = new LinkedList<GameObject>();
        }

        carrierInputfield = new carrierGeneric<GameObject>(
            () => {
                GameObject tempResult = (GameObject)Instantiate(Resources.Load("Prefabs/UI/TMP_Inputfield"));
                tempResult.GetComponent<TMP_InputField>().text = "0";
                RectTransform tempRect = tempResult.GetComponent<RectTransform>();
                tempRect.anchorMin = new Vector2(0f, 1f);
                tempRect.anchorMax = new Vector2(0f, 1f);
                tempRect.pivot = new Vector2(0f, 1f);
                // ★ set size of inpufield
                return tempResult;
            },
            (item) => {
                item.GetComponent<RectTransform>().position = new Vector3(-9999f, -9999f, -9999f);
            }
        );

        GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
        gameObject.SetActive(false);
    }

    /*
    private List<ISingleInfo> getListCircuitType(int parCode) {
        return parCode switch {
            0 or 3 => listSensors,
            1 or 2 => listNavigators,
            4 or 5 => listSelecters,
            _ => null
        };
    }
    */

    private int convertNumToCode(int parCircuitTypeBeingChosen, int parCircuitChosen = 1) {
        return parCircuitTypeBeingChosen switch {
            0 or 3 => 1100,
            1 or 2 => 1200,
            4 or 5 => 1300,
            _ => 91100
        } + parCircuitChosen;
    }

    public void activateSetter(Thing source) {
        curThingBeingSet = source;

        setInputfieldTotal(source);

        GetComponent<uiBasic>().activatePanel();
    }

    public void activateChoicePanel(int parCurCircuitTypeBeingChosen) {
        curCircuitTypeBeingChosen = parCurCircuitTypeBeingChosen;

        // deactivate circuit type choice buttons        
        for (int i = 0; i < arrButtonCircuitType.Count(); i++) {
            if (i != curCircuitTypeBeingChosen_) {
                arrButtonCircuitType[i].GetComponent<Button>().interactable = false;
            }
        }

        // prepare choice buttons
        Transform tempTransform;
        int tempButtonIndex = 0;
        // write descriptions of circuits
        foreach (codableObject co in gameManager.GM.MC.iterateAdequateSet(convertNumToCode(parCurCircuitTypeBeingChosen))) {
            if (co is not ISingleInfo) {
                continue;
            }

            tempTransform = panelChoice.transform.GetChild(tempButtonIndex);
            tempTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (co as ISingleInfo).singleInfo;
            tempTransform.gameObject.SetActive(true);

            if (++tempButtonIndex >= 8) {
                break;
            }
        }
        // deactivate buttons out of range of circuits
        for (; tempButtonIndex < 8; tempButtonIndex++) {
            panelChoice.GetComponent<uiBasic>().activatePanel();
        }
    }

    public void chooseCircuit(int parCircuitChosen) {
        arrCircuitCodes[curCircuitTypeBeingChosen] = parCircuitChosen;

        arrButtonCircuitType[curCircuitTypeBeingChosen].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            gameManager.GM.MC.sneakISingleInfo(convertNumToCode(curCircuitTypeBeingChosen, parCircuitChosen)).singleInfo;
        setInputfieldSingle(curCircuitTypeBeingChosen);
        panelChoice.GetComponent<uiBasic>().deactivatePanel();

        foreach (GameObject obj in arrButtonCircuitType) {
            obj.GetComponent<Button>().interactable = true;
        }
    }

    public void confirm() {
        //★ arrParameters 취합하여 만들기

        /*
        curThingBeingSet.setCircuit(
            arrCircuitCodes[0], arrParameters[0],
            arrCircuitCodes[1], arrParameters[1],
            arrCircuitCodes[2], arrParameters[2],
            arrCircuitCodes[3], arrParameters[3],
            arrCircuitCodes[4], arrParameters[4],
            arrCircuitCodes[5], arrParameters[5]
            );
        */

        gameObject.GetComponent<uiBasic>().deactivatePanel();
    }

    #region utility
    private void setInputfieldSingle(int parCircuitTypeNum) {
        if (parCircuitTypeNum < 0 || parCircuitTypeNum >= arrParameters.Length) {
            return;
        }

        // before placing inputfields, get rid of former inputfields
        foreach (GameObject nod in arrParameters[parCircuitTypeNum]) {
            carrierInputfield.returnSingle(nod);
        }
        arrParameters[parCircuitTypeNum].Clear();

        TextMeshProUGUI tempTMPro = arrButtonCircuitType[parCircuitTypeNum].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TMP_CharacterInfo[] tempCInfo = tempTMPro.GetTextInfo(tempTMPro.text).characterInfo;

        GameObject tempObj;
        foreach (int ind in tempTMPro.text.AllIndexesOf(wordNUMBER)) {
            tempObj = carrierInputfield.getInterceptor();
            tempObj.transform.SetParent(tempTMPro.transform);
            tempObj.GetComponent<RectTransform>().anchoredPosition = tempCInfo[ind].topLeft + new Vector3(0f, 1f, 0f);
            arrParameters[parCircuitTypeNum].AddLast(tempObj);
        }
    }

    // setInputfieldSingle not just place inputfield, but also initiate its values to the source's circuit info
    private void setInputfieldSingle(int parCircuitTypeNum, int[] parParameters) {
        setInputfieldSingle(parCircuitTypeNum);

        // if parParameters' indice are not enought to fill the inputfields, skip and return early 
        if ((parParameters == null) || (arrParameters[parCircuitTypeNum].Count > parParameters.Length)) {
            return;    
        }

        int tempIndex = 0;
        foreach (GameObject obj in arrParameters[parCircuitTypeNum]) {
            obj.GetComponent<TMP_InputField>().text = parParameters[tempIndex++].ToString();
        }
    }

    private void setInputfieldTotal(Thing source) {
        string[] tempStringArray = source.getCircuitInfo();
        string tempString;
        int[] tempParameters;
        int tempIndex0; int tempIndex1;
        for (int i = 0; i < arrButtonCircuitType.Length; i++) {
            // if singleinfo contains wordNumber, fill it by getting the parameters from circuit
            // during combat string is used to pervent changing circuit, during preparing inputfield is used
            tempString = tempStringArray[i];
            if (tempString.Contains(wordNUMBER)) {
                tempIndex1 = 0;
                tempParameters = source.getCircuitParameters(i);

                if (combatManager.CM.combatState != enumCombatState.preparing) {
                    // if wordNUMBER is contained and it's not during preparing, replace all wordNUMBER with string
                    while (tempString.Contains(wordNUMBER)) {
                        tempIndex0 = tempString.IndexOf(wordNUMBER);
                        // replacing isn't by Replace method, because more than one wordNUMBER can have varying numbers
                        tempString.Remove(tempIndex0, wordNUMBER.Length);
                        tempString.Insert(tempIndex0, tempParameters[tempIndex1++].ToString());
                    }
                } else {
                    // if wordNUMBER is contained and it's during preparing, replace all wordNUMBER with inputfields
                    arrButtonCircuitType[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tempString;
                    setInputfieldSingle(i, source.getCircuitParameters(i));
                }
            } else {
                // if wordNUMBER is not contained, just set text
                arrButtonCircuitType[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tempString;
            }
        }

        for (int i = 0; i < 8; i++) {
            setInputfieldSingle(i);
        }
    }
    #endregion utility
}
