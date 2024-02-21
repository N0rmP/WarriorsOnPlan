using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public enum stateWarrior { attack, controlled, focussing, move, skill }

public abstract class warriorAbst : MonoBehaviour
{
    private bool isPlrSide;

    private int maxHp_;
    private int curHp_;
    private int damageTotalGiven_;
    private float moveSpeed_;
    public int maxHp { get; }
    public int curHp { get; }
    #region properties
    public int damageTotalGiven {
        get {
            return damageTotalGiven_;
        }
        set {
            if (value > 0) {
                damageTotalGiven_ += value;
            }
        }
    }
    public float moveSpeed {
        get {
            return moveSpeed;
        }
        set {
            moveSpeed_ = value;
            this.gameObject.GetComponent<NavMeshAgent>().speed = value;
        }
    }
    #endregion properties

    private toolAbst toolSkill;
    private List<toolAbst> listToolAll;
    private List<effectAbst> listEffect;
    private List<toolTimed> listToolTimed;
    private List<cableAbst> listCable;
    public List<toolAbst> copyToolAll { get { return listToolAll.ToList<toolAbst>(); } }
    public List<effectAbst> copyEffects { get { return listEffect.ToList<effectAbst>(); } }
    public List<toolTimed> copyToolTimed { get { return listToolTimed.ToList<toolTimed>(); } }
    public List<cableAbst> copyCable { get { return listCable.ToList<cableAbst>(); } }

    public settingMoveAbst howToMove { get; set; }
    public stateWarrior stateCur { get; set; }

    //private List<Thread> curProcessings;
    //private List<Coroutine> curProcessings;

    public warriorAbst() {
        listToolAll = new List<toolAbst>();
        listEffect = new List<effectAbst>();
        listToolTimed = new List<toolTimed>();
        listCable = new List<cableAbst>();
    }

    #region callback
    public void FixedUpdate() {
        float tempDeltaTime = Time.deltaTime;
        foreach (toolTimed tempToolTimed in listToolTimed.ToArray()) {
            tempToolTimed.timerUpdate(tempDeltaTime);
        }

        switch (stateCur) {
            case (stateWarrior.move):
                howToMove.Move(this);
                break;
            case (stateWarrior.skill): break;
            case (stateWarrior.attack): break;
            case (stateWarrior.focussing): break;
            case (stateWarrior.controlled): break;
        }
    }
    #endregion callback

    #region utility
    public void addCase(caseAbst paramCase, bool isSkill = false) {
        if (isSkill && paramCase is toolAbst) {
            toolSkill = (toolAbst)paramCase;
        } else if (paramCase is controller) {
            listController.Add((controller)paramCase);
        } else if (paramCase is effectAbst) {
            listEffects.Add((effectAbst)paramCase);
        } else if (paramCase is toolAbst) {
            listToolAll.Add((toolAbst)paramCase);
        }

        if (paramCase is toolTimed) {
            listToolTimed.Add((toolTimed)paramCase);
        }
    }

    public void removeCase(caseAbst paramCase) {
        if (paramCase is controller) {
            listController.Remove((controller)paramCase);
        } else if (paramCase is effectAbst) {
            listEffects.Remove((effectAbst)paramCase);
        } else if (paramCase is toolAbst) {
            listToolAll.Remove((toolAbst)paramCase);
        }

        if (paramCase is toolTimed) {
            listToolTimed.Remove((toolTimed)paramCase);
        }
    }

    public int setCurHp(int paramValue, bool isPlus = false) {
        int tempResult = 0;
        if (isPlus) {
            if (curHp_ + paramValue < 0) {
                tempResult = -curHp_;
                curHp_ = 0;
            } else if (curHp_ + paramValue > maxHp_) {
                tempResult = maxHp_ - curHp_;
                curHp_ = maxHp_;
            } else {
                tempResult = paramValue;
                curHp_ += paramValue;
            }
        } else {
            curHp_ = paramValue;
        }
        return tempResult;
    }
    #endregion utility


}
