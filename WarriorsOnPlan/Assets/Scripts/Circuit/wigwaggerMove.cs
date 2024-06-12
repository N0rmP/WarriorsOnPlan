using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wigwaggerMove : caseBase, ICaseTurnStart, ICaseUpdateState {
    private sensorAbst sensor;
    private navigatorAbst navIdle;
    private navigatorAbst navPrioritized;

    private navigatorAbst navCur;

    public wigwaggerMove(sensorAbst parSensor, navigatorAbst parNavIdle, navigatorAbst parNavPrioritized = null) : base(enumCaseType.circuit) {
        sensor = parSensor;
        navIdle = parNavIdle;
        navPrioritized = parNavPrioritized;

        navCur = parNavIdle;
    }

    public node getNextRoute(Thing owner) {
        return navCur.getNextRoute(owner);
    }

    public void onTurnStart(Thing source) {
        if (sensor.checkWigwagging(source) && (navPrioritized != null)) {
            navCur = navPrioritized;
        } else {
            navCur = navIdle;
        }
    }

    public (ICaseUpdateState updater, enumStateWarrior ESW) onUpdateState(Thing source) {
        return navCur.checkIsArrival(source) ? (this, enumStateWarrior.idleAttack) : (this, enumStateWarrior.move);
    }
}
