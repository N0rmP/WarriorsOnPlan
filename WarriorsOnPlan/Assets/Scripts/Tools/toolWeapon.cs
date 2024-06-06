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

public abstract class toolWeapon : caseBase {
    //range of toolWeapon consists of two int nums. each index represents minimum range and maximum range
    //most toolWeapon's min range is 0.
    protected readonly int damageOriginal;

    public int rangeMin { get; private set; }
    public int rangeMax { get; private set; }
    public int damageCur { get; set; }
    public enumDamageType damageType { get; private set; }
    public enumAnimationType animationType { get; private set; }
    public int timerMax { get; private set; }
    public int timerCur { get; private set; }

    public toolWeapon(string parWeaponName, int parDamageOriginal) : base(enumCaseType.tool) {
        dataWeaponEntity tempDWE = gameManager.GM.EC.getWeaponEntiy(parWeaponName);

        rangeMin = tempDWE.RangeMin;
        rangeMax = tempDWE.RangeMax;
        timerMax = tempDWE.TimerMax;
        damageType = tempDWE.DamageType;
        animationType = tempDWE.AnimationType;

        damageCur = damageOriginal = parDamageOriginal;
        timerCur = 0;
    }

    public damageInfo getDamageInfo() {
        return new damageInfo(this, damageCur, damageType, this.showEffect);
    }

    public void updateTimer() {
        if (timerCur > 0) {
            timerCur--;
        }
    }

    public void resetTimer() {
        timerCur = timerMax;
    }

    public abstract void showEffect(Thing source, Thing parTarget);
}
