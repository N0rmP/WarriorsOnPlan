using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class boxCombatResult : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI textWinLose;
    [SerializeField] private boxUpgrade contentBoxUpgrade;
    [SerializeField] private Transform contentTextResult;
    private List<TextMeshProUGUI> listTextResult;
    [SerializeField] private Toggle[] arrToggleSide;
    [SerializeField] private Transform[] arrContentThingResult;
    private List<cellThingResult>[] arrListCellTingResult;

    public void Start() {
        listTextResult = new List<TextMeshProUGUI>();
        arrListCellTingResult = new List<cellThingResult>[3] {
            new List<cellThingResult>(),
            new List<cellThingResult>(),
            new List<cellThingResult>()
        };

        transform.parent.gameObject.SetActive(false);
    }

    public void activate(combatResult parCombatResult) {
        transform.parent.GetComponent<uiActivatable>().activatePanel(new Vector3(0f, 0f, 0f));

        // big text
        textWinLose.text = parCombatResult.isPlayerWin ?
            gameManager.GM.DHouC.bookWords.strVictory :
            gameManager.GM.DHouC.bookWords.strDefeated;

        // set scrollUpgradeResult
        contentBoxUpgrade.prepareBoxUpgrade(combatManager.CM.arrUpgradeActive);

        // set scrollTextResult
        GameObject tempPrefabTextForScroll = Resources.Load<GameObject>("Prefab/UI/textForScroll");
        carrierCrude.updateCarrierCrude<TextMeshProUGUI, string>(
            listTextResult.ToArray(),
            parCombatResult.iterateVisibleTextResult(),
            () => {
                TextMeshProUGUI tempResult = Instantiate(tempPrefabTextForScroll, contentTextResult).GetComponent<TextMeshProUGUI>();
                listTextResult.Add(tempResult);
                return tempResult;
            },
            (a, b) => {
                a.GetComponent<TextMeshProUGUI>().text = b;
                a.gameObject.SetActive(true);
            },
            (a) => {
                a.gameObject.SetActive(false);
            }
        );

        // set toggle player
        arrToggleSide[0].isOn = true;
        activateAdequateThingResult();
        
        // set Thing result
        combatManager.CM.HouC.sortByAO();
        GameObject tempPrefabCellThingResult = Resources.Load<GameObject>("Prefab/UI/cellThingResult");
        for (int i = 0; i < 3; i++) {
            carrierCrude.updateCarrierCrude<cellThingResult, Thing>(
                arrListCellTingResult[i].ToArray(),
                combatManager.CM.HouC.getArrTotal((enumSide)(0b1 << i)),
                () => {
                    cellThingResult tempResult = Instantiate(tempPrefabCellThingResult, arrContentThingResult[i]).GetComponent<cellThingResult>();
                    arrListCellTingResult[i].Add(tempResult);
                    return tempResult;
                },
                (a, b) => {
                    a.setThing(b);
                    a.gameObject.SetActive(true);
                },
                (a) => {
                    a.gameObject.SetActive(false);
                }
            );
        }
    }

    public void activateAdequateThingResult() {
        for (int i=0; i<3; i++) {
            arrContentThingResult[i].parent.parent.gameObject.SetActive(arrToggleSide[i].isOn);
        }
    }

    public void deactivate() {
        transform.parent.GetComponent<uiActivatable>().deactivatePanel();
    }
}