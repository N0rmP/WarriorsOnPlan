using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumCaseType { 
    none = -1,
    circuit = 0,
    skill = 1,
    tool = 2,
    effect = 3,
    others = 99
}

public abstract class caseAll {
    public readonly enumCaseType caseType;

    public warriorAbst owner { get; set; }

    public caseAll(warriorAbst parOwner, enumCaseType parCaseType = enumCaseType.effect) {
        owner = parOwner;
        caseType = parCaseType;
    }

    public void selfRemove() {
        owner.removeCase(this);
    }

    //be aware of that source / target can be different from owner
    //on~ method with none-void-return is separated to interfaces

    //onEngage includes onCombatStart / onEngageDuringCombat
    public virtual void onEngage(Thing source) { }
    public virtual void onAdded(Thing source) { }

    public virtual void onTurnStart(Thing source) { }
    public virtual void onTurnEnd(Thing source) { }

    public virtual void onBeforeAction(Thing source) { }
    public virtual void onAfterAction(Thing source) { }

    //source of onAttack is owner, target is the to-be-attacked warrior
    public virtual void onBeforeAttack(Thing source, Thing target, ref int value, ref enumDamageType DType) { }
    public virtual void onAfterAttack(Thing source, Thing target, int value) { }
    //source of onDamaged is the attacker warrior, target is owner
    public virtual void onBeforeDamaged(Thing source, Thing target, ref int value) { }
    public virtual void onAfterDamaged(Thing source, Thing target, int value) { }
    public virtual void onHpIncrease(Thing source, int value) { }
    public virtual void onHpDecrease(Thing source, int value) { }
    public virtual void onSkillReady(Thing source) { }
    //source of onUseSkill is owner, target is the target of the skill
    public virtual void onBeforeUseSkill(Thing source, Thing target = null) { }
    public virtual void onAfterUseSkill(Thing source, Thing target = null) { }
    //source of onDestroy is owner, target is the destroied warrior
    public virtual void onDestroy(Thing source, Thing target) { }
    //source of onDestroied is the destroyer warrior, target is owner
    public virtual void onDestroied(Thing source, Thing target) { }
    public virtual void onRemoved(Thing source) { }
}
