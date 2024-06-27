using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillPowerShot : skillAbst
{
    private readonly int damageOriginal;

    public skillPowerShot(int parDamage) : base(3) {
        isRanged = true;
        damageOriginal = parDamage;
    }

    protected override void actualUseSkill(Thing source, Thing target) {
        Debug.Log("!!!!!!!!! POW!!!!!!!!!!!!! WER!!!!!!!! SHOOOOOOOOOOOOOOOTTTTTTT!!!!!!!!!!");
        combatManager.CM.processDealDamage(source, target, new damageInfo(source, this, damageOriginal, enumDamageType.basic, this.showEffect));
    }

    private void showEffect(Thing source, Thing target) {
        combatManager.CM.FC.callVFX(enumVFXType.simple, enumMoveType.linear, source.transform.position, target.transform.position, 0.5f);
    }
}
