using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class uiPersonalCanvas : MonoBehaviour
{
    void LateUpdate() {
        transform.LookAt(transform.position + gameManager.GM.camera.transform.forward);
    }

    private void Start() {
        updateHpText(100);
    }

    private void updateHpText(int parValue) {
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
}

