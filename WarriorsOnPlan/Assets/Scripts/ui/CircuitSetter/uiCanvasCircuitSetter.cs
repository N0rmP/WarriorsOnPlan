using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class uiCanvasCircuitSetter : MonoBehaviour
{
    #region editorRequired
    public GameObject panelChoice;
    public GameObject[] listButtonCircuitType;
    #endregion editorRequired

    #region CircuitLists
    // because all these circuits should be used only for getting information in this class, creator parameters don't matter
    private static List<ISingleInfo> listSelecters = new List<ISingleInfo> { 
            new selecterClosest(null, 000)
        };
    private static List<ISingleInfo> listSensors = new List<ISingleInfo> { 
            new sensorNothing(),
            new sensorHp(0, 0, true)
        };
    private static List<ISingleInfo> listNavigators = new List<ISingleInfo> { 
            new navigatorStationary(),
            new navigatorAttackOneWeapon()
        };
    #endregion CircuitLists

    private string wordNUMBER;

    private int[] arrCircuitCodes = new int[6];
    private int[][] arrParameters = new int[6][];

    private static Thing curThingBeingSet;
    private static int curCircuitBeingChosen_ = -1;
    public static int curCircuitBeingChosen {
        get{
            return curCircuitBeingChosen_;
        }
        private set {
            curCircuitBeingChosen_ = (value >= 0 && value <= 7) ? value : -1;
        }
    }

    private List<GameObject> listButtonChoice;

    //�� public TextMeshProUGUI tempTMProU;
    public void Start() {
        listButtonChoice = new List<GameObject>();
        foreach (Transform trn in gameObject.transform.GetChild(0).Find("panelChoice")) {
            listButtonChoice.Add(trn.gameObject);
        }

        wordNUMBER = optionAIO.curTranslation switch {
            enumTranslation.english => "(Number)",
            enumTranslation.korean => "(숫자)",
            _ => ""
        };

        /* �� �׽�Ʈ (characterinfo�� ���� �ؽ�Ʈ ��ġ ã��)
        TextMeshProUGUI tm = null;
        for (int i = 0; i < 8; i++) {

            try {
                tm = transform.GetChild(0).GetChild(i).GetComponent<TextMeshProUGUI>();
            }catch(Exception e){
                
            }

            if (tm == null) {
                Debug.Log("no Text");
                continue;
            }
            Debug.Log("target : " + tm.text);

            foreach (TMP_CharacterInfo tc in tm.GetTextInfo(tm.text).characterInfo) {
                Debug.Log(tc.character + " : " + tc.bottomLeft + " / " + tc.topRight);
            }
            Debug.Log(" - - - - - - - - - - ---- - - - - -");
            foreach (TMP_CharacterInfo tc in tm.textInfo.characterInfo) {
                Debug.Log(tc.character + " : " + tc.bottomLeft + " / " + tc.topRight);
            }

        }
        */
    }

    private List<ISingleInfo> getListCircuitType(int parCode) {
        return parCode switch {
            0 or 3 => listSensors,
            1 or 2 => listNavigators,
            4 or 5 => listSelecters,
            _ => null
        };
    }

    public void activateSetter(Thing source) {
        curThingBeingSet = source;

        string[] tempStringArray = source.getCircuitInfo();
        for (int i = 0; i < 6; i++) {
            listButtonCircuitType[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tempStringArray[i];
        }

        gameObject.GetComponent<uiBasic>().activatePanel();
    }

    public void activateChoicePanel(int parCurCircuitBeingChosen) {
        curCircuitBeingChosen = parCurCircuitBeingChosen;

        List<ISingleInfo> tempListCircuit = getListCircuitType(parCurCircuitBeingChosen);

        // prepare circuit type choice buttons
        GameObject tempObject;
        for (int i = 0; i < listButtonCircuitType.Count(); i++) {
            if (i != curCircuitBeingChosen_) {
                listButtonCircuitType[i].GetComponent<Button>().interactable = false;
            }
        }

        // prepare choice buttons
        for (int i = 0; i < 8; i++) {
            tempObject = panelChoice.transform.GetChild(i).gameObject;
            if (tempListCircuit.Count > i) {
                tempObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tempListCircuit[i].singleInfo;
                tempObject.gameObject.SetActive(true);
            } else {
                tempObject.gameObject.SetActive(false);
            }
        }
        panelChoice.GetComponent<uiBasic>().activatePanel();
    }

    public void chooseCircuit(int parCircuitChosen) {
        arrCircuitCodes[curCircuitBeingChosen] = parCircuitChosen;

        listButtonCircuitType[curCircuitBeingChosen].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            getListCircuitType(curCircuitBeingChosen)[parCircuitChosen].singleInfo;
        // ★ input inputfield in the text if wordNUMBER is contained..., and to do that you should make inputfield pooling system
        panelChoice.GetComponent<uiBasic>().deactivatePanel();

        foreach (GameObject obj in listButtonCircuitType) {
            obj.GetComponent<Button>().interactable = true;
        }
    }

    public void confirm() {
        //★ arrParameters 취합하여 만들기

        curThingBeingSet.setCircuit(
            arrCircuitCodes[0], arrParameters[0],
            arrCircuitCodes[1], arrParameters[1],
            arrCircuitCodes[2], arrParameters[2],
            arrCircuitCodes[3], arrParameters[3],
            arrCircuitCodes[4], arrParameters[4],
            arrCircuitCodes[5], arrParameters[5]
            );

        gameObject.GetComponent<uiBasic>().deactivatePanel();
    }
}
