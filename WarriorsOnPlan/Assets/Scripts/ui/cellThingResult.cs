using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cases;
using Unity.VisualScripting;

public class cellThingResult : MonoBehaviour {
    private static carrierGeneric<GameObject> carrierRRTool_ = null;
    private static carrierGeneric<GameObject> carrierRRTool {
        get {
            if (carrierRRTool_ == null) {
                carrierRRTool_ = new carrierGeneric<GameObject>(
                    () => {
                        GameObject tempResult = Instantiate(Resources.Load<GameObject>("Prefab/UI/imgRoundRectangle"));
                        tempResult.AddComponent<showerCase>().setCaseTypeShown(new enumCaseType[1] { enumCaseType.tool });
                        return tempResult;
                    },
                    (x) => { }
                );
            }
            return carrierRRTool_;
        }
    }

    public void setThing(Thing parThing) {
        // retrieve all imgRoundRectangle
        foreach (Transform tr in transform.GetChild(3).GetChild(0).GetChild(0)) {
            if (tr.TryGetComponent<showerCase>(out _) && tr.TryGetComponent<imgRoundRectangle>(out _)) {
                carrierRRTool.returnSingle(tr.gameObject);
            }
        }

        // background
        GetComponent<Image>().color = parThing.stateCur == enumStateWarrior.dead ? new Color(0.75f, 0.75f, 0.75f, 1f) : Color.white;

        // portrait, action order, Thing's name
        transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = parThing.portrait;
        transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = combatManager.CM.HouC.getPersonalActionOrder(parThing).ToString();
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = parThing.nameThing;

        // boxStatusResult
        // Hp
        transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<Slider>().value = parThing.curHp / (float)parThing.maxHp;
        transform.GetChild(2).GetChild(0).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = parThing.curHp + " / " + parThing.maxHp;
        // DamageDealt
        transform.GetChild(2).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = parThing.damageDealt.ToString();
        // DamageTaken
        transform.GetChild(2).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = parThing.damageTaken.ToString();

        // scrollTool
        GameObject tempRRTool = null;
        foreach (caseBase cb in parThing.getCaseList(enumCaseType.tool)) {
            tempRRTool = carrierRRTool.getInterceptor();
            tempRRTool.GetComponent<imgRoundRectangle>().setCase(cb);
            tempRRTool.GetComponent<showerCase>().setCase(cb);
            tempRRTool.transform.SetParent(transform.GetChild(3).GetChild(0).GetChild(0));
            tempRRTool.GetComponent<RectTransform>().localScale = Vector3.one;
        }

        // frame
        transform.GetChild(4).GetComponent<Image>().color = parThing.thisSide switch {
            enumSide.player => new Color(0.5f, 0.5f, 1f, 1f),
            enumSide.enemy => new Color(1f, 0.5f, 0.5f, 1f),
            enumSide.neutral => new Color(1f, 1f, 0.5f, 1f),
            _ => Color.white
        };
    }
}
