using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : warriorAbst
{
    public void Awake() {
        //��code below is just for test, initiating warrior may follow json/xml file later
        this.setInitialMaxHp(100);
    }
}
