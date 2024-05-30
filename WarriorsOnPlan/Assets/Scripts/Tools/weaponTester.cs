using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponTester : toolWeapon
{
    public weaponTester() : base("training bow", 1) { }

    public override void showEffect(Thing parTarget) {
        combatManager.CM.FC.callVFX(enumVFXType.simple, enumMoveType.parabola, owner.transform.position, parTarget.transform.position, 0.7f);
    }
}
