using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : Thing
{
    public override void init(enumSide parSide, int parMaxHp = 1) {
        base.init(parSide, parMaxHp);
        this.addCase(new skillPowerShot(5));
    }
}