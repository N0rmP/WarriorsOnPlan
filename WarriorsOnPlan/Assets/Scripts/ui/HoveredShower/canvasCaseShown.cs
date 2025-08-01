using Cases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class canvasCaseShown : MonoBehaviour {
    private GameObject objNumRange;
    private GameObject objNumDamage;
    private GameObject objNumTime;

    public void Awake() {
        objNumRange = transform.GetChild(1).GetChild(0).gameObject;
        objNumDamage = transform.GetChild(1).GetChild(1).gameObject;
        objNumTime = transform.GetChild(1).GetChild(2).gameObject;
    }

    public void prepare(caseBase parCase) {
        // ★ 아이콘 뒤에 깔리는 배경 변경 

        switch (parCase?.caseType) {
            case enumCaseType.effect:
                setEffectNumbers(parCase);
                break;
            case enumCaseType.tool:
                if (parCase is toolWeapon tempWeapon) {
                    setWeaponNumbers(tempWeapon);
                } else {
                    foldNumbers();
                }
                break;
            case enumCaseType.skill:
                setSkillnumbers(parCase as skillAbst);
                break;
            case enumCaseType.upgrade:
                foldNumbers();
                break;
            default:
                break;
        }

        transform.GetChild(0).GetChild(0).GetComponent<imgRoundRectangle>().setImg(parCase.caseImage);
        transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = parCase.infoName;
        transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = parCase.caseType.ToString();
        try {
            transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = parCase.infoDescription;
        } catch (FormatException e) {
            Debug.Log(parCase.GetType() + " results in error with \"" + parCase + "\" in canvasCaseShown \n(( " + e);
            transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "preparing skill description failed";
        }
    }

    private void setEffectNumbers(caseBase parCase) {
        foldNumbers();

        // timer
        if (parCase is caseTimer tempCase) {
            objNumTime.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                tempCase.timerCur.ToString();
            objNumTime.SetActive(true);
        }
    }

    private void setWeaponNumbers(toolWeapon parCase) {
        foldNumbers();

        // weapon range
        objNumRange.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            parCase.rangeMax == 1 ?
            gameManager.GM.DHouC.bookWords.strMelee :
            parCase.rangeMax.ToString();
        objNumRange.SetActive(true);

        // weapon damage, change text color if damage is changed
        // ★ 피해 형태에 따라 마법 피해라면 추가 그래픽 처리 만들기
        objNumDamage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            parCase.damageOriginal.ToString();
        objNumDamage.SetActive(true);

        // weapon cool time
        /*
        objNumTime.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            parCase.timerMax.ToString();
        objNumTime.SetActive(true);
        */
    }

    private void setSkillnumbers(skillAbst parSkill) {
        foldNumbers();

        // skill range
        if (parSkill.isRangeNeeded) {
            objNumRange.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                parSkill.rangeMax == 1 ?
                gameManager.GM.DHouC.bookWords.strMelee :
                parSkill.rangeMax.ToString();
            objNumRange.SetActive(true);
        }

        // skill cool time
        if (parSkill.isTimerNeeded) {
            objNumTime.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                (parSkill.isReady ? gameManager.GM.DHouC.bookWords.strReady : parSkill.timerCur) + " / " + parSkill.timerMax;
            objNumTime.SetActive(true);
        }
    }

    // foldNumbers not only fold all numbers but also deactivate the boxNumbers neither, you should activate it to show
    private void foldNumbers() {
        for (int i = transform.GetChild(1).childCount - 1; i >= 0; i--) {
            transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
        }
    }
}
