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
    #endregion properties

    private toolAbst toolSkill;
    private List<toolAbst> listToolAll;
    private List<effectAbst> listEffect;
    private List<ICaseTimed> listToolTimed;
    private List<ICaseAll> listCable;
    public List<toolAbst> copyToolAll { get { return listToolAll.ToList<toolAbst>(); } }
    public List<effectAbst> copyEffects { get { return listEffect.ToList<effectAbst>(); } }
    public List<ICaseTimed> copyToolTimed { get { return listToolTimed.ToList<ICaseTimed>(); } }
    public List<ICaseAll> copyCable { get { return listCable.ToList<ICaseAll>(); } }

    public moverAbst howToMove { get; set; }
    public selecterAbst whatToAttack { get; set; }
    public selecterAbst whatToUseSkill { get; set; }

    public stateWarrior stateCur { get; set; }

    //private List<Thread> curProcessings;
    //private List<Coroutine> curProcessings;

    public warriorAbst() {
        listToolAll = new List<toolAbst>();
        listEffect = new List<effectAbst>();
        listToolTimed = new List<ICaseTimed>();
        listCable = new List<ICaseAll>();
    }

    #region callback
    public void FixedUpdate() {
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
    public void addCase(ICaseAll parCase, bool isSkill = false) {
        if (isSkill && parCase is toolAbst) {
            toolSkill = (toolAbst)parCase;
        } else if (parCase is controller) {
            listController.Add((controller)parCase);
        } else if (parCase is effectAbst) {
            listEffects.Add((effectAbst)parCase);
        } else if (parCase is toolAbst) {
            listToolAll.Add((toolAbst)parCase);
        }

        if (parCase is toolTimed) {
            listToolTimed.Add((toolTimed)parCase);
        }
    }

    public void removeCase(ICaseAll parCase) {
        if (parCase is controller) {
            listController.Remove((controller)parCase);
        } else if (parCase is effectAbst) {
            listEffects.Remove((effectAbst)parCase);
        } else if (parCase is toolAbst) {
            listToolAll.Remove((toolAbst)parCase);
        }

        if (parCase is toolTimed) {
            listToolTimed.Remove((toolTimed)parCase);
        }
    }
    #endregion utility


}
