using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Newtonsoft.Json;
using UnityEngine.UIElements;

using Cases;
using Processes;
using System.Text;

public enum enumCombatState {
    preparing = 0,
    combat = 1,
    combatDone = 2,
    reenact = 3,
    reenactDone = 4
}

public enum enumSide {
    // each number of enumSide is in binary order to be well-comunicated with selecterAbst's creator's target group digits
    player = 0b001,
    enemy = 0b010,
    neutral = 0b100,
    none = 999
}

public class combatManager : MonoBehaviour {
    public static combatManager CM { get; private set; }
    public graphComponent GC { get; private set; }
    public fxComponent FC { get; private set; }
    public houseComponent HouC { get; private set; }
    public historyComponent HisC { get; private set; }

    private enumCombatState combatState_;
    public enumCombatState combatState {
        get {
            return combatState_;
        }
        private set {
            combatState_ = value;
            GC.doOnAllNode((x) => x.autoColor());
        }
    }

    private List<caseBase> toolsProvided;

    public int countDistinguisher { get; private set; }
    private int countAction_;
    public int countAction {
        get {
            return countAction_;
        }
        private set {
            countAction_ = (value < 0) ? 0 : value;
        }
    }

    public enumSide sideTurn { get; private set; }

    private const float fltInterval = 1.5f;
    private const float fltBodyAnimationDuration = fltInterval / 1.5f;
    private int combatSpeed_;
    public int combatSpeed {
        get {
            return combatSpeed_;
        }
        private set {
            combatSpeed_ = Math.Clamp(value, 0, 3);
        }
    }
    private System.Object intervalYieldReturn;

    // members used during actual combat
    private processAbst processLast;
    private Action<processAbst> delSetNext;
    private mementoCombat mementoLast;

    // members used during combat reenacting
    private processAbst processReenactedNext;

    private combatResult curCombatResult;    



    #region callbacks
    public void Awake() {
        //combatManager is also a singleton manager, but it only exists during one scene
        if (CM == null) {
            CM = this;
        } else {
            Destroy(this);
        }

        GC = new graphComponent(7, 7);
        FC = new fxComponent();
        HouC = new houseComponent();
        HisC = new historyComponent();

        combatState = enumCombatState.preparing;

        toolsProvided = new List<caseBase>();

        resetInterval();
    }

    public void Start() {
        systemLevelInitiate("jsonLevel_Test");
        BEPREPARED(true);
    }
    #endregion callbacks



    #region combat_methods
    public void BEPREPARED(bool parIsInitiation = false) {
        combatState = enumCombatState.preparing;

        restoreCombat(parIsInitiation ? HisC.mementoInitial : HisC[0]);
        FC.retrieveAll();
    }

    public void startCombat() {
        COMBAT();
        startREENACT();
    }

    public void executeProcess(processAbst parProcess) {
        if (parProcess == null) {
            Debug.Log("null is tried to be executed as process");
            return;
        }
        // new process's creation & execution is available only during combat, it's impossible even during reenact
        if (combatState != enumCombatState.combat) {
            return;
        }

        if (parProcess is processByproductDelecate && processLast is processByproductDelecate tempProcessLast) {
            foreach (Action del in parProcess as processByproductDelecate) {
                tempProcessLast.addDel(del);
            }
            return;
        }

        // it means parProcess is the next process of processActionAbst, every processActionAbst makes memento and parProcess should be set as processNext of it
        if (mementoLast != null) {
            mementoLast.processNext = parProcess;
            mementoLast = null;
        }

        processAbst tempBefore = processLast;
        parProcess.DO(ref processLast, ref delSetNext);

        if (parProcess is processActionAbst) {
            mementoCombat tempMementoCombat = makeMementoCombat(parProcess);
            mementoLast = tempMementoCombat;
            HisC.addMemento(tempMementoCombat);
        }

        // check if this combat is over after each action
        if (checkCombatEnd() && processLast is not processSystemCombatEnd) {
            //★ 게임 종료 처리
            combatState = enumCombatState.combatDone;
            processSystemCombatEnd tempProcessSCE = new processSystemCombatEnd();
            tempProcessSCE.DO(ref processLast, ref delSetNext);
            delSetNext(null);
            HisC.addMemento(makeMementoCombat(tempProcessSCE));
        }
    }

