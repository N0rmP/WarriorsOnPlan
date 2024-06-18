using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillPowerShot : skillAbst
{
    private readonly int damageOriginal;

    public skillPowerShot(int parDamage) : base(5) {
        isRanged = true;
        damageOriginal = parDamage;
    }

    public override void useSkill(Thing source, Thing target) {
        combatManager.CM.processDealDamage(source, target, new damageInfo(source, this, damageOriginal, enumDamageType.basic, this.showEffect));
    }

    private void showEffect(Thing source, Thing target) {
        combatManager.CM.FC.callVFX(enumVFXType.simple, enumMoveType.linear, source.transform.position, target.transform.position, 0.5f);
    }
}
