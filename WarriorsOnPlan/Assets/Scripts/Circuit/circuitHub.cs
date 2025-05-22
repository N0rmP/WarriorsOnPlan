using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using Circuits;

namespace Cases {
    public class circuitHub : caseTimerSelfishTurn, ICaseUpdateState {
        private sensorAbst sensorForMove;
        private navigatorAbst navigatorPrioritized;
        private navigatorAbst navigatorIdle;
        private navigatorAbst navigatorCur;

        private sensorAbst sensorForSkill;
        private selecterAbst selecterForSkill;

        private selecterAbst selecterForAttack;


        public circuitHub(int[] parParameter) : base(new int[2] { -1, -1 }, enumCaseType.circuit, false) {
            sensorForMove = new sensorNothing(new int[0]);
            navigatorPrioritized = new navigatorStationary(new int[0]);
            navigatorIdle = new navigatorStationary(new int[0]);
            sensorForSkill = new sensorNothing(new int[0]);
            selecterForSkill = new selecterClosest(new int[2] { parParameter[0], parParameter[1] });
            selecterForAttack = new selecterClosest(new int[2] { parParameter[0], 0b010 });
        }

        #region setCircuitHub
        public void setCircuitHub(
        enumSide parSourceSide,
        int parCodeSensorForMove, int[] ppSensorForMove,
        int parCodeNavigatorPrioritized, int[] ppNavigatorPrioritized,
        int parCodenavigatorInferior, int[] ppnavigatorInferior,
        int parCodeSensorForSkill, int[] ppSensorForSkill,
        int parCodeSelecterForSkill, int[] ppSelecterForSkill,
        int parCodeSelecterForAttack, int[] ppSelecterForAttack) {

            // makeOrRestore 
            void makeOrRestore<T>(ref T parCircuit, int parCode, IEnumerable<int> parParameter) where T : circuitAbst<T> {
                if (parCircuit.code == parCode) {
                    parCircuit.restoreParameters(parParameter.GetEnumerator());
                } else {
                    parCircuit = gameManager.GM.MC.makeCodableObject<T>(parCode, parParameter);
                }
            }

            makeOrRestore<sensorAbst>(ref sensorForMove, parCodeSensorForMove, ppSensorForMove);
            makeOrRestore<navigatorAbst>(ref navigatorPrioritized, parCodeNavigatorPrioritized, ppNavigatorPrioritized);
            if (sensorForMove is not sensorAnything) {
                makeOrRestore<navigatorAbst>(ref navigatorIdle, parCodenavigatorInferior, ppnavigatorInferior);
            }
            makeOrRestore<sensorAbst>(ref sensorForSkill, parCodeSensorForSkill, ppSensorForSkill);
            // ★ 일단 당장의 제출을 위해 임시로 만들어볼 것, 나중에 circuitsetter에서 targetGroup 관련 매개변수를 가져오도록 변경할 것
            makeOrRestore<selecterAbst>(ref selecterForSkill, parCodeSelecterForSkill, new int[1] { 0b010 });
            makeOrRestore<selecterAbst>(ref selecterForAttack, parCodeSelecterForAttack, new int[1] { 0b010 });
        }
        #endregion setCircuitHub

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

        // getSingleParameter returns parameters of one-single circuitAbst
        public int[] getSingleParameter(int parCircuitType) {
            IParametable tempIP = (parCircuitType switch {
                0 => sensorForMove,
                1 => navigatorPrioritized,
                2 => navigatorIdle,
                3 => sensorForSkill,
                4 => selecterForSkill,
                5 => selecterForAttack,
                _ => new sensorNothing(new int[0])
            });
            return tempIP?.getParameters()["concrete"];
        }

        public node getNextRoute(Thing source) {
            //if (!navigatorCur.checkIsRouteValid(source)) {
                navigatorCur.calculateNewRoute(source);
            //}

            return navigatorCur.getNextRoute(source);
        }

        public Thing selectAttackTarget(Thing source) {
            return selecterForAttack.select(source);
        }

        public Thing selectSkillTarget(Thing source) {
            return selecterForSkill.select(source);
        }

