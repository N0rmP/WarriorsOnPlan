using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillPowerShot : skillAbst {
    public skillPowerShot(int[] parSkillParameters) : base(parSkillParameters) { }

    protected override void initDerived(int[] parSkillParameters) {
        valueOriginal = parSkillParameters[4];
    }

    protected override void actualUseSkill(Thing source, Thing target) {
        Debug.Log("!!!!!!!!! POW!!!!!!!!!!!!! WER!!!!!!!! SHOOOOOOOOOOOOOOOTTTTTTT!!!!!!!!!!");
        combatManager.CM.processDealDamage(source, target, new damageInfo(source, this, valueOriginal, enumDamageType.basic, this.showEffect));
    }

    private void showEffect(Thing source, Thing target) {
        combatManager.CM.FC.callVFX(enumVFXType.simple, enumMoveType.linear, source.transform.position, target.transform.position, 0.5f);
    }

    public override int[] getParameters() {
        return new int[1] { valueOriginal };
    }
}
