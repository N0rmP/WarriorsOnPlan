using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public enum enumStateWarrior {
    dead = 0,
    deadRecently = 1,
    controlled = 10,
    focussing = 20,
    skill = 30,
    move = 40,
    idleAttack = 50,
    none = 9999
}

public enum enumSide { 
    player = 0,
    enemy = 1,
    neutral = 2
}

public class Thing : movableObject, IMovableSupplement {
    #region variable
    protected int damageTotalDealt_;

    private ICaseUpdateState semaphoreState;

    protected uiPersonalCanvas thisPersonalCanvas;

    protected skillAbst thisSkill;
    protected List<caseBase> listCaseBaseAll;
    protected List<toolWeapon> listToolWeapon;

    private SortedSet<string> setAttackTriggerName;
    protected Animator thisAnimController;

    protected selecterAbst selecterForAttack;
    protected selecterAbst selecterForSkill;
    protected wigwaggerMove wigwaggerForMove;
    protected wigwaggerSkill wigwaggerForSkill;

    #region property
    public enumSide thisSide { get; protected set; }
    public int maxHp { get; protected set; }
    public int curHp { get; protected set; }
    public int damageTotalDealt { get { return damageTotalDealt_; } }
    public Thing whatToAttack { get; private set; }
    public Thing whatToUseSkill { get; private set; }
    public node curPosition { get; set; }
    public enumStateWarrior stateCur { get; private set; }
    public toolWeapon[] copyWeapons {
        get { return listToolWeapon.ToArray(); }
    }
    #endregion property
    #endregion variable

    public virtual void init(enumSide parSide, int parMaxHp, int[] parSkillParameters) {
        semaphoreState = null;
        listCaseBaseAll = new List<caseBase>();
        listToolWeapon = new List<toolWeapon>();
        setAttackTriggerName = new SortedSet<string>();
        thisAnimController = gameObject.GetComponent<Animator>();
        stateCur = enumStateWarrior.idleAttack;
        maxHp = parMaxHp;
        curHp = maxHp;

        setAttackTriggerName = new SortedSet<string>();

        thisSide = parSide;
        damageTotalDealt_ = 0;
        //¡Ú

        initPersonal(parSkillParameters);

        GameObject tempPersonalCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/UI/canvasPersonal"));
        tempPersonalCanvas.transform.SetParent(transform);
        thisPersonalCanvas = tempPersonalCanvas.GetComponent<uiPersonalCanvas>();
        thisPersonalCanvas.setSkillImage(thisSkill.skillName);
        thisPersonalCanvas.updateHpText(curHp);
        thisPersonalCanvas.updateSkillTimer(thisSkill.timerCur, thisSkill.timerMax);
    }

    protected virtual void initPersonal(int[] parSkillParameters = null) { }

    #region interface_implements
    public void whenStartMove() { }

    public void whenEndMove() {
        thisAnimController.SetBool("isRun", false);
    }
    #endregion interface_implements

    #region processes
    public virtual void updateTargets() {
        whatToAttack = selecterForAttack.select(this);
        whatToUseSkill = selecterForSkill.select(this);
    }

    public void updateState() {
        if (stateCur <= enumStateWarrior.deadRecently) { return; }
        /*
        although technically updateState could be processed with onBeforeAction, it's separated due to algorithm below
        
        change of stateCur require to check all ICaseUpdateState, so it can't be done in only one method
        1. change to lower-value-state by any updater : change instantly, update semaphoreState
        2. change to higher-value-state by semaphoreState : change instantly
        3. change to higher-value-state by none-semaphoreState : save it temporarily and use it when proper
        
        main problem is when case 2 and 3 occur at the same time, case 3 should be applied although case 2 holds the semaphore and blocks it
        so the saved data of case 3 could be applied if it has lower-value-state than stateCur after all onUpdateState is called
        */

        (ICaseUpdateState updater, enumStateWarrior ESW) tempMemory = (null, enumStateWarrior.idleAttack);
        (ICaseUpdateState updater, enumStateWarrior ESW) tempBuffer;
        foreach (ICaseUpdateState cb in getCaseList<ICaseUpdateState>()) {
            tempBuffer = cb.onUpdateState(this);
            if (tempBuffer.ESW < stateCur) {
                semaphoreState?.onIntefered(this);
                stateCur = tempBuffer.ESW;
                semaphoreState = tempBuffer.updater;
            } else if (semaphoreState == tempBuffer.updater) {
                stateCur = tempBuffer.ESW;
            } else if (tempBuffer.ESW < tempMemory.ESW) {
                tempMemory = tempBuffer;
            }
        }

        if (tempMemory.ESW < stateCur) {
            stateCur = tempMemory.ESW;
            semaphoreState = tempMemory.updater;
        }
    }