        #region ICaseImplementation
        public (ICaseUpdateState updater, enumStateWarrior ESW) onUpdateState(Thing source) {
            // update circuit on their own
            sensorForMove = sensorForMove?.getValidCircuit(source);
            navigatorPrioritized = navigatorPrioritized?.getValidCircuit(source);
            navigatorIdle = navigatorIdle?.getValidCircuit(source);
            sensorForSkill = sensorForSkill?.getValidCircuit(source);
            selecterForSkill = selecterForSkill?.getValidCircuit(source);
            selecterForAttack = selecterForAttack?.getValidCircuit(source);

            // ★ sensorForMove를 통해 navigator 갱신
            navigatorCur = sensorForMove.checkWigwagging(source) ? navigatorPrioritized : navigatorIdle;

            Debug.Log(source + " : " + sensorForSkill.checkWigwagging(source) + " && " + source.thisSkill.isReady);

            return (this,
                sensorForSkill.checkWigwagging(source) && source.thisSkill.isReady ? enumStateWarrior.skill :
                navigatorCur.checkIsArrival(source) ? enumStateWarrior.idleAttack :
                enumStateWarrior.move);
        }

        protected override void updateTimer(Thing source) {
            sensorForSkill.updateTimer();
            sensorForMove.updateTimer();
        }
        #endregion ICaseImplementation

        #region override
        public override List<object> getReference() {
            try {
                List<object> tempResult = base.getReference();
                tempResult.Add(sensorForMove.getMementoIParametable());
                tempResult.Add(navigatorPrioritized.getMementoIParametable());
                if (sensorForMove is not sensorAnything) {
                    tempResult.Add(navigatorIdle.getMementoIParametable());
                }
                tempResult.Add(sensorForSkill.getMementoIParametable());
                tempResult.Add(selecterForSkill.getMementoIParametable());
                tempResult.Add(selecterForAttack.getMementoIParametable());

                return tempResult;
            } catch (Exception e) {
                Debug.Log("error occured in circuitHub.getReference");
                testAllCircuits();

                List<object> tempResult = new List<object>();
                sensorNothing tempCircuit = gameManager.GM.MC.makeCodableObject<sensorNothing>(1101, new int[0]);
                for (int i = 0; i < 6; i++) {
                    tempResult.Add(tempCircuit.getReference());
                }
                return tempResult;
            }
        }

        public override void restore(mementoIParametable parMementoCase) {
            base.restore(parMementoCase);

            int tempInd = 0;
            sensorForMove = (parMementoCase.listReference[tempInd++] as mementoIParametable)?.getRestoredIt<sensorAbst>();
            navigatorPrioritized = (parMementoCase.listReference[tempInd++] as mementoIParametable)?.getRestoredIt<navigatorAbst>();
            if (sensorForMove is not sensorAnything) {
                navigatorIdle = (parMementoCase.listReference[tempInd++] as mementoIParametable)?.getRestoredIt<navigatorAbst>();
            }
            sensorForSkill = (parMementoCase.listReference[tempInd++] as mementoIParametable)?.getRestoredIt<sensorAbst>();
            selecterForSkill = (parMementoCase.listReference[tempInd++] as mementoIParametable)?.getRestoredIt<selecterAbst>();
            selecterForAttack = (parMementoCase.listReference[tempInd++] as mementoIParametable)?.getRestoredIt<selecterAbst>();
        }
        #endregion override

        #region test
        public void testAllCircuits() {
            StringBuilder temp = new StringBuilder();
            temp.Append("- - - circuitHub TESTING- - - ");
            temp.Append("\nsensorForMove : " + sensorForMove);
            temp.Append("\nnavigatorPrioritized : " + navigatorPrioritized);
            temp.Append("\nnavigatorInferior : " + navigatorIdle);
            temp.Append("\nnavigatorCur : " + navigatorCur);
            temp.Append("\nsensorForSkill : " + sensorForSkill);
            temp.Append("\nselecterForSkill : " + selecterForSkill);
            temp.Append("\nselecterForAttack : " + selecterForAttack);
            Debug.Log(temp.ToString());
        }
        #endregion test
    }
}