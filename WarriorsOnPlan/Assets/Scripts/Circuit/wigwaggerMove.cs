using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wigwaggerMove : caseTimerSelfishTurn, ICaseTurnStart, ICaseUpdateState {
    private sensorAbst sensorIdle;
    private sensorAbst sensorPrioritized;
    private navigatorAbst navIdle;
    private navigatorAbst navPrioritized;

    private sensorAbst sensorCur;
    private navigatorAbst navCur;

    public wigwaggerMove(sensorAbst parSensorIdle, navigatorAbst parNavIdle, sensorAbst parSensorPrioitized = null, navigatorAbst parNavPrioritized = null) : base(-1, enumCaseType.circuit) {
        sensorIdle = parSensorIdle;
        sensorPrioritized = parSensorPrioitized;
        navIdle = parNavIdle;
        navPrioritized = parNavPrioritized;

        navCur = parNavIdle;
    }

    public node getNextRoute(Thing owner) {
        return navCur.getNextRoute(owner);
    }

    public void onTurnStart(Thing source) {
        //sensor update
        sensorCur = ((sensorPrioritized == null) || (sensorPrioritized.checkReturnToIdle(source))) ? sensorIdle : sensorPrioritized;

        //navigator update
        navCur = (sensorCur.checkWigwagging(source) && (navPrioritized != null)) ? navPrioritized : navIdle;
    }

    public (ICaseUpdateState updater, enumStateWarrior ESW) onUpdateState(Thing source) {
        return navCur.checkIsArrival(source) ? (this, enumStateWarrior.idleAttack) : (this, enumStateWarrior.move);
    }

    protected override void updateTimer(Thing source) {
        sensorCur.updateTimer();
    }
}
