
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Threading;
using UnityEditor.SceneTemplate;
using UnityEditor.Animations;



public abstract class warriorAbst : Thing, IMovableSupplement
{
    #region variables
    protected bool isPlrSide_;

    protected int damageTotalDealt_;

    private caseBase toolSkill;
        
    private Thing whatToAttack_;
    private Thing whatToUseSkill_;
    

    #region properties
    public bool isPlrSide { get { return isPlrSide_; } }
    public int damageTotalDealt { get { return damageTotalDealt_; } }

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

        setAttackTriggerName = new SortedSet<string>();
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
    #endregion mainProcesses

    #region overrides
    public override void addCase(caseBase parCase) {
        base.addCase(parCase);

        if (parCase.caseType == enumCaseType.skill) {
            toolSkill = parCase;
        }
    }

    public override void removeCase(caseBase parCase) {
        base.removeCase(parCase);

        if (parCase == toolSkill) {
            toolSkill = null;
        }
    }

    public override void destroied(Thing source) {
        base.destroied(source);

        combatManager.CM.addDeadWarrior(this);
        combatManager.CM.removeWarrior(this);
    }
    #endregion overrides

    #region utility
    public void addDamageTotalDealt(int par) {
        if (par > 0) {
            damageTotalDealt_ += par;
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
