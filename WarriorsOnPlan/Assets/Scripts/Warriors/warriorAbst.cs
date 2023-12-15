using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warriorAbst
{
    private bool isPlrSide;

    private int maxHp;
    private int curHp;

    private toolAbst toolSkill;
    private List<toolAbst> listToolWeapon;
    private List<toolAbst> llistToolOther;
    private List<effectAbst> listEffects;

    //private List<Thread> curProcessings;
    //private List<Coroutine> curProcessings;
}
