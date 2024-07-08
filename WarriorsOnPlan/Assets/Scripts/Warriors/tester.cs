using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : Thing
{
    public override void init(enumSide parSide, int parMaxHp = 1) {
        base.init(parSide, parMaxHp);
        //¡Úcode below is just for test, initiating warrior may follow json/xml file later
        this.addCase(new weaponTester());
        this.addCase(new skillPowerShot(5));
        setCircuit(
            new selecterClosest(new object[1] { (object)0b010 }),
            new wigwaggerMove(new sensorNothing(new object[0]), new navigatorAttackOneWeapon(new object[0])),
            new selecterClosest(new object[1] { (object)0b010 }),
            new wigwaggerSkillSelfReady(new object[1] { 0 })
            );
    }
}
