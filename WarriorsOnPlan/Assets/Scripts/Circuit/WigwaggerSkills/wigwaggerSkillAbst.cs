using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class wigwaggerSkillabst : caseTimerSelfishTurn, ICaseUpdateState
{
    protected wigwaggerSkillabst(int parTimerMax) : base(parTimerMax, enumCaseType.circuit, false, false) {
    }

    public (ICaseUpdateState updater, enumStateWarrior ESW) onUpdateState(Thing source) {
        if (checkIfUseSkill(source)) {
            return (this, enumStateWarrior.skill);
        } else {
            return (this, enumStateWarrior.move);
        }
    }

    protected abstract bool checkIfUseSkill(Thing source);
}
