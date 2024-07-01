using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumDamageType { 
    magic = 0,
    basic = 1
}

public enum enumAnimationType { 
    trigAttackBrandish,
    trigAttackStab,
    trigAttackBow,
    trigAttackCrossbow,
    trigAttackCast,
    trigAttackPunch
}

public abstract class toolWeapon : caseTimerSelfishTurn {
    //range of toolWeapon consists of two int nums. each index represents minimum range and maximum range
    //most toolWeapon's min range is 0.
    protected readonly int damageOriginal;

    public Thing owner { get; set; }
    public int rangeMin { get; protected set; }
    public int rangeMax { get; protected set; }
    public int damageCur { get; set; }
    public bool isReady { get; protected set; }
    public enumDamageType damageType { get; protected set; }
    public enumAnimationType animationType { get; protected set; }

    public toolWeapon(int parTimerMax, string parWeaponName, int parDamageOriginal, bool parIsTimerMax = false) : base(parTimerMax, enumCaseType.tool, parIsTimerMax, false) {
        isReady = false;

        damageCur = damageOriginal = parDamageOriginal;
    }

    protected override void doOnAlarmed(Thing source) {
        isReady = true;
    }

    public damageInfo attack() {
        isReady = false;
        resetTimer();
        return getDamageInfo();
    }

    public damageInfo getDamageInfo() {
        return new damageInfo(owner, this, damageCur, damageType, this.showEffect);
    }

    public abstract void showEffect(Thing source, Thing parTarget);
}
