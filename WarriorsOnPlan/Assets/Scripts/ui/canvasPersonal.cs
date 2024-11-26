using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class canvasPersonal : MonoBehaviour {
    private float destinationSlider;
    private Image imageHp;
    private Slider sliderSkill;

    public void Awake() {
        imageHp = transform.GetChild(0).GetComponent<Image>();
        sliderSkill = transform.GetChild(1).GetComponent<Slider>();
    }

    public void Update() {
        transform.LookAt(transform.position + gameManager.GM.cameraMain.transform.forward);

        if (destinationSlider < sliderSkill.value) {
            sliderSkill.value -= 0.0025f;
            if (Mathf.Abs(destinationSlider - sliderSkill.value) < 0.01f) {
                sliderSkill.value = destinationSlider;
            }
        }
    }

    #region updates
    public void updateHpText(int parValue) {
        int tempChange = parValue - Convert.ToInt32(imageHp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);

        if (tempChange > 0) {
            //heal
            imageHp.color = new Color(0f, 1f, 0f);
        } else if (tempChange < 0) {
            //damage
            imageHp.color = new Color(0.5f, 0f, 0f);
        }

        gameManager.GM.UC.addCount(imageHp.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), parValue);
        gameManager.GM.UC.addColorChange(imageHp, new Color(1f, 0f, 0f), 1f);
    }

    public void updateSkillTimer(int parTimerCur, int parTimerMax) {
        //slider update
        destinationSlider = parTimerCur / (float)parTimerMax;
        if (destinationSlider >= 0.99f) {
            sliderSkill.value = 1f;
        }

        //text update
        sliderSkill.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (parTimerCur > 0) ? parTimerCur.ToString() : "";
    }
    #endregion updates

    public void setSkill(skillAbst parSkill) {
        sliderSkill.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = parSkill.caseImage;
    }

    public void click() {
        combatUIManager.CUM.CStatus.chooseThing(transform.parent.GetComponent<Thing>());
    }
}