    public void COMBAT() {
        combatState = enumCombatState.combat;

        sideTurn = enumSide.player;
        Thing[] tempArrActors;        
        countAction = 0;
        processLast = null;
        delSetNext = (x) => { };
        curCombatResult = null;

        HisC.resetHistory();
        HisC.addMemento(makeMementoCombat(null));
        mementoLast = HisC[0];
        executeProcess(new processSystemCombatStart());

        while (combatState == enumCombatState.combat) {

            //nuetral side's turn precedes enemy's turn to assure that player can make use of the nuetral side perfectly
            HouC.sortByAO(sideTurn);
            tempArrActors = sideTurn switch {
                enumSide.player => HouC.arrPlayerAlive,
                //enumSide.neutral => HouC.arrNeutralActionOrder,
                enumSide.enemy => HouC.arrEnemyAlive,
                _ => new Thing[0]   //prevent error
            };

            // turn start
            Debug.Log(sideTurn + " T U R N   S T A R T " + countAction);
            executeProcess(new processSystemTurnStart(tempArrActors));

            foreach (Thing th in tempArrActors) {
                
                if (th.stateCur <= enumStateWarrior.dead || combatState == enumCombatState.combatDone) {
                    continue;
                }

                // ACTUAL WARRIOR's ACTION
                // sorting houseComponent will be done in each selecterAbst or class which requires sorting
                // update targets, this precedes state decision because targets can affect it
                th.updateTargets();
                // state decision
                th.updateState();
                // actual ACTUAL WARRIOR's ACTION
                executeProcess(th.makeAction());
            }

            if (countAction > 9999) {
                Debug.Log("Ejection : 9999 action passed, inordinary combat expected");  
                return; 
            }

            // turn end
            executeProcess(new processSystemTurnEnd(tempArrActors,
                // turn change delegate, while no nuetral Thing exists there are only two turn types (player & enemy)
                () => sideTurn = (++sideTurn == enumSide.neutral && HouC.arrNeutralAlive.Length < 1) || (int)sideTurn < 3 ? sideTurn : enumSide.player
                ));
        }

        Debug.Log("combat end (( countAction : " + countAction);
        HisC[0].processNext.testChainAfterAll();

        // combat end
        int tempTotalDealtDamage = 0; int tempTotalTakenDamage = 0;
        foreach (Thing t in HouC.arrPlayerAlive) {
            tempTotalDealtDamage += t.damageTotalDealt;
            tempTotalTakenDamage += t.damageTotalTaken;
        }
        curCombatResult = new combatResult(
            HouC.arrEnemyAlive.Length <= 0,
            countAction,
            tempTotalDealtDamage,
            tempTotalTakenDamage
            );
    }

    public void startREENACT() {
        combatState = enumCombatState.reenact;

        // first process should be always processCombatStart
        if (HisC[0].processNext is not processSystemCombatStart) {
            Debug.Log("first process is not processSystemCombatStart, it was " + HisC[0].processLast);
            return;
        }

        restoreCombat(HisC[0]);

        resumeREENACT();
    }

    public void resumeREENACT() {
        if (combatState != enumCombatState.reenact) {
            return;
        }

        StartCoroutine(REENACT());
    }

    private IEnumerator REENACT() {
        while (processReenactedNext != null) {
            Debug.Log("incoming next to-be-reenacted process : " + processReenactedNext);
            if (processReenactedNext != null) {
                countAction = processReenactedNext.thisCountAction;
            }

            processReenactedNext = processReenactedNext.REENACT();
            combatUIManager.CUM.CStatus.updateTotal();
            yield return getInterval();
        }

        yield return new WaitForSeconds(fltInterval / (float)combatSpeed);

        Debug.Log("reenacting end");
        combatState = enumCombatState.reenactDone;
        // ★ 전투 종료 화면 띄우기, combatResult 참조하여 대략적인 통계 제시하기 (세부 통계는 canvasStatistics를 사용하도록 하기)
    }

    #region combat_utility
    private bool checkCombatEnd() {
        return (HouC.arrPlayerAlive.Length <= 0 || HouC.arrEnemyAlive.Length <= 0);
    }

    private void resetInterval() {
        combatSpeed = 1;
        intervalYieldReturn = new WaitForSeconds(fltInterval);
    }

    private System.Object getInterval(float parAdditionalInterval = 0f) {
        return combatSpeed switch {
            // if combatSpeed == 0, combat progresses to next action only when player presses anything
            0 => new WaitUntil(() => Input.anyKeyDown),
            // if combatSpeed is 1~3, combat progresses in speed of 1~3 times of original speed automatically
            > 0 and < 4 => new WaitForSeconds(fltInterval / (float)combatSpeed + parAdditionalInterval),
            // case combatSpeed >= 4 is only for trouble-blocking and you shouldn't go through this case
            _ => new WaitForSeconds(fltInterval + parAdditionalInterval)
        };
    }

