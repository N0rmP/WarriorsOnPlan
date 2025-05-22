/*
�� wigwaggerSkill�� ������ circuitHub�� �̵���, ���� ��Ȱ��ų ���� ������ ���� �ּ�ó���ϰ� ���������� �Ƹ��� �������� ��

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