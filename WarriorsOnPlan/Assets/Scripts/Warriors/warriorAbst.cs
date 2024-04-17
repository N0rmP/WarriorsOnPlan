using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public enum stateWarrior { attack, controlled, focussing, move, skill }

public abstract class warriorAbst : Thing
{
    protected bool isPlrSide_;

    protected int damageTotalDealt_;
    //protected int damageTotalTaken_;  

    private caseAll toolSkill;
    private List<caseAll> listToolAll;
    private List<toolWeapon> listWeapon;
    private List<caseAll> listEffect;
    private List<caseAll> listCable;
        
    private Thing whatToAttack_;
    private Thing whatToUseSkill_;

    public stateWarrior stateCur { get; set; }

    //private List<Thread> curProcessings;
    //private List<Coroutine> curProcessings;

    #region properties
    public bool isPlrSide { get; }
    public int damageTotalDealt {
        get {
            return damageTotalDealt_;
        }
        set {
            if (value > 0) {
                damageTotalDealt_ += value;
            }
        }
    }
    public List<caseAll> copyToolAll { get { return listToolAll.ToList<caseAll>(); } }
    public List<toolWeapon> copyWeapon { get { return listWeapon.ToList<toolWeapon>(); } }
    public List<caseAll> copyEffects { get { return listEffect.ToList<caseAll>(); } }
    public List<caseAll> copyCable { get { return listCable.ToList<caseAll>(); } }
    public navigatorAbst navigator { get; set; }
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

    #region callback
    public void Update() {

    }
    #endregion callback

    #region utility
    public virtual void init(bool parisPlrSide, int parCoor0, int parCoor1, int parMaxHp = 1) {
        isPlrSide_ = parisPlrSide;
        base.init(parMaxHp);
        combatManager.CM.processPlace(this, parCoor0, parCoor1);

        listToolAll = new List<caseAll>();
        listWeapon = new List<toolWeapon>();
        listEffect = new List<caseAll>();
        listCable = new List<caseAll>();
    }

    //insertPosition parameter can be 3 num : below zero = index 0 , zero = index (List.Count / 2) , above zero = the last index
    public void addCase(caseAll parCase, int insertPosition = 0) {
        switch (insertPosition) {
            case < 0:
                insertPosition = -1;
                break;
            case > 0:
                insertPosition = 1;
                break;
            default:
                break;
        }

        switch (parCase.caseType) { 
            case enumCaseType.skill:
                toolSkill = parCase;
                break;
            case enumCaseType.tool:
                if (parCase is toolWeapon) {
                    listWeapon.Insert(insertPosition, (toolWeapon)parCase);
                }
                listToolAll.Insert(insertPosition, parCase);
                break;
            case enumCaseType.circuit:
                listCable.Insert(insertPosition, parCase);
                break;
            case enumCaseType.effect:
                listEffect.Insert(insertPosition, parCase);
                break;
            default:
                break;
        }

        parCase.owner = this;
    }

    public void removeCase(caseAll parCase) {
        switch (parCase.caseType) {
            case enumCaseType.skill:
                toolSkill = null;
                break;
            case enumCaseType.tool:
                if (parCase is toolWeapon) {
                    listWeapon.Remove((toolWeapon)parCase);
                }
                listToolAll.Remove(parCase);
                break;
            case enumCaseType.circuit:
                listCable.Remove(parCase);
                break;
            case enumCaseType.effect:
                listEffect.Remove(parCase);
                break;
            default:
                break;
        }
    }
    #endregion utility


}
