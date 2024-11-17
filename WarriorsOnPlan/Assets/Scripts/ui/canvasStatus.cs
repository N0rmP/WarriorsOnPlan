using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class canvasStatus : MonoBehaviour
{
    public Thing thisThing { get; private set; }

    public releasableInventory RI { get; private set; }

    void Start()
    {
        RI = transform.GetChild(3).GetComponent<releasableInventory>();

        dataArbitraryStringArray tempData = gameManager.GM.JC.getJson<dataArbitraryStringArray>("Tooltip/statusTooltip");
        int tempSet = 0;    // tempSet represents the number of texts set into the text-showers
        int tempEnd = tempData.SwissArmyStringArray.Length;
        uiTextShower tempShower = null;
        foreach (Transform obj in transform) {
            if (tempSet >= tempEnd) {
                break;
            }

            if (obj.TryGetComponent<uiTextShower>(out tempShower)) {
                tempShower.initText(tempData.SwissArmyStringArray[tempSet * 2], tempData.SwissArmyStringArray[tempSet * 2 + 1]);
            }
        }
    }

    public void chooseThing(Thing parThing) {
        if (thisThing == parThing) {
            return;
        }

        thisThing?.setCursorChosen(false);
        thisThing = parThing;
        parThing.setCursorChosen(true);

        updateTotal();
    }

    public void updateTotal() {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = thisThing.name;
        updateHP(thisThing.curHp, thisThing.maxHp);
        // ★ 스킬 정보창 준비
        RI.openInventory();
        updateNumber();

        if (combatManager.CM.checkControllability(thisThing)) {
            transform.GetChild(10).GetComponent<Button>().interactable = true;
            // ★ canvasInventory 조작이 불가능하도록 변경
        } else {
            transform.GetChild(10).GetComponent<Button>().interactable = false;
            // ★ canvasInventory 조작이 가능하도록 변경
        }
    }

    public void updateHP(int parCurHp, int parMaxHp) {
        Transform tempSlider = transform.GetChild(1).GetChild(1);
        tempSlider.GetComponent<Slider>().value = parCurHp / (float)parMaxHp;
        tempSlider.GetChild(2).GetComponent<TextMeshProUGUI>().text = parCurHp + " / " + parMaxHp;
    }

    public void updateNumber() {
        transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + thisThing.weaponAmplifierAdd + " / " + thisThing.weaponAmplifierMultiply + "%";
        transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + thisThing.skillAmplifierAdd + " / " + thisThing.skillAmplifierMultiply + "%";

        // ★ 무기 사거리 취합 메서드 필요
        transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = 0.ToString();

        transform.GetChild(7).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + thisThing.armorAdd + " / " + thisThing.armorMultiply + "%";
        transform.GetChild(8).GetChild(1).GetComponent<TextMeshProUGUI>().text = thisThing.damageTotalDealt.ToString();
        transform.GetChild(9).GetChild(1).GetComponent<TextMeshProUGUI>().text = thisThing.damageTotalTaken.ToString();
    }
}
