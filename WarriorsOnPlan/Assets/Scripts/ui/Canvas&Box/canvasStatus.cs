using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using Cases;

public class canvasStatus : MonoBehaviour
{
    public Thing thisThing { get; private set; }

    private releasableInventory RI { get; set; }
    private canvasCircuitSetter CCS;

    public void Awake() {
        RI = transform.GetChild(3).GetComponent<releasableInventory>();
        CCS = GameObject.Find("canvasCircuitSetter").GetComponent<canvasCircuitSetter>();
    }

    public void chooseThing(Thing parThing) {
        if (thisThing == parThing) {
            return;
        }

        // thisThing?.setCursorChosen(false);
        thisThing = parThing;
        // parThing.setCursorChosen(true);

        updateTotal();
    }

    #region update_methods
    public void updateTotal() {
        if (thisThing == null) {
            updateNULL();
            return;
        }

        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = thisThing.name;
        updateHP(thisThing.curHp, thisThing.maxHp);
        updateSkill();
        updateTool();
        updateNumber();
        updateEffect();

        // curtainInventory is controlled by canvasStatus not combatUIComponent.doWhenCombatStart,
        // because it should be closed not only when combat starts but also when non-player thing is selected
        RI.setInteractivity(combatManager.CM.checkControllability(thisThing));
        transform.GetChild(6).GetComponent<Button>().interactable = true;
    }

    public void updateHP(int parCurHp, int parMaxHp) {
        Transform tempSlider = transform.GetChild(1).GetChild(1);

        // if parMaxHp is zero, do not show HP bar value 
        if (parMaxHp <= 0) {
            tempSlider.GetComponent<Slider>().value = 1f;
            tempSlider.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
            return;
        }

        tempSlider.GetComponent<Slider>().value = parCurHp / (float)parMaxHp;
        tempSlider.GetChild(2).GetComponent<TextMeshProUGUI>().text = parCurHp + " / " + parMaxHp;
    }

    public void updateSkill() {
        transform.GetChild(2).GetComponent<showerCase>().setCase(thisThing?.thisSkill);
    }

    public void updateTool() {
        if (thisThing == null) {
            RI.clear();
        } else {
            RI.openInventory(thisThing);
        }
    }

    public void updateNumber() {
        Transform transformBoxNumbers = transform.GetChild(4);
        if (thisThing == null) {
            foreach (Transform tr in transformBoxNumbers) {
                tr.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
            return;
        }

        structWarriorStatus tempSWS = thisThing.thisStatus;
        transformBoxNumbers.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + tempSWS.weaponAmplifierAdd + " / " + tempSWS.weaponAmplifierMultiply + "%";
        transformBoxNumbers.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + tempSWS.skillAmplifierAdd + " / " + tempSWS.skillAmplifierMultiply + "%";
        transformBoxNumbers.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = tempSWS.armorAdd + " / " + tempSWS.armorMultiply + "%";
        transformBoxNumbers.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = thisThing.damageDealt.ToString();
        transformBoxNumbers.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = thisThing.damageTaken.ToString();
    }

    public void updateEffect() {
        transform.GetChild(5).GetComponent<boxEffect>().openEffect(thisThing);
    }

    // make canvasStatus to show nothing, it works like init method
    public void updateNULL() {
        if (thisThing != null) { 
            thisThing = null;
        }

        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        updateHP(0, 0);
        updateSkill();// transform.GetChild(2).GetComponent<showerCase>().setCase(null);
        updateTool();
        updateNumber();
        updateEffect();


        transform.GetChild(6).GetComponent<Button>().interactable = false;
        RI.setInteractivity(false);
    }
    #endregion update_methods

    public void removeBubble(dragableBubbleInventory parBubble, bool isUnequip) {
        RI.removeBubble(parBubble, isUnequip);
    }

    public void removeTool(caseBase parTool) {
        RI.removeTool(parTool);
    }

    public void openCircuitSetter() {
        if (thisThing == null) {
            return;
        }

        CCS.activateSetter(thisThing);
    }

    public void confirmCircuitSetting() {
        if (CCS.isActiveAndEnabled) {
            CCS.confirm();
        }
    }
}
