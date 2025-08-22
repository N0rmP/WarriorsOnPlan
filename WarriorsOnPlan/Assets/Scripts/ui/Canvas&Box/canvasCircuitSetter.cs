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
using UnityEngine.UIElements;
using System.ComponentModel;
using static Unity.VisualScripting.Member;

public class canvasCircuitSetter : MonoBehaviour {
    private GameObject curCanvasCircuitConcrete;
    private GameObject[] arrButtonCircuitType;
    private UnityEngine.UI.Toggle[,] arrToggleTargetGroup;

    private string wordNUMBER;

    [SerializeField]
    private int circuitCount = 6;
    private int[] arrCircuitCodes;
    private List<GameObject>[] arrInputfield;

    private static Thing curThingBeingSet;
    private static int curCircuitTypeBeingChosen_ = -1;
    public static int curCircuitTypeBeingChosen {
        get{
            return curCircuitTypeBeingChosen_;
        }
        private set {
            curCircuitTypeBeingChosen_ = Math.Clamp(value, 0, 6);
        }
    }

    private carrierGeneric<GameObject> carrierInputfield;

    // public TextMeshProUGUI tempTMProU;
    public void Start() {
        curCanvasCircuitConcrete = GameObject.Find("canvasCircuitConcrete");

        arrToggleTargetGroup = new UnityEngine.UI.Toggle[2,4];
        for (int i = 4; i < 6; i++) {
            for (int j = 2; j < 6; j++) {
                arrToggleTargetGroup[i-4, j-2] = transform.GetChild(0).GetChild(i).GetChild(j).GetComponent<UnityEngine.UI.Toggle>();
            }
        }

        // carrier initiating
        GameObject tempInputfield = Resources.Load<GameObject>("Prefab/UI/TMP_Inputfield");
        carrierInputfield = new carrierGeneric<GameObject>(
            () => {
                GameObject tempResult = (GameObject)Instantiate(tempInputfield);
                tempResult.GetComponent<TMP_InputField>().text = "0";
                RectTransform tempRect = tempResult.GetComponent<RectTransform>();
                tempRect.anchorMin = new Vector2(0f, 0.5f);
                tempRect.anchorMax = new Vector2(0f, 0.5f);
                tempRect.pivot = new Vector2(0f, 1f);
                // ★ set size of inpufield
                return tempResult;
            },
            (item) => {
                item.GetComponent<TMP_InputField>().text = "0";
                item.GetComponent<RectTransform>().position = new Vector2(-9999f, -9999f);
            }
        );

        arrButtonCircuitType = new GameObject[6];
        int tempInd = 0;
        for (int i = 0; i < circuitCount; i++) {
            arrButtonCircuitType[tempInd++] = transform.GetChild(0).GetChild(i).GetChild(1).gameObject;
        }

        wordNUMBER = gameManager.GM.DHouC.bookWords.strNumber;

        arrCircuitCodes = new int[circuitCount];
        arrInputfield = new List<GameObject>[circuitCount];
        for (int i = 0; i < circuitCount; i++) {
            arrInputfield[i] = new List<GameObject>();
        }
    }

    public static int convertNumToCode(int parCircuitTypeBeingChosen, int parCircuitChosen = 1) {
        return parCircuitTypeBeingChosen switch {
            0 or 2 => 1200,
            1 or 3 => 1100,
            4 or 5 => 1300,
            _ => 91100
        } + parCircuitChosen;
    }

