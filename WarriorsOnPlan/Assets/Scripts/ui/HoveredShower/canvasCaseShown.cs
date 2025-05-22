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
            default:
                break;
        }

        transform.GetChild(0).GetChild(0).GetComponent<imgRoundRectangle>().setImg(parCase.caseImage);
        transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = parCase.caseName;
        try {
            foreach (KeyValuePair<string, int[]> p in parCase.getParameters()) {
                Debug.Log(p.Key + " :: ");
                foreach (int i in p.Value) {
                    Debug.Log(i);
                }
            }

            int[] tempParameters = parCase.getParameters()["concrete"];
            object[] tempArgs = new object[tempParameters.Count()];
            for (int i = 0; i < tempParameters.Length; i++) {
                tempArgs[i] = tempParameters[i];
            }

            transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format(
                parCase.caseDescription,
                tempArgs
                );
        } catch (FormatException e) {
            Debug.Log(parCase.GetType() + " results in error with \"" + parCase.caseDescription + "\" in canvasCaseShown \n(( " + e);
            transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "preparing skill description failed";
        }
    }

    private void setEffectNumbers(caseBase parCase) {
        foldNumbers();

        // timer
        if (parCase is caseTimer tempCase) {
            objNumTime.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                tempCase.timerMax.ToString();
            objNumTime.SetActive(true);
        }
    }

    private void setWeaponNumbers(toolWeapon parCase) {
        foldNumbers();

        // weapon range
        objNumRange.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            parCase.rangeMax == 1 ?
            gameManager.GM.option.basicWords.strMelee :
            parCase.rangeMax.ToString();
        objNumRange.SetActive(true);

        // weapon damage, change text color if damage is changed
        // ★ 피해 형태에 따라 마법 피해라면 추가 그래픽 처리 만들기
        objNumDamage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            parCase.damageCur.ToString();
        objNumDamage.SetActive(true);

        // weapon cool time
        objNumTime.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            parCase.timerMax.ToString();
        objNumTime.SetActive(true);
    }

    private void setSkillnumbers(skillAbst parSkill) {
        foldNumbers();

        // skill range
        if (parSkill.isRangeNeeded) {
            objNumRange.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                parSkill.rangeMax == 1 ?
                gameManager.GM.option.basicWords.strMelee :
                parSkill.rangeMax.ToString();
            objNumRange.SetActive(true);
        }

        // skill cool time
        if (parSkill.isCoolTimeNeeded) {
            objNumTime.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                parSkill.timerMax.ToString();
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
