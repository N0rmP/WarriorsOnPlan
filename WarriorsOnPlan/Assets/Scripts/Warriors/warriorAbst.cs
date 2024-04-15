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

    private toolAbst toolSkill;
    private List<toolAbst> listToolAll;
    private List<toolWeapon> listWeapon;
    private List<effectAbst> listEffect;
    private List<ICaseTimed> listToolTimed;
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
    public List<toolAbst> copyToolAll { get { return listToolAll.ToList<toolAbst>(); } }
    public List<toolWeapon> copyWeapon { get { return listWeapon.ToList<toolWeapon>(); } }
    public List<effectAbst> copyEffects { get { return listEffect.ToList<effectAbst>(); } }
    public List<ICaseTimed> copyToolTimed { get { return listToolTimed.ToList<ICaseTimed>(); } }
    public List<caseAll> copyCable { get { return listCable.ToList<caseAll>(); } }
    public navigatorAbst navigator { 
        get; 
        set; 
    }
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
        listToolAll = new List<toolAbst>();
        listWeapon = new List<toolWeapon>();
        listEffect = new List<effectAbst>();
        listToolTimed = new List<ICaseTimed>();
        listCable = new List<caseAll>();
    }

    #region callback
    public void Update() {
        float tempDeltaTime = Time.deltaTime;
        foreach (toolTimed tempToolTimed in listToolTimed.ToArray()) {
            tempToolTimed.timerUpdate(tempDeltaTime);
        }

        /*
        ★이거 combatManager로 옮기고 1초에 1번씩 호출되도록 만드세용
        switch (stateCur) {
            case (stateWarrior.move):
                howToMove.move();
                break;
            case (stateWarrior.skill): break;
            case (stateWarrior.attack): break;
            case (stateWarrior.focussing): break;
            case (stateWarrior.controlled): break;
        }
        */
    }
    #endregion callback

    #region utility
    public void addCase(caseAll parCase, bool isSkill = false) {
        if (isSkill && parCase is toolAbst) {
            toolSkill = (toolAbst)parCase;
            if (parCase is toolWeapon) {
                listWeapon.Add((toolWeapon)parCase);
            }
        //} else if (parCase is Controller) {
        //    listCable.Add((Controller)parCase);
        } else if (parCase is effectAbst) {
            listEffect.Add((effectAbst)parCase);
        } else if (parCase is toolAbst) {
            listToolAll.Add((toolAbst)parCase);
        }

        if (parCase is toolTimed) {
            listToolTimed.Add((toolTimed)parCase);
        }

        parCase.owner = this;
    }

    public void removeCase(caseAll parCase) {
        /*if (parCase is controller) {
            listCable.Remove((controller)parCase);
        } else*/ if (parCase is effectAbst) {
            listEffect.Remove((effectAbst)parCase);
        } else if (parCase is toolAbst) {
            listToolAll.Remove((toolAbst)parCase);
            if (parCase is toolWeapon) {
                listWeapon.Remove((toolWeapon)parCase);
            }
        }

        if (parCase is toolTimed) {
            listToolTimed.Remove((toolTimed)parCase);
        }
    }
    #endregion utility


}
