using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class uiCanvasPersonal : MonoBehaviour
{
    private static uiBoxInformation curBoxInformation = null;

    private float destinationSlider;
    private Slider sliderSkill { get { return transform.GetChild(2).GetComponent<Slider>(); } }
    private Image imageHp { get { return transform.GetChild(1).GetComponent<Image>(); } }

    void Awake() {
        if (curBoxInformation == null) {
            curBoxInformation = GameObject.Find("boxInformation")?.GetComponent<uiBoxInformation>();
        }
    }

    void LateUpdate() {
        transform.LookAt(transform.position + gameManager.GM.camera.transform.forward);

        if (destinationSlider < sliderSkill.value) {
            sliderSkill.value -= 0.0025f;
            if (Mathf.Abs(destinationSlider - sliderSkill.value) < 0.01f) {
                sliderSkill.value = destinationSlider;
            }
        }
    }

    public void setSkillImage(string parSkillName) {
        sliderSkill.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Skill/Image_" + parSkillName);
    }

    public void clickShowStatus() {
        curBoxInformation?.showStatus(transform.parent.GetComponent<Thing>());
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
}

