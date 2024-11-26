using System;
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
    public readonly int damageOriginal;

    public int rangeMin { get; protected set; }
    public int rangeMax { get; protected set; }
    // ★ damageCur 방식 개선, 가능하면 Thing에 case changed를 bool 변수로 나타내게 해서 계산 빈도가 낮도록 메서드 1개 설계
    public int damageCur { get; set; }
    public bool isReady { get; protected set; }
    public enumDamageType damageType { get; protected set; }
    public enumAnimationType animationType { get; protected set; }

    public toolWeapon(int[] parWeaponParameters) : base(
        parWeaponParameters[0],
        enumCaseType.tool,
        true,
        parWeaponParameters[1] == 1,
        false
        ) {
        rangeMin = parWeaponParameters[2] < 1 ? 1 : parWeaponParameters[2];     // rangeMin can't be below 1
        rangeMax = parWeaponParameters[3] < rangeMin ? rangeMin : parWeaponParameters[3];   // rangeMax can't be below rangeMin
        damageCur = damageOriginal = Math.Max(parWeaponParameters[4], 0);       // damage can't be below 0
        initDerived(parWeaponParameters);
        isReady = false;
    }

    protected virtual void initDerived(int[] parWeaponParameters) { }
    protected override void doOnAlarmed(Thing source) {
        isReady = true;
    }

    public damageInfo attack(Thing parOwner) {
        isReady = false;
        resetTimer();
        return getDamageInfo(parOwner);
    }

    public damageInfo getDamageInfo(Thing parOwner) {
        return new damageInfo(parOwner, this, damageCur, damageType, this.showEffect);
    }

    public int checkDamageChanged() {
        return damageCur > damageOriginal ? 1 :
            damageCur == damageOriginal ? 0 :
            -1;
    }

    public abstract void showEffect(Thing source, Thing parTarget);
}
