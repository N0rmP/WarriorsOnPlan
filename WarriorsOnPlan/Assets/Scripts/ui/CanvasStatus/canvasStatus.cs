using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class canvasStatus : MonoBehaviour
{
    public Thing thisThing { get; private set; }

    private releasableInventory RI { get; set; }
    private canvasCircuitSetter CCS;

    public void Awake() {
        RI = transform.GetChild(3).GetComponent<releasableInventory>();
        CCS = GameObject.Find("canvasCircuitSetter").GetComponent<canvasCircuitSetter>();
    }

    public void Start() {

        dataArbitraryStringArray tempData = gameManager.GM.JC.getJson<dataArbitraryStringArray>("Tooltip/statusTooltip");
        int tempSet = 0;    // tempSet represents the number of texts set into the text-showers
        int tempEnd = tempData.SwissArmyStringArray.Length;
        showerText tempShower = null;
        foreach (Transform obj in transform) {
            if (tempSet >= tempEnd) {
                break;
            }

            if (obj.TryGetComponent<showerText>(out tempShower)) {
                tempShower.initText(tempData.SwissArmyStringArray[tempSet * 2], tempData.SwissArmyStringArray[tempSet * 2 + 1]);
                tempSet++;
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
        transform.GetChild(2).GetComponent<boxSkill>().setSkill(thisThing?.thisSkill);
        RI.openInventory(thisThing);
        updateNumber();

        if (combatManager.CM.checkControllability(thisThing)) {
            transform.GetChild(10).GetComponent<Button>().interactable = true;
            RI.setInteractivity(true);
        } else {
            transform.GetChild(10).GetComponent<Button>().interactable = false;
            RI.setInteractivity(false);
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

    public void removeBubble(dragableBubbleInventory parBubble, bool isUnequip) {
        RI.removeBubble(parBubble, isUnequip);
    }

    public void removeTool(caseBase parTool) {
        RI.removeTool(parTool);
    }

    public void openCircuitSetter() {
        CCS.activateSetter(thisThing);
    }
}
