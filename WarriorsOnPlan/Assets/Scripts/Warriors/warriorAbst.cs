using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public enum stateWarrior { attack, controlled, focussing, move, skill }

public abstract class warriorAbst : Thing
{
    protected bool isPlrSide;

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

    public warriorAbst() {
        listToolAll = new List<caseAll>();
        listWeapon = new List<toolWeapon>();
        listEffect = new List<caseAll>();
        listCable = new List<caseAll>();
    }

    #region callback
    public void Update() {

    }
    #endregion callback

    #region utility
    public void addCase(caseAll parCase, bool isSkill = false) {
        switch (parCase.caseType) { 
            case enumCaseType.skill:
                toolSkill = parCase;
                break;
            case enumCaseType.tool:
                if (parCase is toolWeapon) {
                    listWeapon.Add((toolWeapon)parCase);
                }
                listToolAll.Add(parCase);
                break;
            case enumCaseType.circuit:
                listCable.Add(parCase);
                break;
            case enumCaseType.effect:
                listEffect.Add(parCase);
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
