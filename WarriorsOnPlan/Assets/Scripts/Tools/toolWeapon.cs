using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enumDamageType { 
    magic = 0,
    basic = 1
}

public abstract class toolWeapon : caseAll
{
    //range of toolWeapon consists of two int nums. each index represents minimum range and maximum range
    //most toolWeapon's min range is 0.
    protected readonly int rangeMin_;
    protected readonly int rangeMax_;
    protected readonly int damageOriginal;
    protected readonly enumDamageType damageType_;
    //timerMax_ / timerCur_ represent cooltime of this weapon
    protected readonly int timerMax_;
    protected int timerCur_;

    public int rangeMin { get { return rangeMin_; } }
    public int rangeMax { get { return rangeMax_; } }
    public int damageCur { get; set; }
    public enumDamageType damageType { get { return damageType_; } }
    public int timerMax { get { return timerMax_; } }
    public int timerCur { get { return timerCur_; } }

    public toolWeapon(warriorAbst parOwner, int parRangeMin, int parRangeMax, int parDamageOriginal, int parTimerMax) : base(parOwner, enumCaseType.tool) {
        rangeMin_ = parRangeMin;
        rangeMax_ = parRangeMax;
        damageOriginal = parDamageOriginal;
        timerCur_ = 0;
        timerMax_ = parTimerMax;
    }

    public damageInfo getDamageInfo() {
        return new damageInfo(this, damageCur, damageType_);
    }

    public override void onAfterThisAttack(Thing source, Thing target, damageInfo DInfo) {
        timerCur_ = timerMax_;
    }

    public override void onTurnEnd(Thing source) {
        if (timerCur_ > 0) {
            timerCur_--;
        }
    }
}