    #region main_functions
    public void activateSetter(Thing source) {
        carrierInputfield.returnTotal();

        curThingBeingSet = source;

        for (int i = 0; i < circuitCount; i++) {
            arrCircuitCodes[i] = source.getCircuitCode(i);
        }

        prepareCircuitTypeInfoTotal(source);

        bool tempIsInteractableTotal = combatManager.CM.checkControllability(source);
        int tempTargetGroup;
        // initialize skill target-group toggle
        tempTargetGroup = source.getSelecterForSkillTargetGroup();
        for (int j = 0; j < 4; j++) {
            arrToggleTargetGroup[0, j].isOn = tempTargetGroup % 2 == 1;
            arrToggleTargetGroup[0, j].interactable = tempIsInteractableTotal;
            tempTargetGroup = tempTargetGroup >> 1;
        }
        // initialize attack target-group toggle
        tempTargetGroup = source.getSelecterForAttackTargetGroup();
        for (int j = 0; j < 4; j++) {
            arrToggleTargetGroup[1, j].isOn = tempTargetGroup % 2 == 1;
            arrToggleTargetGroup[1, j].interactable = tempIsInteractableTotal;
            tempTargetGroup = tempTargetGroup >> 1;
        }

        foreach (GameObject obj in arrButtonCircuitType) {
            obj.GetComponent<buttonCustom>().interactable = tempIsInteractableTotal;
        }

        GetComponent<uiActivatable>().activatePanel(new Vector3(0f, 0f, 0f));
    }

    public void reactivateButtonCircuitType() {
        foreach (GameObject obj in arrButtonCircuitType) {
            obj.GetComponent<buttonCustom>().interactable = true;
        }
    }

    public void confirm() {
        // inputfield (int value) racking
        int tempInputfieldParameter;
        List<int>[] tempParameterArrArr = new List<int>[circuitCount];
        for (int i = 0; i < circuitCount; i++) {
            tempParameterArrArr[i] = new List<int>();
            for (int j = 0; j < arrInputfield[i].Count; j++) {
                if (Int32.TryParse(arrInputfield[i][j].GetComponent<TMP_InputField>().text, out tempInputfieldParameter)) {
                    tempParameterArrArr[i].Add(tempInputfieldParameter);
                } else {
                    Debug.Log(this + ".confirm results in error with tempParameterArrArr[" + i + "][" + j + "], text was " + arrInputfield[i][j].GetComponent<TMP_InputField>().text);
                    tempParameterArrArr[i].Add(0);
                }
            }
            arrInputfield[i].Clear();
        }

        // toggle (selecter targetGroup) racking
        int tempTargetGroup;
        for (int i = 0; i < 2; i++) {
            tempTargetGroup = 0;
            for (int j = 0; j < 4; j++) {
                tempTargetGroup += (arrToggleTargetGroup[i, j].isOn ? 0b001 : 0b000) << j;
            }
            tempParameterArrArr[i + 4].Insert(0, tempTargetGroup);
        }

        curThingBeingSet.setCircuit(
            arrCircuitCodes[0], tempParameterArrArr[0].ToArray(),
            arrCircuitCodes[1], tempParameterArrArr[1].ToArray(),
            arrCircuitCodes[2], tempParameterArrArr[2].ToArray(),
            arrCircuitCodes[3], tempParameterArrArr[3].ToArray(),
            arrCircuitCodes[4], tempParameterArrArr[4].ToArray(),
            arrCircuitCodes[5], tempParameterArrArr[5].ToArray()
            );

        gameObject.GetComponent<uiActivatable>().deactivatePanel();
    }
    #endregion main_functions

    #region circuit_type
    private void prepareCircuitTypeInfoSingle(Thing source, int parCircuitTypeNum) {
        string tempInfo = source.getCircuitInfo(parCircuitTypeNum);
        int[] tempParameters = source.getCircuitParameter(parCircuitTypeNum);
        if (!combatManager.CM.checkControllability(source)) {
            int tempIndexWordNUMBER;
            int tempIndexParameter = 0;            
            while (tempInfo.Contains(wordNUMBER)) {
                tempIndexWordNUMBER = tempInfo.IndexOf(wordNUMBER);
                // replacing isn't by Replace method, because more than one wordNUMBER can have varying numbers
                tempInfo = tempInfo.Remove(tempIndexWordNUMBER, wordNUMBER.Length).Insert(tempIndexWordNUMBER, tempParameters[tempIndexParameter++].ToString());
            }
        }
        arrButtonCircuitType[parCircuitTypeNum].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tempInfo;

        if (combatManager.CM.checkControllability(source) && tempInfo.Contains(wordNUMBER)) {
            setInputfieldSingle(parCircuitTypeNum, tempParameters);
        }
    }

