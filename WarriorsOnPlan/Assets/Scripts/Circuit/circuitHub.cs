using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class circuitHub : caseTimerSelfishTurn, ICaseUpdateState {
    private sensorAbst sensorForMove;
    private navigatorAbst navigatorPrioritized;
    private navigatorAbst navigatorIdle;
    private navigatorAbst navigatorCur;

    private sensorAbst sensorForSkill;
    private selecterAbst selecterForSkill;

    private selecterAbst selecterForAttack;
    

    public circuitHub(Thing source) : base(-1, enumCaseType.circuit, false) {
        sensorForMove = new sensorNothing();
        navigatorPrioritized = new navigatorStationary();
        navigatorIdle = new navigatorStationary();
        navigatorCur = navigatorIdle;
        sensorForSkill = new sensorNothing();
        selecterForSkill = new selecterClosest(source, 0b010);       //★ source의 skill을 참조하여 유효한 대상만 선택하게 하기
        selecterForAttack = new selecterClosest(source, 0b010);
    }

    public void setCircuitHub(
    Thing source,
    int parCodeSensorForMove,           int[] ppSensorForMove,
    int parCodeNavigatorPrioritized,    int[] ppNavigatorPrioritized,
    int parCodeNavigatorIdle,           int[] ppNavigatorIdle,
    int parCodeSensorForSkill,          int[] ppSensorForSkill,
    int parCodeSelecterForSkill,        int[] ppSelecterForSkill,
    int parCodeSelecterForAttack,       int[] ppSelecterForAttack) {

        sensorForMove = circuitMaker.makeSensor(parCodeSensorForMove, ppSensorForMove);
        //★ sensorForMove에 따라 navigatorPrioritized 설정 여부 결정
        navigatorPrioritized = circuitMaker.makeNavigator(parCodeNavigatorPrioritized, ppNavigatorPrioritized);
        navigatorIdle = circuitMaker.makeNavigator(parCodeNavigatorIdle, ppNavigatorIdle);
        sensorForSkill = circuitMaker.makeSensor(parCodeSelecterForSkill, ppSensorForSkill);
        selecterForSkill = circuitMaker.makeSelecter(source, parCodeSelecterForSkill, ppSelecterForSkill);
        selecterForAttack = circuitMaker.makeSelecter(source, parCodeSelecterForAttack, ppSelecterForAttack);
    }

    public string[] getTotalInfo() {
        return new string[6] {
            sensorForMove.singleInfo,
            navigatorPrioritized.singleInfo,
            navigatorIdle.singleInfo,
            sensorForSkill.singleInfo,
            selecterForSkill.singleInfo,
            selecterForAttack.singleInfo
        };
    }

    public int[] getSingleParameter(int parCircuitType) { 
        return parCircuitType switch { 
            0 => sensorForMove.getParameters(),
            1 => navigatorPrioritized.getParameters(),
            2 => navigatorIdle.getParameters(),
            3 => sensorForSkill.getParameters(),
            4 => selecterForSkill.getParameters(),
            5 => selecterForAttack.getParameters(),
            _ => (new sensorNothing()).getParameters()
        };
    }

    public node getNextRoute(Thing source) {
        return navigatorCur.getNextRoute(source);
    }

    public Thing selectAttackTarget(Thing source) {
        return selecterForAttack.select(source);
    }

    public Thing selectSkillTarget(Thing source) {
        return selecterForSkill.select(source);
    }

    public (ICaseUpdateState updater, enumStateWarrior ESW) onUpdateState(Thing source) {
        #region updateCircuits
        // update circuit on their own
        sensorForMove = sensorForMove.getValidCircuit(source);
        navigatorPrioritized = navigatorPrioritized.getValidCircuit(source);
        navigatorIdle = navigatorIdle.getValidCircuit(source);
        sensorForSkill = sensorForSkill.getValidCircuit(source);
        selecterForSkill = selecterForSkill.getValidCircuit(source);
        selecterForAttack = selecterForAttack.getValidCircuit(source);
        #endregion updateCircuits

        // ★ sensorForMove를 통해 navigator 갱신
        navigatorCur = sensorForMove.checkWigwagging(source) ? navigatorPrioritized : navigatorIdle;

        return (this,
            sensorForSkill.checkWigwagging(source) ? enumStateWarrior.skill :
            navigatorCur.checkIsArrival(source) ? enumStateWarrior.idleAttack :
            enumStateWarrior.move);
    }

    protected override void updateTimer(Thing source) {
        sensorForSkill.updateTimer();
        sensorForMove.updateTimer();
    }
}
