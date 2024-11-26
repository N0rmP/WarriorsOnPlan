using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
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

public abstract class Thing : movableObject, IMovableSupplement {
    #region variable
    private ICaseUpdateState semaphoreState;

    protected canvasPersonal thisCanvasPersonal;
    protected cursor thisCursor;

    protected List<caseBase> listCaseBaseAll;
    protected List<toolWeapon> listToolWeapon;

    private SortedSet<string> setAttackTriggerName;
    protected Animator thisAnimController;

    protected circuitHub thisCircuitHub;

    #region property
    public enumSide thisSide { get; protected set; }
    public int maxHp { get; protected set; }
    public int curHp { get; private set; }
    public int weaponAmplifierAdd { get; private set; }
    public int weaponAmplifierMultiply { get; private set; }
    public int skillAmplifierAdd { get; private set; }
    public int skillAmplifierMultiply { get; private set; }
    public int armorAdd { get; private set; }
    public int armorMultiply { get; private set; }
    public int damageTotalDealt { get; private set; }
    public int damageTotalTaken { get; private set; }
    public skillAbst thisSkill { get; protected set; }
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
        thisCircuitHub = new circuitHub(this);  addCase(thisCircuitHub);
        stateCur = enumStateWarrior.idleAttack;

        setAttackTriggerName = new SortedSet<string>();

        thisSide = parSide;

        weaponAmplifierAdd = 0;
        weaponAmplifierMultiply = 0;
        skillAmplifierAdd = 0;
        skillAmplifierMultiply = 0;
        armorAdd = 0;
        armorMultiply = 0;
        damageTotalDealt = 0;
        damageTotalTaken = 0;

        try {
            addCase(makeSkill(parSkillParameters));
        } catch (Exception e) {
            Debug.Log(GetType() + " results in a error while making skill ((" + e.Message);
            addCase(new skillPowerShot(new int[5] { 2, 1, 1, 3, 1 }));
        }

        GameObject tempObj;

        tempObj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/canvasPersonal"));
        tempObj.transform.SetParent(transform);
        thisCanvasPersonal = tempObj.GetComponent<canvasPersonal>();
        thisCanvasPersonal.setSkill(thisSkill);
        thisCanvasPersonal.updateHpText(curHp);
        thisCanvasPersonal.updateSkillTimer(thisSkill.timerCur, thisSkill.timerMax);

        thisCanvasPersonal.transform.GetChild(2).gameObject.AddComponent<releasablePersonal>().init(this);
        if (thisSide == enumSide.player) {            
            thisCanvasPersonal.transform.GetChild(2).gameObject.AddComponent<dragablePersonal>().init(this);
        }

        tempObj = Instantiate(Resources.Load<GameObject>("Prefabs/Cursor"));
        tempObj.transform.SetParent(transform);
        thisCursor = tempObj.GetComponent<cursor>();

