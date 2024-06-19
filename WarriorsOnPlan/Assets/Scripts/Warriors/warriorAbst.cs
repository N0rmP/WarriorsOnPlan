
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Threading;
using UnityEditor.SceneTemplate;
using UnityEditor.Animations;



public abstract class warriorAbst : Thing
{
    #region variables
    protected bool isPlrSide_;

    protected int damageTotalDealt_;

    private skillAbst skillInstance;

    protected selecterAbst selecterForSkill;

    #region properties
    public bool isPlrSide { get { return isPlrSide_; } }
    public int damageTotalDealt { get { return damageTotalDealt_; } }
    public Thing whatToUseSkill { get; private set; }
    #endregion properties
    #endregion variables

    #region overrides
    public virtual void init(bool parisPlrSide, int parCoor0, int parCoor1, int parMaxHp = 1) {
        base.init(parMaxHp);
        isPlrSide_ = parisPlrSide;
        damageTotalDealt_ = 0;
        //★
        combatManager.CM.processPlace(this, parCoor0, parCoor1);
    }

    //★ 추후 wigwaggerForSkill 집어넣을 것
    public void setCircuit(selecterAbst parSelecterForAttack, wigwaggerMove parWigwaggerForMove, selecterAbst parSelecterForSkill) {
        base.setCircuit(parSelecterForAttack, parWigwaggerForMove);

        selecterForSkill = parSelecterForSkill;

        addCase(parSelecterForSkill);
    }

    public override void addCase(caseBase parCase) {
        base.addCase(parCase);

        if (parCase is skillAbst tempSkill) {
            skillInstance = tempSkill;
        }
    }

    public override void removeCase(caseBase parCase) {
        base.removeCase(parCase);

        if (parCase == skillInstance) {
            skillInstance = null;
        }
    }

    public override void updateTargets() {
        base.updateTargets();
        whatToUseSkill = selecterForSkill.select(this);
    }

    public override void destroied(Thing source) {
        base.destroied(source);

        combatManager.CM.addDeadWarrior(this);
        combatManager.CM.removeWarrior(this);
    }

    public override void animate() {
        base.animate();
        스킬 썼으면 스킬에 해당하는 애니메이션 실행하세요오
    }
    #endregion overrides

    #region utility
    public void addDamageTotalDealt(int par) {
        if (par > 0) {
            damageTotalDealt_ += par;
        }
    }
    #endregion utility

    public void useSkill() {
        skillInstance.useSkill(this, whatToUseSkill);
    }
}
