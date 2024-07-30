using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class uiPersonalCanvas : MonoBehaviour
{
    private float destinationSlider;
    private Slider sliderSkill;

    void Awake() {
        sliderSkill = transform.GetChild(1).GetComponent<Slider>();
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

    private void Start() {
        //¡Ú test
        updateHpText(99);
        setSkillImage("PowerShot");
        updateSkillTimer(0, 1);
    }

    public void setSkillImage(string parSkillName) {
        transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Skill/Image_" + parSkillName);
    }

    #region updates
    public void updateHpText(int parValue) {
        int tempChange = parValue - Convert.ToInt32(transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text);
        Image tempImg = transform.GetChild(0).GetComponent<Image>();

        if (tempChange > 0) {
            //heal
            tempImg.color = new Color(0f, 1f, 0f);
        } else if (tempChange < 0) {
            //damage
            tempImg.color = new Color(0.5f, 0f, 0f);
        }

        gameManager.GM.UC.addCount(transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>(), parValue);
        gameManager.GM.UC.addColorChange(tempImg, new Color(1f, 0f, 0f), 1f);
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

