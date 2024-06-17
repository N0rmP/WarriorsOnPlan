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

    public caseBase(enumCaseType parCaseType = enumCaseType.effect) {
        caseType = parCaseType;
    }
}
