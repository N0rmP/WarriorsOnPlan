using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumCaseType { 
    none = -1,
    circuit = 0,
    skill = 1,
    tool = 2,
    effect = 3,
    others = 99
}

public class caseBase {
    public readonly enumCaseType caseType;

    public readonly bool isVisible;

    public Sprite caseImage { get; protected set; }
    public string caseName { get; protected set; }
    public string caseDescription { get; protected set; }

    public caseBase(enumCaseType parCaseType = enumCaseType.effect, bool parIsVisible = false) {
        caseType = parCaseType;
        isVisible = parIsVisible;

        if (isVisible) {
            string tempHalfPath =
                "Case/" +
                caseType switch {
                    enumCaseType.effect => "Effect/",
                    enumCaseType.tool => "Tool/",
                    enumCaseType.skill => "Skill/",
                    _ => ""
                };

            caseImage = Resources.Load<Sprite>("Image/" + tempHalfPath + "image_" + GetType());

            dataArbitraryStringArray tempASA = gameManager.GM.JC.getJson<dataArbitraryStringArray>(tempHalfPath + GetType());
            caseName = tempASA.SwissArmyStringArray[0];
            caseDescription = tempASA.SwissArmyStringArray[1];
        }
     }

    public virtual int[] getParameters() { 
        return new int[0]; 
    }
}
