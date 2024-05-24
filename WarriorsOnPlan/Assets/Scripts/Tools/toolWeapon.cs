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
    trigAttackCast
}

public abstract class toolWeapon : caseAll {
    //range of toolWeapon consists of two int nums. each index represents minimum range and maximum range
    //most toolWeapon's min range is 0.
    protected readonly int rangeMin_;
    protected readonly int rangeMax_;
    protected readonly int damageOriginal;
    protected readonly enumDamageType damageType_;
    protected readonly enumAnimationType animationType_;
    //timerMax_ / timerCur_ represent cooltime of this weapon
    protected readonly int timerMax_;
    protected int timerCur_;

    public int rangeMin { get { return rangeMin_; } }
    public int rangeMax { get { return rangeMax_; } }
    public int damageCur { get; set; }
    public enumDamageType damageType { get; private set; }
    public enumAnimationType animationType { get; private set; }
    public int timerMax { get { return timerMax_; } }
    public int timerCur { get { return timerCur_; } }

    public toolWeapon(warriorAbst parOwner, int parRangeMin, int parRangeMax, int parDamageOriginal, int parTimerMax, enumDamageType parDamageType, enumAnimationType parAnimationType) : base(parOwner, enumCaseType.tool) {
        //★ 추후 여유가 있으면 외부 파일을 통해 정보를 생성하도록 변경
        rangeMin_ = parRangeMin;
        rangeMax_ = parRangeMax;
        damageOriginal = parDamageOriginal;
        damageCur = damageOriginal;
        timerCur_ = 0;
        timerMax_ = parTimerMax;
        damageType_ = parDamageType;
        animationType = parAnimationType;
    }

    public damageInfo getDamageInfo() {
        return new damageInfo(this, damageCur, damageType_, this.showEffect);
    }

    public void updateTimer() {
        if (timerCur_ > 0) {
            timerCur_--;
        }
    }

    public override void onAfterThisAttack(Thing source, Thing target, damageInfo DInfo) {
        timerCur_ = timerMax_;
    }

    public abstract void showEffect(Thing parTarget);
}
