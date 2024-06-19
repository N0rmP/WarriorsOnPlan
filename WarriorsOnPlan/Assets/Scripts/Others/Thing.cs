using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class Thing : movableObject, IMovableSupplement {
    #region variable    

    private ICaseUpdateState semaphoreState;

    protected caseBase caseSkill;
    protected List<caseBase> listCaseBaseAll;
    protected List<toolWeapon> listToolWeapon;

    private SortedSet<string> setAttackTriggerName;
    private Animator thisAnimController;

    protected wigwaggerMove wigwaggerForMove;
    //★ wigwaggerForSkill needed
    protected selecterAbst selecterForAttack;

    #region property
    public int maxHp { get; protected set; }
    public int curHp { get; protected set; }
    public node curPosition { get; set; }
    public enumStateWarrior stateCur { get; private set; }
    public toolWeapon[] copyWeapons {
        get { return listToolWeapon.ToArray(); }
    }
    public Thing whatToAttack { get; private set; }
    
    #endregion property
    #endregion variable

    public void init(int parMaxHp) {
        semaphoreState = null;
        listCaseBaseAll = new List<caseBase>();
        listToolWeapon = new List<toolWeapon>();
        setAttackTriggerName = new SortedSet<string>();
        thisAnimController = gameObject.GetComponent<Animator>();
        stateCur = enumStateWarrior.idleAttack;
        maxHp = parMaxHp;
        curHp = maxHp;

        setAttackTriggerName = new SortedSet<string>();
    }

    #region overrides
    public void whenStartMove() { }

    public void whenEndMove() {
        thisAnimController.SetBool("isRun", false);
    }
    #endregion overrides

    #region processes
    public virtual void updateTargets() {
        whatToAttack = selecterForAttack.select(this);
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

    public int setCurHp(int parValue, Thing source, bool isPlus = false) {
        int tempResultChange = 0;
        //★ 체력 증감 이전 효과 발동
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
        //★ 체력 증감 이후 효과 발동

        //if curHp_ is below zero, warrior dies
        if (curHp <= 0) {
            destroied(source);
        }

        return tempResultChange;
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
    }

    public virtual void destroiedTotally() {
        stateCur = enumStateWarrior.dead;
    }
    #endregion processes

    #region utility
    public void setCircuit(selecterAbst parSelecterForAttack, wigwaggerMove parWigwaggerForMove) {
        selecterForAttack = parSelecterForAttack;
        wigwaggerForMove = parWigwaggerForMove;

        addCase(parSelecterForAttack);
        addCase(parWigwaggerForMove);
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

    public virtual void animate() {
        switch (stateCur) {
            case enumStateWarrior.move:
                thisAnimController.SetBool("isRun", true);
                break;
            case enumStateWarrior.idleAttack:
                thisAnimController.SetTrigger("trigAttackStart");
                foreach (string trigName in setAttackTriggerName) {
                    thisAnimController.SetTrigger(trigName);
                }
                break;
            case enumStateWarrior.deadRecently:
                thisAnimController.SetTrigger("trigDead");
                //★ 페이드 아웃
                break;
            default:
                break;
        }
    }
    #endregion utility
}
