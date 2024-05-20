using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponTester : toolWeapon
{
    public weaponTester(warriorAbst parOwner) : base(parOwner, 0, 3, 1, 3) { }

    public override void showEffect(Thing parTarget) {
        combatManager.CM.FC.callTest(owner.transform.position, parTarget.transform.position);
    }
}
