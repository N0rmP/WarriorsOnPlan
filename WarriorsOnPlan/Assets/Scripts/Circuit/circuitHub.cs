using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using Circuits;
using System.Linq;

namespace Cases {
    public class circuitHub : caseTimerSelfishTurn, ICaseUpdateState {
        private sensorAbst sensorForMove;
        private navigatorAbst navigatorIdle;
        private navigatorAbst navigatorPrioritized;        
        private navigatorAbst navigatorCur;

        private sensorAbst sensorForSkill;
        private selecterAbst selecterForSkill;

        private selecterAbst selecterForAttack;


        public circuitHub(enumSide parSide, int parTargetGroupDefault) : base(null, enumCaseType.circuit, false) {
            setCircuitHub(
                parSide,
                1202, new int[0],
                1101, new int[0],
                1202, new int[0],
                1102, new int[0],
                1301, new int[1] { parTargetGroupDefault },
                1301, new int[1] { 0b0010 }
                );
        }

        private circuitAbst convertNumToCircuit(int parNum) {
            return parNum switch {
                0 => navigatorIdle,
                1 => sensorForMove,
                2 => navigatorPrioritized,
                3 => sensorForSkill,
                4 => selecterForSkill,
                5 => selecterForAttack,
                _ => navigatorIdle
            };
        }

        #region setCircuitHub
        
        public void setCircuitHub(
        enumSide parSourceSide,
        int parCodenavigatorIdle, int[] ppNavigatorIdle,
        int parCodeSensorForMove, int[] ppSensorForMove,
        int parCodeNavigatorPrioritized, int[] ppNavigatorPrioritized,        
        int parCodeSensorForSkill, int[] ppSensorForSkill,
        int parCodeSelecterForSkill, int[] ppSelecterForSkill,
        int parCodeSelecterForAttack, int[] ppSelecterForAttack) {

            // makeOrRestore 
            void makeOrRestore<T>(ref T parCircuit, int parCode, IEnumerable<int> parParameter) where T : circuitAbst {
                parCircuit = gameManager.GM.MC.makeCodableObject<T>(parCode, parParameter, null);
            }

            makeOrRestore(ref navigatorIdle, parCodenavigatorIdle, ppNavigatorIdle);
            makeOrRestore(ref sensorForMove, parCodeSensorForMove, ppSensorForMove);
            makeOrRestore(ref navigatorPrioritized, parCodeNavigatorPrioritized, ppNavigatorPrioritized);
            makeOrRestore(ref sensorForSkill, parCodeSensorForSkill, ppSensorForSkill);
            makeOrRestore(ref selecterForSkill, parCodeSelecterForSkill, ppSelecterForSkill);
            makeOrRestore(ref selecterForAttack, parCodeSelecterForAttack, ppSelecterForAttack);
        }
        #endregion setCircuitHub

        // region relay_GET relays internal information of each circuit
        #region relay_GET
        public string[] getInfoTotal() {
            return new string[6] {
            navigatorIdle.infoDescription,
            sensorForMove.infoDescription,
            navigatorPrioritized.infoDescription,            
            sensorForSkill.infoDescription,
            selecterForSkill.infoDescription,
            selecterForAttack.infoDescription
        };
        }

        public string getInfoSingle(int parNum) {
            return convertNumToCircuit(parNum).infoDescription;
        }

        public int getCodeSingle(int parNum) {
            return convertNumToCircuit(parNum).code;
        }

        // getSingleParameter returns parameters of one-single circuitAbst
        public int[] getParameterSingle(int parNum) {
            return convertNumToCircuit(parNum).getParameters()["concrete"];
        }

        public int getSelecterForSkillTargetGroup() {
            return selecterForSkill.targetGroup;
        }

        public int getSelecterForAttackTargetGroup() {
            return selecterForAttack.targetGroup;
        }
        #endregion relay_GET

        public node getNextRoute(Thing source) {
            navigatorCur.calculateNewRoute(source);
            return navigatorCur.getNextRoute(source);
        }

        public Thing selectAttackTarget(Thing source) {
            return selecterForAttack.select(source);
        }

        public Thing selectSkillTarget(Thing source) {
            return selecterForSkill.select(source);
        }

        #region ICaseImplementation
        (ICaseUpdateState, enumStateWarrior) ICaseUpdateState.caseFunc(Thing source) {
            navigatorCur = sensorForMove.checkWigwagging(source) ? navigatorPrioritized : navigatorIdle;

            return (this,
                sensorForSkill.checkWigwagging(source) && source.thisSkill.isReady ? enumStateWarrior.skill :
                navigatorCur.checkIsArrival(source) ? enumStateWarrior.idleAttack :
                enumStateWarrior.move);
        }

        public void onInterfered(Thing source) { }

        protected override void updateTimer(Thing source) {
            /*
            ★ 추후 timer 기능이 인터페이스화 되면 type 확인하고 각각 실행시킬 것
            sensorForSkill.updateTimer();
            sensorForMove.updateTimer();
            */
        }
        #endregion ICaseImplementation

        #region override
        public override List<object> getReferences() {
            try {
                List<object> tempResult = base.getReferences();
                tempResult.Add(navigatorIdle.getMementoIParametable());
                tempResult.Add(sensorForMove.getMementoIParametable());
                tempResult.Add(navigatorPrioritized.getMementoIParametable());
                tempResult.Add(sensorForSkill.getMementoIParametable());
                tempResult.Add(selecterForSkill.getMementoIParametable());
                tempResult.Add(selecterForAttack.getMementoIParametable());

                return tempResult;
            } catch (Exception e) {
                Debug.Log("error occured in circuitHub.getReference  ((" + e.Message);
                testAllCircuits();

                List<object> tempResult = new List<object>();
                sensorNothing tempCircuit = gameManager.GM.MC.makeCodableObject<sensorNothing>(1101, new int[0], null);
                for (int i = 0; i < 6; i++) {
                    tempResult.Add(tempCircuit.getReferences());
                }
                return tempResult;
            }
        }

        public override void restoreReferences(List<object> parListReference) {
            base.restoreReferences(parListReference);

            int tempInd = 0;
            navigatorIdle = (parListReference[tempInd++] as mementoIParametable)?.getRestoredIt<navigatorAbst>();
            sensorForMove = (parListReference[tempInd++] as mementoIParametable)?.getRestoredIt<sensorAbst>();
            navigatorPrioritized = (parListReference[tempInd++] as mementoIParametable)?.getRestoredIt<navigatorAbst>();
            sensorForSkill = (parListReference[tempInd++] as mementoIParametable)?.getRestoredIt<sensorAbst>();
            selecterForSkill = (parListReference[tempInd++] as mementoIParametable)?.getRestoredIt<selecterAbst>();
            selecterForAttack = (parListReference[tempInd++] as mementoIParametable)?.getRestoredIt<selecterAbst>();
        }
        #endregion override

        #region test
        public void testAllCircuits() {
            StringBuilder temp = new StringBuilder();
            temp.Append("- - - circuitHub TESTING- - - ");
            temp.Append("\nnavigatorIdle : " + navigatorIdle);
            temp.Append("\nsensorForMove : " + sensorForMove);
            temp.Append("\nnavigatorPrioritized : " + navigatorPrioritized);            
            temp.Append("\n navigatorCur : " + navigatorCur);
            temp.Append("\nsensorForSkill : " + sensorForSkill);
            temp.Append("\nselecterForSkill : " + selecterForSkill);
            temp.Append("\nselecterForAttack : " + selecterForAttack);
            Debug.Log(temp.ToString());
        }        
        #endregion test
    }
}