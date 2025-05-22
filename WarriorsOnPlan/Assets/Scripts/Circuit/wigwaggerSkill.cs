/*
★ wigwaggerSkill의 역할은 circuitHub로 이동됨, 추후 부활시킬 일이 있을지 몰라 주석처리하고 보존하지만 아마도 나가리될 것

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Circuits;

namespace Cases {
    public class wigwaggerSkill : caseTimerSelfishTurn, ICaseTurnStart, ICaseUpdateState {
        private sensorAbst sensorBasic;
        private sensorAbst sensorSpare;

        private sensorAbst sensorCur;

        public wigwaggerSkill(int[] parArrayInitialParameter, sensorAbst parSensorBasic, sensorAbst parSensorSpare = null) : base(parArrayInitialParameter, enumCaseType.circuit, parIsVisible: false) {
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
}
*/