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

public abstract class toolWeapon : caseTimer {
    //range of toolWeapon consists of two int nums. each index represents minimum range and maximum range
    //most toolWeapon's min range is 0.
    protected readonly int damageOriginal;

    public Thing owner { get; set; }
    public int rangeMin { get; private set; }
    public int rangeMax { get; private set; }
    public int damageCur { get; set; }
    public enumDamageType damageType { get; private set; }
    public enumAnimationType animationType { get; private set; }

    public toolWeapon(int parTimerMax, string parWeaponName, int parDamageOriginal, bool parIsTimerMax = false) : base(parTimerMax, enumCaseType.tool, parIsTimerMax) {
        dataWeaponEntity tempDWE = gameManager.GM.EC.getWeaponEntiy(parWeaponName);

        rangeMin = tempDWE.RangeMin;
        rangeMax = tempDWE.RangeMax;
        damageType = tempDWE.DamageType;
        animationType = tempDWE.AnimationType;

        damageCur = damageOriginal = parDamageOriginal;
    }

    public damageInfo getDamageInfo() {
        return new damageInfo(owner, this, damageCur, damageType, this.showEffect);
    }

    public abstract void showEffect(Thing source, Thing parTarget);
}
