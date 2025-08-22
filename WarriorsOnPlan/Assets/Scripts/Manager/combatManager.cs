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
    initiating = -1,
    preparing = 0,
    combat = 1,
    combatDone = 2,
    reenact = 3,
    reenactHalted = 4,
    reenactDone = 5
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
    public combatUIComponent CUM { get; private set; }    // it's not that, you lewd animal

    public dataLevel curDataLevel { get; private set; }

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

    // countDistinguisher helps deciding where to stop restoring / reenacting, it's required because countAction can help only with processActionAbst not with processSystemCombatEnd etc.
    public int countDistinguisher { get; private set; }
    private int countActionMax = 0;
    private int countAction_;
    public int countAction {
        get {
            return countAction_;
        }
        private set {
            countAction_ = Math.Clamp(value, 0, countActionMax);
        }
    }

    public enumSide sideTurn { get; private set; }

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
    private upgradeAbst[] arrUpgradeActive_;
    public upgradeAbst[] arrUpgradeActive{
        get {
            return arrUpgradeActive_.Clone() as upgradeAbst[];
        }
    }

    // members used during combat reenacting
    private processAbst processReenactedNext;

    private ICombatEnder curCombatEnder;
    private combatResult curCombatResult;

    #region callbacks
    public void Awake() {
        //combatManager is also a singleton manager, but it only exists during one scene
        if (CM == null) {
            CM = this;
        } else {
            Destroy(this);
        }

        CUM = new combatUIComponent();
        GC = new graphComponent(7, 7);
        FC = new fxComponent();
        HouC = new houseComponent();
        HisC = new historyComponent();        

        combatState = enumCombatState.preparing;

        toolsProvided = new List<caseBase>();
        arrUpgradeActive_ = null;

        resetInterval();
    }
    #endregion callbacks

    #region combat_methods
    public void BEPREPARED(bool parIsInitiation = false) {
        combatState = enumCombatState.preparing;

        restoreCombat(parIsInitiation ? HisC.mementoInitial : HisC[0]);
        CUM.doWhenPreparingStart();
        foreach (Thing thingFriendly in HouC.getArrTotal(enumSide.player)) {
            thingFriendly.updatePanelDragableReleasable();
        }
        FC.retrieveAll();
    }

    public void startCombat() {
        // player can't start combat while already in combat
        if (combatState > enumCombatState.preparing) {
            return;
        }

        CUM.doWhenCombatStart();
        COMBAT();
        foreach (Thing thingFriendly in HouC.getArrTotal(enumSide.player)) {
            thingFriendly.updatePanelDragableReleasable();
        }
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

        if (parProcess is processByproductDelecate tempParProcess && processLast is processByproductDelecate tempProcessLast) {
            tempProcessLast.addDel(tempParProcess);
            return;
        }

        // it means parProcess is the next process of processActionAbst, every processActionAbst makes memento and parProcess should be set as processNext of it
        if (mementoLast != null) {
            mementoLast.processNext = parProcess;
            mementoLast = null;
        }

        processAbst tempBefore = processLast;
        try {
            parProcess.DO(ref processLast, ref delSetNext);
        } catch (Exception e) {
            Debug.Log("error occured in " + countAction + " process " + parProcess.GetType().Name);
            parProcess.testChainAfterAll();
        }

        if (parProcess is processAction) {
            mementoCombat tempMementoCombat = makeMementoCombat(parProcess);
            mementoLast = tempMementoCombat;
            HisC.addMemento(tempMementoCombat);
        }

        // check if this combat is over after each execution
        if (checkCombatEnd() && processLast is not processSystemCombatEnd) {
            combatState = enumCombatState.combatDone;
            processSystemCombatEnd tempProcessSCE = new processSystemCombatEnd();
            tempProcessSCE.DO(ref processLast, ref delSetNext);
            delSetNext(null);
            HisC.addMemento(makeMementoCombat(tempProcessSCE));
            countActionMax = countAction;
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
        countActionMax = int.MaxValue;

        while (combatState == enumCombatState.combat) {
            tempArrActors = HouC.getArrActionOrder(sideTurn);

            // turn start
            Debug.Log(sideTurn + " T U R N   S T A R T " + countAction);
            executeProcess(new processSystemTurnStart(tempArrActors));

            foreach (Thing th in tempArrActors) {
                if (th.stateCur <= enumStateWarrior.dead) { 
                    continue; 
                }
                if (combatState == enumCombatState.combatDone) { 
                    break; 
                }

                // actual ACTUAL WARRIOR's ACTION
                executeProcess(new processAction(th));
            }

            if (combatState == enumCombatState.combatDone) { 
                break; 
            }

            // turn end
            executeProcess(new processSystemTurnEnd(tempArrActors,
                // turn change delegate, while no nuetral Thing exists there are only two turn types (player & enemy)
                () => {
                    sideTurn = (enumSide)((int)sideTurn << 1);
                    // if sideTurn is player or enemy, return
                    if (sideTurn < enumSide.neutral) {
                        return;
                    } else {
                        // if sideTurn is neutral and neutral thing exists, return
                        if (HouC.getArrAlive(enumSide.neutral).Length > 0) {
                            return;
                        }
                    }
                    // if all exception cases above are passed, sideTurn is set to player
                    sideTurn = enumSide.player;
                    }
                ));

            // ★ 무한루프 방지용 긴급탈출버튼
            if (countAction > 999) {
                Debug.Log("Ejection : 999 action passed, COMBAT method escape");
                return;
            }
        }        

        // ★ 테스트용 프로세스 싹 출력하기
        Debug.Log("combat end (( countAction : " + countAction);
        HisC[0].processNext.testChainAfterAll();   
    }
    private IEnumerator REENACT() {
        while (processReenactedNext != null) {
            Debug.Log("incoming next to-be-reenacted process : " + processReenactedNext);
            if (processReenactedNext != null) {
                countAction = processReenactedNext.thisCountAction;
            }

            // ★ 무한루프 방지용 긴급탈출버튼
            if (countAction > 999) {
                Debug.Log("Ejection : 999 action passed, REENACT method escape");
                yield break;
            }

            processReenactedNext = processReenactedNext.REENACT();
            // CUM.CStatus.updateTotal();
            yield return getInterval();
        }

        yield return new WaitForSeconds(structInterValsAndDurations.fltInterval / (float)combatSpeed);

        Debug.Log("reenacting end");
        combatState = enumCombatState.reenactDone;
        // ★ 전투 종료 화면 띄우기, combatResult 참조하여 대략적인 통계 제시하기 (세부 통계는 canvasStatistics를 사용하도록 하기)
    }

    public void startREENACT() {
        // first process should be always processCombatStart
        if (HisC[0].processNext is not processSystemCombatStart) {
            Debug.Log("first process is not processSystemCombatStart, it was " + HisC[0].processLast);
            return;
        }

        restoreCombat(HisC[0]);

        combatState = enumCombatState.reenact;
        StartCoroutine(REENACT());
    }

    public void resumeREENACT() {
        if (combatState is not enumCombatState.reenactHalted) {
            return;
        }
        
        combatState = enumCombatState.reenact;
        StartCoroutine(REENACT());
    }

    #region utility
    // checkCombatEnd is used during reenacting, it should check the vary current combat state to judge
    private bool checkCombatEnd() {
        return curCombatEnder.checkIsCombatEnd();
    }

    // ★ 이거 그냥 combatResult 제출하게 만들자
    /*
    public bool checkIsPlayerWin() {
        // if 
        if (combatState < enumCombatState.combatDone) {
            return false;
        }

        return curCombatResult != null ? curCombatResult.isPlayerWin : curCombatEnder.checkIsPlayerWin();
    }
    */

    private void resetInterval() {
        combatSpeed = 1;
        intervalYieldReturn = new WaitForSeconds(structInterValsAndDurations.fltInterval);
        CUM.setCombatSpeedText();
    }

    private System.Object getInterval(float parAdditionalInterval = 0f) {
        return combatSpeed switch {
            // if combatSpeed == 0, combat progresses to next action only when player presses anything
            0 => new WaitUntil(() => Input.anyKeyDown),
            // if combatSpeed is 1~3, combat progresses in speed of 1~3 times of original speed automatically
            > 0 and < 4 => new WaitForSeconds(structInterValsAndDurations.fltInterval / (float)combatSpeed + parAdditionalInterval),
            // case combatSpeed >= 4 is only for trouble-blocking and you shouldn't go through this case
            _ => new WaitForSeconds(structInterValsAndDurations.fltInterval + parAdditionalInterval)
        };
    }

    public float getBodyAnimationDuration() {
        return structInterValsAndDurations.fltBodyAnimationDuration / (float)combatManager.CM.combatSpeed;
    }

    public void changeSpeed() {
        if (combatSpeed >= 3) {
            combatSpeed = 1;
        } else {
            combatSpeed++;
        }
        CUM.setCombatSpeedText();
    }

    public void skipReenating() {
        intervalYieldReturn = null;
        combatSpeed = 99;
    }

    public combatResult getCombatResult() {
        // combat end
        if (curCombatResult == null) {
            int tempTotalDamageDealt = 0; int tempTotalDamageTaken = 0;
            foreach (Thing t in HouC.getArrTotal(enumSide.player)) {
                tempTotalDamageDealt += t.damageDealt;
                tempTotalDamageTaken += t.damageTaken;
            }
            curCombatResult = new combatResult(
                HouC.getArrAlive(enumSide.enemy).Length <= 0,
                countAction,
                tempTotalDamageDealt,
                tempTotalDamageTaken
                );
        }

        return curCombatResult;
    }
    #endregion utility

    #region memento
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
        if (combatState < enumCombatState.reenact || combatState > enumCombatState.reenactDone) {
            return;
        }

        decrementCountAction();
        restoreCombat(HisC[countAction]);
    }

    public void restoreNextAction() {
        if (combatState < enumCombatState.reenact || combatState > enumCombatState.reenactDone) {
            return;
        }

        incrementCountAction();
        restoreCombat(HisC[countAction]);
    }

    private void restoreCombat(mementoCombat parMC) {
        // clear several objects before restoring
        StopAllCoroutines();
        if (combatState is enumCombatState.reenact) {
            combatState = enumCombatState.reenactHalted;
        }
        dragableObjectAbst.emergencyEndDrag();
        gameManager.GM.TC.clearDelegate();
        gameManager.GM.UC.clearAll();

        HouC.restore(parMC.house);
        countAction = parMC.countAction;
        sideTurn = parMC.turn;
        processLast = parMC.processLast;
        processReenactedNext = parMC.processNext;

        // tools provided
        toolsProvided.Clear();
        foreach (caseBase cb in parMC.toolsProvided) {
            toolsProvided.Add(cb);
        }
        CUM.TS.updateBubbles(toolsProvided.ToArray());

        restoreUI();
    }

    private void restoreUI() {
        // ★ if (CUM.CStatus is shown)
        // CUM.CStatus.updateTotal();

        // this is necessary when new spawned thing has ability to change ActionOrder
        CUM.SAO.prepareBoxActionOrderBelt();

        CUM.setActionCounter(countAction, true);
        CUM.testShowTurn(); //★ 폴리싱 필요,

        gameManager.GM.PC.returnTotal();
    }
    #endregion memento
    #endregion combat_methods

    #region system_methods
    public void systemLevelEnter(dataLevel parDataLevel, upgradeAbst[] parArrUpgrade) {
        combatState = enumCombatState.initiating;
        resetInterval();
        arrUpgradeActive_ = parArrUpgrade;

        systemLevelInitiate(parDataLevel);
        BEPREPARED(true);
    }

    // systemLevelInitiate focus on making combat-preparing state from json-level-data, it has no need to graphic it
    private void systemLevelInitiate(dataLevel parDataLevel) {
        curDataLevel = parDataLevel;
        Thing tempThing;

        // spawn enemy warriors
        foreach (dataNotFriendlyThing et in parDataLevel.EnemyWarriors) {
            tempThing = systemSpawn(et.NameThing, enumSide.enemy, et.HP, (et.Coordinate0, et.Coordinate1), et.SkillParameters);
            foreach (dataIParametable dt in et.ToolList) {
                tempThing.addCase(gameManager.GM.MC.makeCodableObject<caseBase>(dt.CodeIParametable, dt.Parameters, null));
            }
            tempThing.setCircuit(
                et.CodeNavigatorIdle, et.Parameter2,
                et.CodeSensorForMove, et.Parameter0,
                et.CodeNavigatorPrioritized, et.Parameter1,                
                et.CodeSensorForSkill, et.Parameter3,
                et.CodeSelecterForSkill, et.Parameter4,
                et.CodeSelecterForAttack, et.Parameter5
                );
        }

        // spawn neutral warriors
        foreach (dataNotFriendlyThing nt in parDataLevel.NeutralThings) {
            tempThing = systemSpawn(nt.NameThing, enumSide.neutral, nt.HP, (nt.Coordinate0, nt.Coordinate1), nt.SkillParameters);
            foreach (dataIParametable dt in nt.ToolList) {
                tempThing.addCase(gameManager.GM.MC.makeCodableObject<caseBase>(dt.CodeIParametable, dt.Parameters, null));
            }
            tempThing.setCircuit(
                nt.CodeNavigatorIdle, nt.Parameter2,
                nt.CodeSensorForMove, nt.Parameter0,
                nt.CodeNavigatorPrioritized, nt.Parameter1,                
                nt.CodeSensorForSkill, nt.Parameter3,
                nt.CodeSelecterForSkill, nt.Parameter4,
                nt.CodeSelecterForAttack, nt.Parameter5);
        }

        // spawn friendly warriors
        foreach (dataFriendlyThing ft in parDataLevel.FriendlyWarriors) {
            tempThing = systemSpawn(ft.NameThing, enumSide.player, ft.HP, (ft.Coordinate0, ft.Coordinate1), ft.SkillParameters);
        }

        // make toolsProvided
        toolsProvided.Clear();
        foreach (dataIParametable dt in parDataLevel.ToolsProvided) {
            toolsProvided.Add(
                gameManager.GM.MC.makeCodableObject<caseBase>(dt.CodeIParametable, dt.Parameters, null)
                );
        }

        // set Ender
        curCombatEnder = new enderBasic();  //★ json 레벨 파일에서 가져오도록 하기

        // upgrade actualActivate & set boxUpgradeActive
        foreach (upgradeAbst ua in arrUpgradeActive) {
            ua.actualActivate();
        }
        CUM.CUA.prepareBoxUpgrade(arrUpgradeActive);

        // ICaseSystemicBeforePrepare
        foreach (ICaseSystemicBeforePrepare cb in from th in HouC.arrTotalAlive where th.thisSkill is ICaseSystemicBeforePrepare select th.thisSkill ) {
            cb.caseFunc();
        }

        HisC.setMementoInitial(makeMementoCombat(null));
    }
    
    public void systemAddToolsProvided(caseBase parTool) {
        if (parTool.caseType != enumCaseType.tool) {
            return;
        }

        toolsProvided.Add(parTool);
        CUM.TS.updateBubbles(toolsProvided.ToArray());
    }

    public void systemRemoveToolsProvided(caseBase parTool) {
        if (parTool.caseType != enumCaseType.tool) {
            return;
        }

        toolsProvided.Remove(parTool);
        CUM.TS.updateBubbles(toolsProvided.ToArray());
    }

    public Thing systemSpawn(string parThingName, enumSide parSide, int parMaxHp, (int c0, int c1) parCoor, int[] parSkillParameters) {
        GameObject tempW = Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/Warrior/" + parThingName));

        try {
            Thing tempThing = tempW.GetComponent<Thing>();
            tempThing.init(parSide, parMaxHp, parSkillParameters);
            HouC.addAliveThing(tempThing);

            systemPlace(tempThing, parCoor);

            return tempThing;
        } catch (Exception e) {
            Debug.Log("combatManager.systemSpawn error : instanciated GameObjec = " + tempW + "\n" + e.Message);
            return null;
        }
    }

    public void systemPlace(Thing parThing, (int c0, int c1) parCoor) {
        node tempNode = GC[parCoor.c0, parCoor.c1];

        if (tempNode == null || tempNode.thingHere != null) {
            Debug.Log("systemPlace failed : node (" + parCoor.c0 + " , " + parCoor.c1 + ") / " + tempNode.thingHere + " on it / " + parThing + " to be placed");
            return;
        }

        tempNode.placeThing(parThing, true);
    }

    // caution : systemDestroyLevel destories or initiates almost all objects in combat-scene, it's recommended to be called only during loading
    public void systemDestroyLevel() {
        CUM.CStatus.updateNULL();
        HouC.clearTotal(true);
        HisC.resetHistoryTotal();
        CUM.SAO.clearLineTotal();
        GC.vacateGraph();
    }
    #endregion system_methods

    #region utility
    public void setCombatEnder(int parCodeEnder) {
        curCombatEnder = parCodeEnder switch {
            0 => new enderBasic(),
            _ => new enderBasic()
        };
    }

    public bool checkControllability(Thing parThing) {
        return (
            parThing.thisSide == enumSide.player &&
            combatManager.CM.combatState == enumCombatState.preparing
            );
    }
    #endregion utility

    #region count
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
    #endregion count

    #region internalClasses
    private interface ICombatEnder { public bool checkIsCombatEnd(); public bool checkIsPlayerWin(); }
    private class enderBasic : ICombatEnder {
        public bool checkIsCombatEnd() {
            return combatManager.CM.HouC.getArrAlive(enumSide.player).Length <= 0 || combatManager.CM.HouC.getArrAlive(enumSide.enemy).Length <= 0;
        }
        public bool checkIsPlayerWin() {
            return combatManager.CM.HouC.getArrAlive(enumSide.enemy).Length <= 0;
        }
    }
    #endregion internalClasses

    #region test
    public void testToolsProvided() {
        StringBuilder tempSB = new StringBuilder("testToolsProvided : ");
        foreach (caseBase cb in toolsProvided) {
            tempSB.Append(cb.infoName);
            tempSB.Append(" , ");
        }
        Debug.Log(tempSB.ToString());
    }
    #endregion test
}