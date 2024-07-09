using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wigwaggerSkill : caseTimerSelfishTurn, ICaseTurnStart, ICaseUpdateState {
    private sensorAbst sensorIdle;
    private sensorAbst sensorPrioritized;

    private sensorAbst sensorCur;

    public wigwaggerSkill(sensorAbst parSensorIdle, sensorAbst parSensorPrioritized = null) : base(-1, enumCaseType.circuit, false, false) {
        sensorIdle = parSensorIdle;
        sensorPrioritized = parSensorPrioritized;
    }

    public void onTurnStart(Thing source) {
        //sensor update
        sensorCur = ((sensorPrioritized == null) || (sensorCur.checkReturnToIdle(source))) ? sensorIdle : sensorPrioritized;
    }

    public (ICaseUpdateState updater, enumStateWarrior ESW) onUpdateState(Thing source) {
        if (sensorCur.checkWigwagging(source)) {
            return (this, enumStateWarrior.skill);
        } else {
            return (this, enumStateWarrior.idleAttack);
        }
    }

    protected override void updateTimer(Thing source) {
        sensorCur.updateTimer();
    }
}