    public node getNextRoute() {
        return wigwaggerForMove.getNextRoute(this);
    }

    public int setCurHp(int parValue, Thing source, bool isPlus = true) {
        //onBeforeHp Increase / Decrease
        bool tempIsIncrease = (isPlus && parValue >= 0) || (!isPlus && (parValue - curHp) >= 0);
        if (tempIsIncrease) {
            foreach (ICaseBeforeHpIncrease cb in getCaseList<ICaseBeforeHpIncrease>()) {
                cb.onBeforeHpIncrease(source, ref parValue);
            }
        } else {
            foreach (ICaseBeforeHpDecrease cb in getCaseList<ICaseBeforeHpDecrease>()) {
                cb.onBeforeHpDecrease(source, ref parValue);
            }
        }

        int tempResultChange = 0;
        if (isPlus) {
            if (curHp + parValue < 0) {
                tempResultChange = -curHp;
                curHp = 0;
            } else if (curHp + parValue > maxHp) {
                tempResultChange = maxHp - curHp;
                curHp = maxHp;
            } else {
                tempResultChange = parValue;
                curHp += parValue;
            }
        } else {
            tempResultChange = (parValue > curHp) ? (parValue - curHp) : (curHp - parValue);
            curHp = parValue;
        }

        //onAfterHp Increase / Decrease
        if (tempIsIncrease) {
            foreach (ICaseAfterHpIncrease cb in getCaseList<ICaseAfterHpIncrease>()) {
                cb.onAfterHpIncrease(source, tempResultChange);
            }
        } else {
            foreach (ICaseAfterHpDecrease cb in getCaseList<ICaseAfterHpDecrease>()) {
                cb.onAfterHpDecrease(source, tempResultChange);
            }
        }

        //if curHp_ is below zero, warrior dies
        if (curHp <= 0) {
            destroied(source);
        }

        thisPersonalCanvas.updateHpText(curHp);

        return tempResultChange;
    }

    public void useSkill() {
        thisSkill.useSkill(this, whatToUseSkill);
    }

    public void destroy(Thing target) {
        foreach (ICaseDestroy ca in getCaseList<ICaseDestroy>()) {
            ca.onDestroy(this, target);
        }
    }

    public virtual void destroied(Thing source) {
        //onDestroy of source
        source.destroy(this);

        //onDestroied
        foreach (ICaseDestroied ca in getCaseList<ICaseDestroied>()) {
            ca.onDestroied(this, source);
        }

        stateCur = enumStateWarrior.deadRecently;

        combatManager.CM.addDeadThing(this);
        combatManager.CM.removeThing(this);
    }

    public virtual void destroiedTotally() {
        stateCur = enumStateWarrior.dead;
    }
    #endregion processes

    #region utility
    public void setCircuit(
        int parCodeSelecterForAttack,       int[] ppSelecterForAttack,
        int parCodeSelecterForSkill,        int[] ppSelecterForSkill,
        int parCodeMoveSensorIdle,          int[] ppMoveSensorIdle,
        int parCodeMoveSensorPrioritized,   int[] ppMoveSensorPrioritized,
        int parCodeNavigatorIdle,           int[] ppNavigatorIdle,
        int parCodeNavigatorPrioritized,    int[] ppNavigatorPrioritized,
        int parCodeSkillSensorIdle,         int[] ppSkillSensorIdle,
        int parCodeSkillSensorPrioritized,  int[] ppSkillSensorPrioritized) {

        selecterForAttack = circuitMaker.makeSelecter(parCodeSelecterForAttack, ppSelecterForAttack);
        selecterForSkill = circuitMaker.makeSelecter(parCodeSelecterForSkill, ppSelecterForSkill);

        wigwaggerForMove = new wigwaggerMove(
            circuitMaker.makeSensor(parCodeMoveSensorIdle, ppMoveSensorIdle),
            circuitMaker.makeNavigator(parCodeNavigatorIdle, ppNavigatorIdle),
            circuitMaker.makeSensor(parCodeMoveSensorPrioritized, ppMoveSensorPrioritized),
            circuitMaker.makeNavigator(parCodeNavigatorPrioritized, ppNavigatorPrioritized)
            );

        wigwaggerForSkill = new wigwaggerSkill(
            circuitMaker.makeSensor(parCodeSkillSensorIdle, ppSkillSensorIdle),
            circuitMaker.makeSensor(parCodeSkillSensorPrioritized, ppSkillSensorPrioritized)
            );

        addCase(wigwaggerForMove);
        addCase(wigwaggerForSkill);
    }

