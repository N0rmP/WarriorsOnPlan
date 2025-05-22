using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Cases;

public class canvasPersonal : MonoBehaviour {
    private static carrierGeneric<imgRoundRectangle> carrierIRR = null;

    private float destinationSlider;
    private Image imageHp;
    private Slider sliderSkill;

    public void Awake() {
        if (carrierIRR == null) {
            GameObject tempIEPrefab = Resources.Load<GameObject>("Prefab/UI/imgRoundRectangle");
            carrierIRR = new carrierGeneric<imgRoundRectangle>(
                () => {
                    GameObject tempObj = GameObject.Instantiate(tempIEPrefab);
                    tempObj.GetComponent<RectTransform>().sizeDelta = new Vector2(0.6f, 0.6f);
                    return tempObj.GetComponent<imgRoundRectangle>();

                },
                (x) => {
                    x.transform.SetParent(null);
                }
                );
        }

        imageHp = transform.GetChild(0).GetComponent<Image>();
        sliderSkill = transform.GetChild(1).GetComponent<Slider>();
    }

    public void Update() {
        transform.LookAt(transform.position + Camera.main.transform.forward);

        if (destinationSlider < sliderSkill.value) {
            sliderSkill.value -= 0.0025f;
            if (Mathf.Abs(destinationSlider - sliderSkill.value) < 0.01f) {
                sliderSkill.value = destinationSlider;
            }
        }
    }

    public void setSkill(skillAbst parSkill) {
        sliderSkill.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = parSkill.caseImage;
    }

    public void click() {
        combatUIManager.CUM.CStatus.chooseThing(transform.parent.GetComponent<Thing>());
    }

    #region updates
    public void updateHpText(int parValue, bool parIsStrict = false) {
        int tempChange = parValue - Convert.ToInt32(imageHp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);

        gameManager.GM.UC.addCount(imageHp.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), parValue);

        if (combatManager.CM.combatState != enumCombatState.reenact || parIsStrict) {
            return;
        }

        if (tempChange > 0) {
            //heal
            imageHp.color = new Color(0f, 1f, 0f);
        } else if (tempChange < 0) {
            //damage
            imageHp.color = new Color(0.5f, 0f, 0f);
        }
        gameManager.GM.UC.addColorChange(imageHp, new Color(1f, 0f, 0f), 0.75f);
    }

    public void updateSkillTimer(int parTimerCur, int parTimerMax) {
        // if combatState is not reenact, skip intransparent UI
        if (combatManager.CM.combatState is not enumCombatState.reenact) {
            sliderSkill.value = 0f;
            sliderSkill.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
        }

        // slider update
        destinationSlider = parTimerCur / (float)parTimerMax;
        if (destinationSlider >= 0.99f) {
            sliderSkill.value = 1f;
        }

        // text update
        sliderSkill.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (parTimerCur > 0) ? parTimerCur.ToString() : "";
    }

    // if skill doesn't require timer, you can make the skill icon fully open always by openSkillTimer
    public void openSkillTimer() {
        destinationSlider = 0f;
        sliderSkill.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
    }
    #endregion updates

    #region effect_management
    public void addImgEffect(caseBase parEffect) {
        if (!parEffect.isVisible) {
            return;
        }

        imgRoundRectangle tempIRR = carrierIRR.getInterceptor();
        tempIRR.setCase(parEffect);
        tempIRR.transform.SetParent(transform.GetChild(2));
        tempIRR.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        tempIRR.transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    public void removeImgEffect(caseBase parEffect) {
        imgRoundRectangle tempIRR;
        foreach (Transform tr in transform.GetChild(2)) {
            if (tr.TryGetComponent<imgRoundRectangle>(out tempIRR) && tempIRR.drawingRoom == parEffect) {
                carrierIRR.returnSingle(tempIRR);
            }
        }
    }

    public void removeImgEffect(imgRoundRectangle parIRR) {
        imgRoundRectangle tempIRR;
        foreach (Transform tr in transform.GetChild(2)) {
            if (tr.TryGetComponent<imgRoundRectangle>(out tempIRR) && tempIRR == parIRR) {
                carrierIRR.returnSingle(tempIRR);
            }
        }
    }

    public void clearImgEffect() {
        imgRoundRectangle tempIRR;
        foreach (Transform tr in transform.GetChild(2)) {
            if (tr.TryGetComponent<imgRoundRectangle>(out tempIRR)) {
                carrierIRR.returnSingle(tempIRR);
            }
        }
    }
    #endregion effect_management
}

