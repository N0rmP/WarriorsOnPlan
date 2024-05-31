using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : warriorAbst
{
    public override void init(bool parisPlrSide, int parCoor0, int parCoor1, int parMaxHp = 1) {
        base.init(parisPlrSide, parCoor0, parCoor1, parMaxHp);
        //¡Úcode below is just for test, initiating warrior may follow json/xml file later
        this.addCase(new weaponTester());
        this.selecterForAttack = new selecterClosest(this);
        this.selecterForSkill = new selecterClosest(this);
        this.navigator = new navigatorAttackOneWeapon(this);
    }
}