    private void prepareCircuitTypeInfoTotal(Thing source) {
        for (int i = 0; i < circuitCount; i++) {
            prepareCircuitTypeInfoSingle(source, i);
        }
    }
    #endregion circuit_type

    #region inputfield
    private void setInputfieldSingle(int parCircuitTypeNum) {
        if (parCircuitTypeNum < 0 || parCircuitTypeNum >= arrInputfield.Length) {
            return;
        }

        // before placing inputfields, get rid of former inputfields
        foreach (GameObject nod in arrInputfield[parCircuitTypeNum]) {
            carrierInputfield.returnSingle(nod);
        }
        arrInputfield[parCircuitTypeNum].Clear();

        TextMeshProUGUI tempTMPro = arrButtonCircuitType[parCircuitTypeNum].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TMP_CharacterInfo[] tempCInfo = tempTMPro.GetTextInfo(tempTMPro.text).characterInfo;
        GameObject tempObj;
        foreach (int ind in tempTMPro.text.AllIndexesOf(wordNUMBER)) {
            tempObj = carrierInputfield.getInterceptor();
            tempObj.transform.SetParent(tempTMPro.transform);
            tempObj.GetComponent<TMP_InputField>().text = "0";
            tempObj.GetComponent<RectTransform>().offsetMin = tempCInfo[ind].bottomLeft - new Vector3(3f, 3f, 0f);
            tempObj.GetComponent<RectTransform>().offsetMax = tempCInfo[ind + wordNUMBER.Length - 1].topRight + new Vector3(3f, 3f, 0f);
            arrInputfield[parCircuitTypeNum].Add(tempObj);
        }
    }

    // setInputfieldSingle innitialize its values to the source's circuit info after setting input fields
    private void setInputfieldSingle(int parCircuitTypeNum, int[] parParameters) {
        setInputfieldSingle(parCircuitTypeNum);

        // if parParameters' indice are not enought to fill the inputfields, skip and return early 
        if (parParameters == null) {
            Debug.Log("canvasCircuitSetter.setInputfieldSingle results in error because parParameter is null ( parCircuitNum : " + parCircuitTypeNum + ", input field count : " + arrInputfield[parCircuitTypeNum].Count);
            return;
        } else if (arrInputfield[parCircuitTypeNum].Count > parParameters.Length){
            Debug.Log("canvasCircuitSetter.setInputfieldSingle results in error due to indice lack ( parCircuitTypeNum : " + parCircuitTypeNum + " / parParameters.Length : " + parParameters.Length);
            return;
        }

        int tempIndex = 0;
        foreach (GameObject obj in arrInputfield[parCircuitTypeNum]) {
            obj.GetComponent<TMP_InputField>().text = parParameters[tempIndex++].ToString();
        }
    }
    #endregion inputfield

    #region circuit_concrete
    public void activateBoxCircuitConcrete(int parCurCircuitTypeBeingChosen) {
        curCircuitTypeBeingChosen = parCurCircuitTypeBeingChosen;

        // deactivate circuit type choice buttons        
        for (int i = 0; i < arrButtonCircuitType.Count(); i++) {
            if (i != curCircuitTypeBeingChosen) {
                arrButtonCircuitType[i].GetComponent<buttonCustom>().interactable = false;
            }
        }
        
        curCanvasCircuitConcrete.GetComponent<boxCircuitConcrete>().activateBoxCircuitConcrete(parCurCircuitTypeBeingChosen);
    }

    public void chooseCircuitConcrete(int parCircuitConcreteChosen) {
        arrCircuitCodes[curCircuitTypeBeingChosen] = convertNumToCode(curCircuitTypeBeingChosen, parCircuitConcreteChosen + 1);

        arrButtonCircuitType[curCircuitTypeBeingChosen].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            gameManager.GM.MC.sneakISingleInfo(convertNumToCode(curCircuitTypeBeingChosen, parCircuitConcreteChosen + 1)).infoDescription;
        setInputfieldSingle(curCircuitTypeBeingChosen);
        curCanvasCircuitConcrete.GetComponent<uiActivatable>().deactivatePanel();
    }    
    #endregion circuit_concrete    
}
