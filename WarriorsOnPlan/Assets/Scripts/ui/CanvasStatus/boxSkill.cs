using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class boxSkill : showerCase {

    protected override void init() {
        base.init();
        setCaseTypeShown((int)Cases.enumCaseType.skill);
    }

    protected override void doWhenSetCase() {
        if (thisCase == null) {
            transform.GetChild(0).GetComponent<imgRoundRectangle>().setImg(null);
            transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            return;
        }

        try {
            transform.GetChild(0).GetComponent<imgRoundRectangle>().setImg(thisCase.caseImage);
        } catch (Exception e) {
            transform.GetChild(0).GetComponent<imgRoundRectangle>().setImg(null);
            Debug.Log("error made in boxSkill : " + e.Message);
        }
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = thisCase.caseName;
    }
}
