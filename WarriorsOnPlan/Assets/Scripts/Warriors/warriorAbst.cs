
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

    private caseBase toolSkill;
    

    #region properties
    public bool isPlrSide { get { return isPlrSide_; } }
    public int damageTotalDealt { get { return damageTotalDealt_; } }
    #endregion properties
    #endregion variables

    #region override
    public virtual void init(bool parisPlrSide, int parCoor0, int parCoor1, int parMaxHp = 1) {
        base.init(parMaxHp);
        isPlrSide_ = parisPlrSide;
        damageTotalDealt_ = 0;
        //¡Ú
        combatManager.CM.processPlace(this, parCoor0, parCoor1);
    }

    

    
    #endregion

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
    #endregion utility
}
