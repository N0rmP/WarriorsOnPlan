using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : warriorAbst
{
    public void Awake() {
        //¡Úcode below is just for test, initiating warrior may follow json/xml file later
        this.setInitialMaxHp(100);
        this.addCase(new weaponTester());
        this.selecterForAttack = new selecterClosest();
        this.navigator = new navigatorAttackOneWeapon();
    }
}
