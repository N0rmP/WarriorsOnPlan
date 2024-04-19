using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

//move state is same as idle state
public enum enumStateWarrior {
    controlled = 0,
    focussing = 1,
    skill = 2,
    movePrioritized = 3,
    attack = 4,
    move = 5
    }

public abstract class warriorAbst : Thing
{
    protected bool isPlrSide_;

    protected int damageTotalDealt_;
    //protected int damageTotalTaken_;  

    private caseAll toolSkill;
    private List<caseAll> listCaseAllAll;
    private List<caseAll> listToolAll;
    private List<toolWeapon> listWeapon;
    private List<caseAll> listCircuit;
        
    private Thing whatToAttack_;
    private Thing whatToUseSkill_;

    public enumStateWarrior stateCur { get; set; }

    #region properties
    public bool isPlrSide { get { return isPlrSide_; } }
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

    #region callback
    public void Update() {

    }
    #endregion callback

    #region utility
    public void updateTargets() {
        whatToAttack_ = selecterForAttack.select(isPlrSide_);
        whatToUseSkill_ = selecterForSkill.select(isPlrSide_);
    }

    public virtual void init(bool parisPlrSide, int parCoor0, int parCoor1, int parMaxHp = 1) {
        isPlrSide_ = parisPlrSide;
        damageTotalDealt_ = 0;
        this.setInitialMaxHp(parMaxHp);
        stateCur = enumStateWarrior.move;
        base.init(parMaxHp);
        combatManager.CM.processPlace(this, parCoor0, parCoor1);

        listCaseAllAll = new List<caseAll>();
        listToolAll = new List<caseAll>();
        listWeapon = new List<toolWeapon>();
        listCircuit = new List<caseAll>();
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
        //★ 생각해보면 어차피 on~ 메서드 실행하는 순서만 중요한데 inserPosition 이거 listCaseAllAll한테만 중요한 거 아니냐?, 나중에 아랫줄 지우고 Add로 모두 바꿔라;;
        insertPosition = 0;
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
                listCircuit.Insert(insertPosition, parCase);
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
                if (parCase is toolWeapon) {
                    listWeapon.Remove((toolWeapon)parCase);
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
    #endregion utility


}
