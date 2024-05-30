
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Threading;
using UnityEditor.SceneTemplate;
using UnityEditor.Animations;

//move state is same as idle state
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

public abstract class warriorAbst : Thing, IMovableSupplement
{
    #region variables
    protected bool isPlrSide_;

    protected int damageTotalDealt_;

    private caseAll toolSkill;
    private List<caseAll> listCaseAllAll;
    private List<caseAll> listToolAll;
    private List<toolWeapon> listWeapon;
    //private List<> listEffect;
    private List<caseAll> listCircuit;

    private SortedSet<string> setAttackTriggerName;
        
    private Thing whatToAttack_;
    private Thing whatToUseSkill_;
    private Animator thisAnimController;

    public enumStateWarrior stateCur { get; set; }
    private ICaseUpdateState semaphoreState;

    #region properties
    public bool isPlrSide { get { return isPlrSide_; } }
    public int damageTotalDealt { get { return damageTotalDealt_; } }
    public List<caseAll> copyCaseAllAll { get { return listCaseAllAll.ToList<caseAll>(); } }
    public List<caseAll> copyToolAll { get { return listToolAll.ToList<caseAll>(); } }
    public List<toolWeapon> copyWeapon { get { return listWeapon.ToList<toolWeapon>(); } }
    public List<caseAll> copyCircuit { get { return listCircuit.ToList<caseAll>(); } }
    public navigatorAbst navigator { get; set; }
    public wigwaggerAbst wigwagger { get; set; }
    public selecterAbst selecterForAttack { get; set; }
    public selecterAbst selecterForSkill { get; set; }
    public Thing whatToAttack {
        get {
            return whatToAttack_;
        }
    }
    public Thing whatToUseSkill {
        get {
            return whatToUseSkill_;
        }
    }
    #endregion properties
    #endregion variables

    #region override
    public virtual void init(bool parisPlrSide, int parCoor0, int parCoor1, int parMaxHp = 1) {
        base.init(parMaxHp);
        isPlrSide_ = parisPlrSide;
        damageTotalDealt_ = 0;
        stateCur = enumStateWarrior.idleAttack;
        combatManager.CM.processPlace(this, parCoor0, parCoor1);
        thisAnimController = gameObject.GetComponent<Animator>();

        listCaseAllAll = new List<caseAll>();
        listToolAll = new List<caseAll>();
        listWeapon = new List<toolWeapon>();
        //listEffect = new List<caseAll>();
        listCircuit = new List<caseAll>();

        setAttackTriggerName = new SortedSet<string>();
    }

    public override void destroied(warriorAbst source) {
        //onDestroy of source
        source.destroy(this);

        //onDestroied
        foreach (caseAll ca in copyCaseAllAll) {
            ca.onDestroied(this, source);
        }

        stateCur = enumStateWarrior.deadRecently;
        combatManager.CM.addDeadWarrior(this);
        combatManager.CM.removeWarrior(this);
    }

    public void whenStartMove() { }

    public void whenEndMove() {
        thisAnimController.SetBool("isRun", false);
    }
    #endregion


    #region mainProcesses
    public void updateTargets() {
        whatToAttack_ = selecterForAttack.select(isPlrSide_);
        whatToUseSkill_ = selecterForSkill.select(isPlrSide_);
    }

    public void destroy(warriorAbst target) {
        foreach (caseAll ca in copyCaseAllAll) {
            ca.onDestroy(this, target);
        }
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
        foreach (caseAll ca in copyCaseAllAll) {
            if (ca is not ICaseUpdateState) {
                continue;
            }

            tempBuffer = ((ICaseUpdateState)ca).onUpdateState(this);
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
    #endregion mainProcesses

    #region utility
    public void addDamageTotalDealt(int par) {
        if (par > 0) {
            damageTotalDealt_ += par;
        }
    }

    //insertPosition parameter can be 3 num : below zero = index 0 , zero = index (List.Count / 2) , above zero = the last index
    public void addCase(caseAll parCase, int insertPosition = 0) {
        switch (insertPosition) {
            case < 0:
                insertPosition = 0;
                break;
            case > 0:
                insertPosition = listCaseAllAll.Count;
                break;
            default:
                insertPosition = listCaseAllAll.Count / 2;
                break;
        }

        listCaseAllAll.Insert(insertPosition, parCase);
        insertPosition = 0;
        switch (parCase.caseType) { 
            case enumCaseType.skill:
                toolSkill = parCase;
                break;
            case enumCaseType.tool:
                if (parCase is toolWeapon tempToolWeapon) {
                    listWeapon.Add(tempToolWeapon);
                    setAttackTriggerName.Add(tempToolWeapon.animationType.ToString());
                    thisAnimController.SetFloat("multiplierAttack", setAttackTriggerName.Count);
                }
                listToolAll.Add(parCase);
                break;
            case enumCaseType.circuit:
                listCircuit.Add(parCase);
                switch (parCase) {
                    case navigatorAbst tempN:
                        this.navigator = tempN;
                        break;
                    default:
                        break;
                }
                break;
            /*case enumCaseType.effect:
                listEffect.Insert(insertPosition, parCase);
                break;*/
            default:
                break;
        }

        parCase.owner = this;
    }

    public void removeCase(caseAll parCase) {
        listCaseAllAll.Remove(parCase);
        switch (parCase.caseType) {
            case enumCaseType.skill:
                toolSkill = null;
                break;
            case enumCaseType.tool:
                if (parCase is toolWeapon tempToolWeapon) {
                    listWeapon.Remove(tempToolWeapon);
                    setAttackTriggerName.Remove(tempToolWeapon.animationType.ToString());
                    thisAnimController.SetFloat("multiplierAttack", Mathf.Max(setAttackTriggerName.Count, 1f));
                }
                listToolAll.Remove(parCase);
                break;
            case enumCaseType.circuit:
                listCircuit.Remove(parCase);
                break;
            /*case enumCaseType.effect:
                listEffect.Remove(parCase);
                break;*/
            default:
                break;
        }
    }

    public void clearAttackAnimation() {
        setAttackTriggerName.Clear();
    }

    public void addAttackAnimation(string parString) {
        setAttackTriggerName.Add(parString);
    }

    public void animate() {
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
                //¡Ú ÆäÀÌµå ¾Æ¿ô
                break;
            default:
                break;
        }
    }
    #endregion utility
}
