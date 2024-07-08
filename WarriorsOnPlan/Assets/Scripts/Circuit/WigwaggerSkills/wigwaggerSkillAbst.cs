using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class wigwaggerSkillabst : caseTimerSelfishTurn, ICaseTurnStart, ICaseUpdateState {
    private sensorAbst sensorIdle;
    private sensorAbst sensorPrioritized;

    private sensorAbst sensorCur;

    protected wigwaggerSkillabst(int parTimerMax) : base(parTimerMax, enumCaseType.circuit, false, false) {
    }

    public void onTurnStart(Thing source) {
        //sensor update
        ¿©±â
    }

    public (ICaseUpdateState updater, enumStateWarrior ESW) onUpdateState(Thing source) {
        if (checkIfUseSkill(source)) {
            return (this, enumStateWarrior.skill);
        } else {
            return (this, enumStateWarrior.idleAttack);
        }
    }

    protected abstract bool checkIfUseSkill(Thing source);
}
