using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class boxSkill : showerCase {
    public void Start() {
        base.Start();
        setSkill(null);
    }

    public void setSkill(skillAbst parSkill) {
        thisCase = parSkill;

        if (thisCase == null) {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            return;
        }

        try {
            transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = parSkill.caseImage;
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        } catch (Exception e) {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            Debug.Log("error made in boxSkill : " + e.Message);
        }
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = parSkill.caseName;
    }
}