        maxHp = parMaxHp;
        setCurHp(maxHp, null, false);
    }

    protected abstract skillAbst makeSkill(int[] parSkillParameters);

    #region interface_implements
    public void whenStartMove() { }

    public void whenEndMove() {
        thisAnimController.SetBool("isRun", false);
    }
    #endregion interface_implements

    #region processes
    public virtual void updateTargets() {
        whatToAttack = thisCircuitHub.selectAttackTarget(this);
        whatToUseSkill = thisCircuitHub.selectSkillTarget(this);
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
        return thisCircuitHub.getNextRoute(this);
    }

    // isPlus ain't asking is value positive or negative, it's asking is newly-setting curHp or adding value to the origial curHp
    public int setCurHp(int parValue, Thing source, bool isPlus = true) {
        //onBeforeHp Increase / Decrease
        bool tempIsIncrease = (isPlus && parValue > 0) || (!isPlus && (curHp != maxHp) && (parValue > curHp));
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

        thisCanvasPersonal.updateHpText(curHp);

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
        if (source != this || source != null) {
            source.destroy(this);
        }

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
        int parCodeSensorForMove, int[] ppSensorForMove,
        int parCodeNavigatorPrioritized, int[] ppNavigatorPrioritized,
        int parCodeNavigatorIdle, int[] ppNavigatorIdle,
        int parCodeSensorForSkill, int[] ppSensorForSkill,
        int parCodeSelecterForSkill, int[] ppSelecterForSkill,
        int parCodeSelecterForAttack, int[] ppSelecterForAttack) {

        thisCircuitHub.setCircuitHub(
        this,
        parCodeSensorForMove, ppSensorForMove,
        parCodeNavigatorPrioritized, ppNavigatorPrioritized,
        parCodeNavigatorIdle, ppNavigatorIdle,
        parCodeSensorForSkill, ppSensorForSkill,
        parCodeSelecterForSkill, ppSelecterForSkill,
        parCodeSelecterForAttack, ppSelecterForAttack
            );
    }

    public string[] getCircuitInfo() {
        return thisCircuitHub.getTotalInfo();
    }

    public int[] getCircuitParameters(int parCircuitType) {
        return thisCircuitHub.getSingleParameter(parCircuitType);
    }

    public virtual void addCase(caseBase parCase) {
        listCaseBaseAll.Add(parCase);
        switch (parCase.caseType) {
            case enumCaseType.tool:
                if (parCase is toolWeapon tempToolWeapon) {
                    listToolWeapon.Add(tempToolWeapon);
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

        updateCaseResult();
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

        updateCaseResult();
    }

    public void updateCaseResult() {
        // ★ 각 능력치 재계산
        if (combatUIManager.CUM.CStatus.thisThing == this) {
            combatUIManager.CUM.CStatus.updateTotal();
        }
    }

    public List<T> getCaseList<T>() {
        // to prevent on~ methods to be called during prearing step getCaseList doesn't work by returning only empty list
        // you can use it anyway by setting parIsForObserving to false
        if (combatManager.CM.combatState == enumCombatState.preparing) {
            return new List<T> { };
        }

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

    public List<caseBase> getCaseList(enumCaseType parCaseType, bool parIsForObserving = true) {
        // to prevent on~ methods to be called during prearing step getCaseList doesn't work by returning only empty list
        // you can use it anyway by setting parIsForObserving to false
        if (parIsForObserving && combatManager.CM.combatState == enumCombatState.preparing) {
            return new List<caseBase> { };
        }

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

    public bool checkContainCase(caseBase parCase) {
        foreach (caseBase cb in listCaseBaseAll) {
            if (cb.GetType() == parCase.GetType()) {
                return true;
            }
        }
        return false;
    }

    public bool checkContainConcreteCase(caseBase parCase) {
        foreach (caseBase cb in listCaseBaseAll) {
            if (cb == parCase) {
                return true;
            }
        }
        return false;
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
                //★ 페이드 아웃
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
        thisCanvasPersonal.updateSkillTimer(parTimerCur, parTimerMax);
    }

    public void setCursorChosen(bool par) {
        thisCursor.setIsChosen(par);
    }

    public void setCursorHovered(bool par) {
        thisCursor.setIsHovered(par);
    }
    #endregion utility

    #region number
    // most numbers can be negative, damageTotal-Delat & Taken can't be negative
    public void addWeaponAmplifierAdd(int par) {
        weaponAmplifierAdd += par;
    }

    public void addWeaponAmplifierMultiply(int par) {
        weaponAmplifierMultiply += par;
    }

    public void addSkillAmplifierAdd(int par) {
        skillAmplifierAdd += par;
    }

    public void addSkillAmplifierMultiply(int par) {
        skillAmplifierMultiply += par;
    }

    public void addArmorAdd(int par) {
        armorAdd += par;
    }

    public void addArmorMultiply(int par) {
        armorMultiply += par;
    }

    public void addDamageTotalDealt(int par) {
        if (par > 0) {
            damageTotalDealt += par;
        }
    }

    public void addDamageTotalTaken(int par) {
        if (par > 0) {
            damageTotalTaken += par;
        }
    }
    #endregion number
}