    public float getBodyAnimationDuration() {
        return fltBodyAnimationDuration / (float)combatManager.CM.combatSpeed;
    }

    public void changeSpeed() {
        if (combatSpeed >= 3) {
            combatSpeed = 1;
        } else {
            combatSpeed++;
        }
        
    }

    public void skipReenating() {
        intervalYieldReturn = null;
        combatSpeed = 99;
    }    

    private mementoCombat makeMementoCombat(processAbst parProcessPrev) {
        try {
            return new mementoCombat(
                countAction,
                sideTurn,
                HouC.makeMementoHouse(),
                parProcessPrev,
                toolsProvided.ToArray()
            );
        } catch (Exception e) {
            StringBuilder tempSB = new StringBuilder("error in making mementoCombat : ");

            tempSB.Append("\nAction Count : ");
            tempSB.Append(processLast != null ? processLast.thisCountAction : 0);

            tempSB.Append("\nprocessPrev : ");
            tempSB.Append(parProcessPrev);
            tempSB.Append("\ntools provided : ");
            foreach (caseBase cb in toolsProvided) {
                tempSB.Append(cb);
                tempSB.Append(" , ");
            }

            tempSB.Append("\n\n");
            tempSB.Append(e);

            Debug.Log(tempSB.ToString());
            return HisC.mementoInitial;
        }
    }

    private mementoCombat makeMementoInitial() {
        return new mementoCombat(-1, sideTurn, HouC.makeMementoHouse(), null, toolsProvided.ToArray());
    }

    public void restorePreviousAction() {
        decrementCountAction();
        restoreCombat(HisC[countAction]);
    }

    private void restoreCombat(mementoCombat parMC) {
        if (combatState != enumCombatState.reenact) {
            return;
        }

        StopAllCoroutines();
        gameManager.GM.TC.clearDelegate();
        gameManager.GM.UC.clearAll();

        HouC.restore(parMC.house);
        countAction = parMC.countAction;
        sideTurn = parMC.turn;
        processLast = parMC.processLast;
        processReenactedNext = parMC.processNext;

        restoreUI();
    }

    private void restoreUI() {
        // ★ if (combatUIManager.CUM.CStatus is shown)
        // combatUIManager.CUM.CStatus.updateTotal();

        combatUIManager.CUM.setActionCounter(countAction, true);
        combatUIManager.CUM.testShowTurn();

        /*
            3. boxInformation의 canvasStatistics 복구, 아마도 mementoStatistics가 필요할 것 (★ 추후 많은 구현 필요)
        */
    }

    public bool checkIsPlayerWin() {
        return curCombatResult.isPlayerWin;
    }
    #endregion combat_utility
    #endregion combat_methods



