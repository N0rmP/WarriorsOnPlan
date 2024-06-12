using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : warriorAbst
{
    public override void init(bool parisPlrSide, int parCoor0, int parCoor1, int parMaxHp = 1) {
        base.init(parisPlrSide, parCoor0, parCoor1, parMaxHp);
        //¡Úcode below is just for test, initiating warrior may follow json/xml file later
        this.addCase(new weaponTester());
        setCircuit(
            new selecterClosest(new System.Object[1] { (System.Object)0b010 }),
            new selecterClosest(new System.Object[1] { (System.Object)0b010 }),
            new wigwaggerMove(new sensorNothing(), new navigatorAttackOneWeapon())
            );
    }
}
