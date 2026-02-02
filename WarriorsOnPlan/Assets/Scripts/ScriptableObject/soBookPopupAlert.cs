using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dataBookPopupAlert_default", menuName = "ScriptabbleObject/soBookPopupAlert")]
public class soBookPopupAlert : ScriptableObject, IDataInsurance {
    public string strAlertNoAttackTarget;
    public string strAlertNoSkillTarget;

    public void emergencyInit() {
        strAlertNoAttackTarget = "No Attack Target";
        strAlertNoSkillTarget = "No Skill Target";
    }
}