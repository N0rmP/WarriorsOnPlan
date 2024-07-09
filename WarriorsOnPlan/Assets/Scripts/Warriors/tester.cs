using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : Thing
{
    public override void init(enumSide parSide, int parMaxHp = 1) {
        base.init(parSide, parMaxHp);
        //¡Úcode below is just for test, initiating warrior may follow json/xml file later
        this.addCase(new weaponTester(new object[1] { (object)1 }));
        this.addCase(new skillPowerShot(5));
        setCircuit(
            new selecterClosest(0b010),
            new wigwaggerMove(new sensorNothing(0), new navigatorAttackOneWeapon()),
            new selecterClosest(0b010),
            new wigwaggerSkill(new sensorNothing(0))
            );
    }
}