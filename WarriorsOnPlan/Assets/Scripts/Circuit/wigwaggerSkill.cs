using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wigwaggerSkill : caseTimerSelfishTurn, ICaseTurnStart, ICaseUpdateState {
    private sensorAbst sensorBasic;
    private sensorAbst sensorSpare;

    private sensorAbst sensorCur;

    public wigwaggerSkill(sensorAbst parSensorBasic, sensorAbst parSensorSpare = null) : base(-1, enumCaseType.circuit, false, false) {
        sensorBasic = parSensorBasic;
        sensorSpare = parSensorSpare;
    }
     
    public void onTurnStart(Thing source) {
        //sensor update
        sensorCur = ((sensorSpare == null) || (sensorBasic.checkKeepBasic(source))) ? sensorBasic : sensorSpare;
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