    #region system_methods
    public void systemLevelInitiate(string parLevelName) {
        dataLevel tempDataLevel = gameManager.GM.JC.getJson<dataLevel>(parLevelName, false);

        Thing tempThing;

        //spawn enemy warriors
        foreach (dataNotFriendlyThing et in tempDataLevel.EnemyWarriors) {
            tempThing = systemSpawn(et.NameThing, enumSide.enemy, et.HP, (et.Coordinate0, et.Coordinate1), et.SkillParameters);
            foreach (dataIParametable dt in et.ToolList) {
                tempThing.addCase(gameManager.GM.MC.makeCodableObject<caseBase>(dt.CodeIParametable, dt.Parameters));
            }
            tempThing.setCircuit(
                et.CodeSensorForMove, et.Parameter0,
                et.CodeNavigatorPrioritized, et.Parameter1,
                et.CodeNavigatorIdle, et.Parameter2,
                et.CodeSensorForSkill, et.Parameter3,
                et.CodeSelecterForSkill, et.Parameter4,
                et.CodeSelecterForAttack, et.Parameter5
                );
        }

        //spawn neutral warriors
        foreach (dataNotFriendlyThing nt in tempDataLevel.NeutralThings) {
            tempThing = systemSpawn(nt.NameThing, enumSide.neutral, nt.HP, (nt.Coordinate0, nt.Coordinate1), nt.SkillParameters);
            foreach (dataIParametable dt in nt.ToolList) {
                tempThing.addCase(gameManager.GM.MC.makeCodableObject<caseBase>(dt.CodeIParametable, dt.Parameters));
            }
            tempThing.setCircuit(
                nt.CodeSensorForMove, nt.Parameter0,
                nt.CodeNavigatorPrioritized, nt.Parameter1,
                nt.CodeNavigatorIdle, nt.Parameter2,
                nt.CodeSensorForSkill, nt.Parameter3,
                nt.CodeSelecterForSkill, nt.Parameter4,
                nt.CodeSelecterForAttack, nt.Parameter5);
        }

        //spawn friendly warriors
        foreach (dataFriendlyThing ft in tempDataLevel.FriendlyWarriors) {
            tempThing = systemSpawn(ft.NameThing, enumSide.player, ft.HP, (ft.Coordinate0, ft.Coordinate1), ft.SkillParameters);
            //★ 제출용 player thing circuitHub 세팅, 추후 canvasCircuitSetter에서 확인 버튼 누를 때마다 세팅하게 변경 예정
            tempThing.setCircuit(
                1101 , new int[2] { -1, -1},
                1204, new int[0],
                1202, new int[0],
                1102, new int[2] { -1, -1 },
                1301, new int[0],
                1301, new int[0]
                );
        }

        //make toolsProvided
        foreach (dataIParametable dt in tempDataLevel.ToolsProvided) {
            toolsProvided.Add(
                gameManager.GM.MC.makeCodableObject<caseBase>(dt.CodeIParametable, dt.Parameters)
                );
        }
        combatUIManager.CUM.TS.prepareBubbles(toolsProvided.ToArray());

        //ui 준비

        //플레이어 준비 단계 시작
        //HisC.setMementoInitial(makeMementoInitial());
        HisC.setMementoInitial(makeMementoCombat(null));
    }
    public Thing systemSpawn(string parThingName, enumSide parSide, int parMaxHp, (int c0, int c1) parCoor, int[] parSkillParameters) {
        GameObject tempW = Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/Warrior/" + parThingName));

        try {
            Thing tempThing = tempW.GetComponent<Thing>();
            tempThing.init(parSide, parMaxHp, parSkillParameters);
            HouC.addThing(tempThing);

            systemPlace(tempThing, parCoor);

            return tempThing;
        } catch (Exception e) {
            Debug.Log("combatManager.systemSpawn error : instanciated GameObjec = " + tempW + "\n" + e.Message);
            return null;
        }
    }

    //processPlace 
    public void systemPlace(Thing parThing, (int c0, int c1) parCoor) {
        node tempNode = GC[parCoor.c0, parCoor.c1];

        if (tempNode == null || tempNode.thingHere != null) {
            Debug.Log("systemPlace failed : node (" + parCoor.c0 + " , " + parCoor.c1 + ") / " + tempNode.thingHere + " on it / " + parThing + " to be placed");
            return;
        }

        tempNode.placeThing(parThing);
    }
    #endregion system_methods



    #region utility
    public bool checkControllability(Thing parThing) {
        return (
            parThing.thisSide == enumSide.player &&
            combatManager.CM.combatState == enumCombatState.preparing
            );
    }

    public void incrementCountAction() {
        countAction++;
    }

    public void decrementCountAction() {
        countAction--;
    }

    public void incrementCountDistinguisher() {
        countDistinguisher++;
    }

    public void decrementCountDistinguisher() {
        countDistinguisher--;
    }
    #endregion utility



    #region internalClasses
    private class comparerHp : IComparer<Thing> {
        public int Compare(Thing w1, Thing w2) {
            return (w1.curHp - w2.curHp);
        }
    }

    private class comparerDamageDealt : IComparer<Thing> {
        public int Compare(Thing w1, Thing w2) {
            return (w1.damageTotalDealt - w2.damageTotalDealt);
        }
    }

    // technically it's not class, anyway...
    public record combatResult {
        public bool isPlayerWin;
        public int totalActionExecuted;
        // both damage below is player side's
        public int totalDealtDamage;
        public int totalTakenDamage;

        public combatResult(bool parIsPlayerWin, int parActionExecuted, int parTotalDealtDamage, int parTotalTakenDamage) {
            isPlayerWin = parIsPlayerWin;
            totalActionExecuted = parActionExecuted;
            totalDealtDamage = parTotalDealtDamage;
            totalTakenDamage = parTotalTakenDamage;
        }
    }
    #endregion internalClasses
}