    public void addDamageTotalDealt(int par) {
        if (par > 0) {
            damageTotalDealt_ += par;
        }
    }

    public virtual void addCase(caseBase parCase) {
        listCaseBaseAll.Add(parCase);
        switch (parCase.caseType) {
            case enumCaseType.tool:
                if (parCase is toolWeapon tempToolWeapon) {
                    listToolWeapon.Add(tempToolWeapon);
                    tempToolWeapon.owner = this;
                    setAttackTriggerName.Add(tempToolWeapon.animationType.ToString());
                    thisAnimController.SetFloat("multiplierAttack", setAttackTriggerName.Count);
                }
                break;
            case enumCaseType.circuit:
                break;
            /*case enumCaseType.effect:
                listEffect.Insert(insertPosition, parCase);
                break;*/
            case enumCaseType.skill:
                thisSkill = (skillAbst)parCase;
                break;
            default:
                break;
        }
    }

    public virtual void removeCase(caseBase parCase) {
        listCaseBaseAll.Remove(parCase);
        switch (parCase.caseType) {
            case enumCaseType.tool:
                if (parCase is toolWeapon tempToolWeapon) {
                    listToolWeapon.Remove(tempToolWeapon);
                    setAttackTriggerName.Remove(tempToolWeapon.animationType.ToString());
                    thisAnimController.SetFloat("multiplierAttack", Mathf.Max(setAttackTriggerName.Count, 1f));
                }
                break;
            case enumCaseType.circuit:
                break;
            /*case enumCaseType.effect:
                listEffect.Remove(parCase);
                break;*/
            case enumCaseType.skill:
                thisSkill = null;
                break;
            default:
                break;
        }
    }

    public List<T> getCaseList<T>() {
        List<T> tempResult = null;

        foreach (caseBase cb in listCaseBaseAll) {
            if (tempResult == null) {
                tempResult = new List<T>();
            }
            if (cb is T tempCA)
                tempResult.Add(tempCA);
        }

        return tempResult;
    }

    public List<caseBase> getCaseList(enumCaseType parCaseType) {
        List<caseBase> tempResult = null;

        foreach (caseBase cb in listCaseBaseAll) {
            if (tempResult == null) {
                tempResult = new List<caseBase>();
            }
            if (cb.caseType == parCaseType) {
                tempResult.Add(cb);
            }
        }

        return tempResult;
    }

    public void clearAttackAnimation() {
        setAttackTriggerName.Clear();
    }

    public void addAttackAnimation(string parString) {
        setAttackTriggerName.Add(parString);
    }

    public virtual void animate(Vector3 parLookDirection) {
        void Look(Vector3 parLookDirection) {
            if (parLookDirection != null) {
                transform.rotation = Quaternion.LookRotation(parLookDirection - transform.position);
            }
        }

        thisAnimController.SetBool("isFocussing", false);
        thisAnimController.SetBool("isControlled", false);
        switch (stateCur) {
            case enumStateWarrior.deadRecently:
                thisAnimController.SetTrigger("trigDead");
                //¡Ú ÆäÀÌµå ¾Æ¿ô
                break;
            case enumStateWarrior.controlled:
                thisAnimController.SetBool("isControlled", true);
                break;
            case enumStateWarrior.focussing:
                Look(parLookDirection);
                thisAnimController.SetBool("isFocussing", true);
                break;
            case enumStateWarrior.move:
                Look(parLookDirection);
                thisAnimController.SetBool("isRun", true);
                break;
            case enumStateWarrior.idleAttack:
                Look(parLookDirection);
                thisAnimController.SetTrigger("trigAttackStart");
                foreach (string trigName in setAttackTriggerName) {
                    thisAnimController.SetTrigger(trigName);
                }
                break;
            case enumStateWarrior.skill:
                Look(parLookDirection);
                thisAnimController.SetTrigger("trigUseSkill");
                break;
            default:
                break;
        }
    }

    public void updateSkillTimer(int parTimerCur, int parTimerMax) {
        thisPersonalCanvas.updateSkillTimer(parTimerCur, parTimerMax);
    } 
    #endregion utility
}
