using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponTester : toolWeapon
{
    public weaponTester(int[] parArray) : base(parArray) { }

    public override void showEffect(Thing source, Thing parTarget) {
        combatManager.CM.FC.callVFX(enumVFXType.simple, enumMoveType.parabola, source.transform.position, parTarget.transform.position, 0.7f);
    }
}
